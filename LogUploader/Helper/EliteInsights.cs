using System;
using System.Collections.Generic;
using System.IO;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogUploader.Properties;
using LogUploader.JSONHelper;
using System.IO.Compression;
using System.Reflection;
using System.Net;
using LogUploader.Data.Settings;

namespace LogUploader.Helper
{
    internal static class EliteInsights
    {
        private static readonly string BASE_PATH = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\LogUploader\EliteInsights\";
        public static IEliteInsightsSettings Settings { private get; set; }
        private const string BIN = @"bin\";
        private const string LOGS = @"logs\";
        private const string TEMP_FOLDER = @"temp\";
        private const string TEMP_OLD_FOLDER = @"tempOLD\";
        private const string GitHubApiLink = @"https://api.github.com/repos/baaron4/GW2-Elite-Insights-Parser/releases/latest";
        private const string USER_AGENT = "LogUploader";
        private const string ZIP_NAME = "NewEI.zip";

        public static string LogsPath { get => BASE_PATH + LOGS; }
        private static string TempPath { get => BASE_PATH + TEMP_FOLDER; }
        private static string TempOldPath { get => BASE_PATH + TEMP_OLD_FOLDER; }
        private static string BinPath { get => BASE_PATH + BIN; }
        private static string ZipFilePath { get => BASE_PATH + ZIP_NAME; }

        public static Version LocalVersion { get; private set; } = new Version(0, 0, 0, 0);
        public static Version NewestVersion { get; private set; } = new Version(0, 0, 0, 0);

        public static void Init(IEliteInsightsSettings settings)
        {
            Settings = settings;
            Directory.CreateDirectory(BASE_PATH + BIN);
            Directory.CreateDirectory(BASE_PATH + LOGS);
            UpdateLocalVersion();
        }

        public static Version UpdateLocalVersion()
        {
            if (File.Exists(BASE_PATH + BIN + "GuildWars2EliteInsights.exe"))
            {
                var fi = FileVersionInfo.GetVersionInfo(BASE_PATH + BIN + "GuildWars2EliteInsights.exe");
                LocalVersion = new Version(fi.ProductMajorPart, fi.ProductMinorPart, fi.ProductBuildPart, fi.ProductPrivatePart);
            }
            else
                LocalVersion = new Version(0, 0, 0, 0);
            return LocalVersion;
        }

        public static Version UpdateNewestVersion(IProxySettings settings, IProgress<double> progress = null)
        {
            string res;
            progress?.Report(0);
            using (var wc = GetWebClient(settings))
            {
                try
                {
                    res = wc.DownloadString(GitHubApiLink);
                }
                catch (WebException)
                {
                    NewestVersion = new Version(0, 0, 0, 0);
                    return NewestVersion;
                }
            }
            progress?.Report(0.5);
            var jsonData = new JSONHelper.JSONHelper().Desirealize(res);
            var tag = jsonData.GetTypedElement<string>("tag_name").TrimStart('v', 'V');
            progress?.Report(0.9);
            NewestVersion = new Version(tag);
            return NewestVersion;
        }

        public static bool UpdateAviable()
        {
            return NewestVersion > LocalVersion;
        }

        public static bool IsInstalled()
        {
            return File.Exists(BASE_PATH + BIN + "GuildWars2EliteInsights.exe");
        }

        public static Version Update(IProxySettings settings, IProgress<double> progress = null)
        {
            try
            {
                Directory.CreateDirectory(BASE_PATH);
                FolderCleanup();
                progress?.Report(0.05);
                using (var wc = GetWebClient(settings))
                {
                    try
                    {
                        var res = wc.DownloadString(GitHubApiLink);
                        progress?.Report(0.15);
                        var jsonData = new JSONHelper.JSONHelper().Desirealize(res);
                        progress?.Report(0.20);
                        var gw2EiZipURL = jsonData.GetTypedList<JSONObject>("assets")
                            .Where(json => json.GetTypedElement<string>("name") == "GW2EI.zip")
                            .Select(json => json.GetTypedElement<string>("browser_download_url"))
                            .First();
                        wc.DownloadFile(gw2EiZipURL, ZipFilePath);
                    }
                    catch (WebException)
                    {
                        throw new OperationCanceledException("Update EI faild");
                    }
                }
                progress?.Report(0.60);
                Directory.CreateDirectory(TempPath);
                progress?.Report(0.80);
                ZipFile.ExtractToDirectory(ZipFilePath, TempPath);
            }
            catch (Exception)
            {
                FolderCleanup();
                return LocalVersion;
            }
            try
            {
                Directory.Move(BinPath, TempOldPath);
                if (Directory.Exists(BinPath))
                    Directory.Delete(BinPath, true);
                Directory.Move(TempPath, BinPath);
            }
            catch (Exception)
            {
                if (Directory.Exists(BinPath))
                    Directory.Delete(BinPath, true);
                Directory.Move(TempOldPath, BinPath);
            }
            progress?.Report(0.90);
            FolderCleanup();
            UpdateLocalVersion();
            progress?.Report(1);
            return LocalVersion;
        }

        private static void FolderCleanup()
        {
            if (Directory.Exists(TempPath))
                Directory.Delete(TempPath, true);
            if (Directory.Exists(TempOldPath))
                Directory.Delete(TempOldPath, true);
            if (File.Exists(ZipFilePath))
                File.Delete(ZipFilePath);
        }

        private static WebClient GetWebClient(IProxySettings settings)
        {
            var wc = Helpers.WebHelper.GetWebClient(settings);
            wc.Headers.Add(HttpRequestHeader.UserAgent, USER_AGENT);
            return wc;
        }

        public static List<string> Parse(string log)
        {
            if (!IsInstalled())
            {
                throw new OperationCanceledException("Can't parse log locally: No EliteInsights installation present!");
            }

            string destConf = PrepearConfig();
            //-p requiered for silent execution!!
            var args = $"-p -c \"{destConf}\" \"{log}\"";
            var psi = new ProcessStartInfo
            {
                FileName = BASE_PATH + BIN + "GuildWars2EliteInsights.exe",
                WorkingDirectory = BinPath,
                CreateNoWindow = true,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                WindowStyle = ProcessWindowStyle.Hidden,
                Arguments = args
            };

            string stdOut = "";
            string stdErr = "";
            List<string> newFiles = new List<string>();
            using (var watchDog = new FileSystemWatcher(LogsPath))
            {
                watchDog.Created += (sender, e) => newFiles.Add(e.Name);
                watchDog.Changed += (sender, e) => newFiles.Add(e.Name);
                watchDog.Renamed += (sender, e) => newFiles.Add(e.Name);
                watchDog.EnableRaisingEvents = true;
                
                using (var p = new Process())
                {
                    p.StartInfo = psi;
                    p.OutputDataReceived += (sender, e) => stdOut += e.Data + "\n";
                    p.ErrorDataReceived += (sender, e) => stdErr += e.Data + "\n";

                    p.Start();
                    p.WaitForExit();
                }

                watchDog.EnableRaisingEvents = false;
            }

            return newFiles.Distinct().Select(f => LogsPath + f).ToList();
        }

        private static string PrepearConfig()
        {
            var defaultConf = $"{Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + @"\Data\EIconf.conf"}";
            var destConf = BASE_PATH + "EIconf.conf";
            File.Copy(defaultConf, destConf, true);
            File.AppendAllLines(destConf, new string[] { $@"OutLocation={LogsPath}" });
            if (Settings.CreateCombatReplay)
                File.AppendAllLines(destConf, new string[] { $@"ParseCombatReplay=True" });
            else
                File.AppendAllLines(destConf, new string[] { $@"ParseCombatReplay=False" });
            if (Settings.LightTheme)
                File.AppendAllLines(destConf, new string[] { $@"LightTheme=True" });
            else
                File.AppendAllLines(destConf, new string[] { $@"LightTheme=False" });
            return destConf;
        }
    }
}
