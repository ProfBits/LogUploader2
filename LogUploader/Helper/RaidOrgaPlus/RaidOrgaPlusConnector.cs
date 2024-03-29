﻿using LogUploader.Data.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogUploader.Data.RaidOrgaPlus;
using System.Net;
using System.IO;
using LogUploader.Data;
using System.Threading;
using Newtonsoft.Json;

namespace LogUploader.Helper.RaidOrgaPlus
{
    internal class RaidOrgaPlusConnector
    {
        //TODO implement ProxySettings
        private readonly IProxySettings ProxySettings;

        private const int TimeoutMs = 10_000;
        private const string BASE_ADDRESS = "https://sv.rising-light.de:8080";

        public RaidOrgaPlusConnector(IProxySettings proxySettings)
        {
            ProxySettings = proxySettings;
        }

        public Session Connect(IRaidOrgaPlusSettings orgaSettings)
        {
            return Connect(orgaSettings, $"LogUploader{new Random().Next(0, 1000000).ToString().PadLeft(6, '0')}");
        }

        public Session Connect(IRaidOrgaPlusSettings orgaSettings, string userAgent)
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

                if (sessionToken.Length != 36)
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

        public List<RaidSimple> GetRaids(Session session, IProgress<double> progress = null)
        {
            if (!session?.Valid ?? false)
                return null;

            progress?.Report(0);

            var request = GetGetRequest(BASE_ADDRESS + $@"/termine?auth={session.Token}", session.UserAgent);

            string termineRAW;
            var httpResponse = (HttpWebResponse)request.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                termineRAW = streamReader.ReadToEnd();

            /* Fromat
             * Termine
             * old: [{"id":582,"raidID":1351,"name":"xyz","icon":null,"role":2,"date":"Fr, 17.07.2020","time":"19:00","endtime":"22:00","type":0}]
             * new: [[{"id":222,"raidID":1111,"name":"xyz","icon":null,"role":2,"date":"2021-08-07T22:00:00.000Z","time":"20:00","endtime":"22:00","type":2,"dateString":"So, 08.08.2021"}
             * 
             * Role=2 == leader
             * Role=1 == leutnant
             * Role=0 == member
             */

            progress?.Report(0.8);

            var termineParsed = Newtonsoft.Json.Linq.JObject.Parse($@"{{""wrapper"":{termineRAW}}}");
            var termine = termineParsed["wrapper"]
                .Where(termin => (int)termin["role"] >= 1)
                .Select(termin => new RaidSimple(
                    (long)termin["id"],
                    (long)termin["raidID"],
                    ((DateTime)termin["date"]).Date,
                    TimeSpan.Parse((string)termin["time"] + ":00"),
                    TimeSpan.Parse(((string)termin["endtime"] ?? (string)termin["time"]) + ":00"),
                    (string)termin["name"]
                    )).ToList();

            request = null;
            termineRAW = null;
            termineParsed = null;
            progress?.Report(1);
            return termine;
        }

        public Raid GetRaid(Session session, long terminID, long raidID, CancellationToken ct, IProgress<ProgressMessage> progress = null)
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
            var elements = elementsParsed["wrapper"].Select(element => new { id = (long)element["aufstellung"], pos = new Position((int)element["pos"], (long)element["id"], (string)element["accname"], element["roles"].Select(r => (Role)(byte)r["id"]).ToArray(), GetClass((string)element["class"])) });

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
            return new Raid(terminID, raidID, group, helper, inviteable, aufstellungen);
        }

        private List<Account> GetGroup(Session session, long terminID)
        {
            var request = GetGetRequest(BASE_ADDRESS + $@"/termine/anmeldungenAll?termin={terminID}&auth={session.Token}", session.UserAgent);

            string groupRAW;
            var httpResponse = (HttpWebResponse)request.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                groupRAW = streamReader.ReadToEnd();

            var groupParsed = Newtonsoft.Json.Linq.JObject.Parse($@"{{""wrapper"":{groupRAW}}}");
            return groupParsed["wrapper"].Select(player => new Account((long)player["id"], (string)player["accname"], (string)player["name"])).ToList();
        }

        private List<Account> GetHelper(Session session, long terminID)
        {
            var request = GetGetRequest(BASE_ADDRESS + $@"/termine/ersatz?termin={terminID}&auth={session.Token}", session.UserAgent);

            string ersatzRAW;
            var httpResponse = (HttpWebResponse)request.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                ersatzRAW = streamReader.ReadToEnd();

            var ersatzParsed = Newtonsoft.Json.Linq.JObject.Parse($@"{{""wrapper"":{ersatzRAW}}}");
            return ersatzParsed["wrapper"].Select(player => new Account((long)player["id"], (string)player["accname"], (string)player["name"])).ToList();
        }

        private List<Account> GetInvitealbe(Session session, long groupID)
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
            return Boss.GetByRaidOragPlusID(bosses[roPlusBossAbbreviation]);
        }

        //TODO export to professiondata.json?
        private Profession GetClass(string classAbbreviation)
        {
            switch (classAbbreviation)
            {
                case "Ele": return Profession.Get(eProfession.Elementalist);
                case "Tmp": return Profession.Get(eProfession.Tempest);
                case "Wea": return Profession.Get(eProfession.Weaver);
                case "Cat": return Profession.Get(eProfession.Catalyst);
                case "Mes": return Profession.Get(eProfession.Mesmer);
                case "Chr": return Profession.Get(eProfession.Chronomancer);
                case "Mir": return Profession.Get(eProfession.Mirage);
                case "Vit": return Profession.Get(eProfession.Virtuoso);
                case "Nec": return Profession.Get(eProfession.Necromancer);
                case "Rea": return Profession.Get(eProfession.Reaper);
                case "Scg": return Profession.Get(eProfession.Scourge);
                case "Har": return Profession.Get(eProfession.Harbinger);
                case "Rgr": return Profession.Get(eProfession.Ranger);
                case "Dru": return Profession.Get(eProfession.Druid);
                case "Slb": return Profession.Get(eProfession.Soulbeast);
                case "Utd": return Profession.Get(eProfession.Untamed);
                case "Eng": return Profession.Get(eProfession.Engineer);
                case "Scr": return Profession.Get(eProfession.Scrapper);
                case "Hls": return Profession.Get(eProfession.Holosmith);
                case "Mec": return Profession.Get(eProfession.Mechanist);
                case "Thf": return Profession.Get(eProfession.Thief);
                case "Dar": return Profession.Get(eProfession.Daredevil);
                case "Ded": return Profession.Get(eProfession.Deadeye);
                case "Spc": return Profession.Get(eProfession.Specter);
                case "War": return Profession.Get(eProfession.Warrior);
                case "Brs": return Profession.Get(eProfession.Berserker);
                case "Spb": return Profession.Get(eProfession.Spellbreaker);
                case "Bls": return Profession.Get(eProfession.Bladesworn);
                case "Gdn": return Profession.Get(eProfession.Guardian);
                case "Dgh": return Profession.Get(eProfession.Dragonhunter);
                case "Fbd": return Profession.Get(eProfession.Firebrand);
                case "Wlb": return Profession.Get(eProfession.Willbender);
                case "Rev": return Profession.Get(eProfession.Revenant);
                case "Her": return Profession.Get(eProfession.Herald);
                case "Ren": return Profession.Get(eProfession.Renegade);
                case "Vin": return Profession.Get(eProfession.Vindicator);
                default: return Profession.Unknown;
            }
        }

        public void SetRaid(Session session, Raid raid)
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

        public void ToggleHelper(Session session, long terminID, long userID)
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
