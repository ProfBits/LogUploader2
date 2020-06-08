using LogUploader.Data;
using LogUploader.Data.Settings;
using LogUploader.JSONHelper;
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
            var jsonData = new JSONHelper.JSONHelper().Desirealize(res);
            var tag = jsonData.GetTypedElement<string>("tag_name").TrimStart('v', 'V');
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
            Environment.Exit(100);
        }

        /// <summary>
        /// Downloads the newest installer for github
        /// </summary>
        /// <param name="settings">the proxy settings</param>
        /// <param name="progress">the progress reporting</param>
        /// <returns></returns>
        private static async Task<string> DownloadInstaller(IProxySettings settings, Progress<double> progress = null)
        {
            var path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\Temp\\LogUploaderInstaller.exe";
            using (var wc = GetWebClient(settings))
            {
                var res = await wc.DownloadStringTaskAsync(GitHubApiLink);
                var data = new JSONHelper.JSONHelper().Desirealize(res);
                var installerUrl = data.GetTypedList<JSONObject>("assets")
                    .Where(json => json.GetTypedElement<string>("name") == "installer.exe")
                    .Select(json => json.GetTypedElement<string>("browser_download_url"))
                    .First();
                if (File.Exists(path))
                    File.Delete(path);
                await wc.DownloadFileTaskAsync(installerUrl, path);
            }
            return path;
        }

        private static WebClient GetWebClient(IProxySettings settings)
        {
            var wc = Helpers.WebHelper.GetWebClient(settings);
            wc.Headers.Add(HttpRequestHeader.UserAgent, USER_AGENT);
            return wc;
        }

        public static DialogResult ShowUpdateMgsBox() {
            var ui = new GUIs.UpdateAvailableUI();
            return ui.ShowDialog();
        }
    }
}
