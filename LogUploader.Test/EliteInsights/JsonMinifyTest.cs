
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using NUnit.Framework;
using LogUploader.Test.Constraints;
using Newtonsoft.Json.Linq;

using LogUploader.Tools.EliteInsights.MinifyJson;
using Throws = LogUploader.Test.Constraints.Throws;

namespace LogUploader.Test.EliteInsights
{
    public class JsonMinifyTest
    {
        [Test]
        public void RemoveTokensDoesNotAllowNull()
        {
            Assert.That(() => JsonMinifier.RemoveTokens(null, new HashSet<string>()), Throws.ValidateArgumentNullException);
            Assert.That(() => JsonMinifier.RemoveTokens(JToken.Parse("{}"), null), Throws.ValidateArgumentNullException);
            Assert.That(() => JsonMinifier.RemoveTokens(null, null), Throws.ValidateArgumentNullException);
        }

        [Test]
        public void RemoveTokensDoesDoNoting()
        {
            string json = @"{""list"":[[{""int"":1,""prop"":1}],{""prop"":1}]}";
            JToken parsed = JToken.Parse(json);
            HashSet<string> toRemove = new HashSet<string> {  };

            Assert.That(() => JsonMinifier.RemoveTokens(parsed, toRemove), Throws.Nothing);

            Assert.That(parsed.ToString(Newtonsoft.Json.Formatting.None), Is.EqualTo(@"{""list"":[[{""int"":1,""prop"":1}],{""prop"":1}]}"));
        }

        [Test]
        public void ReduceJsonFinale()
        {
        }

        [Test]
        public void RemoveTokensTopLevelTest()
        {
            string json = @"{""propBeginn"":1,""obj"":2,""propMiddle"":2,""obj1"":2,""propEnd"":3}";
            JToken parsed = JToken.Parse(json);
            HashSet<string> toRemove = new HashSet<string> { "propBeginn", "propMiddle", "propEnd" };


            JsonMinifier.RemoveTokens(parsed, toRemove);

            var reminingTokens = parsed.Children<JProperty>().Select(jp => jp.Name).ToList();
            Assert.That(reminingTokens, Does.Contain("obj"));
            Assert.That(reminingTokens, Does.Contain("obj1"));
            foreach (var element in toRemove)
            {
                Assert.That(reminingTokens, Does.Not.Contain(element));
            }
            Assert.That(reminingTokens, Has.Count.EqualTo(2));
        }

        [Test]
        public void RemoveTokensInListsTest()
        {
            string json = @"{""list"":[{""prop"":1},{""prop"":2}],""list2"":[{""prop"":1,""keep"":1}]}";
            JToken parsed = JToken.Parse(json);
            HashSet<string> toRemove = new HashSet<string> { "prop" };
            
            JsonMinifier.RemoveTokens(parsed, toRemove);

            var reminingTokens = parsed.Children<JProperty>().Select(jp => jp.Name).ToList();
            Assert.That(reminingTokens, Does.Contain("list"));
            Assert.That(reminingTokens, Does.Contain("list2"));
            Assert.That(reminingTokens.Count(), Is.EqualTo(2));
            Assert.That(((JArray)parsed["list"]).Select(e => e.ToString()), Has.All.EqualTo("{}"));
            Assert.That((JArray)parsed["list2"], Has.Count.EqualTo(1));
            reminingTokens = parsed["list2"][0].Children<JProperty>().Select(jp => jp.Name).ToList();
            Assert.That(reminingTokens, Does.Contain("keep"));
            Assert.That(reminingTokens, Has.Count.EqualTo(1));
        }

        [Test]
        public void RemoveTokensRecursiveTest()
        {
            string json = @"{""obj"":{""obj"":{""int"":1,""prop"":1},""prop"":1},""prop"":1}";
            JToken parsed = JToken.Parse(json);
            HashSet<string> toRemove = new HashSet<string> { "prop" };
            
            JsonMinifier.RemoveTokens(parsed, toRemove);

            Assert.That(parsed.ToString(Newtonsoft.Json.Formatting.None), Is.EqualTo(@"{""obj"":{""obj"":{""int"":1}}}"));
        }

        [Test]
        public void RemoveTokensRecursiveListTest()
        {
            string json = @"{""list"":[[{""int"":1,""prop"":1}],{""prop"":1}]}";
            JToken parsed = JToken.Parse(json);
            HashSet<string> toRemove = new HashSet<string> { "prop" };
            
            JsonMinifier.RemoveTokens(parsed, toRemove);

            Assert.That(parsed.ToString(Newtonsoft.Json.Formatting.None), Is.EqualTo(@"{""list"":[[{""int"":1}],{}]}"));
        }

        [Test]
        public void RemoveTokensRecursiveListAndObjTest()
        {
            string json = @"{""obj"":{""list"":[[{""obj"":{""int"":1,""prop"":1},""prop"":1}],{""prop"":1}],""prop"":1}}";
            JToken parsed = JToken.Parse(json);
            HashSet<string> toRemove = new HashSet<string> { "prop" };
            
            JsonMinifier.RemoveTokens(parsed, toRemove);

            Assert.That(parsed.ToString(Newtonsoft.Json.Formatting.None), Is.EqualTo(@"{""obj"":{""list"":[[{""obj"":{""int"":1}}],{}]}}"));
        }
    }

    public static class JsonFragmentGenerator
    {
        public static IEnumerable<string> GenreateIntegerElement(int i = 0)
        {
            while (true)
            {
                yield return $"\"i{i}\":{i}";
                i++;
            }
        }

        public static IEnumerable<string> GenreateStringElement(int i = 0)
        {
            while (true)
            {
                yield return $"\"s{i}\":\"str{i}\"";
                i++;
            }
        }

        public static IEnumerable<string> GenreateBoolElement(int i = 0, bool b = false)
        {
            while (true)
            {
                yield return $"\"b{i}\":\"{b}\"";
                i++;
                b = !b;
            }
        }
    }
}
