using Extensiones;

using LogUploader.Data;
using LogUploader.Tools.Logging;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LogUploader.Tools
{
    /// <summary>
    /// A general purpose helper for various tasks
    /// </summary>
    public static class GP
    {

        public static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dateTime = dateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dateTime;
        }

        public static string GetLocalOffset()
        {
            var offest = Math.Round(TimeZone.CurrentTimeZone.GetUtcOffset(DateTime.Now.ToUniversalTime()).TotalHours, 1);
            var Zone = TimeZone.CurrentTimeZone.StandardName;
            return Zone + " " + offest;
        }
        
        public static string GetDateTimeString(DateTime dateTime)
        {
            string date = $"{dateTime.Year.ToString().PadLeft(4, '0')}-{dateTime.Month.ToString().PadLeft(2, '0')}-{dateTime.Day.ToString().PadLeft(2, '0')}";
            string time = $"{dateTime.Hour.ToString().PadLeft(2, '0')}:{dateTime.Minute.ToString().PadLeft(2, '0')}:{dateTime.Second.ToString().PadLeft(2, '0')}";
            return $"{date} {time}";
        }

        public static Version GetVersion()
        {
            var fi = FileVersionInfo.GetVersionInfo(System.Reflection.Assembly.GetEntryAssembly().Location);
            return new Version(fi.ProductMajorPart, fi.ProductMinorPart, fi.ProductBuildPart, fi.ProductPrivatePart);
        }

        public static T IntToEnum<T>(int id) where T : struct
        {
            T e = (T)(object)id;
            if (Enum.GetNames(typeof(T)).Contains(Enum.GetName(typeof(T), e)))
                return e;
            return (T)(object)0;
        }

        public static void Exit(ExitCode exitCode)
        {
            Logger.Error($"Programm Exit Reason: {(int)exitCode} {exitCode}");
            Environment.Exit((int)exitCode);
        }

        private static readonly Regex DiscordEmoteRegEx = new Regex("^((:\\w+:)|(<:\\w+:\\d{18}>))$", RegexOptions.Compiled | RegexOptions.CultureInvariant);
        
        /// <summary>
        /// Throws a Argumgent exception if the string does not represent a valid discord emote
        /// </summary>
        /// <param name="emote"></param>
        public static string ValidateDiscordEmote(string emote)
        {
            if (emote is null) throw new ArgumentNullException(nameof(emote), "A Discord emote can not be null");
            if (string.IsNullOrWhiteSpace(emote)) throw new ArgumentOutOfRangeException(nameof(emote), emote, "A Discord emote can not be empty or white space");
            if (emote != emote.Trim()) throw new ArgumentOutOfRangeException(nameof(emote), emote, "A Discord emote can not have white space at the beginning or end");
            if (!DiscordEmoteRegEx.IsMatch(emote)) throw new ArgumentOutOfRangeException(nameof(emote), emote, "The string does not represent a valid discord emote");
            return emote;
        }

        public static string ValidateStringMultiWord(string str)
        {
            if (str is null) throw new ArgumentNullException(nameof(str), "A string can not be null");
            if (string.IsNullOrWhiteSpace(str)) throw new ArgumentOutOfRangeException(nameof(str), str, "A string can not be empty or white space");
            if (str != str.Trim()) throw new ArgumentOutOfRangeException(nameof(str), str, "A string can not have white space at the beginning or end");
            return str;
        }

        public static string ValidateStringOneWord(string str)
        {
            if (str is null) throw new ArgumentNullException(nameof(str), "The string can not be null");
            if (string.IsNullOrWhiteSpace(str)) throw new ArgumentOutOfRangeException(nameof(str), str, "The string can not be empty or white space");
            if (str != str.Trim()) throw new ArgumentOutOfRangeException(nameof(str), str, "The string can not have white space at the beginning or end");
            if (str.Trim().Split(null).Length != 1) throw new ArgumentOutOfRangeException(nameof(str), str, "The string can not have multible section seperated by white space");
            return str;
        }
    }
}
