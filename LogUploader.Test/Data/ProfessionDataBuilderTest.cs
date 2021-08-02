using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LogUploader.Data.Repositories;
using LogUploader.Data;
using NUnit.Framework;
using Newtonsoft.Json;

namespace LogUploader.Test.Data
{
    class ProfessionDataBuilderTest
    {
        private string ProfessionIconPath { get => TestSetup.GetRelativePathToTestFiles("static", "profIcon.png"); }
        private string AbsolutProfessionIconPath { get => TestSetup.GetPathToTestFiles("static", "profIcon.png"); }
        private string SmallValidTestJson { get => $@"
{{
  ""Professions"": [
    {{
      ""ID"": 0,
      ""NameEN"": ""Unknown"",
      ""NameDE"": ""Unbekannt"",
      ""IconPath"": ""{JsonEscapeString(ProfessionIconPath)}"",
      ""Emote"": "":ghost:"",
      ""RaidOrgaPlusID"": 0,
      ""RaidOrgaPlusAbbreviation"": ""XXX""
    }},
    {{
      ""ID"": 2,
      ""NameEN"": ""Guardian"",
      ""NameDE"": ""Wächter"",
      ""IconPath"": ""{JsonEscapeString(ProfessionIconPath)}"",
      ""Emote"": ""<:gdn:999999999999999999>"",
      ""RaidOrgaPlusID"": 8,
      ""RaidOrgaPlusAbbreviation"": ""Gdn""
    }}
  ]
}}"; }

        private string SmallInvalidThroughDupelicateTestJson { get => $@"
{{
  ""Professions"": [
    {{
      ""ID"": 2,
      ""NameEN"": ""Guardian"",
      ""NameDE"": ""Wächter"",
      ""IconPath"": ""{JsonEscapeString(ProfessionIconPath)}"",
      ""Emote"": ""<:gdn:999999999999999999>"",
      ""RaidOrgaPlusID"": 8,
      ""RaidOrgaPlusAbbreviation"": ""Gdn""
    }},
    {{
      ""ID"": 2,
      ""NameEN"": ""Guardian"",
      ""NameDE"": ""Wächter"",
      ""IconPath"": ""{JsonEscapeString(ProfessionIconPath)}"",
      ""Emote"": ""<:gdn:999999999999999999>"",
      ""RaidOrgaPlusID"": 8,
      ""RaidOrgaPlusAbbreviation"": ""Gdn""
    }}
  ]
}}"; }

        private string SmallInvalidThroughValueTestJson { get => $@"
{{
  ""Professions"": [
    {{
      ""ID"": 2,
      ""NameEN"": null,
      ""NameDE"": ""Wächter"",
      ""IconPath"": ""{JsonEscapeString(ProfessionIconPath)}"",
      ""Emote"": ""<:gdn:999999999999999999>"",
      ""RaidOrgaPlusID"": 8,
      ""RaidOrgaPlusAbbreviation"": ""Gdn""
    }}
  ]
}}"; }

        private static string JsonEscapeString(string str)
        {
            string escaped = JsonConvert.ToString(str);
            //Remove encasing double quotes
            return escaped.Substring(1, escaped.Length - 2);
        }

        [Test]
        public void ParseProfessionDataNoErrorTest()
        {
            ProfessionRepository testProfessionRepo = new ProfessionRepository();
            Assert.DoesNotThrow(() => ProfessionDataBuilder.ParseJson(SmallValidTestJson, testProfessionRepo));
        }

        [Test]
        public void ParseProfessionDataCorrectTest()
        {
            TestProfessionRepo testProfessionRepo = new TestProfessionRepo();
            ProfessionDataBuilder.ParseJson(SmallValidTestJson, testProfessionRepo);
            Assert.That(testProfessionRepo.Count, Is.EqualTo(2));
            ValidateProfessionCorrectlyParsed(testProfessionRepo, eProfession.Guardian, "Guardian", "Wächter", "Gdn", 8, "<:gdn:999999999999999999>", AbsolutProfessionIconPath);
            ValidateProfessionCorrectlyParsed(testProfessionRepo, eProfession.Unknown, "Unknown", "Unbekannt", "XXX", 0, ":ghost:", AbsolutProfessionIconPath);
        }

        private void ValidateProfessionCorrectlyParsed(ProfessionProvider professionProvider, eProfession profession, string nameEN, string nameDE, string abbreviation, int ropId, string emote, string professionIconPath)
        {
            Profession prof = professionProvider.Get(profession);

            Assert.That(prof.ProfessionEnum, Is.EqualTo(profession));
            Assert.That(prof.NameEN, Is.EqualTo(nameEN));
            Assert.That(prof.NameDE, Is.EqualTo(nameDE));
            Assert.That(prof.Abbreviation, Is.EqualTo(abbreviation));
            Assert.That(prof.RaidOrgaPlusID, Is.EqualTo(ropId));
            Assert.That(prof.Emote, Is.EqualTo(emote));
            Assert.That(prof.IconPath, Is.EqualTo(professionIconPath));
        }

        [Test]
        public void ParseProfessionNullParameterTest()
        {
            ProfessionRepository testProfessionRepo = new ProfessionRepository();
            TestHelper.ValidateArugumentException(Assert.Catch<ArgumentNullException>(() => ProfessionDataBuilder.ParseJson(null, testProfessionRepo)));
            TestHelper.ValidateArugumentException(Assert.Catch<ArgumentNullException>(() => ProfessionDataBuilder.ParseJson(SmallValidTestJson, (ProfessionRepository)null)));
            TestHelper.ValidateArugumentException(Assert.Catch<ArgumentNullException>(() => ProfessionDataBuilder.ParseJson(null, (ProfessionRepository)null)));
        }

        [Test]
        public void ParseProfessionInvalidJsonTest()
        {
            ProfessionRepository testProfessionRepo = new ProfessionRepository();
            TestHelper.ValidateArugumentException(Assert.Catch<ArgumentException>(() => ProfessionDataBuilder.ParseJson("", testProfessionRepo)));
            TestHelper.ValidateArugumentException(Assert.Catch<ArgumentException>(() => ProfessionDataBuilder.ParseJson("this is non valid json:really not", testProfessionRepo)));
            TestHelper.ValidateArugumentException(Assert.Catch<ArgumentException>(() => ProfessionDataBuilder.ParseJson(@"{""some"":""other JSON""}", testProfessionRepo)));
            TestHelper.ValidateArugumentException(Assert.Catch<ArgumentException>(() => ProfessionDataBuilder.ParseJson(SmallInvalidThroughValueTestJson, testProfessionRepo)));
            TestHelper.ValidateArugumentException(Assert.Catch<ArgumentException>(() => ProfessionDataBuilder.ParseJson(SmallInvalidThroughDupelicateTestJson, testProfessionRepo)));
        }


        [Test]
        public void ParseProfessionJsonOnErrorAddsNothingTest()
        {
            TestProfessionRepo testProfessionRepo = new TestProfessionRepo();
            const eProfession prof = eProfession.Scourge;
            testProfessionRepo.Add(new Profession(prof, "Scourge", "Scourge", AbsolutProfessionIconPath, ":emote:", 99, "scg"));
            
            Assert.Catch<ArgumentException>(() => ProfessionDataBuilder.ParseJson("", testProfessionRepo));
            Assert.That(testProfessionRepo.Count, Is.EqualTo(1), "There should have nothing been added or removed with empty string");
            Assert.That(testProfessionRepo.Get(prof), Is.Not.Null);

            Assert.Catch<ArgumentException>(() => ProfessionDataBuilder.ParseJson("this is non valid json:really not", testProfessionRepo));
            Assert.That(testProfessionRepo.Count, Is.EqualTo(1), "There should have nothing been added or removed with ivalid json");
            Assert.That(testProfessionRepo.Get(prof), Is.Not.Null);

            Assert.Catch<ArgumentException>(() => ProfessionDataBuilder.ParseJson(@"{""some"":""other JSON""}", testProfessionRepo));
            Assert.That(testProfessionRepo.Count, Is.EqualTo(1), "There should have nothing been added or removed with diffrent json");
            Assert.That(testProfessionRepo.Get(prof), Is.Not.Null);

            Assert.Catch<ArgumentException>(() => ProfessionDataBuilder.ParseJson(SmallInvalidThroughValueTestJson, testProfessionRepo));
            Assert.That(testProfessionRepo.Count, Is.EqualTo(1), "There should have nothing been added or removed with SmallInvalidThroughValueTestJson");
            Assert.That(testProfessionRepo.Get(prof), Is.Not.Null);
            Assert.Catch<ArgumentException>(() => testProfessionRepo.Get(eProfession.Guardian));

            Assert.Catch<ArgumentException>(() => ProfessionDataBuilder.ParseJson(SmallInvalidThroughDupelicateTestJson, testProfessionRepo));
            Assert.That(testProfessionRepo.Count, Is.EqualTo(1), "There should have nothing been added or removed with SmallInvalidThroughDupelicateTestJson");
            Assert.That(testProfessionRepo.Get(prof), Is.Not.Null);
            Assert.Catch<ArgumentException>(() => testProfessionRepo.Get(eProfession.Guardian));
        }
    }

    internal class TestProfessionRepo : ProfessionRepository
    {
        public TestProfessionRepo()
        {

        }

        internal int Count { get => Professions.Count; }
    }

}
