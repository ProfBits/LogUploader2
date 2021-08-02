using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using LogUploader.Data;
using System.Threading;
using Newtonsoft.Json;
using LogUploader.Tools.Logging;
using LogUploader.Tools.Settings;
using Extensiones;
using LogUploader.Helper;
using LogUploader.Tools.RaidOrgaPlus.Data;

namespace LogUploader.Tools.RaidOrgaPlus
{
    public class RaidOrgaPlusConnector
    {
        //TODO implement ProxySettings
        private readonly IProxySettings ProxySettings;

        private const int TimeoutMs = 10_000;
        private const string BASE_ADDRESS = "https://sv.rising-light.de:8080";

        public RaidOrgaPlusConnector(IProxySettings proxySettings)
        {
            ProxySettings = proxySettings;
        }

        public ISession Connect(IRaidOrgaPlusSettings orgaSettings)
        {
            return Connect(orgaSettings, $"LogUploader{new Random().Next(0, 1000000).ToString().PadLeft(6, '0')}");
        }

        public ISession Connect(IRaidOrgaPlusSettings orgaSettings, string userAgent)
        {

            var httpWebRequest = GetPostRequest(BASE_ADDRESS + "/users/sessions", userAgent);

            try
            {
                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream(), Encoding.UTF8))
                {
                    string json = $@"{{""accName"":""{orgaSettings.RaitOrgaPlusUser}"",""pwd"":""{orgaSettings.RaidOrgaPlusPassword}""}}";

                    streamWriter.Write(json);
                }

                string sessionToken;
                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                    sessionToken = streamReader.ReadToEnd();

                if (sessionToken.Length <= 3)
                    return null;

                return new Session(sessionToken, userAgent);
            }
            catch (WebException e)
            {
                Logger.Warn("RO+ Login Faild with exception");
                Logger.LogException(e);
                return null;
            }
        }

        public List<RaidSimple> GetRaids(ISession session, IProgress<double> progress = null)
        {
            if (!session?.Valid ?? false)
                return null;

            progress?.Report(0);

            var request = GetGetRequest(BASE_ADDRESS + $@"/termine?auth={session.Token}", session.UserAgent);

            string termineRAW;
            var httpResponse = (HttpWebResponse)request.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                termineRAW = streamReader.ReadToEnd();

            progress?.Report(0.4);

            request = GetGetRequest(BASE_ADDRESS + $@"/raids?auth={session.Token}", session.UserAgent);

            string raidsRAW;
            httpResponse = (HttpWebResponse)request.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                raidsRAW = streamReader.ReadToEnd();

            progress?.Report(0.8);

            /* Fromat
             * Termine
             * [{"id":582,"raidID":1351,"name":"xyz","icon":null,"role":2,"date":"Fr, 17.07.2020","time":"19:00","endtime":"22:00","type":0}]
             * 
             * Raids
             * [{"id":6,"name":"xyz","icon":"","role":0}]
             * Role=2 == leader
             */

            var raidsParsed = Newtonsoft.Json.Linq.JObject.Parse($@"{{""wrapper"":{raidsRAW}}}");
            var raids = raidsParsed["wrapper"].ToDictionary(raid => (long)raid["id"], raid => (int)raid["role"] == 2);

            progress?.Report(0.9);

            var termineParsed = Newtonsoft.Json.Linq.JObject.Parse($@"{{""wrapper"":{termineRAW}}}");
            var termine = termineParsed["wrapper"]
                .Where(termin => raids[(long)termin["raidID"]])
                .Select(termin => new RaidSimple(
                    (long)termin["id"],
                    (long)termin["raidID"],
                    DateTime.Parse(((string)termin["date"]).Split(' ').Last(), System.Globalization.CultureInfo.GetCultureInfo("de-de")),
                    TimeSpan.Parse((string)termin["time"] + ":00"),
                    TimeSpan.Parse(((string)termin["endtime"] ?? (string)termin["time"]) + ":00"),
                    (string)termin["name"]
                    ));

            progress?.Report(1);
            return termine.ToList();
        }

        public Data.Raid GetRaid(ISession session, long terminID, long raidID, CancellationToken ct, IProgress<ProgressMessage> progress = null)
        {
            if (!session.Valid)
                return null;

            progress.Report(new ProgressMessage(0, "Bosses"));
            var request = GetGetRequest(BASE_ADDRESS + $@"/aufstellungen?termin={terminID}&auth={session.Token}", session.UserAgent);
            if (ct.IsCancellationRequested) return null;

            string aufstellungenRAW;
            var httpResponse = (HttpWebResponse)request.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                aufstellungenRAW = streamReader.ReadToEnd();

            if (ct.IsCancellationRequested) return null;
            progress.Report(new ProgressMessage(0.10, "Clases and roles"));
            request = GetGetRequest(BASE_ADDRESS + $@"/aufstellungen/elements?termin={terminID}&auth={session.Token}", session.UserAgent);
            if (ct.IsCancellationRequested) return null;

            string elementsRAW;
            httpResponse = (HttpWebResponse)request.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                elementsRAW = streamReader.ReadToEnd();

            if (ct.IsCancellationRequested) return null;
            progress.Report(new ProgressMessage(0.35, "Static data"));
            request = GetGetRequest(BASE_ADDRESS + $@"/gamedata/encounter", session.UserAgent);
            if (ct.IsCancellationRequested) return null;

            string bossesRAW;
            httpResponse = (HttpWebResponse)request.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                bossesRAW = streamReader.ReadToEnd();

            /* Fromat
             * aufstellungen (bosse)
             * [{"id":7048,"name":"Soulless Horror","abbr":"Horror","success":0,"report":null,"has_cm":1,"is_cm":0}]
             * 
             * elements (alle positionen für jeden boss)
             * [{"aufstellung":7048,"pos":1,"class":"Chr","role":"T","id":127,"name":"xyz","accname":"xyz.1234"}]
             * 
             * bosses
             * [{"id":1,"name":"Vale Guardian","abbr":"VG","apiname":"vale_guardian","wing":1,"main":1,"kp_id":77705,"has_cm":0}]
             */

            if (ct.IsCancellationRequested) return null;
            var bossesParsed = Newtonsoft.Json.Linq.JObject.Parse($@"{{""wrapper"":{bossesRAW}}}");
            var bosses = bossesParsed["wrapper"].ToDictionary(boss => (string)boss["abbr"], boss => (int)boss["id"]);

            if (ct.IsCancellationRequested) return null;
            var elementsParsed = Newtonsoft.Json.Linq.JObject.Parse($@"{{""wrapper"":{elementsRAW}}}");
            var elements = elementsParsed["wrapper"].Select(element => new { id = (long)element["aufstellung"], pos = new Position((int)element["pos"], (long)element["id"], (string)element["accname"], GetRoleByAbbreviation((string)element["role"]), GetClass((string)element["class"])) });

            if (ct.IsCancellationRequested) return null;
            var aufstellungenParsed = Newtonsoft.Json.Linq.JObject.Parse($@"{{""wrapper"":{aufstellungenRAW}}}");
            var aufstellungen = aufstellungenParsed["wrapper"].Select(aufstellung => new TeamComp((long)aufstellung["id"], GetBoss(bosses, (string)aufstellung["abbr"]), (int)aufstellung["is_cm"] == 1, elements.Where(e => e.id == (long)aufstellung["id"]).Select(e => e.pos).ToList(), (int)aufstellung["success"] == 1)).ToList();


            if (ct.IsCancellationRequested) return null;
            progress.Report(new ProgressMessage(0.45, "Members"));
            var group = GetGroup(session, terminID);
            if (ct.IsCancellationRequested) return null;
            progress.Report(new ProgressMessage(0.60, "Helpers"));
            var helper = GetHelper(session, terminID);
            if (ct.IsCancellationRequested) return null;
            progress.Report(new ProgressMessage(0.75, "Inveteable"));
            if (ct.IsCancellationRequested) return null;
            var inviteable = GetInvitealbe(session, raidID);


            /* Format
             * anmeldungen all (gruppe)
             * [{"id":204,"name":"xyz","accname":"xyz.1234","type":2}]
             * type 2 abgemelded, 1 evlt, 0 angemeldet
             * 
             * ersatz
             * [{"id":141,"accname":"xyz.1234","name":"xyz"}]
             * 
             * inviteable
             * [{"id":201,"name":"xyz","accname":"xyz.1234"}]
             */
            progress.Report(new ProgressMessage(0.95, "Finish up"));
            return new Data.Raid(terminID, raidID, group, helper, inviteable, aufstellungen);
        }

        private static Role GetRoleByAbbreviation(string roleAbbreviation)
        {
            foreach (var r in Enum.GetValues(typeof(Role)).Cast<Role>())
            {
                if (r.GetAttribute<StringValueAttribute>().Value == roleAbbreviation)
                    return r;
            }
            return Role.Empty;
        }

        private List<Account> GetGroup(ISession session, long terminID)
        {
            var request = GetGetRequest(BASE_ADDRESS + $@"/termine/anmeldungenAll?termin={terminID}&auth={session.Token}", session.UserAgent);

            string groupRAW;
            var httpResponse = (HttpWebResponse)request.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                groupRAW = streamReader.ReadToEnd();

            var groupParsed = Newtonsoft.Json.Linq.JObject.Parse($@"{{""wrapper"":{groupRAW}}}");
            return groupParsed["wrapper"].Select(player => new Account((long)player["id"], (string)player["accname"], (string)player["name"])).ToList();
        }

        private List<Account> GetHelper(ISession session, long terminID)
        {
            var request = GetGetRequest(BASE_ADDRESS + $@"/termine/ersatz?termin={terminID}&auth={session.Token}", session.UserAgent);

            string ersatzRAW;
            var httpResponse = (HttpWebResponse)request.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                ersatzRAW = streamReader.ReadToEnd();

            var ersatzParsed = Newtonsoft.Json.Linq.JObject.Parse($@"{{""wrapper"":{ersatzRAW}}}");
            return ersatzParsed["wrapper"].Select(player => new Account((long)player["id"], (string)player["accname"], (string)player["name"])).ToList();
        }

        private List<Account> GetInvitealbe(ISession session, long groupID)
        {
            var request = GetGetRequest(BASE_ADDRESS + $@"/raids/invitable?raid={groupID}&auth={session.Token}", session.UserAgent);

            string inviteableRAW;
            var httpResponse = (HttpWebResponse)request.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                inviteableRAW = streamReader.ReadToEnd();

            var inviteableParsed = Newtonsoft.Json.Linq.JObject.Parse($@"{{""wrapper"":{inviteableRAW}}}");
            return inviteableParsed["wrapper"].Select(player => new Account((long)player["id"], (string)player["accname"], (string)player["name"])).ToList();
        }

        private Boss GetBoss(Dictionary<string, int> bosses, string roPlusBossAbbreviation)
        {
            return StaticData.Bosses.GetByRaidOrgaPlusID(bosses[roPlusBossAbbreviation]);
        }
        
        private Profession GetClass(string classAbbreviation)
        {
            return StaticData.Professions.GetByAbbreviation(classAbbreviation);
        }

        public void SetRaid(ISession session, Data.Raid raid)
        {
            if (!session.Valid)
                return;

            foreach (var helper in raid.ToInvite)
            {
                ToggleHelper(session, raid.TerminID, helper.ID);
            }

            var httpWebRequest = GetPostRequest(BASE_ADDRESS + @"/api/aufstellungen", session.UserAgent);

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream(), Encoding.UTF8))
            {
                string json = $@"{{""auth"":""{session.Token}"",""session"":""{session.Token}"",""body"":{raid.GetPostJson()}}}";
                streamWriter.Write(json);
                streamWriter.Close();
            }
            string response;

            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            try
            {
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                    response = streamReader.ReadToEnd();
                if (response.Trim() == "[]"|| httpResponse.StatusCode != HttpStatusCode.OK)
                {
                    Logger.Error($@"RO+ SetRaid Returned ""{response}"" body was: request body raid.getPostJson()");
                }
            }
            catch (Exception e)
            {
                Logger.Error($@"RO+ SetRaid has throwen an exception");
                Logger.LogException(e);
                throw new OperationCanceledException("Something went wrong when updating RO+", e);
            }
        }

        internal void ToggleHelper(ISession session, long terminID, long userID)
        {
            var httpWebRequest = GetPostRequest(BASE_ADDRESS + "/termine/ersatz", session.UserAgent);

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream(), Encoding.UTF8))
            {
                string json = $@"{{""auth"":""{session.Token}"",""termin"":{terminID},""user"":{userID}}}";

                streamWriter.Write(json);
            }

            string sessionToken;

            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                sessionToken = streamReader.ReadToEnd();
        }

        private HttpWebRequest GetPostRequest(string uri, string userAgent) => GetRequest(uri, "POST", userAgent);
        private HttpWebRequest GetGetRequest(string uri, string userAgent) => GetRequest(uri, "GET", userAgent);

        private HttpWebRequest GetRequest(string uri, string methode, string userAgent)
        {
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(uri);
            httpWebRequest.UserAgent = userAgent;
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = methode;
            httpWebRequest.Timeout = TimeoutMs;
            return httpWebRequest;
        }
    }
}
