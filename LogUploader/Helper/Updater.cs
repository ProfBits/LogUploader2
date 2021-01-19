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
        private const string GitHubApiLinkStabel = @"https://api.github.com/repos/ProfBits/LogUploader2/releases/latest";
        private const string GitHubApiLinkPreRelease = @"https://api.github.com/repos/ProfBits/LogUploader2/releases";

        private static string InstallerUrlCache { get; set; } = null;

        public static Version NewestVersion { get; private set; } = null;

        static async Task<Version> GetNewestVersion(IProxySettings settings, IGeneralSettings generalSettings, IProgress<double> progress = null)
        {
            progress?.Report(0);
            Newtonsoft.Json.Linq.JToken jsonData;
            try
            {
                if (generalSettings.AllowPrerelases)
                    jsonData = await GetLatestPreRelease(settings);
                else
                    jsonData = await GetLatestStableRelease(settings);
            }
            catch (HttpRequestException)
            {
                return new Version(0, 0, 0, 0);
            }
            progress?.Report(0.5);
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

        public static async Task<bool> UpdateAvailable(IProxySettings settings, IGeneralSettings generalSettings, IProgress<double> progress = null)
        {
            var GitTask = GetNewestVersion(settings, generalSettings);
            var LocalVersion = GetLocalVersion();
            var NewestVersion = await GitTask;
            return NewestVersion > LocalVersion;
        }

        public static async Task Update(IProxySettings settings, IGeneralSettings generalSettings, IProgress<ProgressMessage> progress = null, CancellationToken ct = default)
        {
            string installer = await DownloadInstaller(settings, generalSettings, new Progress<double>(p => progress?.Report(new ProgressMessage(p * 0.98, "Downloading Installer"))), ct);
            if (ct.IsCancellationRequested) return;
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
        private static async Task<string> DownloadInstaller(IProxySettings settings, IGeneralSettings generalSettings, IProgress<double> progress = null, CancellationToken cancellationToken = default)
        {
            var path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\Temp\\LogUploaderInstaller.exe";
            progress?.Report(0.0);
            using (var client = GetHttpClient(settings))
            {
                string installerUrl;
                double currProgress = 0;
                if (InstallerUrlCache == null)
                {
                    Newtonsoft.Json.Linq.JToken jsonData;
                    if (generalSettings.AllowPrerelases)
                        jsonData = await GetLatestPreRelease(settings);
                    else
                        jsonData = await GetLatestStableRelease(settings);
                    progress?.Report(0.30);
                    installerUrl = GetInstallerUrl(jsonData);
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
            if (cancellationToken.IsCancellationRequested) return null;
            return path;
        }

        private static string GetInstallerUrl(Newtonsoft.Json.Linq.JToken data)
        {
            return data["assets"]
                .Where(json => ((string)json["name"]).StartsWith("installer") && ((string)json["name"]).EndsWith(".exe"))
                .Select(json => (string)json["url"])
                .First();
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

        internal async static Task<Newtonsoft.Json.Linq.JToken> GetLatestStableRelease(IProxySettings settings)
        {
            string answer;

            using (var client = GetHttpClient(settings))
            {
                answer = await client.GetStringAsync(GitHubApiLinkStabel);
            }

            return Newtonsoft.Json.Linq.JObject.Parse(answer);

        }

        internal async static Task<Newtonsoft.Json.Linq.JToken> GetLatestPreRelease(IProxySettings settings)
        {
            string answer;

            using (var client = GetHttpClient(settings))
            {
                answer = await client.GetStringAsync(GitHubApiLinkPreRelease);
            }

            return Newtonsoft.Json.Linq.JObject.Parse("{data:" + answer + "}")["data"][0];
        }
    }
}
