

using Newtonsoft.Json;

using System.ComponentModel;

namespace LogUploader.Tools.Discord.Data
{
    internal class Footer
    {
        private static readonly JsonSerializerSettings JsonSerializerSettings;

        static Footer()
        {
            JsonSerializerSettings = new JsonSerializerSettings
            {
                StringEscapeHandling = StringEscapeHandling.EscapeNonAscii
            };
        }

        [JsonProperty("text", DefaultValueHandling = DefaultValueHandling.Ignore)]
        [DefaultValue("")]
        public string Text { get; set; } = $"LogUploader v{GP.GetVersion()}";

        [JsonProperty("icon_url", DefaultValueHandling = DefaultValueHandling.Ignore)]
        [DefaultValue("")]
        public string IconURL { get; set; } = @"https://i.imgur.com/o0QwGPV.png";

        public Footer() { }

        public Footer(string text, string iconURL)
        {
            Text = text;
            IconURL = iconURL;
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this, JsonSerializerSettings);
        }
    }
}
