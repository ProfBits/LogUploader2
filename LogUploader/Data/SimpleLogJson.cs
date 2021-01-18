using LogUploader.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Drawing;

namespace LogUploader.Data
{
    class SimpleLogJson
    {
        public const int CurrentVersion = 2;

        public SimpleLogJson(string data) : this(JObject.Parse(data))
        {
        }

        public SimpleLogJson(JObject parsed)
        {
            if (parsed.ContainsKey("version"))
                Version = (int)parsed["version"];
            RecordedBy = (string)parsed["recordedBy"];
            Targets = ((JArray)parsed["targets"])
                .Select(targetData => new SimmpleTarget((JObject)targetData))
                .ToList();
            Players = ((JArray)parsed["players"])
                .Select(playerData => new SimplePlayer((JObject)playerData, Version))
                .ToList();

            if (Version > 1)
            {
                var idCorrector = parsed["targets"].Select((t, i) => new { index = i, id = (int)t["id"] }).ToDictionary(e => e.index, e => e.id);
                Players.ForEach(p => p.UpdateTargetIDs(idCorrector));
            }
        }

        [JsonProperty("recordedBy")]
        public string RecordedBy { get; }

        [JsonProperty("targets")]
        public List<SimmpleTarget> Targets { get; }

        [JsonProperty("players")]
        public List<SimplePlayer> Players { get; }

        [JsonProperty("version")]
        public int Version { get; } = CurrentVersion;

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }

    }

    public class SimplePlayer
    {
        public SimplePlayer(JObject data) : this(data, SimpleLogJson.CurrentVersion) { }
        public SimplePlayer(JObject data, int version)
        {
            Version = version;
            switch (version)
            {
                case 2:
                    Condition = (int)data["condition"];
                    Concentration = (int)data["concentration"];
                    Healing = (int)data["healing"];
                    Toughness = (int)data["toughness"];
                    if (data.ContainsKey("groupQuickness"))
                    {
                        GroupQuickness = (int)data["groupQuickness"];
                        GroupAlacrity = (int)data["groupAlacrity"];
                    }
                    else
                    {
                        //Quickness = 1187
                        GroupQuickness = (float)(data["groupBuffs"]?.Where(e => (int)e["id"] == 1187).FirstOrDefault()?["buffData"][0]["generation"] ?? 0f);
                        //alac = 30328
                        GroupAlacrity = (float)(data["groupBuffs"]?.Where(e => (int)e["id"] == 30328).FirstOrDefault()?["buffData"][0]["generation"] ?? 0f);
                    }
                    if (data.ContainsKey("dpsTarget"))
                    {
                        DpsTarget = data["dpsTarget"].Select(e => new SimpleDps((JObject)e)).ToList();
                    }
                    else
                    {
                        DpsTarget = data["dpsTargets"]?.Select(e => e[0]).Select((e, i) => new SimpleDps(i, (int)e["dps"], (int)e["powerDps"], (int)e["condiDps"])).ToList() ?? new List<SimpleDps>();
                        DpsTargetsIDNeedsFix = true;
                    }
                    goto case 1;
                case 1:
                default:
                    Account = (string)data["account"];
                    Name = (string)data["name"];
                    ProfessionStr = (string)data["profession"];
                    Profession = Profession.Get(ProfessionStr);
                    Group = (int)data["group"];
                    if (data["dpsAll"] is JArray)
                    {
                        DpsAll = (int)data["dpsAll"][0]["dps"];
                        DpsAllPower = (int)data["dpsAll"][0]["powerDps"];
                        DpsAllCondi = (int)data["dpsAll"][0]["condiDps"];
                    }
                    else
                    {
                        DpsAll = (int)data["allDps"];
                        DpsAllPower = (int)data["allPowerDps"];
                        DpsAllCondi = (int)data["allCondiDps"];
                    }
                    break;
            }

        }
        [JsonIgnore]
        public int Version { get; }

        private readonly bool DpsTargetsIDNeedsFix = false;

        //Version 1
        [JsonProperty("account")]
        public string Account { get; }

        [JsonProperty("name")]
        public string Name { get; }

        [JsonProperty("profession")]
        public string ProfessionStr { get; }
        [JsonIgnore]
        public Profession Profession { get; }
        [JsonIgnore]
        public Image ProfessionIcon { get => Profession.Icon; }

        [JsonProperty("group")]
        public int Group { get; }

        [JsonProperty("allDps")]
        public int DpsAll { get; }

        [JsonProperty("allPowerDps")]
        public int DpsAllPower { get; }

        [JsonProperty("allCondiDps")]
        public int DpsAllCondi { get; }

        //Version 2
        [JsonProperty("condition")]
        public int Condition { get; } = 0;

        [JsonProperty("concentration")]
        public int Concentration { get; } = 0;

        [JsonProperty("healing")]
        public int Healing { get; } = 0;

        [JsonProperty("toughness")]
        public int Toughness { get; } = 0;

        [JsonProperty("groupQuickness")]
        public float GroupQuickness { get; } = 0;

        [JsonProperty("groupAlacrity")]
        public float GroupAlacrity { get; } = 0;

        [JsonProperty("dpsTarget")]
        public List<SimpleDps> DpsTarget { get; } = new List<SimpleDps>();

        [JsonIgnore]
        public int DpsTargets { get => DpsTarget.Where(e => Boss.ExistsID(e.Id)).Sum(e => e.Damage); }

        [JsonIgnore]
        public int DpsTargetsPower { get => DpsTarget.Where(e => Boss.ExistsID(e.Id)).Sum(e => e.Power); }

        [JsonIgnore]
        public int DpsTargetsCondi { get => DpsTarget.Where(e => Boss.ExistsID(e.Id)).Sum(e => e.Condi); }

        internal void UpdateTargetIDs (Dictionary<int, int> table)
        {
            if (DpsTargetsIDNeedsFix)
                DpsTarget.ForEach(e => e.Id = table[e.Id]);
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }

    public class SimmpleTarget
    {
        public SimmpleTarget(JObject data)
        {
            ID = (int)data["id"];
            TotalHealth = (int)data["totalHealth"];
            FinalHealth = (int)data["finalHealth"];
            FirstAware = (int)data["firstAware"];
            LastAware = (int)data["lastAware"];
        }


        [JsonProperty("id")]
        public int ID { get; }

        [JsonProperty("totalHealth")]
        public int TotalHealth { get; }

        [JsonProperty("finalHealth")]
        public int FinalHealth { get; }

        [JsonProperty("firstAware")]
        public int FirstAware { get; }

        [JsonProperty("lastAware")]
        public int LastAware { get; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }

    public class SimpleDps
    {
        [JsonProperty("id")]
        public int Id { get; internal set; }
        [JsonProperty("damage")]
        public int Damage { get; }
        [JsonProperty("power")]
        public int Power { get; }
        [JsonProperty("condi")]
        public int Condi { get; }

        public SimpleDps(JObject data) : this(
            (int)data["id"],
            (int)data["damage"],
            (int)data["power"],
            (int)data["condi"])
        {
        }

        public SimpleDps(int id, int damage, int power, int condi)
        {
            Id = id;
            Damage = damage;
            Power = power;
            Condi = condi;
        }
    }
}
