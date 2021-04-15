

using Newtonsoft.Json;

using System.ComponentModel;

namespace LogUploader.Tools.Discord
{
    internal class Thumbmail : IThumbmail
    {
        private static readonly JsonSerializerSettings JsonSerializerSettings;

        static Thumbmail()
        {
            JsonSerializerSettings = new JsonSerializerSettings
            {
                StringEscapeHandling = StringEscapeHandling.EscapeNonAscii
            };
        }

        [JsonProperty("url", DefaultValueHandling = DefaultValueHandling.Ignore)]
        [DefaultValue("")]
        string URL { get; set; } = "";

        public Thumbmail(string uRL)
        {
            URL = uRL;
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this, JsonSerializerSettings);
        }
    }
}
