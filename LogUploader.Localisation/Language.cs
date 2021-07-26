using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;
using LogUploader;
using LogUploader.Tools.Logging;

namespace LogUploader.Localisation
{
    public static class Language
    {
        internal static readonly string LANG_XML_FOLDER_PATH = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        internal const string DATA_FOLDER = "LanguageData";
        internal const string BUILTIN_EN = "English (integrated)";
        internal const string BUILTIN_DE = "German (integrated)";
        internal const string BUILTIN_EN_FILE_NAME = "English";
        internal const string BUILTIN_DE_FILE_NAME = "German";

        public static eLanguage Current { get; private set; } = eLanguage.EN;
        public static string CurrentLanguage { get; private set; } = BUILTIN_EN;

        static Language() {
            //HACK not nice, prevents circel references
            LogUploader.ObjectName.GetCurrentLanguage = () => Current;
        }

        public static IReadOnlyList<string> GetAvailabeLanguages()
        {
            var available = Directory.GetFiles(LANG_XML_FOLDER_PATH + $@"\{DATA_FOLDER}\", "*.xml")
                .Select(path => Path.GetFileNameWithoutExtension(path))
                .OrderBy(fileName => fileName)
                .ToList();
            if (!available.Contains(BUILTIN_DE_FILE_NAME)) available.Insert(0, BUILTIN_DE);
            if (!available.Contains(BUILTIN_EN_FILE_NAME)) available.Insert(0, BUILTIN_EN);
            return available;
        }

        public static void SetLanguage(eLanguage language)
        {
            switch (language)
            {
                case eLanguage.EN:
                    SetLanguage(BUILTIN_EN_FILE_NAME);
                    break;
                case eLanguage.DE:
                    SetLanguage(BUILTIN_DE_FILE_NAME);
                    break;
                default:
                    throw new ArgumentException($"Invalid argument eLanguage.{language}. Only eLanguage.EN or eLanguage.DE are allowed.");
            }
        }

        public static void SetLanguage(string language)
        {
            if (!GetAvailabeLanguages().Contains(language))
            {
                Logger.Error($"Language: {language} does not exist! Defaulting to {BUILTIN_EN}");
                language = BUILTIN_EN;
            }

            Logger.Message("Loading language " + language);
            switch (language)
            {
                case BUILTIN_EN:
                    Data = new English();
                    break;
                case BUILTIN_DE:
                    Data = new German();
                    break;
                default:
                    var newData = LoadFromXML(language);
                    if (newData == null)
                    {
                        Logger.Warn("Failed to load xml file.");
                        return;
                    }
                    else
                        Data = newData;
                    if (Data.CultureName.StartsWith("de-"))
                        Current = eLanguage.DE;
                    else
                        Current = eLanguage.EN;
                    break;

            }
        }

        public static ILanguage Data { get; private set; } = new English();

        public static void ReloadFromXML()
        {
            SetLanguage(CurrentLanguage);
        }

        internal static XMLLanguage LoadFromXML(string file)
        {
            var xmlPath = LANG_XML_FOLDER_PATH + $@"\{DATA_FOLDER}\{file}.xml";
            Logger.Message("loading " + xmlPath);
            XMLLanguage data = new XMLLanguage();
            var ser = new System.Xml.Serialization.XmlSerializer(typeof(XMLLanguage));
            try
            {
                using (TextReader sr = new StreamReader(xmlPath, Encoding.UTF8))
                {
                    data = (XMLLanguage)ser.Deserialize(sr);
                    data.Culture = new CultureInfo(data.CultureName);
                }
            }
            catch (Exception e)
            {
                Logger.Error("Error on loading xml language file");
                Logger.LogException(e);
                return null;
            }
            return data;
        }

#if CREATE_LANGUAGE_XMLS
        public static void WriteOutLanguageXmls()
        {
            var ser = new XmlSerializer(typeof(XMLLanguage));
            using (var tw = new StringWriter())
            {
                var xmlLang = new XMLLanguage();
                Jitbit.Utils.PropMapper<ILanguage, XMLLanguage>.CopyTo(new English(), xmlLang);
                ser.Serialize(tw, xmlLang);
                var exePath = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
                File.WriteAllText(exePath + @"\LanguageData\English.xml", tw.ToString());
            }
            using (var tw = new StringWriter())
            {
                var xmlLang = new XMLLanguage();
                Jitbit.Utils.PropMapper<ILanguage, XMLLanguage>.CopyTo(new German(), xmlLang);
                ser.Serialize(tw, xmlLang);
                var exePath = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
                File.WriteAllText(exePath + @"\LanguageData\German.xml", tw.ToString());
            }
            var res = MessageBox.Show("Copy into project?", "Copy XMLs", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
            if (res == DialogResult.Yes)
            {
                var exePath = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
                File.Copy(exePath + @"\LanguageData\English.xml", exePath + @"\..\..\..\LogUploader.Localisation\LanguageData\English.xml", true);
                File.Copy(exePath + @"\LanguageData\German.xml", exePath + @"\..\..\..\LogUploader.Localisation\LanguageData\German.xml", true);
            }
        }
#endif
    }
}
