using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using LogUploader.Data;

namespace LogUploader.Tools.Database.JSONExtensiones
{
    public class SimmpleTarget : ISimmpleTarget
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
