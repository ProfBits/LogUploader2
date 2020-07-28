using LogUploader.JSONHelper;
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

        public int ID { get; set; } = -1;
        public int BossID { get; set; }
        public string BossName { get => Boss.getByID(BossID).Name; }
        public string EvtcPath { get; set; }
        public string JsonPath { get; set; }
        public string HtmlPath { get; set; }
        public string Link { get; set; }
        public int SizeKb { get; set; }
        public DateTime Date { get; set; }
        public bool DataCorrected { get; private set; } = false;
        public TimeSpan Duration { get; set; } = new TimeSpan(0, 0, 0);
        
        
        
        public bool Succsess { get; set; } = false;
        public float RemainingHealth { get; set; } = 100;
        public bool IsCM { get; set; } = false;

        public string LocalAvailable { get => string.IsNullOrEmpty(HtmlPath) ? "-" : "open"; }
        public string LinkAvailable { get => string.IsNullOrEmpty(Link) ? "-" : "open"; }

        public IReadOnlyList<CachedPlayer> Players { get; set; } = new List<CachedPlayer>();

        public CachedLog(int iD, int bossID, string evtcPath, string jsonPath, string htmlPath, string link, int sizeKb, DateTime date, bool dataCorrected, TimeSpan duration, bool succsess, float remainingHealth, bool isCM, IReadOnlyList<CachedPlayer> players = null)
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
            if (players != null)
                Players = players;
        }

        public CachedLog(int iD, string evtcPath, string jsonPath, string htmlPath, string link, int sizeKb, JObject data)
        {
            ID = iD;
            EvtcPath = evtcPath;
            JsonPath = jsonPath;
            HtmlPath = htmlPath;
            Link = link;
            SizeKb = sizeKb;
            UpdateEi(data);
        }

        private DateTime GetDate(string dateStr)
        {
            DateTime date;
            if (DateTime.TryParse(dateStr, out date))
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

        private IReadOnlyList<CachedPlayer> ParsePlayers(JArray playerRawData)
        {
            return playerRawData.Select(data => new CachedPlayer((JObject)data)).ToList();
        }

        public CachedLog(int iD, int bossID, string evtcPath, string jsonPath, string htmlPath, string link, int sizeKb, DateTime date)
        {
            ID = iD;
            BossID = bossID;
            EvtcPath = evtcPath;
            JsonPath = jsonPath;
            HtmlPath = htmlPath;
            Link = link;
            SizeKb = sizeKb;
            Date = date;
            DataCorrected = false;
            Duration = new TimeSpan(0);
            Succsess = false;
            RemainingHealth = 100;
            IsCM = false;
            Players = new List<CachedPlayer>();
        }

        public CachedLog(DBLog log) : this(
            log.ID,
            log.BossID,
            log.EvtcPath,
            log.JsonPath,
            log.HtmlPath,
            log.Link,
            log.SizeKb,
            log.Date,
            log.DataCorrected,
            log.Duration,
            log.Succsess,
            log.RemainingHealth,
            log.IsCM
            )
        { }

        public CachedLog()
        {
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
            Players = ParsePlayers((JArray)data["players"]);
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

        public void UpdateDpsReport(JObject data)
        {
            BossID = (int)data["triggerID"];

            Date = GetDate((string)data["timeStartStd"]);
            DataCorrected = true;
            Duration = TimeSpan.ParseExact((string)data["duration"], "mm'm 'ss's 'fff'ms'", CultureInfo.InvariantCulture);
            Succsess = (bool)data["success"];
            RemainingHealth = getRemainingHealth((JArray)data["targets"], BossID);
            IsCM = (bool)data["isCM"];
            Players = ParsePlayers((JArray)data["players"]);
        }

        public DBLog GetDBLog()
        {
            return new DBLog(ID, BossID, EvtcPath, JsonPath, HtmlPath, Link, SizeKb, Date, Duration, DataCorrected, Succsess, IsCM, RemainingHealth);
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
