using Extensiones;
using LogUploader.Data;
using LogUploader.Languages;
using LogUploader.Properties;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace LogUploader.Helpers
{
    /// <summary>
    /// A general purpose helper for various tasks
    /// </summary>
    public static class GP
    {
        public static string GetName(int id)
        {
            return GetEnemyByID(id)?.Name;
        }

        public static bool IsInteresting(int id)
        {
            var enemy = GetEnemyByID(id);
            if (enemy is Boss)
                return true;
            if (enemy is AddEnemy add)
                return add.IsInteresting;
            return true;
        }

        public static Enemy GetEnemyByID(int id)
        {
            if (Boss.ExistsID(id))
                return Boss.getByID(id);
            if (AddEnemy.ExistsID(id))
                return AddEnemy.getByID(id);
            return null;
        }

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

        public static bool Compare<T>(T obj1, T obj2)
        {
            if ((object)obj1 == null)
                return (object)obj2 == null;

            return obj1.Equals(obj2);
        }

        public static string ReadJsonFile(string path)
        {
            return File.ReadAllText(path, Encoding.GetEncoding("iso-8859-1"));
        }
        public static void WriteJsonFile(string path, string text)
        {
            File.WriteAllText(path, text, Encoding.GetEncoding("iso-8859-1"));
        }

        public static Version GetVersion()
        {
            var fi = FileVersionInfo.GetVersionInfo(System.Reflection.Assembly.GetEntryAssembly().Location);
            return new Version(fi.ProductMajorPart, fi.ProductMinorPart, fi.ProductBuildPart, fi.ProductPrivatePart);
        }

    }

    public static class SettingsHelper
    {
        private const eLanguage DEFAULT_LANGUAGE = eLanguage.EN;
        private const eDiscordPostFormat DEFAULT_DISCORD_POST_FORMAT = eDiscordPostFormat.PerArea;

        [Obsolete]
        internal static string GetUserToken(Settings settings)
        {
            var encryptedStr = settings.UserToken;
            if (encryptedStr == "")
                return "";
            var encryptedBytes = encryptedStr.Split('|').Select(e => byte.Parse(e)).ToArray();
            var tokenBytes = ProtectedData.Unprotect(encryptedBytes, new byte[] { 15, 7, 20, 19 }, DataProtectionScope.CurrentUser);
            var token = Encoding.UTF8.GetString(tokenBytes);
            return token;
        }

        [Obsolete]
        internal static void StoreUserToken(Settings settings, string token)
        {
            var tokenBytes = Encoding.UTF8.GetBytes(token);
            var encryptedBytes = ProtectedData.Protect(tokenBytes, new byte[] { 15, 7, 20, 19 }, DataProtectionScope.CurrentUser);
            var encryptedString = "";
            encryptedBytes.ToList().ForEach(b => encryptedString += b.ToString() + "|");
            settings.UserToken = encryptedString.TrimEnd('|');
        }

        [Obsolete]
        internal static string GetProxyPassword(Settings settings)
        {
            var encryptedStr = settings.ProxyPassword;
            if (encryptedStr == "")
                return "";
            var encryptedBytes = encryptedStr.Split('|').Select(e => byte.Parse(e)).ToArray();
            var tokenBytes = ProtectedData.Unprotect(encryptedBytes, new byte[] { 74, 93, 110, 89, 123 }, DataProtectionScope.CurrentUser);
            var token = Encoding.UTF8.GetString(tokenBytes);
            return token;
        }

        [Obsolete]
        internal static void StoreProxyPassword(Settings settings, string password)
        {
            var tokenBytes = Encoding.UTF8.GetBytes(password);
            var encryptedBytes = ProtectedData.Protect(tokenBytes, new byte[] { 74, 93, 110, 89, 123 }, DataProtectionScope.CurrentUser);
            var encryptedString = "";
            encryptedBytes.ToList().ForEach(b => encryptedString += b.ToString() + "|");
            settings.ProxyPassword = encryptedString.TrimEnd('|');
        }

        [Obsolete]
        internal static string GetDiscordWebHookLink(Settings settings)
        {
            var encryptedStr = settings.DiscordWebHookLink;
            if (encryptedStr == "")
                return "";
            var encryptedBytes = encryptedStr.Split('|').Select(e => byte.Parse(e)).ToArray();
            var tokenBytes = ProtectedData.Unprotect(encryptedBytes, new byte[] { 61, 71, 111, 90, 117 }, DataProtectionScope.CurrentUser);
            var token = Encoding.UTF8.GetString(tokenBytes);
            return token;
        }

        [Obsolete]
        internal static void StoreDiscordWebHookLink(Settings settings, string link)
        {
            var tokenBytes = Encoding.UTF8.GetBytes(link);
            var encryptedBytes = ProtectedData.Protect(tokenBytes, new byte[] { 61, 71, 111, 90, 117 }, DataProtectionScope.CurrentUser);
            var encryptedString = "";
            encryptedBytes.ToList().ForEach(b => encryptedString += b.ToString() + "|");
            settings.DiscordWebHookLink = encryptedString.TrimEnd('|');
        }

        [Obsolete]
        internal static eLanguage GetLanguage(Settings settings)
        {
            try
            {
                return (eLanguage)Enum.Parse(typeof(eLanguage), settings.Language);
            }
            catch (ArgumentNullException)
            {
                StoreLanguage(settings, DEFAULT_LANGUAGE);
                settings.Save();
                return DEFAULT_LANGUAGE;
            }
        }

        [Obsolete]
        internal static void StoreLanguage(Settings settings, eLanguage language)
        {
            settings.Language = language.ToString();
        }

        [Obsolete]
        internal static eDiscordPostFormat GetDiscordPostFormat(Settings settings)
        {
            try
            {
                return (eDiscordPostFormat)Enum.Parse(typeof(eDiscordPostFormat), settings.DiscordPostFormat);
            }
            catch (ArgumentNullException)
            {
                StoreDiscordPostFormat(settings, DEFAULT_DISCORD_POST_FORMAT);
                settings.Save();
                return DEFAULT_DISCORD_POST_FORMAT;
            }
        }

        [Obsolete]
        internal static void StoreDiscordPostFormat(Settings settings, eDiscordPostFormat format)
        {
            settings.DiscordPostFormat = format.ToString();
        }

        internal static string UnprotectString(string s)
        {
            try
            {
                return Encoding.GetEncoding("iso-8859-1").GetString(ProtectedData.Unprotect(Convert.FromBase64String(s), new byte[] { 60, 72, 128, 65, 117 }, DataProtectionScope.CurrentUser));
            }
            catch (NotSupportedException)
            {
                return s;
            }
            catch (Exception e) when (e is FormatException || e is CryptographicException || e is DecoderFallbackException)
            {
                return "";
            }
        }
        internal static string ProtectString(string s)
        {
            try
            {
                return Convert.ToBase64String(ProtectedData.Protect(Encoding.GetEncoding("iso-8859-1").GetBytes(s), new byte[] { 60, 72, 128, 65, 117 }, DataProtectionScope.CurrentUser));
            }
            catch (NotSupportedException)
            {
                return s;
            }
            catch (Exception e) when (e is FormatException || e is CryptographicException || e is DecoderFallbackException)
            {
                return "";
            }
        }
    }
}
