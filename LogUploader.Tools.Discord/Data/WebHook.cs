
using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace LogUploader.Tools.Discord.Data
{
    public class WebHook
    {
        private static long nextID = 0;
        [JsonProperty("id")]
        public long ID { get; set; }


        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonIgnore]
        public const string DEFAULT_USER = "LogUploader";


        [JsonIgnore]
        private string uRL;

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


        [JsonIgnore]
        public const eDiscordPostFormat DEFAULT_FROMAT = eDiscordPostFormat.PerArea;

        [JsonIgnore]
        public eDiscordPostFormat Format { get; set; }

        [JsonProperty("format")]
        public string FormatStr { get => Format.ToString(); set => SetFormat(value); }


        [JsonIgnore]
        private const string DEFAULT_AVATAR_URL = @"https://i.imgur.com/o0QwGPV.png";

        [JsonIgnore]
        private string avaturURL = DEFAULT_AVATAR_URL;

        [JsonProperty("avatarURL")]
        public string AvatarURL
        {
            get => avaturURL == DEFAULT_AVATAR_URL ? "<Default>" : avaturURL;
            set => avaturURL = value == "<Default>" || string.IsNullOrWhiteSpace(value) ? DEFAULT_AVATAR_URL : value;
        }

        public WebHook(string uRL, string name, eDiscordPostFormat format = DEFAULT_FROMAT) : this(uRL, name, nextID + 1, format)
        {
        }

        public WebHook(string uRL, string name, long iD, eDiscordPostFormat format = DEFAULT_FROMAT)
        {
            URL = uRL;
            Name = name;
            ID = iD;
            nextID = Math.Max(iD, nextID) + 1;
            Format = format;
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

        private void SetFormat(string value)
        {
            try
            {
                Format = (eDiscordPostFormat)Enum.Parse(typeof(eDiscordPostFormat), value);
            }
            catch (Exception e) when (e is InvalidCastException || e is ArgumentException)
            {
                Format = DEFAULT_FROMAT;
            }
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
