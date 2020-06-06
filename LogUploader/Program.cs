using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using LogUploader.Helpers;
using LogUploader.Properties;
using LogUploader.Languages;
using System.Reflection;
using System.Diagnostics;
using LogUploader.GUI;
using LogUploader.Data;
using System.Threading.Tasks;
using System.Threading;
using LogUploader.Helper;
using LogUploader.GUIs;
using LogUploader.Data.Settings;
using System.Xml.Serialization;

namespace LogUploader
{
    class Program
    {
        // Version 2.0 Targets
        /*  Task: DONE: refactor and modulize Main
         *  Task: DONE: usertokenless mode, remove from first install and dpsReport?
         *  Task: DONE: add multi webhook support
         *  Task: DONE: refactor WebHookHelper
         *  Task: DONE: Add counter for selected logs (uploaded and local)
         *  Task: DONE: Local and remot logprocessor, loguploader with blocking queues and proper reporting -> log db + redesigen
         *  Task: DONE: Disable Proxy support
         *  Task: DONE: Delete old/unused stuff
         *  Task: DONE: Redo settings...
         *  Task: DONE: Fail Saves if .evtc or simple.json or html does not esist.
         *  Task: DONE: SoftwareUsed + licenses screens
         *  Task: DONE: erroro for removed detaild generator or reimplement
         *  Task: DONE: implement missing Ei settings
         *  Task: DONE: implement selected webhook
         *  Task: DONE: cla + small doc
         *  Task: DONE: localization
         *              in to do markings
         *              in main ui
         *              in about and software ui
         *  Task: spell checking
         *  Task: DONE: get user tocken in Settings broken
         *  Task: Tester + Helper in about
         *  Task: DONE: Auto Updater / Repo change
         *  Task: DONE: Qadim CM 30.05 15:13 - does not parse
         *  Task: DONE: improve job faulting
         *  Task: DONE: update emotes to default for rise instead of gn
         *    
         * Maybe: DONE: Succsess Detection for local logs (reverseengeener EI) and add only upload succsessful trys for auto upload
         *        correction for eyes bug as dhumm??
         *        fight duration?? (min duration for upload???) -> local EI + LogDB
         */

        // Version 2.1 Targets
        /*  Task: Darkmode / Thems (integrated into win darkmode?)
         *        Accent Color: HKEY_CURRENT_USER\Software\Microsoft\Windows\DWM\AccentColor format #aabbccdd => aa opacity, bb b, cc g, dd r
         *        Darkmode: HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Themes\Personalize\AppsUseLightTheme 0 => dark mode
         *  Task: Language out of compieled into .txt file
         *  Task: sanity check for ustertoken (on entering it?)
         *  Task: Add optional emote support for discordposts (and add area emotes)
         *  Task: Add help and/or user doc...
         *  Task: Introduce more failsaves
         *  Task: Min filesize for autoupload/parse upload respriction base on local parse?
         *  Task: Log state and cleand up tools....
         *  Task: Whats new Screen after update
         *  Task: Add logs manually
         *  Task: switch over to Newtonsoft (performance)
         *  
         * Maybe: sanity checks for data.json / versionssupport for data.json
         * Maybe: test and reaktivate proxy
         * Maybe: more filter options
         */

        [STAThread]
        static void Main(string[] args)
        {
            //Write out language xmls
            //var ser = new XmlSerializer(typeof(XMLLanguage));
            //using (var tw = new StringWriter())
            //{
            //    var xmlLang = new XMLLanguage(new English());
            //    ser.Serialize(tw, xmlLang);
            //    var exePath = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            //    File.WriteAllText(exePath + @"\Data\English.xml", tw.ToString());
            //}
            //using (var tw = new StringWriter())
            //{
            //    var xmlLang = new XMLLanguage(new German());
            //    ser.Serialize(tw, xmlLang);
            //    var exePath = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            //    File.WriteAllText(exePath + @"\Data\German.xml", tw.ToString());
            //}

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Settings settings = new Settings();
            if (settings.FirstBoot)
            {
                Application.Run(new InitConfigUI());
                settings.Reload();
                if (settings.FirstBoot)
                    return;
            }
            Form ui = null;

            LoadingScreenUI loadingUI = new LoadingScreenUI(async (progress, ct) =>
            {
                ui = await LoadApplication(progress, ct, args);
            });

            Application.Run(loadingUI);

            GC.Collect();

            if (ui == null)
                return;

            //display ui and run
            Application.Run(ui);
            Cleanup();
        }

        private static async Task<Form> LoadApplication(IProgress<ProgressMessage> progress, CancellationToken ct, string[] args)
        {
            //await Task.Delay(10000);

            SetUpLocalisation();

            if (!CheckForOtherInstances())
                return null;

            progress.Report(new ProgressMessage(0.01, "Processing command line Arguments"));
            var flags = ProcessCommandLineArgs(args);

            progress.Report(new ProgressMessage(0.02, "Loading Settings"));
            SettingsData settings;
            {
                Settings mainSettings;
                try
                {
                    mainSettings = LoadSettings(flags);
                }
                catch (SettingInitException)
                {
                    MessageBox.Show("Faild to Load Settings", "Load Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return null;
                }
                settings = new SettingsData(mainSettings);
            }

            SetLanguage(settings);
            LoadJsonData(new Progress<ProgressMessage>((p) => progress.Report(new ProgressMessage(0.04 + (p.Percent * 0.02), "Loading static Data" + " - " + p.Message))));

            if (ct.IsCancellationRequested)
                return null;

            Action UpdateReuest = await CheckForUpdates(settings, new Progress<ProgressMessage>(p => new ProgressMessage(0.06 + (p.Percent * 0.02), p.Message)));
            
            if (ct.IsCancellationRequested)
                return null;

            await InitEliteInsights(settings, settings, new Progress<ProgressMessage>((p) => progress.Report(new ProgressMessage(0.08 + (p.Percent * 0.2), "Init EliteInsights" + " - " + p.Message))));

            if (ct.IsCancellationRequested)
                return null;

            InitDB(new Progress<ProgressMessage>((p) => progress.Report(new ProgressMessage(0.010 + (p.Percent * 0.2), "Local DB" + " - " + p.Message))));

            progress.Report(new ProgressMessage(0.13, "Loading"));

            if (ct.IsCancellationRequested)
                return null;

            Helper.DiscordPostGen.DiscordPostGenerator.Settings = settings;

            var newLogic = new LogUploaderLogic();
            await newLogic.InitLogUploaderLogic(settings,
                flags.EnableAutoParse,
                flags.EnableAutoUpload,
                new Progress<ProgressMessage>(pm => progress.Report(new ProgressMessage(0.13f + (pm.Percent * 0.82f), "Loading - " + pm.Message))));

            if (ct.IsCancellationRequested)
                return null;

            Cleanup = () =>
            {
                newLogic.Dispose();
                UpdateReuest?.Invoke();
            };
            //return await CreateUI(logic, flags);
            return await CreateUI2(newLogic, flags, new Progress<double>(p => progress.Report(new ProgressMessage(0.95 + (p * 0.05), "Creating UI"))));
        }

        private static async Task<Action> CheckForUpdates(SettingsData settings, IProgress<ProgressMessage> progress = null)
        {
            if (await Updater.UpdateAvailable(settings, new Progress<double>(p => progress?.Report(new ProgressMessage(p * 0.2, "Checking for Update")))))
            {
                switch (Updater.ShowUpdateMgsBox())
                {
                    case DialogResult.Yes:
                        await Updater.Update(settings, new Progress<ProgressMessage>(p => progress?.Report(new ProgressMessage((48 * p.Percent), p.Message))));
                        break;
                    case DialogResult.No:
                        break;
                    default:
                        return () =>
                        {
                            switch (Updater.ShowUpdateMgsBox())
                            {
                                case DialogResult.Yes:
                                    Updater.Update(settings).Wait();
                                    break;
                                default:
                                    break;
                            }
                        };
                }
            }

            return null;
        }

        private static Action Cleanup;

        private static void InitDB(Progress<ProgressMessage> progress)
        {
            LogDBConnector.DBConnectionString = $@"Data Source=""{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\LogUploader\LogDB.db""; Version=3;Connect Timeout=30";
            try
            {
                LogDBConnector.GetCount();
            }
            catch (System.Data.SQLite.SQLiteException)
            {
                LogDBConnector.CreateTable();
            }
        }

        private static async Task InitEliteInsights(IEliteInsightsSettings settings, IProxySettings proxySettings, IProgress<ProgressMessage> progress = null)
        {
            await Task.Run(() =>
            {
                progress?.Report(new ProgressMessage(0, "Init"));
                EliteInsights.Init(settings);
                EliteInsights.UpdateNewestVersion(proxySettings, new Progress<double>(p => progress?.Report(new ProgressMessage((p * 0.2) + 0.05, "Checking for Update"))));
                var newVersion = EliteInsights.UpdateAviable();
                if (newVersion)
                {
                    progress?.Report(new ProgressMessage(0.25, "Starting Update"));
                    if (settings.AutoUpdateEI || MessageBox.Show("New Version of EliteInsights is aviable\nUpdate now?", "EliteInsights Update", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                    {
                        try
                        {
                            EliteInsights.Update(proxySettings, new Progress<double>(p => progress?.Report(new ProgressMessage((p * 0.75) + 0.25, "Updating"))));
                        }
                        catch (OperationCanceledException)
                        {
                            if (!EliteInsights.IsInstalled())
                            {
                                MessageBox.Show("Faild to install EliteInsights", "Missing EliteInsights installation", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                Environment.Exit(10);
                            }
                        }
                    }
                }
            });
        }

        private static async Task<LogUploaderUI2> CreateUI2(LogUploaderLogic logic, Flags flags, IProgress<double> progress = null)
        {
            LogUploaderUI2 ui = null;
            await Task.Run(() => ui = new LogUploaderUI2(logic, flags.EnableAutoParse, flags.EnableAutoUpload, progress));
            return ui;
        }

        private static void LoadJsonData(IProgress<ProgressMessage> progress = null)
        {
            progress?.Report(new ProgressMessage(0, "Processing Boss data"));
            var exePath = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            var rawDataJson = GP.ReadJsonFile(exePath + @"\Data\DataConfig.json");
            progress?.Report(new ProgressMessage(0.5, "Processing Boss data"));
            Helper.DataBuilder.LoadDataJson(rawDataJson);
            Profession.Init(exePath + @"\Data\ProfessionData.json",
                new Progress<double>(p => progress?.Report(new ProgressMessage((p * 0.5) + 0.5, "Processing Class data"))));
        }

        private static void SetLanguage(IGeneralSettings settings)
        {
            Language.XMLModeEnable(true);
            Language.Current = settings.Language;
            System.Threading.Thread.CurrentThread.CurrentUICulture = Language.Data.Culture;
        }

        private static Settings LoadSettings(Flags flags)
        {
            Settings settings = new Settings();

            if (flags.ResetSettings)
            {
                var res = MessageBox.Show("Really Reset settings?\nThis cannot be undone", "Confirm Reset", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                if (res == DialogResult.OK)
                {
                    settings.Reset();
                    settings.Save();
                    settings.Reload();
                }
            }

            if (settings.FirstBoot)
            {
                Application.Run(new InitConfigUI());
            }
            settings.Reload();
            if (settings.FirstBoot)
                throw new SettingInitException("Init of settings faild");
            return settings;
        }

        private static bool CheckForOtherInstances()
        {
            var thisProcess = Process.GetCurrentProcess();
            var processes = Process.GetProcessesByName(thisProcess.ProcessName);
            foreach (var process in processes)
            {
                if (process.MainModule.FileName == thisProcess.MainModule.FileName
                    && process.Id != thisProcess.Id)
                {
                    MessageBox.Show("Programm already running!", "Error Programm running", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                    return false;
                }
            }
            return true;
        }

        private static void SetUpLocalisation()
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.InvariantCulture;

            if (System.Globalization.CultureInfo.InstalledUICulture.Name.StartsWith("de-"))
                Language.Current = eLanguage.DE;
        }

        private static Flags ProcessCommandLineArgs(params string[] args)
        {
            var flags = new Flags(args);
            
            return flags;
        }

        private struct Flags
        {
            public const string FlagEnableAutoParse = "-eap";
            public const string FlagEnableAutoUpload = "-eau";
            public const string FlagReset = "-r";

            public bool EnableAutoParse { get; set; }
            public bool EnableAutoUpload { get; set; }
            public bool ResetSettings { get; set; }

            public Flags(bool enableAutoUpload, bool resetSettings, bool enableAutoParse)
            {
                EnableAutoUpload = enableAutoUpload;
                ResetSettings = resetSettings;
                EnableAutoParse = enableAutoParse;
            }

            public Flags(string[] args) : this(false, false, false)
            {
                var argumentErrors = "";
                foreach (var arg in args)
                {
                    switch (arg)
                    {
                        case FlagEnableAutoUpload:
                            EnableAutoUpload = true;
                            break;
                        case FlagEnableAutoParse:
                            EnableAutoParse = true;
                            break;
                        case FlagReset:
                            ResetSettings = true;
                            break;
                        default:
                            argumentErrors += $"\n- \"{arg}\"";
                            break;
                    }
                }
                if (argumentErrors != "")
                {
                    MessageBox.Show("Unknown command line arguments:" + argumentErrors, "Argument parse errors", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }

        }



        [Serializable]
        public class SettingInitException : Exception
        {
            public SettingInitException() { }
            public SettingInitException(string message) : base(message) { }
            public SettingInitException(string message, Exception inner) : base(message, inner) { }
            protected SettingInitException(
              System.Runtime.Serialization.SerializationInfo info,
              System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
        }

    }
}
