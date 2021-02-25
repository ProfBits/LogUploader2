﻿

using Newtonsoft.Json;

using System.ComponentModel;

namespace LogUploader.Tools.Discord.DiscordPostGen
{
    public partial class WebHookData
    {
        public class Image : IImage
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
}
