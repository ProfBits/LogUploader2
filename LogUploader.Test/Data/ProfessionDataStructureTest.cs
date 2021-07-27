using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using LogUploader.Data;
using LogUploader.Tools;
using Newtonsoft.Json.Linq;

using NUnit.Framework;

namespace LogUploader.Test.Data
{
    public class ProfessionDataStructureTest
    {
        private JObject DataConfigContent = null;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            var assamblyPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            var relPath = "Data" + System.IO.Path.DirectorySeparatorChar + "ProfessionData.json";

            Assert.DoesNotThrow(() =>
            {
                var jsonStr = JsonHandling.ReadJsonFile(assamblyPath + System.IO.Path.DirectorySeparatorChar + relPath);

                DataConfigContent = JObject.Parse(jsonStr);
            }, "Could not parse ProfessionData.json");
        }

        [Test]
        public void ProfessionDataCompletness()
        {
            Assert.AreEqual(1, DataConfigContent.Count, "Only the Professions token is expeted at the root level, no more");
            Assert.NotNull(DataConfigContent["Professions"], "A Professions token is expeted at the root level");
            Assert.AreEqual(JTokenType.Array, DataConfigContent["Professions"].Type, "A Professions array is expeted at the root level");
            foreach (JToken item in DataConfigContent["Professions"])
            {
                Assert.AreEqual(JTokenType.Object, item.Type, "At item:\n" + item.ToString());
                JObject element = item as JObject;
                Assert.AreEqual(7, element.Children().Count(), "7 tokens below each Profession are expected. At item:\n" + element.ToString());

                Assert.NotNull(element["ID"], "A ID token is expeted. At item:\n" + element.ToString());
                Assert.AreEqual(JTokenType.Integer, element["ID"].Type, "A ID Integer is expeted. At item:\n" + element.ToString());

                Assert.NotNull(element["NameEN"], "A NameEN token is expeted. At item:\n" + element.ToString());
                Assert.AreEqual(JTokenType.String, element["NameEN"].Type, "A NameEN String is expeted. At item:\n" + element.ToString());

                Assert.NotNull(element["NameDE"], "A NameDE token is expeted. At item:\n" + element.ToString());
                Assert.AreEqual(JTokenType.String, element["NameDE"].Type, "A NameDE String is expeted. At item:\n" + element.ToString());

                Assert.NotNull(element["IconPath"], "A IconPath token is expeted. At item:\n" + element.ToString());
                Assert.AreEqual(JTokenType.String, element["IconPath"].Type, "A IconPath String is expeted. At item:\n" + element.ToString());

                Assert.NotNull(element["Emote"], "A Emote token is expeted. At item:\n" + element.ToString());
                Assert.AreEqual(JTokenType.String, element["Emote"].Type, "A Emote String is expeted. At item:\n" + element.ToString());

                Assert.NotNull(element["RaidOrgaPlusID"], "A RaidOrgaPlusID token is expeted. At item:\n" + element.ToString());
                Assert.AreEqual(JTokenType.Integer, element["RaidOrgaPlusID"].Type, "A RaidOrgaPlusID Integer is expeted. At item:\n" + element.ToString());

                Assert.NotNull(element["RaidOrgaPlusAbbreviation"], "A RaidOrgaPlusAbbreviation token is expeted. At item:\n" + element.ToString());
                Assert.AreEqual(JTokenType.String, element["RaidOrgaPlusAbbreviation"].Type, "A RaidOrgaPlusAbbreviation String is expeted. At item:\n" + element.ToString());
            }
        }

        [Test]
        public void ProfessionDataPlausibility()
        {
            Assert.NotNull(DataConfigContent["Professions"]);
            List<int> ids = new List<int>();
            List<int> ropIds = new List<int>();
            List<string> abbrivations = new List<string>();
            var assamblyPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);

            foreach (JToken item in DataConfigContent["Professions"])
            {
                JObject element = item as JObject;
                Assert.NotNull(element);

                ids.Add((int)element["ID"]);
                ropIds.Add((int)element["RaidOrgaPlusID"]);

                ValidateString((string)element["NameEN"]);
                ValidateString((string)element["NameDE"]);

                ValidateString((string)element["IconPath"]);
                var fi = new System.IO.FileInfo(assamblyPath + (string)element["IconPath"]);
                Assert.IsTrue(fi.Exists, $"Profession icon \"{(string)element["IconPath"]}\" does not exist on disk");

                ValidateDiscordEmote((string)element["Emote"]);

                string abbriviation = (string)element["RaidOrgaPlusAbbreviation"];
                ValidateString(abbriviation);
                abbrivations.Add(abbriviation);
            }

            CheckForDuplicates(ids, "ID");
            CheckForDuplicates(ropIds, "RaidOrgaPlusID");
            CheckForDuplicates(abbrivations, "RaidOrgaPlusAbbreviation");
        }

        private static void CheckForDuplicates<T>(List<T> ids, string name)
        {
            var count = ids.Count;
            var distinctCount = ids.Distinct().Count();
            if (count != distinctCount)
            {
                var errMsg = GetDuplicateErrorMessage(ids);
                Assert.Fail(count - distinctCount + " duplicated " + name + "(s) Present: " + errMsg);
            }
        }

        private static string GetDuplicateErrorMessage<T>(IEnumerable<T> ids)
        {
            var duplicates = ids
                .Distinct()
                .AsParallel()
                .Where(num => ids.Count(e => e.Equals(num)) > 1);
            return string.Join(", ", duplicates);
        }

        private void ValidateString(string s)
        {
            Assert.NotNull(s, "ValidateString not null check");
            Assert.IsFalse(string.IsNullOrWhiteSpace(s), $"String should not be empty or whitespace \"{s}\"");
            Assert.AreEqual(s, s.Trim(), $"NotWhite space at front or end. Error: \"{s}\"");
            Assert.AreEqual(1, s.Split('\n', '\r').Count(), $"Strings should only have one line. Error: \"{s}\"");
        }

        private readonly Regex DiscordEmoteRegEx = new Regex("^((:\\w+:)|(<:\\w+:\\d{18}>))$", RegexOptions.Compiled);

        private void ValidateDiscordEmote(string s)
        {
            ValidateString(s);
            Assert.IsTrue(DiscordEmoteRegEx.IsMatch(s), $"Discord Emote \"{s}\" has invlid format");
        }
    }
}
