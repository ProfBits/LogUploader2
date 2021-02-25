

using Newtonsoft.Json;

using System.ComponentModel;

namespace LogUploader.Tools.Discord.DiscordPostGen
{
    public partial class WebHookData
    {
        public class Author : IAuthor
        {
            private static readonly JsonSerializerSettings JsonSerializerSettings;

            static Author()
            {
                JsonSerializerSettings = new JsonSerializerSettings
                {
                    StringEscapeHandling = StringEscapeHandling.EscapeNonAscii
                };
            }

            public const int NAME_MAX_LENGHT = 256;
            private string name = "";
            [JsonProperty("name", DefaultValueHandling = DefaultValueHandling.Ignore)]
            [DefaultValue("")]
            public string Name
            {
                get => name;
                set => name = value.Length > NAME_MAX_LENGHT ? value.Substring(0, NAME_MAX_LENGHT) : value;
            }

            [JsonProperty("url", DefaultValueHandling = DefaultValueHandling.Ignore)]
            [DefaultValue("")]
            public string URL { get; set; } = "";

            [JsonProperty("icon_url", DefaultValueHandling = DefaultValueHandling.Ignore)]
            [DefaultValue("")]
            public string IconURL { get; set; } = "";

            public Author(string name, string uRL, string iconURL)
            {
                Name = name;
                URL = uRL;
                IconURL = iconURL;
            }

            public override string ToString()
            {
                return JsonConvert.SerializeObject(this, JsonSerializerSettings);
            }
        }
    }
}
