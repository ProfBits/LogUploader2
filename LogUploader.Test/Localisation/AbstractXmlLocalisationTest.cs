
using LogUploader.Localisation;

namespace LogUploader.Test.Localisation
{
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
