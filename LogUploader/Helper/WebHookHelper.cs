using Extensiones;
using LogUploader.Data;
using LogUploader.Data.GameAreas;
using LogUploader.Data.Settings;
using LogUploader.Helper.DiscordPostGen;
using LogUploader.Interfaces;
using LogUploader.Properties;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Net;
using System.Threading.Tasks;

namespace LogUploader.Helper
{
    internal class WebHookHelper
    {
        internal static async Task<string> PostToDiscordWebHookAsync(string uri, WebHookData data, IProxySettings settings)
        {
            using (var wc = WebHelper.GetWebClient(settings))
            {
                wc.Headers.Add(HttpRequestHeader.UserAgent, "LogUploader");
                wc.Headers.Add(HttpRequestHeader.ContentType, "application/json");
                return await wc.UploadStringTaskAsync(uri, "POST", data.ToString());
            }

        }
        internal static async Task PostWebHookPosts(WebHook webHook, List<WebHookData> posts, IProxySettings settings) => await PostWebHookPosts(webHook.URL, posts, settings);
        internal static async Task PostWebHookPosts(string webHookURL, List<WebHookData> posts, IProxySettings settings)
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
