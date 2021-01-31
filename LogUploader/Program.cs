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
using System.ComponentModel;

namespace LogUploader
{
    class Program
    {
        // Future Versions
        /*  Task: Log state and cleand up tools....
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
        static int Main(string[] args)
        {
            InitLogger();

            try
            {
                if (!CheckForOtherInstances())
                    Exit(ExitCode.ALREADY_RUNNING);
            }
            #region CheckForOtherInstances Error Handling
            catch (System.ComponentModel.Win32Exception e)
            {
                Logger.Error("Win32 Error Code: " + e.NativeErrorCode + " (native) " + e.ErrorCode + " (managed)");
                Logger.LogException(e);
                //MessageBox.Show(GetWin23ExeptionMessage(e), "Win32 error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                var errorUI = new FatalErrorUI("Win32 error", GetWin23ExeptionMessage(e));
                errorUI.ShowDialog();
                Exit(ExitCode.WIN32_EXCPTION);
            }
            catch (Exception e)
            {
                Logger.Error("Normal ERROR");
                Logger.LogException(e);
                //MessageBox.Show(GetExceptionMessage(e), "Normal Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
                var errorUI = new FatalErrorUI("Normal Exception", GetExceptionMessage(e));
                errorUI.ShowDialog();
                Exit(ExitCode.CLR_EXCPTION);
            }
            #endregion

            if (args.Length > 0) Logger.Message("Args: " + args.Aggregate("", (str1, str2) => str1 + " " + str2));
            else Logger.Message("No args");

#if CREATE_LANGUAGE_XMLS
            WriteOutLanguageXmls();
#endif

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Settings settings = new Settings();
            if (settings.FirstBoot)
            {
                Logger.Message("First boot");
                Application.Run(new InitConfigUI());
                settings = new Settings();
                if (settings.FirstBoot)
                {
                    Logger.Error("Exit: INIT_SETUP_FAILED");
                    return (int)ExitCode.INIT_SETUP_FAILED;
                }
            }
            Form ui = null;

            LoadingScreenUI loadingUI = new LoadingScreenUI(async (progress, ct, cts) =>
            {
                try
                {
                    ui = await LoadApplication(progress, args, ct, cts);
                }
                catch (System.ComponentModel.Win32Exception e)
                {
                    Logger.Error("Win32 Error Code: " + e.NativeErrorCode + " (native) " + e.ErrorCode + " (managed)");
                    Logger.LogException(e);
                    //MessageBox.Show(GetWin23ExeptionMessage(e), "Win32 error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    var errorUI = new FatalErrorUI("Win32 error", GetWin23ExeptionMessage(e));
                    errorUI.ShowDialog(ui);
                    Exit(ExitCode.WIN32_EXCPTION);
                }
                catch (Exception e)
                {
                    Logger.Error("Normal ERROR");
                    Logger.LogException(e);
                    //MessageBox.Show(GetExceptionMessage(e), "Normal Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    var errorUI = new FatalErrorUI("Normal Exception", GetExceptionMessage(e));
                    errorUI.ShowDialog(ui);
                    Exit(ExitCode.CLR_EXCPTION);
                }
            });


            Application.Run(loadingUI);

            GC.Collect();

            if (ui == null)
                return (int)ExitCode.STARTUP_FAILED;

            try
            {
                //display ui and run
                Application.Run(ui);
            }
            catch (System.ComponentModel.Win32Exception e)
            {
                Logger.Error("Win32 Error Code: " + e.NativeErrorCode + " (native) " + e.ErrorCode + " (managed)");
                Logger.LogException(e);
                MessageBox.Show(GetWin23ExeptionMessage(e), "Win32 error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Exit(ExitCode.WIN32_EXCPTION);
            }
            catch (Exception e)
            {
                Logger.Error("Normal ERROR");
                Logger.LogException(e);
                MessageBox.Show(GetExceptionMessage(e), "Normal Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Exit(ExitCode.CLR_EXCPTION);
            }
            Cleanup();
            return (int)ExitCode.OK;
        }

        private static void InitLogger()
        {

#if DEBUG
            Logger.Init(Updater.GetLocalVersion().ToString(), eLogLevel.DEBUG);
            Logger.Debug("DEBUG BUILD");
#elif BETA
            Logger.Init(Updater.GetLocalVersion().ToString(), eLogLevel.DEBUG);
            Logger.Debug("BETA BUILD");
#elif ALPHA
            Logger.Init(Updater.GetLocalVersion().ToString(), eLogLevel.DEBUG);
            Logger.Debug("ALPHA BUILD");
#else
            Logger.Init(Updater.GetLocalVersion().ToString(), eLogLevel.NORMAL);
#endif
        }

        private static string GetExceptionMessage(Exception e)
        {
            return $"Exception: {e.GetType()}\n" +
                                $"Message: {e.Message}\n" +
                                "StacTrace:\n" +
                                $"{e.StackTrace}";
        }

        private static string GetWin23ExeptionMessage(System.ComponentModel.Win32Exception e)
        {
            var res = $"Exception: {e.GetType()}\n" +
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

        private static async Task<Form> LoadApplication(IProgress<ProgressMessage> progress, string[] args, CancellationToken ct, CancellationTokenSource cts)
        {
            //await Task.Delay(10000);
            SetUpLocalisation();

            progress.Report(new ProgressMessage(0.01, "Processing command line Arguments"));
            var flags = ProcessCommandLineArgs(args);

#if DEBUG
#elif BETA
#elif ALPHA
#else
            Logger.LogLevel = flags.LogLevel;
#endif

            progress.Report(new ProgressMessage(0.02, "Loading Settings"));
            SettingsData settings = null;
            {
                Settings mainSettings;
                try
                {
                    mainSettings = LoadSettings(flags);
                    settings = new SettingsData(mainSettings);
                }
                catch (SettingInitException e)
                {
                    Logger.LogException(e);
                    MessageBox.Show("Faild to Load Settings", "Load Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Exit(ExitCode.LOAD_SETTINGS_ERROR);
                }
            }

            Logger.Message("Setup - SetLanguage");
            SetLanguage(settings);
            Logger.Message("Setup - Load json's");
            LoadJsonData(new Progress<ProgressMessage>((p) => progress.Report(new ProgressMessage(0.06 + (p.Percent * 0.04), "Loading static Data" + " - " + p.Message))));

            if (ct.IsCancellationRequested)
                return null;

            cts.CancelAfter(TimeSpan.FromMinutes(5));
            Logger.Message("Setup - Check for updates");
            Action UpdateReuest = await CheckForUpdates(settings, new Progress<ProgressMessage>(p => new ProgressMessage(0.10 + (p.Percent * 0.05), p.Message)), ct);
            cts.CancelAfter(Timeout.Infinite);

            if (ct.IsCancellationRequested)
                return null;

            cts.CancelAfter(TimeSpan.FromMinutes(5));
            Logger.Message("Setup - Check for updates EI");
            await InitEliteInsights(settings, settings, new Progress<ProgressMessage>((p) => progress.Report(new ProgressMessage(0.15 + (p.Percent * 0.05), "Init EliteInsights" + " - " + p.Message))), ct);
            cts.CancelAfter(Timeout.Infinite);

            if (ct.IsCancellationRequested)
                return null;

            Logger.Message("Setup - Init DB");
            InitDB(new Progress<ProgressMessage>((p) => progress.Report(new ProgressMessage(0.20 + (p.Percent * 0.05), "Local DB" + " - " + p.Message))));

            progress.Report(new ProgressMessage(0.26, "Loading"));

            if (ct.IsCancellationRequested)
                return null;

            Helper.DiscordPostGen.DiscordPostGenerator.Settings = settings;

            Logger.Message("Setup - Logic");
            var newLogic = new LogUploaderLogic();
            await newLogic.InitLogUploaderLogic(settings,
                flags.EnableAutoParse,
                flags.EnableAutoUpload,
                new Progress<ProgressMessage>(pm => progress.Report(new ProgressMessage(0.27f + (pm.Percent * 0.64f), "Loading - " + pm.Message)))
                );

            if (ct.IsCancellationRequested)
                return null;

            Cleanup = () =>
            {
                newLogic.Dispose();
                UpdateReuest?.Invoke();
            };

            Logger.Message("Setup - UI");
            //return await CreateUI(logic, flags);
            return await CreateUI2(newLogic, new Progress<double>(p => progress.Report(new ProgressMessage(0.91 + (p * 0.07), "Creating UI"))));
        }

        private static async Task<Action> CheckForUpdates(SettingsData settings, IProgress<ProgressMessage> progress = null, CancellationToken ct = default)
        {
            if (await Updater.UpdateAvailable(settings, settings, new Progress<double>(p => progress?.Report(new ProgressMessage(p * 0.2, "Checking for Update")))))
            {
                return await AskForUpdate(settings, progress, ct);
            }

            return null;
        }

        private static async Task<Action> AskForUpdate(SettingsData settings, IProgress<ProgressMessage> progress, CancellationToken ct)
        {
            Logger.Message("Update available.\nNew version: " + (Updater.NewestVersion?.ToString() ?? "na"));
            var userCoice = Updater.ShowUpdateMgsBox();
            switch (userCoice)
            {
                case DialogResult.Yes:
                    _ = await PerformUpdate(settings, progress, ct);
                    return null;
                case DialogResult.No:
                    Logger.Warn("Product updatde skipped");
                    return null;
                default:
                    Logger.Error("dialog exit: " + userCoice);
                    return () =>
                    {
                        Logger.Message("Re ask user to update");
                        var userSelection = Updater.ShowUpdateMgsBox();
                        Logger.Message("user: " + userSelection);
                        switch (userSelection)
                        {
                            case DialogResult.Yes:
                                //TODO add progress
                                PerformUpdate(settings, null, new CancellationTokenSource(TimeSpan.FromMinutes(5)).Token).Wait();
                                break;
                            default:
                                break;
                        }
                    };
            }
        }

        private static async Task<bool> PerformUpdate(SettingsData settings, IProgress<ProgressMessage> progress, CancellationToken ct)
        {
            try
            {
                await Updater.Update(settings, settings, new Progress<ProgressMessage>(p => progress?.Report(new ProgressMessage((17.8 * p.Percent) + 0.2, p.Message))), ct);
                return true;
            }
            catch (Exception e)
            {
                Logger.Error("Update Failed");
                Logger.LogException(e);
                MessageBox.Show("Update Failed. Reaseon:\n" + e.Message, "Update Failed", MessageBoxButtons.OK);
                return false;
            }
        }

        private static Action Cleanup;

        private static void InitDB(IProgress<ProgressMessage> progress)
        {
            progress?.Report(new ProgressMessage(0, "Init"));
            string dbFilePaht = $@"{ Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\LogUploader\LogDB.db";
            LogDBConnector.DBConnectionString = $@"Data Source=""{dbFilePaht}""; Version=3;Connect Timeout=30";
            try
            {
                progress?.Report(new ProgressMessage(0.25, "Connect"));
                var count = LogDBConnector.GetCount();
                Logger.Message("DB found " + count + " entries");
            }
            catch (System.Data.SQLite.SQLiteException)
            {
                //Create DB
                progress?.Report(new ProgressMessage(0.50, "Create"));
                Logger.Message("Creating DB");
                LogDBConnector.CreateTable();
            }
            progress?.Report(new ProgressMessage(1, "Done"));
        }

        private static async Task InitEliteInsights(IEliteInsightsSettings settings, IProxySettings proxySettings, IProgress<ProgressMessage> progress = null, CancellationToken cancellationToken = default)
        {
            progress?.Report(new ProgressMessage(0, "Init"));
            EliteInsights.Init(settings);
            await EliteInsights.UpdateNewestVersion(proxySettings, new Progress<double>(p => progress?.Report(new ProgressMessage((p * 0.2) + 0.05, "Checking for Update"))));
            var newVersion = EliteInsights.UpdateAviable();
            if (EliteInsights.IsInstalled()) Logger.Message($"Installed EI Version: {EliteInsights.LocalVersion}"); else Logger.Warn("EI not installed");
            if (newVersion)
            {
                Logger.Message("EI update available. New version: " + EliteInsights.NewestVersion);
                Logger.Message("Auto update EI: " + settings.AutoUpdateEI);
                if (settings.AutoUpdateEI || MessageBox.Show("New Version of EliteInsights is aviable\nUpdate now?", "EliteInsights Update", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                {
                    progress?.Report(new ProgressMessage(0.25, "Starting Update"));
                    try
                    {
                        await EliteInsights.Update(proxySettings, new Progress<double>(p => progress?.Report(new ProgressMessage((p * 0.70) + 0.25, $"Updating {p*100:.}%"))), cancellationToken);
                        Logger.Message("Update EI completed");
                    }
                    catch (OperationCanceledException e)
                    {
                        Logger.Warn("EI update failed");
                        Logger.LogException(e);
                        if (!EliteInsights.IsInstalled())
                        {
                            MessageBox.Show("Faild to install EliteInsights", "Missing EliteInsights installation", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            //TODO really crash here?
                            Exit(ExitCode.EI_UPDATE_FATAL_ERROR);
                        }
                    }

                    progress?.Report(new ProgressMessage(1, "Update Done"));
                }
            }
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

#if CREATE_LANGUAGE_XMLS
        private static void WriteOutLanguageXmls()
        {
            var ser = new XmlSerializer(typeof(XMLLanguage));
            using (var tw = new StringWriter())
            {
                var xmlLang = new XMLLanguage();
                Jitbit.Utils.PropMapper<ILanguage, XMLLanguage>.CopyTo(new English(), xmlLang);
                ser.Serialize(tw, xmlLang);
                var exePath = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
                File.WriteAllText(exePath + @"\Data\English.xml", tw.ToString());
            }
            using (var tw = new StringWriter())
            {
                var xmlLang = new XMLLanguage();
                Jitbit.Utils.PropMapper<ILanguage, XMLLanguage>.CopyTo(new German(), xmlLang);
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
            Exit(ExitCode.LANG_XML_CREATION_BUILD);
        }
#endif

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
            public const string FlagLogLevelDebug = "-d";
            public const string FlagLogLevelWarning = "-w";
            public const string FlagLogLevelNormal = "-i";
            public const string FlagLogLevelVerbose = "-v";

            public bool EnableAutoParse { get; set; }
            public bool EnableAutoUpload { get; set; }
            public bool ResetSettings { get; set; }
            public eLogLevel LogLevel { get; set; }

            public Flags(bool enableAutoUpload = false, bool resetSettings = false, bool enableAutoParse = false, eLogLevel logLevel = eLogLevel.WARN)
            {
                EnableAutoUpload = enableAutoUpload;
                ResetSettings = resetSettings;
                EnableAutoParse = enableAutoParse;
                LogLevel = logLevel;
            }

            public Flags(string[] args) : this()
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
                        case FlagLogLevelDebug:
                            LogLevel = eLogLevel.DEBUG;
                            break;
                        case FlagLogLevelNormal:
                            LogLevel = eLogLevel.NORMAL;
                            break;
                        case FlagLogLevelWarning:
                            LogLevel = eLogLevel.WARN;
                            break;
                        case FlagLogLevelVerbose:
                            LogLevel = eLogLevel.VERBOSE;
                            break;
                        default:
                            argumentErrors += $"\n- \"{arg}\"";
                            break;
                    }
                }
                if (!string.IsNullOrEmpty(argumentErrors))
                {
                    Logger.Warn("Argument errors:\n" + argumentErrors);
                    MessageBox.Show("Unknown command line arguments:" + argumentErrors, "Argument parse errors", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        public static void Exit(ExitCode exitCode)
        {
            Logger.Error($"Programm Exit Reason: {(int)exitCode} {exitCode}");
            Environment.Exit((int)exitCode);
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
