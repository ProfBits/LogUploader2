﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using LogUploader;
using LogUploader.Helper;

namespace LogUploader.Languages
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
            using (StringReader sr = new StringReader(ReadFile(exePath + @"\Data\English.xml")))
            {
                English = (XMLLanguage)ser.Deserialize(sr);
                English.Culture = new CultureInfo("en-us");
            }
            using (StringReader sr = new StringReader(ReadFile(exePath + @"\Data\German.xml")))
            {
                German = (XMLLanguage)ser.Deserialize(sr);
                German.Culture = new CultureInfo("de-de");
            }
            Current = m_Current;
        }

        private static string ReadFile(string filePath)
        {
            try
            {
                return File.ReadAllText(filePath, Encoding.UTF8);
            }
            catch (Exception e)
            {
                Logger.Error($@"Unable to read localisation file ""{filePath}"".");
                Logger.LogException(e);
                throw e;
            }
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
