using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LogUploader.Helper;

using Newtonsoft.Json.Linq;
using NUnit.Framework;

namespace LogUploaderTests
{
    public class SkippingJsonReaderTest
    {
        [Test]
        public void SkippingJsonReaderIntegrationTest()
        {
            var jsonStr = @"
{
    ""root"": {
        ""keepMe"": {
            ""id"": 1
        },
        ""toSkip"": {
            ""id"": 1,
            ""data"": ""Hello World""
        },
        ""keepMeAlso"": {
            ""id"": 2
        }
    }
}
";

            var parsed = JObject.Load(new SkippingJsonReader(jsonStr, new HashSet<string>() { "toSkip" }));
            Assert.Multiple(() =>
            {
                Assert.That(parsed["root"]["keepMe"], Is.Not.Null);
                Assert.That(parsed["root"]["keepMeAlso"], Is.Not.Null);
                Assert.That(parsed["root"].Children<JProperty>().Select(jp => jp.Name), Does.Not.Contain("toSkip"));
            });
        }
    }
}
