
using LogUploader.Helper.DiscordPostGen;
using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace LogUploader.Data
{
    //TODO clean up WebHook
    public class WebHook
    {
        [JsonIgnore]
        public const eDiscordPostFormat DEFAULT_FROMAT = eDiscordPostFormat.PerArea;
        private static long nextID = 0;
        [JsonIgnore]
        private string avaturURL = DEFAULT_AVATAR_URL;
        [JsonIgnore]
        private string uRL;

        public WebHook(string uRL, string name, eDiscordPostFormat format = DEFAULT_FROMAT) : this(uRL, name, nextID + 1, format)
        {
        }

        public WebHook(string uRL, string name, long iD, eDiscordPostFormat format = DEFAULT_FROMAT)
        {
            URL = uRL;
            Name = name;
            ID = iD;
            nextID = Math.Max(iD, nextID) + 1;
        }

        public WebHook(JObject WebHookJson)
        {
            ID = long.Parse((string)WebHookJson["id"]);
            nextID = Math.Max(ID, nextID) + 1;
            URL = (string)WebHookJson["url"];
            Name = (string)WebHookJson["name"];
            SetFormat((string)WebHookJson["format"]);
            AvatarURL = (string)WebHookJson["avatarURL"] ?? DEFAULT_AVATAR_URL;
        }

        [JsonProperty("url")]
        public string URL
        {
            get => uRL; set
            {
                if (value == null)
                {
                    uRL = null;
                    return;
                }
                if (value.Contains("discordapp.com"))
                {
                    var index = value.IndexOf("discordapp.com");
                    uRL = value.Substring(0, index) + "discord.com" + value.Substring(index + "discordapp.com".Length);
                }
                else uRL = value;
            }
        }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("id")]
        public long ID { get; set; }
        [JsonIgnore]
        public eDiscordPostFormat Format { get; set; }
        [JsonProperty("format")]
        public string FormatStr { get => Format.ToString(); set => SetFormat(value); }

        private void SetFormat(string value)
        {
            try
            {
                avaturURL = WebHookJson.GetTypedElement<string>("avatarURL");
            }
            catch (Exception e) when (e is InvalidCastException || e is ArgumentException)
            {
                avaturURL = DEFAULT_AVATAR_URL;
            }
        }

        [JsonIgnore]
        public const string DEFAULT_USER = "LogUploader";
        [JsonIgnore]
        private const string DEFAULT_AVATAR_URL = @"https://i.imgur.com/o0QwGPV.png";
        [JsonProperty("avatarURL")]
        public string AvatarURL
        {
            get => avaturURL == DEFAULT_AVATAR_URL ? "<Default>" : avaturURL;
            set => avaturURL = value == "<Default>" || string.IsNullOrWhiteSpace(value) ? DEFAULT_AVATAR_URL : value;
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }

        public List<WebHookData> GetPosts(params CachedLog[] logs)
        {
            var generator = DiscordPostGenerator.Get(Format);
            return generator.Generate(logs, Name, avaturURL);
        }
    }
}
