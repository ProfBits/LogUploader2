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

namespace LogUploader.Helper
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

        public static T IntToEnum<T>(int id) where T : struct
        {
            T e = (T)(object)id;
            if (Enum.GetNames(typeof(T)).Contains(Enum.GetName(typeof(T), e)))
                return e;
            return (T)(object)0;
        }

        public static Data.RaidOrgaPlus.Role getRoleById(int id)
        {
            if (Enum.IsDefined(typeof(Data.RaidOrgaPlus.Role), (byte)id))
                return (Data.RaidOrgaPlus.Role)(byte)(id);
            return Data.RaidOrgaPlus.Role.Empty;
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
            if (string.IsNullOrEmpty(encryptedStr))
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
            if (string.IsNullOrEmpty(encryptedStr))
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
            if (string.IsNullOrEmpty(encryptedStr))
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

        /// <summary>
        /// Writes the settings to the given file. Overrides a existing file
        /// The settings are encryted if a non null or empty password string is prvided.
        /// </summary>
        /// <param name="settings">the Settings to export</param>
        /// <param name="path">the file to export to</param>
        /// <param name="password">the password to protect the file</param>
        /// <exception cref="IOException"></exception>
        /// <exception cref="UnauthorizedAccessException"></exception>
        /// <exception cref="FileNotFoundException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="PathTooLongException"></exception>
        internal static void ExportSettings(Data.Settings.SettingsData settings, string path, string password = "")
        {
            if (settings == null) new ArgumentNullException($"SettingsData settings was null.");
            if (path == null) new ArgumentNullException($"string path was null.");
            if (password == null) new ArgumentNullException($"string password was null.");

            string data = settings.ToString();
            byte[] dataBytes;
            if (!string.IsNullOrEmpty(password))
            {
                var key = GetKeyAndIv(password);
                dataBytes = EncryptStringToBytes_Aes(data, key[0], key[1]);
            }
            else
            {
                dataBytes = Encoding.UTF8.GetBytes(data);
            }
            Directory.CreateDirectory(Path.GetDirectoryName(path));
            using (var fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None))
            {
                if (string.IsNullOrEmpty(password))
                    fs.WriteByte(0x44);
                else
                    fs.WriteByte(0x50);
                fs.WriteByte(0x0A);
                fs.WriteByte(0x0D);
                fs.Write(dataBytes, 0, dataBytes.Length);
            }
        }

        private static byte[][] GetKeyAndIv(string password)
        {
            byte[][] res = new byte[2][];

            using (SHA256 mySHA256 = SHA256.Create())
            {
                res[0] = Encoding.UTF8.GetBytes(password);
                for (int i = 0; i < 1011; i++)
                {
                    res[0] = mySHA256.ComputeHash(res[0]);
                }
                var tmp = mySHA256.ComputeHash(res[0]);
                for (int i = 0; i < tmp.Length - 1; i += 2)
                {
                    tmp[i / 2] = (byte) (tmp[i] ^ tmp[i + 1]);
                }
                res[1] = new byte[16];
                Array.Copy(tmp, res[1], 16);
            }

            return res;
        }

        private static byte[] EncryptStringToBytes_Aes(string plainText, byte[] Key, byte[] IV)
        {

            // Check arguments.
            if (plainText == null || plainText.Length <= 0)
                throw new ArgumentNullException("plainText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("IV");
            byte[] encrypted;

            // Create an Aes object
            // with the specified key and IV.
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                // Create an encryptor to perform the stream transform.
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for encryption.
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            //Write all data to the stream.
                            swEncrypt.Write(plainText);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }

            // Return the encrypted bytes from the memory stream.
            return encrypted;
        }

        /// <summary>
        /// Reads the settings form the given file and saved them to the current settings
        /// </summary>
        /// <param name="settings">the current setttings</param>
        /// <param name="path">the path to the file to import</param>
        /// <param name="password">the password that protects the file</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="InvalidDataException">If the provided file has invalid headers</exception>
        /// <exception cref="InvalidOperationException">If password required, hoverve none was given</exception>
        /// <exception cref="CryptographicException">If invalid password was given</exception>
        /// <exception cref="IOException"></exception>
        /// <exception cref="UnauthorizedAccessException"></exception>
        /// <exception cref="FileNotFoundException"></exception>
        internal static void ImportSettings(Data.Settings.SettingsData settings, string path, string password = "")
        {
            if (settings == null) new ArgumentNullException($"SettingsData settings was null.");
            if (path == null) new ArgumentNullException($"string path was null.");
            if (password == null) new ArgumentNullException($"string password was null.");
            if (!File.Exists(path))
                throw new FileNotFoundException($"File \"{path}\" does not exist.");

            var file = File.ReadAllBytes(path);
            byte[] dataBytes;
            if ((file[0] == 0x44 || file[0] == 0x50) && file[0] == 0x0A && file[0] == 0x0D)
            {
                throw new InvalidDataException("Invalid SettingsExport file.");
            }

            dataBytes = new byte[file.Length - 3];
            Array.Copy(file, 3, dataBytes, 0, dataBytes.Length);

            string data;
            if (file[0] == 0x50)
            {
                if (string.IsNullOrEmpty(password))
                    throw new InvalidOperationException("Password required.");
                var key = GetKeyAndIv(password);
                data = DecryptStringFromBytes_Aes(dataBytes, key[0], key[1]);
            }
            else
            {
                data = Encoding.UTF8.GetString(dataBytes);
            }

            settings.ApplyJson(data);
            var s = new Settings();
            settings.ApplyTo(s);
            s.Save();
        }


        static string DecryptStringFromBytes_Aes(byte[] cipherText, byte[] Key, byte[] IV)
        {
            // Check arguments.
            if (cipherText == null || cipherText.Length <= 0)
                throw new ArgumentNullException("cipherText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("IV");

            // Declare the string used to hold
            // the decrypted text.
            string plaintext = null;

            // Create an Aes object
            // with the specified key and IV.
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                // Create a decryptor to perform the stream transform.
                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for decryption.
                using (MemoryStream msDecrypt = new MemoryStream(cipherText))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {

                            // Read the decrypted bytes from the decrypting stream
                            // and place them in a string.
                            try
                            {
                                plaintext = srDecrypt.ReadToEnd();
                            }
                            catch (CryptographicException)
                            {
                                throw new CryptographicException("Invalid Password");
                            }
                        }
                    }
                }
            }

            return plaintext;
        }
    }

    public static class ZipHelper
    {

        private static void CopyTo(Stream src, Stream dest)
        {
            byte[] bytes = new byte[4096];

            int cnt;

            while ((cnt = src.Read(bytes, 0, bytes.Length)) != 0)
            {
                dest.Write(bytes, 0, cnt);
            }
        }

        public static byte[] Zip(string str)
        {
            var bytes = Encoding.UTF8.GetBytes(str);

            using (var msi = new MemoryStream(bytes))
            using (var mso = new MemoryStream())
            {
                using (var gs = new System.IO.Compression.GZipStream(mso, System.IO.Compression.CompressionMode.Compress))
                {
                    //msi.CopyTo(gs);
                    CopyTo(msi, gs);
                }

                return mso.ToArray();
            }
        }

        public static string Unzip(byte[] bytes)
        {
            using (var msi = new MemoryStream(bytes))
            using (var mso = new MemoryStream())
            {
                using (var gs = new System.IO.Compression.GZipStream(msi, System.IO.Compression.CompressionMode.Decompress))
                {
                    //gs.CopyTo(mso);
                    CopyTo(gs, mso);
                }

                return Encoding.UTF8.GetString(mso.ToArray());
            }
        }
    }
}
