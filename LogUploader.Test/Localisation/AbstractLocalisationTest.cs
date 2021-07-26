using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

using NUnit.Framework;

using LogUploader.Localisation;
using System.IO;

namespace LogUploader.Test.Localisation
{
    public class LocalisationTest
    {

    }

    public abstract class AbstractLocalisationTest
    {
        internal abstract ILanguage GetLanguage();

        [Test]
        public void CultureInfoSetTest()
        {
            Assert.IsNotNull(GetLanguage().Culture);
        }

        [Test]
        public void StringsAreNonWhitespaceOrNullTest()
        {
            ILanguage lang = GetLanguage();
            var members = lang.GetType().GetProperties();
            foreach (var member in members)
            {
                if (member.CanRead && member.PropertyType == typeof(string))
                {
                    Assert.False(string.IsNullOrWhiteSpace((string)member.GetValue(lang, new object[] { })),
                        $"Property \"{member.Name}\" of \"{lang.GetType().FullName}\" should not be null or whitespace");
                }
            }
        }
    }

    public class EnglischDefaultLocalisationTest : AbstractLocalisationTest
    {
        internal override ILanguage GetLanguage()
        {
            return new English();
        }
    }

    public class GermanDefaultLocalisationTest : AbstractLocalisationTest
    {
        internal override ILanguage GetLanguage()
        {
            return new German();
        }
    }

    public class XmlLocalisationTest
    {
        [Test]
        public void AllXmlFilesHaveATest()
        {
            IEnumerable<string> xmlFilesCovered = null;
            Assert.Multiple(() => xmlFilesCovered = GetTestedXMLFilesFromCrrentDomain().Distinct());

            var available = Directory.GetFiles(Language.LANG_XML_FOLDER_PATH + $@"\{Language.DATA_FOLDER}\", "*.xml")
                .Select(path => Path.GetFileNameWithoutExtension(path))
                .OrderBy(fileName => fileName)
                .ToList();

            foreach (var langFile in available)
            {
                CollectionAssert.Contains(xmlFilesCovered, langFile);
            }
        }

        private static List<string> GetTestedXMLFilesFromCrrentDomain()
        {
            List<string> xmlFilesCovered = new List<string>();

            foreach (Assembly asm in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (Type type in asm.GetTypes())
                {
                    if (type.IsSubclassOf(typeof(AbstractXmlLocalisationTest)) && !type.IsAbstract)
                    {
                        if (type.GetConstructor(Type.EmptyTypes) != null)
                        {
                            AbstractXmlLocalisationTest testClass = (AbstractXmlLocalisationTest)asm.CreateInstance(type.FullName);
                            xmlFilesCovered.Add(testClass.FileName);
                        }
                        else
                            Assert.Fail($"Concrete subtype \"{type.FullName}\" of \"{typeof(AbstractXmlLocalisationTest).FullName}\" shold provide a default constructor");
                    }
                }
            }

            return xmlFilesCovered;
        }
    }

    public abstract class AbstractXmlLocalisationTest : AbstractLocalisationTest
    {
        internal abstract string FileName { get; }

        internal override ILanguage GetLanguage()
        {
            return Language.LoadFromXML(FileName);
        }
    }

    public class EnglishXmlLocalisationTest : AbstractXmlLocalisationTest
    {
        internal override string FileName { get => "English"; }
    }

    public class GermanXmlLocalisationTest : AbstractXmlLocalisationTest
    {
        internal override string FileName { get => "German"; }
    }
}
