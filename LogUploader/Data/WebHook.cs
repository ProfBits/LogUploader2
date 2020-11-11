
using LogUploader.Helper.DiscordPostGen;
using System;
using System.Collections.Generic;

namespace LogUploader.Data
{
    public class WebHook
    {
        public const eDiscordPostFormat DEFAULT_FROMAT = eDiscordPostFormat.PerArea;
        private static long nextID = 0;
        private string avaturURL = DEFAULT_AVATAR_URL;
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

        public WebHook(JSONHelper.JSONObject WebHookJson)
        {
            ID = long.Parse(WebHookJson.GetTypedElement<string>("id"));
            nextID = Math.Max(ID, nextID) + 1;
            URL = WebHookJson.GetTypedElement<string>("url");
            Name = WebHookJson.GetTypedElement<string>("name");
            try
            {
                Format = (eDiscordPostFormat)Enum.Parse(typeof(eDiscordPostFormat), WebHookJson.GetTypedElement<string>("format"));
            }
            catch (Exception e) when (e is JSONHelper.JSONException || e is InvalidCastException || e is ArgumentException)
            {
                Format = DEFAULT_FROMAT;
            }
            try
            {
                AvatarURL = WebHookJson.GetTypedElement<string>("avatarURL");
            }
            catch (JSONHelper.JSONException)
            {
                AvatarURL = DEFAULT_AVATAR_URL;
            }
        }

        public string URL { get => uRL; set {
                if (value.Contains("discordapp.com"))
                {
                    var index = value.IndexOf("discordapp.com");
                    uRL = value.Substring(0, index) + "discord.com" + value.Substring(index + "discordapp.com".Length);
                }
                else uRL = value;
            }
        }
        public string Name { get; set; }
        public long ID { get; set; }
        public eDiscordPostFormat Format { get; set; }

        public const string DEFAULT_USER = "LogUploader";
        private const string DEFAULT_AVATAR_URL = @"https://i.imgur.com/o0QwGPV.png";
        public string AvatarURL
        {
            get => avaturURL == DEFAULT_AVATAR_URL ? "<Default>" : avaturURL;
            set => avaturURL = value == "<Default>" || string.IsNullOrWhiteSpace(value) ? DEFAULT_AVATAR_URL : value;
        }

        public JSONHelper.JSONObject ToJsonObject()
        {
            var jsonNode = new JSONHelper.JSONObject();
            jsonNode.Values.Add("id", ID.ToString());
            jsonNode.Values.Add("url", URL);
            jsonNode.Values.Add("name", Name);
            jsonNode.Values.Add("format", Format.ToString());
            jsonNode.Values.Add("avatarURL", avaturURL.ToString());
            return jsonNode;
        }

        public List<WebHookData> GetPosts(params CachedLog[] logs)
        {
            var generator = DiscordPostGenerator.Get(Format);
            return generator.Generate(logs, Name, avaturURL);
        }
    }
}
