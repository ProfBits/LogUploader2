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

namespace LogUploader.Localisation
{
    public static class Language
    {
        private static bool XMLMode = false;
        private static XMLLanguage English;
        private static XMLLanguage German;

        //TODO improve performance, only load one languate at a time
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
        public static ILanguage Data { get; private set; } = new English();

        public static void ReloadFromXML()
        {
            var exePath = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            //Exception "System.IO.FileNotFoundException: 'Die Datei oder Assembly "LogUploader.XmlSerializers, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null" oder eine Abhängigkeit davon wurde nicht gefunden. Das System kann die angegebene Datei nicht finden.'"
            //Will not be fixed by microsoft. cant do anything about it.
            var ser = new System.Xml.Serialization.XmlSerializer(typeof(XMLLanguage));
            using (StringReader sr = new StringReader(File.ReadAllText(exePath + @"\LanguageData\English.xml", Encoding.UTF8)))
            {
                English = (XMLLanguage)ser.Deserialize(sr);
                English.Culture = new CultureInfo("en-us");
            }
            using (StringReader sr = new StringReader(File.ReadAllText(exePath + @"\LanguageData\German.xml", Encoding.UTF8)))
            {
                German = (XMLLanguage)ser.Deserialize(sr);
                German.Culture = new CultureInfo("de-de");
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
