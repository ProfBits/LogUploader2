using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using Newtonsoft.Json;
using System.ComponentModel;

namespace LogUploader.Tools.Discord.Data
{
    public class WebHookData
    {
        private static readonly JsonSerializerSettings JsonSerializerSettings;

        static WebHookData()
        {
            JsonSerializerSettings = new JsonSerializerSettings
            {
                StringEscapeHandling = StringEscapeHandling.EscapeNonAscii
            };
        }

        internal WebHookData(string username, string avaturURL, string content, List<Embed> embeds) : this(embeds)
        {
            Username = username;
            AvaturURL = avaturURL;
            Content = content;
        }

        internal WebHookData(List<Embed> embeds)
        {
            Embeds.AddRange(embeds);
        }

        internal WebHookData(Embed embed)
        {
            Embeds.Add(embed);
        }

        internal WebHookData()
        {
        }

        [JsonProperty("username", DefaultValueHandling = DefaultValueHandling.Ignore)]
        [DefaultValue("")]
        internal string Username { get; set; } = "LogUploader";
        [JsonProperty("avatar_url", DefaultValueHandling = DefaultValueHandling.Ignore)]
        [DefaultValue("")]
        internal string AvaturURL { get; set; } = @"https://i.imgur.com/o0QwGPV.png";
        [JsonProperty("content", DefaultValueHandling = DefaultValueHandling.Ignore)]
        [DefaultValue("")]
        internal string Content { get; set; } = "";
        [JsonProperty("embeds", DefaultValueHandling = DefaultValueHandling.Ignore)]
        internal List<Embed> Embeds { get; set; } = new List<Embed>();
        [JsonIgnore]
        internal const int MAX_Embeds = 10;

        internal bool ShouldSerializeEmbeds()
        {
            return Embeds.Count > 0;
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this, JsonSerializerSettings);
        }
    }
}
