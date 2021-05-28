

using Newtonsoft.Json;

using System.ComponentModel;

namespace LogUploader.Tools.Discord
{
    internal class Image
    {
        private static readonly JsonSerializerSettings JsonSerializerSettings;

        static Image()
        {
            JsonSerializerSettings = new JsonSerializerSettings
            {
                StringEscapeHandling = StringEscapeHandling.EscapeNonAscii
            };
        }

        [JsonProperty("url", DefaultValueHandling = DefaultValueHandling.Ignore)]
        [DefaultValue("")]
        public string URL { get; set; } = "";

        public Image(string uRL)
        {
            URL = uRL;
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this, JsonSerializerSettings);
        }
    }
}
