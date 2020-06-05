using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using LogUploader;
using LogUploader.Helpers;

namespace LogUploader.Languages
{
    public static class Language
    {
        private static bool XMLMode = false;
        private static XMLLanguage English;
        private static XMLLanguage German;

        private static eLanguage m_Current = eLanguage.EN;
        public static eLanguage Current { get => m_Current; set {
                m_Current = value;
                switch (value)
                {
                    case eLanguage.DE:
                        if (XMLMode)
                            Data = German;
                        else
                            Data = new German();
                        break;
                    case eLanguage.EN:
                    default:
                        if (XMLMode)
                            Data = English;
                        else
                            Data = new English();
                        break;
                }
            }
        }
        public static BaseLanguage Data { get; private set; } = new English();

        public static void ReloadFromXML()
        {
            var exePath = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            var ser = new XmlSerializer(typeof(XMLLanguage));
            using (StringReader sr = new StringReader(File.ReadAllText(exePath + @"\Data\English.xml", Encoding.UTF8)))
            {
                English = (XMLLanguage)ser.Deserialize(sr);
                English.culture = new CultureInfo("en-us");
            }
            using (StringReader sr = new StringReader(File.ReadAllText(exePath + @"\Data\German.xml", Encoding.UTF8)))
            {
                German = (XMLLanguage)ser.Deserialize(sr);
                German.culture = new CultureInfo("de-de");
            }
            Current = m_Current;
        }

        public static void XMLModeEnable(bool enable)
        {
            if (enable)
            {
                ReloadFromXML();
                XMLMode = true;
            }
            else
            {
                XMLMode = false;
            }
            Current = m_Current;
        }
    }
}
