using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace LogUploader.Data
{
    public class SimpleDps : ISimpleDps
    {
        [JsonProperty("id")]
        public int Id { get; set; }
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
