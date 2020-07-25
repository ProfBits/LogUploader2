using LogUploader.Data.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogUploader.Data.RaidOrgaPlus;
using System.Net;
using System.IO;
using LogUploader.Data;

namespace LogUploader.Helper.RaidOrgaPlus
{
    internal class RaidOrgaPlusConnector
    {
        //TODO implement ProxySettings
        private IProxySettings ProxySettings;

        private const int TimeoutMs = 10_000;

        public RaidOrgaPlusConnector(IProxySettings proxySettings)
        {
            ProxySettings = proxySettings;
        }

        public Session Connect(IRaidOrgaPlusSettings orgaSettings, string userAgent = "LogUploader")
        {

            var httpWebRequest = GetPostRequest("https://sv.sollunad.de:8080/users/sessions", userAgent);

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream(), Encoding.UTF8))
            {
                string json = $@"{{""accName"":""{orgaSettings.User}"",""pwd"":""{orgaSettings.Password}""}}";

                streamWriter.Write(json);
            }

            string sessionToken;

            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                sessionToken = streamReader.ReadToEnd();

            return new Session(sessionToken, userAgent);
        }

        public List<RaidSimple> GetRaids(Session session)
        {
            var request = GetGetRequest($@"https://sv.sollunad.de:8080/termine?auth={session.Token}", session.UserAgent);

            string termineRAW;
            var httpResponse = (HttpWebResponse)request.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                termineRAW = streamReader.ReadToEnd();

            request = GetGetRequest($@"https://sv.sollunad.de:8080/raids?auth={session.Token}", session.UserAgent);

            string raidsRAW;
            httpResponse = (HttpWebResponse)request.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                raidsRAW = streamReader.ReadToEnd();

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

            var termineParsed = Newtonsoft.Json.Linq.JObject.Parse($@"{{""wrapper"":{termineRAW}}}");
            var termine = termineParsed["wrapper"]
                .Where(termin => raids[(long)termin["raidID"]])
                .Select(termin => new RaidSimple(
                    (long)termin["id"],
                    (long)termin["raidID"],
                    DateTime.Parse(((string)termin["date"]).Split(' ').Last()),
                    TimeSpan.Parse((string)termin["time"] + ":00"),
                    TimeSpan.Parse((string)termin["endtime"] + ":00"),
                    (string)termin["name"]
                    ));
            return termine.ToList();
        }

        public Raid GetRaid(Session session, int terminID, int raidID)
        {

            var request = GetGetRequest($@"https://sv.sollunad.de:8080/aufstellungen?termin={terminID}&auth={session.Token}", session.UserAgent);

            string aufstellungenRAW;
            var httpResponse = (HttpWebResponse)request.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                aufstellungenRAW = streamReader.ReadToEnd();

            request = GetGetRequest($@"https://sv.sollunad.de:8080/aufstellungen/elements?termin={terminID}&auth={session.Token}", session.UserAgent);

            string elementsRAW;
            httpResponse = (HttpWebResponse)request.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                elementsRAW = streamReader.ReadToEnd();
            
            request = GetGetRequest($@"https://sv.sollunad.de:8080/gamedata/encounter", session.UserAgent);

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

            var bossesParsed = Newtonsoft.Json.Linq.JObject.Parse($@"{{""wrapper"":{bossesRAW}}}");
            var bosses = bossesParsed["wrapper"].ToDictionary(boss => (string)boss["abbr"], boss => (int)boss["id"]);

            var elementsParsed = Newtonsoft.Json.Linq.JObject.Parse($@"{{""wrapper"":{elementsRAW}}}");
            var elements = elementsParsed["wrapper"].Select(element => new { id = (long)element["aufstellung"], pos = new Position((int)element["pos"], (long)element["id"], (string)element["accname"], GetRole((string)element["T"]), GetClass((string)element["class"])) });

            var aufstellungenParsed = Newtonsoft.Json.Linq.JObject.Parse($@"{{""wrapper"":{aufstellungenRAW}}}");
            var aufstellungen = elementsParsed["wrapper"].Select(aufstellung => new TeamComp((long)aufstellung["id"], GetBoss(bosses, (string)aufstellung["name"]), (int)aufstellung["is_cm"] == 1, elements.Where(e => e.id == (long)aufstellung["id"]).Select(e => e.pos).ToList())).ToList();


            var group = GetGroup(session, terminID);
            var helper = GetHelper(session, terminID);
            var inviteable = GetInvitealbe(session, raidID);

            
            /* Format
             * anmeldungen all (gruppe)
             * [{"id":204,"name":"xyz","accname":"xyz.1234","type":2}]
             * type 2 abgemelded, 1 evlt, 0 angemeldet
             * 
             * ersatzt
             * [{"id":141,"accname":"xyz.1234","name":"xyz"}]
             * 
             * inviteable
             * [{"id":201,"name":"xyz","accname":"xyz.1234"}]
             */
            return new Raid(terminID, raidID, group, helper, inviteable, aufstellungen);
        }

        private List<Account> GetGroup(Session session, long terminID)
        {
            var request = GetGetRequest($@"https://sv.sollunad.de:8080/termine/anmeldungenAll?termin={terminID}&auth={session.Token}", session.UserAgent);

            string groupRAW;
            var httpResponse = (HttpWebResponse)request.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                groupRAW = streamReader.ReadToEnd();

            var groupParsed = Newtonsoft.Json.Linq.JObject.Parse($@"{{""wrapper"":{groupRAW}}}");
            return groupParsed["wrapper"].Select(player => new Account((long)player["id"], (string)player["accname"], (string)player["name"])).ToList();
        }

        private List<Account> GetHelper(Session session, long terminID)
        {
            var request = GetGetRequest($@"https://sv.sollunad.de:8080/termine/ersatz?termin={terminID}&auth={session.Token}", session.UserAgent);

            string ersatzRAW;
            var httpResponse = (HttpWebResponse)request.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                ersatzRAW = streamReader.ReadToEnd();

            var ersatzParsed = Newtonsoft.Json.Linq.JObject.Parse($@"{{""wrapper"":{ersatzRAW}}}");
            return ersatzParsed["wrapper"].Select(player => new Account((long)player["id"], (string)player["accname"], (string)player["name"])).ToList();
        }

        private List<Account> GetInvitealbe(Session session, long groupID)
        {
            var request = GetGetRequest($@"https://sv.sollunad.de:8080/raids/invitable?raid={groupID}&auth={session.Token}", session.UserAgent);

            string inviteableRAW;
            var httpResponse = (HttpWebResponse)request.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                inviteableRAW = streamReader.ReadToEnd();

            var inviteableParsed = Newtonsoft.Json.Linq.JObject.Parse($@"{{""wrapper"":{inviteableRAW}}}");
            return inviteableParsed["wrapper"].Select(player => new Account((long)player["id"], (string)player["accname"], (string)player["name"])).ToList();
        }

        private Boss GetBoss(Dictionary<string, int> bosses, string roPlusBossAbbreviation)
        {
            return Boss.getByRaidOragPlusID(bosses[roPlusBossAbbreviation]);
        }

        private Profession GetClass(string classAbbreviation)
        {
            switch (classAbbreviation)
            {
                case "Ele": return Profession.Get(1);
                case "Tmp": return Profession.Get(10);
                case "Wea": return Profession.Get(19);
                case "Mes": return Profession.Get(2);
                case "Chr": return Profession.Get(11);
                case "Mir": return Profession.Get(20);
                case "Nec": return Profession.Get(3);
                case "Rea": return Profession.Get(12);
                case "Scg": return Profession.Get(21);
                case "Rgr": return Profession.Get(4);
                case "Dru": return Profession.Get(13);
                case "Slb": return Profession.Get(22);
                case "Eng": return Profession.Get(5);
                case "Scr": return Profession.Get(14);
                case "Hls": return Profession.Get(23);
                case "Thf": return Profession.Get(6);
                case "Dar": return Profession.Get(15);
                case "Ded": return Profession.Get(24);
                case "War": return Profession.Get(7);
                case "Brs": return Profession.Get(16);
                case "Spb": return Profession.Get(25);
                case "Gdn": return Profession.Get(8);
                case "Dgh": return Profession.Get(17);
                case "Fbd": return Profession.Get(26);
                case "Rev": return Profession.Get(9);
                case "Her": return Profession.Get(18);
                case "Ren": return Profession.Get(27);
                default: return Profession.Unknown;
            }
        }

        private Role GetRole(string roleAbbreviation)
        {
            switch (roleAbbreviation)
            {
                case "P": return Role.Power;
                case "C": return Role.Condi;
                case "H": return Role.Heal;
                case "T": return Role.Tank;
                case "U": return Role.Utility;
                case "B": return Role.Banner;
                case "S": return Role.Special;
                case "K": return Role.Kiter;
                default: return Role.Empty;
            }
        }

        //TODO implement sender
        public void SetRaid(Session session, Raid raid)
        {
            
        }

        public void ToggleHelper(Session session, int raidID, int userID)
        {
            var httpWebRequest = GetPostRequest("https://sv.sollunad.de:8080/termine/ersatz", session.UserAgent);

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream(), Encoding.UTF8))
            {
                string json = $@"{{""auth"":""{session.Token}"",""termin"":{raidID},""user"":{userID}}}";

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
