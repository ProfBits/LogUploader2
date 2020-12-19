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
        private static string LogFile;

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
            var text = $@"### LogUploader Log
### Version:  {version}
### Date:     {DateTime.Now:yyyy'-'MM'-'dd HH':'mm}
### LogLevel: {LogLevel}

";
            File.AppendAllText(LogFile, text, Encoding.UTF8);
        }

        private static void Log(string prefix, string msg)
        {
            if (!InitDone)
                throw new InvalidOperationException("Init logger before use");
            var header = $"{DateTime.Now.ToString("MM'.'dd HH':'mm':'ss','fff")} {prefix} ";
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
