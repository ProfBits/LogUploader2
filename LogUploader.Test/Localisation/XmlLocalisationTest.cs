using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using NUnit.Framework;

using LogUploader.Localisation;
using System.IO;

namespace LogUploader.Test.Localisation
{
    public class XmlLocalisationTest
    {
        [Test]
        public void AllXmlFilesHaveATest()
        {
            IEnumerable<string> xmlFilesCovered = null;
            Assert.Multiple(() => xmlFilesCovered = GetTestedXMLFilesFromCrrentDomain().Distinct());
            List<string> available = GetAvailableLanguageFilesFromDisk();

            foreach (var langFile in available)
            {
                CollectionAssert.Contains(xmlFilesCovered, langFile);
            }
        }

        private static List<string> GetAvailableLanguageFilesFromDisk()
        {
            return Directory.GetFiles(Language.LANG_XML_FOLDER_PATH + $@"\{Language.DATA_FOLDER}\", "*.xml")
                .Select(path => Path.GetFileNameWithoutExtension(path))
                .OrderBy(fileName => fileName)
                .ToList();
        }

        private static List<string> GetTestedXMLFilesFromCrrentDomain()
        {
            List<string> xmlFilesCovered = new List<string>();

            foreach (Assembly asm in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (Type type in GetValidSubTypes(asm))
                {
                    string file = GetCoveredFileNameFromType(asm, type);
                    xmlFilesCovered.Add(file);
                }
            }

            return xmlFilesCovered;
        }

        private static IEnumerable<Type> GetValidSubTypes(Assembly asm)
        {
            foreach (Type type in asm.GetTypes())
            {
                if (type.IsSubclassOf(typeof(AbstractXmlLocalisationTest)) && !type.IsAbstract)
                    yield return type;
            }
        }

        private static string GetCoveredFileNameFromType(Assembly asm, Type type)
        {
            string fileName = null;
            if (type.GetConstructor(Type.EmptyTypes) != null)
            {
                AbstractXmlLocalisationTest testClass = (AbstractXmlLocalisationTest)asm.CreateInstance(type.FullName);
                fileName = testClass.FileName;
            }
            else
                Assert.Fail($"Concrete subtype \"{type.FullName}\" of \"{typeof(AbstractXmlLocalisationTest).FullName}\" shold provide a default constructor");
            return fileName;
        }
    }
}
