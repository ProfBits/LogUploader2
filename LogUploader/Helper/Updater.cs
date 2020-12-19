using LogUploader.Data;
using LogUploader.Data.Settings;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Extensiones.HTTPClient;

namespace LogUploader.Helper
{
    class Updater
    {
        private const string USER_AGENT = "LogUploader";
        private const string GitHubApiLink = @"https://api.github.com/repos/ProfBits/LogUploader2/releases/latest";

        private static string InstallerUrlCache { get; set; } = null;

        public static Version NewestVersion { get; private set; } = null;

        static async Task<Version> GetNewestVersion(IProxySettings settings, IProgress<double> progress = null)
        {
            string res;
            progress?.Report(0);
            using (var wc = GetWebClient(settings))
            {
                try
                {
                    res = await wc.DownloadStringTaskAsync(GitHubApiLink);
                }
                catch (WebException)
                {
                    return new Version(0, 0, 0, 0);
                }
            }
            progress?.Report(0.5);
            var jsonData = Newtonsoft.Json.Linq.JObject.Parse(res);
            var tag = ((string)jsonData["tag_name"]).TrimStart('v', 'V');
            InstallerUrlCache = GetInstallerUrl(jsonData);
            progress?.Report(0.9);
            var gitHubVersion = new Version(tag);
            NewestVersion = gitHubVersion;
            return gitHubVersion;
        }

        public static Version GetLocalVersion()
        {
            return new Version(FileVersionInfo.GetVersionInfo(Assembly.GetEntryAssembly().Location).ProductVersion);
        }

        public static async Task<bool> UpdateAvailable(IProxySettings settings, IProgress<double> progress = null)
        {
            var GitTask = GetNewestVersion(settings, progress);
            var LocalVersion = GetLocalVersion();
            var NewestVersion = await GitTask;
            return NewestVersion > LocalVersion;
        }

        public static async Task Update(IProxySettings settings, IProgress<ProgressMessage> progress = null)
        {
            string installer = await DownloadInstaller(settings, new Progress<double>(p => progress?.Report(new ProgressMessage(p * 0.98, "Downloading Installer"))));
            progress?.Report(new ProgressMessage(0.99, "Starting Installer"));
            Process.Start(installer);
            Program.Exit(ExitCode.UPDATING);
        }

        /// <summary>
        /// Downloads the newest installer for github
        /// </summary>
        /// <param name="settings">the proxy settings</param>
        /// <param name="progress">the progress reporting</param>
        /// <returns></returns>
        private static async Task<string> DownloadInstaller(IProxySettings settings, IProgress<double> progress = null, CancellationToken cancellationToken = default)
        {
            var path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\Temp\\LogUploaderInstaller.exe";
            progress?.Report(0.0);
            using (var client = GetHttpClient(settings))
            {
                string installerUrl;
                double currProgress = 0;
                if (InstallerUrlCache == null)
                {
                    var res = await client.GetStringAsync(GitHubApiLink);
                    progress?.Report(0.3);
                    var data = Newtonsoft.Json.Linq.JObject.Parse(res);
                    installerUrl = GetInstallerUrl(data);
                    progress?.Report(0.35);
                    currProgress = 0.35;
                }
                else
                {
                    installerUrl = InstallerUrlCache;
                    currProgress = 0.05;
                }
                if (File.Exists(path))
                    File.Delete(path);
                currProgress += 0.05;
                progress?.Report(currProgress);
                using (var fs = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    await client.DownloadAsync(installerUrl, fs, new Progress<double>(p => progress?.Report((p * (1 - currProgress)) + currProgress)), cancellationToken);
                }
            }
            progress?.Report(1);
            return path;
        }

        private static string GetInstallerUrl(Newtonsoft.Json.Linq.JObject data)
        {
            return data["assets"]
                .Where(json => ((string)json["name"]).StartsWith("installer") && ((string)json["name"]).EndsWith(".exe"))
                .Select(json => (string)json["browser_download_url"])
                .First();
        }

        private static WebClient GetWebClient(IProxySettings settings)
        {
            var wc = Helper.WebHelper.GetWebClient(settings);
            wc.Headers.Add(HttpRequestHeader.UserAgent, USER_AGENT);
            return wc;
        }

        private static HttpClient GetHttpClient(IProxySettings settings)
        {
            var client = WebHelper.GetHttpClient(settings);
            if (!client.DefaultRequestHeaders.UserAgent.TryParseAdd(USER_AGENT))
            {
                Logger.Warn($"[Updater] Was not able to add useraget \"{USER_AGENT}\" to the headers, forcing it");
                client.DefaultRequestHeaders.Add("User-Agent", USER_AGENT);
            }
            client.Timeout = Timeout.InfiniteTimeSpan;
            return client;
        }

        public static DialogResult ShowUpdateMgsBox() {
            var ui = new GUIs.UpdateAvailableUI();
            return ui.ShowDialog();
        }
    }
}
