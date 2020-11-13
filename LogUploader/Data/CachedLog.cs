using LogUploader.Languages;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace LogUploader.Data
{
    public class CachedLog
    {
        /*
         * DB Section: Has to be present
         */
        private DBLog DataDB { get; set; } = new DBLog();
        public int ID { get => DataDB.ID; set => DataDB.ID = value; }
        public int BossID { get => DataDB.BossID; set => DataDB.BossID = value; }
        public string BossName { get => Boss.getByID(BossID).Name; }
        public string EvtcPath { get => DataDB.EvtcPath; set => DataDB.EvtcPath = value; }
        public string JsonPath { get => DataDB.JsonPath; set => DataDB.JsonPath = value; }
        public string HtmlPath { get => DataDB.HtmlPath; set => DataDB.HtmlPath = value; }
        public string Link { get => DataDB.Link; set => DataDB.Link = value; }
        public int SizeKb { get => DataDB.SizeKb; set => DataDB.SizeKb = value; }
        public DateTime Date { get => DataDB.Date; set => DataDB.Date = value; }
        public bool DataCorrected { get => DataDB.DataCorrected; set => DataDB.DataCorrected = value; }
        public TimeSpan Duration { get => DataDB.Duration; set => DataDB.Duration = value; }

        public bool Succsess { get => DataDB.Succsess; set => DataDB.Succsess = value; }
        public float RemainingHealth { get => DataDB.RemainingHealth; set => DataDB.RemainingHealth = value; }
        public bool IsCM  { get => DataDB.IsCM; set => DataDB.IsCM = value; }

        //TODO remove?
        [Obsolete("Should be removed if unnecesary")]
        public string LocalAvailable { get => string.IsNullOrEmpty(HtmlPath) ? "-" : "open"; }
        //TODO remove?
        [Obsolete("Should be removed if unnecesary")]
        public string LinkAvailable { get => string.IsNullOrEmpty(Link) ? "-" : "open"; }

        /*
         * Json Section: Additional data, may not be present
         */
        private SimpleLogJson DataJson { get; set; }

        public int DataVersion { get => DataJson?.Version ?? 0; }
        public string RecordedBy { get => DataJson?.RecordedBy ?? ""; }
        public IReadOnlyList<SimplePlayer> PlayersNew { get => DataJson?.Players ?? new List<SimplePlayer>(); }
        public List<SimmpleTarget> Targets { get => DataJson?.Targets ?? new List<SimmpleTarget>(); }

        public CachedLog(int iD, int bossID, string evtcPath, string jsonPath, string htmlPath, string link, int sizeKb, DateTime date, bool dataCorrected, TimeSpan duration, bool succsess, float remainingHealth, bool isCM)
        {
            ID = iD;
            BossID = bossID;
            EvtcPath = evtcPath;
            JsonPath = jsonPath;
            HtmlPath = htmlPath;
            Link = link;
            SizeKb = sizeKb;
            Date = date;
            DataCorrected = dataCorrected;
            Duration = duration;
            Succsess = succsess;
            RemainingHealth = remainingHealth;
            IsCM = isCM;
        }
        public CachedLog(int iD, int bossID, string evtcPath, string jsonPath, string htmlPath, string link, int sizeKb, DateTime date) : this(
            iD,
            bossID,
            evtcPath,
            jsonPath,
            htmlPath,
            link,
            sizeKb,
            date,
            false,
            new TimeSpan(0),
            false,
            100,
            false
            )
        { }

        public CachedLog(DBLog log)
        {
            DataDB = log;
        }

        private DateTime GetDate(string dateStr)
        {
            if (DateTime.TryParse(dateStr, out var date))
            {
                return date;
            }
            //TODO check this
            if (File.Exists(EvtcPath))
            {
                return new FileInfo(EvtcPath).CreationTime;
            }
            if (File.Exists(JsonPath))
            {
                return new FileInfo(EvtcPath).CreationTime;
            }
            return DateTime.Now;
        }
        private static TimeSpan GetDuration(string durationStr)
        {
            try
            {
                return TimeSpan.ParseExact(durationStr, "mm'm 'ss's 'fff'ms'", CultureInfo.InvariantCulture);
            }
            catch (FormatException)
            {
                var duration = new TimeSpan(0);
                var parts = durationStr.Split(' ');
                foreach (var part in parts)
                {
                    if (part.EndsWith("ms"))
                        duration = duration.Add(new TimeSpan(0, 0, 0, 0, int.Parse(part.TrimEnd('m', 's'))));
                    else if (part.EndsWith("s"))
                        duration = duration.Add(new TimeSpan(0, 0, int.Parse(part.TrimEnd('s'))));
                    else if (part.EndsWith("m"))
                        duration = duration.Add(new TimeSpan(0, int.Parse(part.TrimEnd('m')), 0));
                    else if (part.EndsWith("h"))
                        duration = duration.Add(new TimeSpan(int.Parse(part.TrimEnd('h')), 0, 0));
                }
                return duration;
            }
        }

        private float getRemainingHealth(JArray list, int bossID)
        {
            var data = list.Select(target => new
            {
                id = (int)target["id"],
                totalHealth = (int)target["totalHealth"],
                finalHealth = (int)target["finalHealth"]
            });

            switch (bossID)
            {

                case 16088: //Trio
                case 16137:
                case 16125:
                    var berg = data.Where(target => target.id == 16088).FirstOrDefault();
                    var zane = data.Where(target => target.id == 16137).FirstOrDefault();
                    var narella = data.Where(target => target.id == 16125).FirstOrDefault();
                    var healthTrio = (berg?.totalHealth ?? 6881700) + (zane?.totalHealth ?? 5898600) + (narella?.totalHealth ?? 4915500);
                    var remainingTrio = (berg?.finalHealth ?? 6881700) + (zane?.finalHealth ?? 5898600) + (narella?.finalHealth ?? 4915500);
                    return (float)Math.Round((double)remainingTrio / healthTrio * 100, 2);
                case 19651: //Eyes
                case 19844:
                    var Judgment = data.Where(target => target.id == 19651).FirstOrDefault();
                    var Fate = data.Where(target => target.id == 19844).FirstOrDefault();
                    var healthEyes = (Judgment?.totalHealth ?? 2457750) + (Fate?.totalHealth ?? 2457750);
                    var remainingEyes = (Judgment?.finalHealth ?? 2457750) + (Fate?.finalHealth ?? 2457750);
                    return (float)Math.Round((double)remainingEyes / healthEyes * 100, 2);
                case 21105: //Largos
                case 21089:
                    var Nikare = data.Where(target => target.id == 21105).FirstOrDefault();
                    var Kenut = data.Where(target => target.id == 21089).FirstOrDefault();
                    var healthLargos = (Nikare?.totalHealth ?? 17548336) + (Kenut?.totalHealth ?? 15877066);
                    var remainingLargos = (Nikare?.finalHealth ?? 17548336) + (Kenut?.finalHealth ?? 15877066);
                    return (float)Math.Round((double)remainingLargos / healthLargos * 100, 2);
                case 16247: //TwistedCastle
                    return 0;
                default:
                    var boss = data.Where(target => target.id == BossID).FirstOrDefault();
                    return (float)((double)boss?.finalHealth / boss?.totalHealth * 100 ?? 100);
            }
        }

        private IReadOnlyList<SimplePlayer> ParsePlayersNew(JArray playerRawData)
        {
            return playerRawData.Select(data => new SimplePlayer((JObject)data)).ToList();
        }


        public void UpdateEi(string json) => UpdateEi(JObject.Parse(json));
        public void UpdateEi(JObject data)
        {
            BossID = (int)data["triggerID"];

            Date = GetDate((string)data["timeStartStd"]);
            DataCorrected = true;
            Duration = GetDuration((string)data["duration"]);
            Succsess = (bool)data["success"];
            RemainingHealth = getRemainingHealth((JArray)data["targets"], BossID);
            IsCM = (bool)data["isCM"];
            ApplySimpleLog(new SimpleLogJson(data));
        }

        [Obsolete("untested do not use!", true)]
        public void UpdateDpsReport(JObject data)
        {
            BossID = (int)data["triggerID"];

            Date = GetDate((string)data["timeStartStd"]);
            DataCorrected = true;
            Duration = TimeSpan.ParseExact((string)data["duration"], "mm'm 'ss's 'fff'ms'", CultureInfo.InvariantCulture);
            Succsess = (bool)data["success"];
            RemainingHealth = getRemainingHealth((JArray)data["targets"], BossID);
            IsCM = (bool)data["isCM"];
            ApplySimpleLog(new SimpleLogJson(data));
        }

        public DBLog GetDBLog()
        {
            return new DBLog(ID, BossID, EvtcPath, JsonPath, HtmlPath, Link, SizeKb, Date, Duration, DataCorrected, Succsess, IsCM, RemainingHealth);
        }

        internal void ApplySimpleLog(SimpleLogJson data)
        {
            DataJson = data;
        }

        public override bool Equals(object obj)
        {
            if (this == obj)
                return true;
            if (obj is CachedLog log)
                return this.ID == log.ID;
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return ID.GetHashCode();
        }
    }
}
