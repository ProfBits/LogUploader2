using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using LogUploader.Helper;
using LogUploader.Properties;
using LogUploader.Languages;
using System.Reflection;
using System.Diagnostics;
using LogUploader.GUI;
using LogUploader.Data;
using System.Threading.Tasks;
using System.Threading;
using LogUploader.GUIs;
using LogUploader.Data.Settings;
using System.Xml.Serialization;
using System.Configuration;

namespace LogUploader
{
    class Program
    {
        // Version 2.0 Targets
        /*  
         *  Task: spell checking
         *  Task: Tester in about
         *  
         */

        // Future Versions
        /*  Task: Language out of compieled into.txt file
         *  Task: Collect exit codes
         *  Task: Whats new Screen after update
         *  Task: add View changelog to about and update UI
         *  Task: Misc category in dataJson for kill/wipe emotes
         *        add emotes to professions
         *  Task: add more webhook formats
         *        with classesemote (compact and area)
         *  Task: Update EI Helper to accout for EI now outputting generated files -> remove file system watcher
         *  Task: Log state and cleand up tools....
         *  Task: switch over to Newtonsoft (performance)
         *  Task: Add logs manually
         *  Task: Add help and/or user doc...
         *  Task: Darkmode / Thems (integrated into win darkmode?)
         *        Accent Color: HKEY_CURRENT_USER\Software\Microsoft\Windows\DWM\AccentColor format #aabbccdd => aa opacity, bb b, cc g, dd r
         *        Darkmode: HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Themes\Personalize\AppsUseLightTheme 0 => dark mode
         *  Task: sanity check for ustertoken (on entering it?)
         *  
         * Maybe: sanity checks for data.json / versionssupport for data.json
         * Maybe: test and reaktivate proxy
         * Maybe: more filter options
         */

        [STAThread]
        static void Main(string[] args)
        {
#if CREATE_LANGUAGE_XMLS
            WriteOutLanguageXmls();
#endif

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
                try
                {
                    ui = await LoadApplication(progress, ct, args);
                }
                catch (System.ComponentModel.Win32Exception e)
                {
                    MessageBox.Show(GetWin23ExeptionMessage(e), "Win32 error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Environment.Exit(1);
                }
                catch (Exception e)
                {
                    MessageBox.Show(GetExceptionMessage(e), "Normal Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Environment.Exit(2);
                }
            });


            Application.Run(loadingUI);

            GC.Collect();

            //RoPlusTesting.Run(((LogUploaderUI2)ui).DEBUGgetLogic());
            //Environment.Exit(42);

            if (ui == null)
                return;

            try
            {
                //display ui and run
                Application.Run(ui);
            }
            catch (System.ComponentModel.Win32Exception e)
            {
                MessageBox.Show(GetWin23ExeptionMessage(e), "Win32 error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Environment.Exit(1);
            }
            catch (Exception e)
            {
                MessageBox.Show(GetExceptionMessage(e), "Normal Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Environment.Exit(2);
            }
            Cleanup();
            Environment.Exit(0);
        }

        private static string GetExceptionMessage(Exception e)
        {
            return $"Exception: {e.GetType().ToString()}\n" +
                                $"Message: {e.Message}\n" +
                                "StacTrace:\n" +
                                $"{e.StackTrace}";
        }

        private static string GetWin23ExeptionMessage(System.ComponentModel.Win32Exception e)
        {
            var res = $"Exception: {e.GetType().ToString()}\n" +
                                    $"Message: {e.Message}\n" +
                                    $"NativeErrorCode: {e.NativeErrorCode}\n" +
                                    $"ErrorCode: {e.ErrorCode}\n";
            try
            {
                res += $"Data: {string.Join("\n", e?.Data?.Keys.Cast<object>().Select(k => k.ToString() + " : " + e.Data[k].ToString()))}\n";
            }
            catch (Exception)
            { }
            res += $"\nStackTrace: " +
                $"{e.StackTrace}";


            return res;
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
            LoadJsonData(new Progress<ProgressMessage>((p) => progress.Report(new ProgressMessage(0.06 + (p.Percent * 0.04), "Loading static Data" + " - " + p.Message))));

            if (ct.IsCancellationRequested)
                return null;

            Action UpdateReuest = await CheckForUpdates(settings, new Progress<ProgressMessage>(p => new ProgressMessage(0.10 + (p.Percent * 0.05), p.Message)));
            
            if (ct.IsCancellationRequested)
                return null;

            await InitEliteInsights(settings, settings, new Progress<ProgressMessage>((p) => progress.Report(new ProgressMessage(0.15 + (p.Percent * 0.5), "Init EliteInsights" + " - " + p.Message))));

            if (ct.IsCancellationRequested)
                return null;

            InitDB(new Progress<ProgressMessage>((p) => progress.Report(new ProgressMessage(0.20 + (p.Percent * 0.5), "Local DB" + " - " + p.Message))));

            progress.Report(new ProgressMessage(0.13, "Loading"));

            if (ct.IsCancellationRequested)
                return null;

            Helper.DiscordPostGen.DiscordPostGenerator.Settings = settings;

            var newLogic = new LogUploaderLogic();
            await newLogic.InitLogUploaderLogic(settings,
                flags.EnableAutoParse,
                flags.EnableAutoUpload,
                new Progress<ProgressMessage>(pm => progress.Report(new ProgressMessage(0.25f + (pm.Percent * 0.70f), "Loading - " + pm.Message)))
                );

            if (ct.IsCancellationRequested)
                return null;

            Cleanup = () =>
            {
                newLogic.Dispose();
                UpdateReuest?.Invoke();
            };
            //return await CreateUI(logic, flags);
            return await CreateUI2(newLogic, new Progress<double>(p => progress.Report(new ProgressMessage(0.95 + (p * 0.05), "Creating UI"))));
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
                    if (settings.AutoUpdateEI || MessageBox.Show("New Version of EliteInsights is aviable\nUpdate now?", "EliteInsights Update", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                    {
                        progress?.Report(new ProgressMessage(0.25, "Starting Update"));
                        try
                        {
                            EliteInsights.Update(proxySettings, new Progress<double>(p => progress?.Report(new ProgressMessage((p * 0.70) + 0.25, "Updating"))));
                        }
                        catch (OperationCanceledException)
                        {
                            if (!EliteInsights.IsInstalled())
                            {
                                MessageBox.Show("Faild to install EliteInsights", "Missing EliteInsights installation", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                //TODO refactor exit code
                                Environment.Exit(10);
                            }
                        }
                        progress?.Report(new ProgressMessage(1, "Update Done"));
                    }
                }
            });
        }

        private static async Task<LogUploaderUI2> CreateUI2(LogUploaderLogic logic, IProgress<double> progress = null)
        {
            LogUploaderUI2 ui = null;
            await Task.Run(() => ui = new LogUploaderUI2(logic, progress));
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

        private static void WriteOutLanguageXmls()
        {
            var ser = new XmlSerializer(typeof(XMLLanguage));
            using (var tw = new StringWriter())
            {
                var xmlLang = new XMLLanguage(new English());
                ser.Serialize(tw, xmlLang);
                var exePath = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
                File.WriteAllText(exePath + @"\Data\English.xml", tw.ToString());
            }
            using (var tw = new StringWriter())
            {
                var xmlLang = new XMLLanguage(new German());
                ser.Serialize(tw, xmlLang);
                var exePath = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
                File.WriteAllText(exePath + @"\Data\German.xml", tw.ToString());
            }
            var res = MessageBox.Show("Copy into project?", "Copy XMLs", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
            if (res == DialogResult.Yes)
            {
                var exePath = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
                File.Copy(exePath + @"\Data\English.xml", exePath + @"\..\..\Data\English.xml", true);
                File.Copy(exePath + @"\Data\German.xml", exePath + @"\..\..\Data\German.xml", true);
            }
            Environment.Exit(2);
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
                for (int i = 0; i < args.Length; i++)
                {
                    var arg = args[i];
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
                if (!string.IsNullOrEmpty(argumentErrors))
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
