using System;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using LogUploader.Data.Settings;
using LogUploader.Properties;

namespace LogUploader
{
    public class DPSReport
    {
        internal string UserToken { private get; set; } = "";
        private IProxySettings Settings { get; set; }

        private const string BaseAddress = @"http://a.dps.report";

        internal DPSReport(IProxySettings settings)
        {
            Settings = settings;
        }

        internal DPSReport(IProxySettings settings, string userToken) : this(settings)
        {
            UserToken = userToken;
        }

        private string URLUpladContent()
        {
            var temp = BaseAddress + @"/uploadContent?json=1&generator=ei";
            var temp2 = !string.IsNullOrEmpty(UserToken) ? $"&userToken={UserToken}" : "";
            return temp + temp2;
        }

        private string URLGetEnounterData(string id)
        {
            return BaseAddress + @"/getJson?id=" + id;
        }

        private string URLGetEnounterDataPermalink(string permalink)
        {
            permalink = permalink.Trim('"', '\'');
            if (permalink.StartsWith("https://dps.report/"))
                permalink.Remove("https://dps.report/".Length);
            return BaseAddress + @"/getJson?permalink=" + permalink;
        }

        private string URLGetUploades(int page)
        {
            return BaseAddress + @"/getUploads?page=" + page + $"&userToken={UserToken}";
        }

        public string UpladContent(string path)
        {
            return UploadFile(URLUpladContent(), path);
        }

        public string GetEncounterData(string id)
        {
            return DownloadString(URLGetEnounterData(id));
        }

        public async Task<string> GetEncounterDataAsync(string id)
        {
            return await DownloadStringAsync(URLGetEnounterData(id));
        }

        public string GetPastUploads(int page = 1)
        {
            return DownloadString(URLGetUploades(page));
        }

        public string GetEncounterDataPermalink(string permalink)
        {
            return DownloadString(URLGetEnounterDataPermalink(permalink));
        }

        public async Task<string> GetEncounterDataPermalinkAsync(string permalink)
        {
            return await DownloadStringAsync(URLGetEnounterDataPermalink(permalink));
        }

        private string DownloadString(string address)
        {
            string data;
            using (MyWebClient wc = GetWebClient(Settings))
            {
                try
                {
                    data = wc.DownloadString(address);
                }
                catch (WebException e)
                {
                    data = $"{{\"Error\":\"{e.Message}\"}}";
                }
            }
            return data;
        }

        private async Task<string> DownloadStringAsync(string address)
        {
            string data;
            using (MyWebClient wc = GetWebClient(Settings))
            {
                try
                {
                    data = await wc.DownloadStringTaskAsync(address);
                }
                catch (WebException e)
                {
                    data = $"{{\"Error\":\"{e.Message}\"}}";
                }
            }
            return data;
        }

        private string UploadFile(string address, string path)
        {
            string answerStr;
            using (MyWebClient wc = GetWebClient(Settings))
            {
                try
                {
                    var answerByte = wc.UploadFile(address, path);
                    answerStr = Encoding.ASCII.GetString(answerByte);
                }
                catch (WebException e)
                {
                    answerStr = $"{{\"Error\":\"{e.Message}\"}}";
                }
            }

            return answerStr;
        }

        private async Task<string> UploadFileAsync(string address, string path)
        {
            string answerStr;
            using (MyWebClient wc = GetWebClient(Settings))
            {
                wc.Timeout = 600_000;
                try
                {
                    var answerByte = await wc.UploadFileTaskAsync(address, path);
                    answerStr = Encoding.ASCII.GetString(answerByte);
                }
                catch (WebException e)
                {
                    answerStr = $"{{\"Error\":\"{e.Message}\"}}";
                }
            }

            return answerStr;
        }

        private class MyWebClient : WebClient
        {
            public int Timeout { get; set; } = 240_000;

            protected override WebRequest GetWebRequest(Uri uri)
            {
                WebRequest lWebRequest = base.GetWebRequest(uri);
                lWebRequest.Timeout = Timeout;
                ((HttpWebRequest) lWebRequest).ReadWriteTimeout = Timeout;
                return lWebRequest;
            }
        }

        private MyWebClient GetWebClient(IProxySettings settings)
        {
            var mywc = new MyWebClient();
            mywc = Helper.WebHelper.ConfigureWebClientProxy(mywc, settings);
            return mywc;
        }
    }
}
