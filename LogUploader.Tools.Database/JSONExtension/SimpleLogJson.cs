using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using LogUploader.Data;

namespace LogUploader.Tools.Database.JSONExtensiones
{
    public class SimpleLogJson : ISimpleLogJson
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
                .Cast<ISimmpleTarget>()
                .ToList();
            Players = ((JArray)parsed["players"])
                .Select(playerData => new SimplePlayer((JObject)playerData, Version))
                .Cast<ISimplePlayer>()
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
        public List<ISimmpleTarget> Targets { get; }

        [JsonProperty("players")]
        public List<ISimplePlayer> Players { get; }

        [JsonProperty("version")]
        public int Version { get; } = CurrentVersion;

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }

    }
}
