using System.Text;
using System.Threading.Tasks;

using NUnit.Framework;

using LogUploader.Localisation;

namespace LogUploader.Test.Localisation
{

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

}
