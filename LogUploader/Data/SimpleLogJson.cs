using LogUploader.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace LogUploader.Data
{
    class SimpleLogJson
    {
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
                .Select(playerData => new SimplePlayer((JObject)playerData))
                .ToList();
        }

        [JsonProperty("recordedBy")]
        public string RecordedBy { get; }

        [JsonProperty("targets")]
        public List<SimmpleTarget> Targets { get; }

        [JsonProperty("players")]
        public List<SimplePlayer> Players { get; }

        [JsonProperty("version")]
        public int Version { get; } = 1;

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }

    }

    public class SimplePlayer
    {
        public SimplePlayer(JObject data)
        {
            Account = (string)data["account"];
            Name = (string)data["name"];
            Profession = (string)data["profession"];
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
        }


        [JsonProperty("account")]
        public string Account { get; }

        [JsonProperty("name")]
        public string Name { get; }

        [JsonProperty("profession")]
        public string Profession { get; }

        [JsonProperty("group")]
        public int Group { get; }

        [JsonProperty("allDps")]
        public int DpsAll { get; }

        [JsonProperty("allPowerDps")]
        public int DpsAllPower { get; }

        [JsonProperty("allCondiDps")]
        public int DpsAllCondi { get; }

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
}
