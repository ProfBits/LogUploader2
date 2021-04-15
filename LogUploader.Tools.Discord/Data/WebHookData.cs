using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using Newtonsoft.Json;
using System.ComponentModel;
using LogUploader.Tools.Discord;

namespace LogUploader.Data.Discord
{
    internal class WebHookData : IWebHookData
    {
        private static readonly JsonSerializerSettings JsonSerializerSettings;

        static WebHookData()
        {
            JsonSerializerSettings = new JsonSerializerSettings
            {
                StringEscapeHandling = StringEscapeHandling.EscapeNonAscii
            };
        }

        public WebHookData(string username, string avaturURL, string content, List<IEmbed> embeds) : this(embeds)
        {
            Username = username;
            AvaturURL = avaturURL;
            Content = content;
        }

        public WebHookData(List<IEmbed> embeds)
        {
            Embeds.AddRange(embeds);
        }

        public WebHookData(IEmbed embed)
        {
            Embeds.Add(embed);
        }

        public WebHookData()
        {
        }

        [JsonProperty("username", DefaultValueHandling = DefaultValueHandling.Ignore)]
        [DefaultValue("")]
        public string Username { get; set; } = "LogUploader";
        [JsonProperty("avatar_url", DefaultValueHandling = DefaultValueHandling.Ignore)]
        [DefaultValue("")]
        public string AvaturURL { get; set; } = @"https://i.imgur.com/o0QwGPV.png";
        [JsonProperty("content", DefaultValueHandling = DefaultValueHandling.Ignore)]
        [DefaultValue("")]
        public string Content { get; set; } = "";
        [JsonProperty("embeds", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public List<IEmbed> Embeds { get; set; } = new List<IEmbed>();
        [JsonIgnore]
        public const int MAX_Embeds = 10;

        public bool ShouldSerializeEmbeds()
        {
            return Embeds.Count > 0;
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this, JsonSerializerSettings);
        }
    }
}
