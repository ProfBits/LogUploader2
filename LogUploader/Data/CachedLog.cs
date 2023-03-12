using LogUploader.Languages;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Newtonsoft.Json.Linq;
using LogUploader.Helper;

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
        public string BossName { get => Boss.GetByID(DataDB.BossID).Name; }
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
        public static int CurrentDataVersion { get => SimpleLogJson.CurrentVersion; }
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
                return new FileInfo(JsonPath).CreationTime;
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

        private float GetRemainingHealth(JArray list, int bossID)
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
                case 23254: //Sunqua
                    var darkAi = data.Where(target => target.id == -8).FirstOrDefault();
                    var ai = data.Where(target => target.id == 23254).FirstOrDefault();
                    var healthAi = (ai?.totalHealth ?? 14905666) + (darkAi?.totalHealth ?? 14905666);
                    var remainingAi = (ai?.finalHealth ?? (darkAi == null ? 14905666 : 0)) + (darkAi?.finalHealth ?? 14905666);
                    return (float)Math.Round((double)remainingAi / healthAi * 100, 2);
                case 24033: //Aetherblade Hideout
                    var maiTrin = data.Where(target => target.id == 24033).FirstOrDefault();
                    var scalet = data.Where(target => target.id == 24768).FirstOrDefault();
                    var healthHideout = (maiTrin?.totalHealth) ?? 5898600 + (scalet?.totalHealth ?? 17695800);
                    var remainingHideout = (maiTrin?.finalHealth) ?? 5898600 + (scalet?.finalHealth ?? 17695800);
                    return (float)Math.Round((double)remainingHideout / healthHideout * 100, 2);
                case 24485: //Minister Li
                    var Li = data.Where(target => target.id == 24485).FirstOrDefault();
                    var Enforcer = data.Where(target => target.id == 24261).FirstOrDefault();
                    var Mindblade = data.Where(target => target.id == 24254).FirstOrDefault();
                    var MechRider = data.Where(target => target.id == 24660).FirstOrDefault();
                    var Ritualist = data.Where(target => target.id == 23618).FirstOrDefault();
                    var Sniper = data.Where(target => target.id == 23612).FirstOrDefault();
                    var healthLiTotal =
                          (Li?.totalHealth ?? 29493000)
                        + (Enforcer?.totalHealth ?? 8680800)
                        + (Mindblade?.totalHealth ?? 8680800)
                        + (MechRider?.totalHealth ?? 19662000)
                        + (Ritualist?.totalHealth ?? 8684800)
                        + (Sniper?.totalHealth ?? 5731968);
                    var remainingLiTotal =
                          (Li?.finalHealth ?? 29493000)
                        + (Enforcer?.finalHealth ?? 8680800)
                        + (Mindblade?.finalHealth ?? 8680800)
                        + (MechRider?.finalHealth ?? 19662000)
                        + (Ritualist?.finalHealth ?? 8684800)
                        + (Sniper?.finalHealth ?? 5731968);
                    return (float)Math.Round((double)remainingLiTotal / healthLiTotal * 100, 2);
                case 24375: //Harvest Temple
                    var JormagVoid = data.Where(target => target.id == -21).FirstOrDefault();
                    var PrimordiusVoid = data.Where(target => target.id == -22).FirstOrDefault();
                    var KralkatorrikVoid = data.Where(target => target.id == -19).FirstOrDefault();
                    var VoidTimeCaster = data.Where(target => target.id == 25025).FirstOrDefault();
                    var MordremothVoid = data.Where(target => target.id == -20).FirstOrDefault();
                    var ZhaitanVoid = data.Where(target => target.id == -17).FirstOrDefault();
                    var VoidSaltsprayDragon = data.Where(target => target.id == 23846).FirstOrDefault();
                    var SooWonVoid = data.Where(target => target.id == -18).FirstOrDefault();
                    var VoidObliterator = data.Where(target => target.id == 23995).FirstOrDefault();
                    var totalHealth =
                          (JormagVoid?.totalHealth ?? 6723000)
                        + (PrimordiusVoid?.totalHealth ?? 6723000)
                        + (KralkatorrikVoid?.totalHealth ?? 6723000)
                        + (VoidTimeCaster?.totalHealth ?? 1769600)
                        + (MordremothVoid?.totalHealth ?? 6723000)
                        + (ZhaitanVoid?.totalHealth ?? 6723000)
                        + (VoidSaltsprayDragon?.totalHealth ?? 2123520)
                        + (SooWonVoid?.totalHealth ?? 9711000)
                        + (VoidObliterator?.totalHealth ?? 1769600);
                    var remainingHealth =
                          (JormagVoid?.finalHealth ?? 6723000)
                        + (PrimordiusVoid?.finalHealth ?? 6723000)
                        + (KralkatorrikVoid?.finalHealth ?? 6723000)
                        + (VoidTimeCaster?.finalHealth ?? 1769600)
                        + (MordremothVoid?.finalHealth ?? 6723000)
                        + (ZhaitanVoid?.finalHealth ?? 6723000)
                        + (VoidSaltsprayDragon?.finalHealth ?? 2123520)
                        + (SooWonVoid?.finalHealth ?? 9711000)
                        + (VoidObliterator?.finalHealth ?? 1769600);
                    return (float)Math.Round((double)remainingHealth / totalHealth * 100, 2);
                case 25413: //OLC
                case 25419:
                case 25415:
                    var vermilion = data.Where(target => target.id == 25413).FirstOrDefault();
                    var indigo = data.Where(target => target.id == 25419).FirstOrDefault();
                    var arsenite = data.Where(target => target.id == 25415).FirstOrDefault();
                    var healthOLC = (vermilion?.totalHealth ?? 14156640) + (indigo?.totalHealth ?? 14156640) + (arsenite?.totalHealth ?? 14156640);
                    var remainingOLC = (vermilion?.finalHealth ?? 14156640) + (indigo?.finalHealth ?? 14156640) + (arsenite?.finalHealth ?? 14156640);
                    return (float)Math.Round((double)remainingOLC / healthOLC * 100, 2);
                default:
                    var boss = data.Where(target => target.id == BossID).FirstOrDefault();
                    if (boss is null)
                    {
                        return 100f;
                    }
                    return (float)((double)boss.finalHealth / boss.totalHealth * 100);
            }
        }

        private IReadOnlyList<SimplePlayer> ParsePlayersNew(JArray playerRawData)
        {
            return playerRawData.Select(data => new SimplePlayer((JObject)data)).ToList();
        }

        private readonly ISet<string> TokensToIgnore = new HashSet<string>
        {
            "combatReplayMetaData",
            "personalBuffs",
            "damageModMap",
            "buffMap",
            "skillMap",
            "combatReplayData",
            "breakbarDamage1S",
            "conditionDamage1S",
            "powerDamage1S",
            "damage1S",
            "rotation",
            "targetBreakbarDamage1S",
            "targetConditionDamage1S",
            "targetPowerDamage1S",
            "targetDamage1S",
            "buffs",
            "buffUptimes", //Hepfull now, later not
            "barrierPercents",
            "healthPercents",
            "boonsStates",
            "conditionsStates",
            "statsTargets",
            "targetDamageDist",
            "damageModifiersTarget",
            "damageModifiers",
        };

        public void UpdateEi(string json) => UpdateEi(JObject.Load(new SkippingJsonReader(json, TokensToIgnore)));
        public void UpdateEi(JObject data)
        {
            BossID = (int)data["triggerID"];
            //Correction for Ai, EI uses 3 names depended of phases
            if (BossID == 23255 || BossID == 23256 /*Fake Ai's*/) BossID = 23254;
            
            Date = GetDate((string)data["timeStartStd"]);
            DataCorrected = true;
            Duration = GetDuration((string)data["duration"]);
            Succsess = (bool)data["success"];
            if (BossID == Boss.Get(eBosses.Desmina).ID)
                RemainingHealth = Succsess ? 0 : 100;
            else if (BossID == Boss.Get(eBosses.TheDragonvoid).ID)
                RemainingHealth = Succsess ? 0 : GetRemainingHealth((JArray)data["targets"], BossID);
            else
                RemainingHealth = GetRemainingHealth((JArray)data["targets"], BossID);
            if (!(0 <= RemainingHealth && RemainingHealth <= 100))
                RemainingHealth = Succsess ? 0 : 100;
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
