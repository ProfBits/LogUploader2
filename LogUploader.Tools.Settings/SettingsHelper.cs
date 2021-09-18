using LogUploader.Localisation;
using LogUploader.Tools.Discord;

using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;


namespace LogUploader.Tools.Settings
{
    public static class SettingsHelper
    {
        private const eLanguage DEFAULT_LANGUAGE = eLanguage.EN;
        private const eDiscordPostFormat DEFAULT_DISCORD_POST_FORMAT = eDiscordPostFormat.PerArea;

        [Obsolete]
        internal static string GetUserToken(Properties.Settings settings)
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
        internal static void StoreUserToken(Properties.Settings settings, string token)
        {
            var tokenBytes = Encoding.UTF8.GetBytes(token);
            var encryptedBytes = ProtectedData.Protect(tokenBytes, new byte[] { 15, 7, 20, 19 }, DataProtectionScope.CurrentUser);
            var encryptedString = "";
            encryptedBytes.ToList().ForEach(b => encryptedString += b.ToString() + "|");
            settings.UserToken = encryptedString.TrimEnd('|');
        }

        [Obsolete]
        internal static string GetProxyPassword(Properties.Settings settings)
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
        internal static void StoreProxyPassword(Properties.Settings settings, string password)
        {
            var tokenBytes = Encoding.UTF8.GetBytes(password);
            var encryptedBytes = ProtectedData.Protect(tokenBytes, new byte[] { 74, 93, 110, 89, 123 }, DataProtectionScope.CurrentUser);
            var encryptedString = "";
            encryptedBytes.ToList().ForEach(b => encryptedString += b.ToString() + "|");
            settings.ProxyPassword = encryptedString.TrimEnd('|');
        }

        [Obsolete]
        internal static string GetDiscordWebHookLink(Properties.Settings settings)
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
        internal static void StoreDiscordWebHookLink(Properties.Settings settings, string link)
        {
            var tokenBytes = Encoding.UTF8.GetBytes(link);
            var encryptedBytes = ProtectedData.Protect(tokenBytes, new byte[] { 61, 71, 111, 90, 117 }, DataProtectionScope.CurrentUser);
            var encryptedString = "";
            encryptedBytes.ToList().ForEach(b => encryptedString += b.ToString() + "|");
            settings.DiscordWebHookLink = encryptedString.TrimEnd('|');
        }

        [Obsolete]
        internal static eDiscordPostFormat GetDiscordPostFormat(Properties.Settings settings)
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
        internal static void StoreDiscordPostFormat(Properties.Settings settings, eDiscordPostFormat format)
        {
            settings.DiscordPostFormat = format.ToString();
        }

        public static string UnprotectString(string s)
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
        public static string ProtectString(string s)
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
        public static void ExportSettings(SettingsData settings, string path, string password = "")
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
        public static void ImportSettings(SettingsData settings, string path, string password = "")
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

            settings.Save();
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
}
