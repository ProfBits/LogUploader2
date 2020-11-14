using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace LogUploader.Helper
{
    internal static class Logger
    {
        private static bool InitDone = false;
        private const string PREFIX_MESSAGE = "[Message]";
        private const string PREFIX_ERROR = "[ERROR]";
        private const string PREFIX_WARN = "[WARN]";
        private const string PREFIX_DEBUG = "[Debug]";
        private const string PREFIX_VERBOSE = "[Verbose]";

        private const string LOG_DIR = "\\LogUploader\\Logs\\";
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
            LogFile = logPath + DateTime.Now.ToString("yyMMdd_hhmmssfff") + ".log";
            File.Create(LogFile).Close();
            WriteHeader(version);
            InitDone = true;
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

        public static void Message(string msg)
        {
            if (LogLevel >= eLogLevel.NORMAL)
                Log(PREFIX_MESSAGE, msg);
        }
        public static void Error(string msg)
        {
            if (LogLevel >= eLogLevel.ERROR)
                Log(PREFIX_ERROR, msg);
        }

        public static void Warn(string msg)
        {
            if (LogLevel >= eLogLevel.WARN)
                Log(PREFIX_WARN, msg);
        }

        public static void Debug(string msg)
        {
            if (LogLevel >= eLogLevel.DEBUG)
                Log(PREFIX_DEBUG, msg);
        }

        public static void Verbose(string msg)
        {
            if (LogLevel >= eLogLevel.VERBOSE)
                Log(PREFIX_VERBOSE, msg);
        }

        public static void LogException(Exception e)
        {
            Error(e.GetType().ToString());
            Error("Message:" + Environment.NewLine + e.Message);
            Error("Stacktrace:" + Environment.NewLine + e.StackTrace);
        }
    }
}
