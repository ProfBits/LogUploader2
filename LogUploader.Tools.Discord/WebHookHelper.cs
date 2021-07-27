using LogUploader.Tools.Discord.Data;
using LogUploader.Tools.Settings;

using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Net;
using System.Threading.Tasks;

namespace LogUploader.Tools.Discord
{
    public class WebHookHelper
    {
        public static async Task<string> PostToDiscordWebHookAsync(string uri, WebHookData data, IProxySettings settings)
        {
            using (var wc = WebHelper.GetWebClient(settings))
            {
                wc.Headers.Add(HttpRequestHeader.UserAgent, "LogUploader");
                wc.Headers.Add(HttpRequestHeader.ContentType, "application/json");
                return await wc.UploadStringTaskAsync(uri, "POST", data.ToString());
            }

        }
        public static async Task PostWebHookPosts(WebHook webHook, List<WebHookData> posts, IProxySettings settings) => await PostWebHookPosts(webHook.URL, posts, settings);
        public static async Task PostWebHookPosts(string webHookURL, List<WebHookData> posts, IProxySettings settings)
        {
            foreach (var post in posts)
            {
                await PostToDiscordWebHookAsync(webHookURL, post, settings);
                // Discord Spamprotection
                if (posts.Count > 3)
                    await Task.Delay(500);
            }
        }
    }
}
