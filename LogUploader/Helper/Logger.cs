using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.CompilerServices;

namespace LogUploader.Helper
{
    internal static class Logger
    {
        private static bool InitDone = false;
        private const string PREFIX_MESSAGE = "[msg]";
        private const string PREFIX_ERROR = "[ERR]";
        private const string PREFIX_WARN = "[WRN]";
        private const string PREFIX_DEBUG = "[dbg]";
        private const string PREFIX_VERBOSE = "[veb]";

        private const string LOG_DIR = "\\LogUploader\\Logs\\";
        private const int LOGS_TO_KEEP = 29;
        public static string LogFile { get; private set; }

        private static eLogLevel logLevel;
        internal static eLogLevel LogLevel { get => logLevel; set
            {
                if (InitDone)
                    Log("[LOGLEVEL]", $"New Loglevel is: {value}");
                logLevel = value;
            }
        }

        private static readonly object sync = new object();

        public static void Init(string version, eLogLevel logLevel)
        {
            LogLevel = logLevel;
            var logPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + LOG_DIR;
            Directory.CreateDirectory(logPath);
            CleanUpLogs(logPath);

            LogFile = logPath + DateTime.Now.ToString("yyMMdd_hhmmssfff") + ".log";
            WriteHeader(version);
            
            InitDone = true;
        }

        private static void CleanUpLogs(string logPath)
        {
            var logs = new DirectoryInfo(logPath).GetFiles().Where(file => file.Name.EndsWith(".log"));
            if (logs.Count() > LOGS_TO_KEEP)
            {
                foreach (var file in logs.OrderBy(file => file.CreationTime).Take(logs.Count() - LOGS_TO_KEEP))
                {
                    try
                    {
                        file.Delete();
                    }
                    catch (IOException e)
                    {
                        Warn("Could not delelet log (IOException) " + file.FullName);
                        Warn(e.Message);
                    }
                    catch (System.Security.SecurityException e)
                    {
                        Warn("Could not delelet log (SecurityException) " + file.FullName);
                        Warn(e.Message);
                    }
                    catch (UnauthorizedAccessException e)
                    {
                        Warn("Could not delelet log (UnauthorizedAccessException) " + file.FullName);
                        Warn(e.Message);
                    }
                }
            }
        }

        private static void WriteHeader(string version)
        {
            string build;
#if DEBUG
            build = "DEBUG";
#elif BETA
            build = "BETA";
#elif ALPHA
            build = "ALPHA";
#else
            build = "Release";
#endif

            var text = $@"### LogUploader Log
### Version:  {version}
### Build:    {build}
### OS:       {GetOSString()}
### Date:     {DateTime.Now:yyyy'-'MM'-'dd HH':'mm}
### LogLevel: {LogLevel}

";
            File.AppendAllText(LogFile, text, Encoding.UTF8);
        }

        private static string GetOSString()
        {
            OperatingSystem os = Environment.OSVersion;
            Version vs = os.Version;

            string operatingSystem = "";

            if (os.Platform == PlatformID.Win32Windows)
            {
                switch (vs.Minor)
                {
                    case 0:
                        operatingSystem = "95";
                        break;
                    case 10:
                        if (vs.Revision.ToString() == "2222A")
                            operatingSystem = "98SE";
                        else
                            operatingSystem = "98";
                        break;
                    case 90:
                        operatingSystem = "Me";
                        break;
                    default:
                        break;
                }
            }
            else if (os.Platform == PlatformID.Win32NT)
            {
                switch (vs.Major)
                {
                    case 3:
                        operatingSystem = "NT 3.51";
                        break;
                    case 4:
                        operatingSystem = "NT 4.0";
                        break;
                    case 5:
                        if (vs.Minor == 0)
                            operatingSystem = "2000";
                        else
                            operatingSystem = "XP";
                        break;
                    case 6:
                        if (vs.Minor == 0)
                            operatingSystem = "Vista";
                        else if (vs.Minor == 1)
                            operatingSystem = "7";
                        else if (vs.Minor == 2)
                            operatingSystem = "8";
                        else
                            operatingSystem = "8.1";
                        break;
                    case 10:
                        operatingSystem = "10";
                        break;
                    default:
                        break;
                }
            }
            if (os.Platform == PlatformID.Win32Windows || os.Platform == PlatformID.Win32NT)
            {
                if (operatingSystem != "")
                {
                    operatingSystem = "Windows " + operatingSystem;
                    if (os.ServicePack != "")
                    {
                        operatingSystem += " " + os.ServicePack;
                    }
                }
                return operatingSystem;
            }
            return $"{os.Platform} {os.Version}";
        }

        private static void Log(string prefix, string msg)
        {
            if (!InitDone)
                throw new InvalidOperationException("Init logger before use");
            var header = $"{DateTime.Now:MM'.'dd HH':'mm':'ss','fff} {prefix} ";
            var padding = new string(' ', header.Length);
            var lines = msg.Trim().Split('\n').Select(line => padding + line.Trim());
            var text = lines.Aggregate("", (l1, l2) => l1 + l2 + Environment.NewLine).TrimStart();
            lock (sync) File.AppendAllText(LogFile, header + text, Encoding.UTF8);
        }

        public static void Message(string msg, [CallerMemberName] string cmn = "", [CallerLineNumber] int line = -1, [CallerFilePath] string cfp = "")
        {
            if (LogLevel >= eLogLevel.NORMAL)
                Log(PREFIX_MESSAGE, CreateCaller(cmn, line, cfp) + " " + msg);
        }
        public static void Error(string msg, [CallerMemberName] string cmn = "", [CallerLineNumber] int line = -1, [CallerFilePath] string cfp = "")
        {
            if (LogLevel >= eLogLevel.ERROR)
                Log(PREFIX_ERROR, CreateCaller(cmn, line, cfp) + " " + msg);
        }

        public static void Warn(string msg, [CallerMemberName] string cmn = "", [CallerLineNumber] int line = -1, [CallerFilePath] string cfp = "")
        {
            if (LogLevel >= eLogLevel.WARN)
                Log(PREFIX_WARN, CreateCaller(cmn, line, cfp) + " " + msg);
        }

        public static void Debug(string msg, [CallerMemberName] string cmn = "", [CallerLineNumber] int line = -1, [CallerFilePath] string cfp = "")
        {
            if (LogLevel >= eLogLevel.DEBUG)
                Log(PREFIX_DEBUG, CreateCaller(cmn, line, cfp) + " " + msg);
        }

        public static void Verbose(string msg, [CallerMemberName] string cmn = "", [CallerLineNumber] int line = -1, [CallerFilePath] string cfp = "")
        {
            if (LogLevel >= eLogLevel.VERBOSE)
                Log(PREFIX_VERBOSE, CreateCaller(cmn, line, cfp) + " " + msg);
        }

        public static void LogException(Exception e, [CallerMemberName] string cmn = "", [CallerLineNumber] int line = -1, [CallerFilePath] string cfp = "")
        {
            Error("Exception: " + e.GetType().ToString(), cmn, line, cfp);
            Error("Message:" + e.Message, cmn, line, cfp);
            Error("Stacktrace:" + Environment.NewLine + e.StackTrace, cmn, line, cfp);
        }

        private static string CreateCaller(string cmn, int line, string cfp)
        {
            return $"[{cmn} in {Path.Combine(new DirectoryInfo(Path.GetDirectoryName(cfp)).Name, Path.GetFileName(cfp))} L{line}]";
        }
    }
}
