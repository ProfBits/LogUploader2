using LogUploader.Data.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogUploader.Data.RaidOrgaPlus;
using System.Net;
using System.IO;

namespace LogUploader.Helper.RaidOrgaPlus
{
    internal class RaidOrgaPlusConnector
    {
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

            string termine;
            var httpResponse = (HttpWebResponse)request.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                termine = streamReader.ReadToEnd();

            request = GetGetRequest($@"https://sv.sollunad.de:8080/raids?auth={session.Token}", session.UserAgent);

            string raids;
            httpResponse = (HttpWebResponse)request.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                raids = streamReader.ReadToEnd();

            //TODO Process and filter

            /* Fromat
             * Termine
             * [{"id":582,"raidID":1351,"name":"xyz","icon":null,"role":2,"date":"Fr, 17.07.2020","time":"19:00","endtime":"22:00","type":0}]
             * 
             * Raids
             * [{"id":6,"name":"xyz","icon":"","role":0}]
             * Role=2 == leader
             */
        }

        public Raid GetRaid(Session session, int raidID)
        {
            //TODO
            int groupID = 0;

            var request = GetGetRequest($@"https://sv.sollunad.de:8080/aufstellungen?termin={raidID}&auth={session.Token}", session.UserAgent);

            string aufstellungen;
            var httpResponse = (HttpWebResponse)request.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                aufstellungen = streamReader.ReadToEnd();

            request = GetGetRequest($@"https://sv.sollunad.de:8080/aufstellungen/elements?termin={raidID}&auth={session.Token}", session.UserAgent);

            string elements;
            httpResponse = (HttpWebResponse)request.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                elements = streamReader.ReadToEnd();

            /* Fromat
             * aufstellungen (bosse)
             * [{"id":7048,"name":"Soulless Horror","abbr":"Horror","success":0,"report":null,"has_cm":1,"is_cm":0}]
             * 
             * elements (alle positionen für jeden boss)
             * [{"aufstellung":7048,"pos":1,"class":"Chr","role":"T","id":127,"name":"xyz","accname":"xyz.1234"}]
             */

            request = GetGetRequest($@"https://sv.sollunad.de:8080/termine/anmeldungenAll?termin={raidID}&auth={session.Token}", session.UserAgent);

            string group;
            httpResponse = (HttpWebResponse)request.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                group = streamReader.ReadToEnd();

            request = GetGetRequest($@"https://sv.sollunad.de:8080/termine/ersatz?termin={raidID}&auth={session.Token}", session.UserAgent);

            string ersatz;
            httpResponse = (HttpWebResponse)request.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                ersatz = streamReader.ReadToEnd();

            //evlt über all?
            request = GetGetRequest($@"https://sv.sollunad.de:8080/raids/invitable?raid={groupID}&auth={session.Token}", session.UserAgent);

            string inviteable;
            httpResponse = (HttpWebResponse)request.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                inviteable = streamReader.ReadToEnd();
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
        }

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
