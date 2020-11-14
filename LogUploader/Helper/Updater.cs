using LogUploader.Data;
using LogUploader.Data.Settings;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LogUploader.Helper
{
    class Updater
    {
        private const string USER_AGENT = "LogUploader";
        private const string GitHubApiLink = @"https://api.github.com/repos/ProfBits/LogUploader2/releases/latest";

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
            progress?.Report(0.9);
            var gitHubVersion = new Version(tag);
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
        private static async Task<string> DownloadInstaller(IProxySettings settings, IProgress<double> progress = null)
        {
            var path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\Temp\\LogUploaderInstaller.exe";
            using (var wc = GetWebClient(settings))
            {
                progress?.Report(0.1);
                var res = await wc.DownloadStringTaskAsync(GitHubApiLink);
                progress?.Report(0.3);
                var data = Newtonsoft.Json.Linq.JObject.Parse(res);
                var installerUrl = data["assets"]
                    .Where(json => (string)json["name"] == "installer.exe")
                    .Select(json => (string)json["browser_download_url"])
                    .First();
                progress?.Report(0.4);
                if (File.Exists(path))
                    File.Delete(path);
                progress?.Report(0.5);
                await wc.DownloadFileTaskAsync(installerUrl, path);
            }
            progress?.Report(1);
            return path;
        }

        private static WebClient GetWebClient(IProxySettings settings)
        {
            var wc = Helper.WebHelper.GetWebClient(settings);
            wc.Headers.Add(HttpRequestHeader.UserAgent, USER_AGENT);
            return wc;
        }

        public static DialogResult ShowUpdateMgsBox() {
            var ui = new GUIs.UpdateAvailableUI();
            return ui.ShowDialog();
        }
    }
}
