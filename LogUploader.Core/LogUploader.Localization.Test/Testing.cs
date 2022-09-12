using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogUploader.Localization.Test
{
    internal class Testing
    {
        [Test]
        public void Test()
        {
            var actual = new LanuguageSpec();
            actual.Spec = new Dictionary<Strings, string>() {
                { Strings.Ok, "Ok" },
                { Strings.Cancel, "Cancel" },
                { Strings.MainWindow_Title, "Spec" },
            };

            var serialice = System.Text.Json.JsonSerializer.Serialize(actual, actual.GetType(), (System.Text.Json.JsonSerializerOptions)null);
            
            var output = (LanuguageSpec?)System.Text.Json.JsonSerializer.Deserialize(serialice, typeof(LanuguageSpec), (System.Text.Json.JsonSerializerOptions)null);

            Assert.That(output, Is.Not.Null);
            Assert.That(output, Is.EqualTo(actual));
        }
    }

    internal class LanuguageSpec
    {
        public IDictionary<Strings, string> Spec { get; set; }
    }

    internal enum Strings
    {
        Ok,
        Cancel,
        MainWindow_Title

    }
}
