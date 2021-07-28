using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NUnit.Framework;

using LogUploader.Data;
using LogUploader.Data.Repositories;

namespace LogUploader.Test.Data
{
    public class ProfessionDataTest
    {
        private const eProfession DefualtEProfession = eProfession.Scourge;
        private const string DefaultNameEN = "ScourgeEN";
        private const string DefaultNameDE = "ScourgeDE";
        private string DefaultIconPath { get => TestSetup.GetPathToTestFiles("static", "profIcon.png"); }
        private const string DefaultEmote = ":emote:";
        private const int DefaultRaidOrgaPlusID = 1;
        private const string DefaultAbbreviation = "scg";

        [Test, Parallelizable]
        public void ProfessionDataConstructorTest([ValueSource(typeof(TestHelper), nameof(TestHelper.ValidDiscordEmotes))] string emote)
        {
            Profession prof = new Profession(DefualtEProfession, DefaultNameEN, DefaultNameDE, DefaultIconPath, emote, DefaultRaidOrgaPlusID, DefaultAbbreviation);
            ValidateProfessionObject(prof, emote: emote);
        }

        [Test, Parallelizable]
        public void ProfessionDataConstructorInvalidParamsTest(
            [ValueSource(typeof(TestHelper), nameof(TestHelper.InvalidOneWordStrings))] string invalidStr,
            [ValueSource(typeof(TestHelper), nameof(TestHelper.ValidDiscordEmotes))] string emote
            )
        {
            TestHelper.ValidateArugumentException(Assert.Catch<ArgumentException>(() => new Profession(DefualtEProfession, invalidStr, DefaultNameDE, DefaultIconPath, emote, DefaultRaidOrgaPlusID, DefaultAbbreviation)));
            TestHelper.ValidateArugumentException(Assert.Catch<ArgumentException>(() => new Profession(DefualtEProfession, DefaultNameEN, invalidStr, DefaultIconPath, emote, DefaultRaidOrgaPlusID, DefaultAbbreviation)));
            TestHelper.ValidateArugumentException(Assert.Catch<ArgumentException>(() => new Profession(DefualtEProfession, DefaultNameEN, DefaultNameDE, DefaultIconPath, emote, DefaultRaidOrgaPlusID, invalidStr)));
            TestHelper.ValidateArugumentException(Assert.Catch<ArgumentException>(() => new Profession(DefualtEProfession, invalidStr, invalidStr, DefaultIconPath, emote, DefaultRaidOrgaPlusID, invalidStr)));
        }

        [Test, Parallelizable]
        public void ProfessionDataConstructorInvalidEmoteTest([ValueSource(typeof(TestHelper), nameof(TestHelper.InvalidDiscordEmotes))] string emote)
        {
            TestHelper.ValidateArugumentException(Assert.Catch<ArgumentException>(() => new Profession(DefualtEProfession, DefaultNameEN, DefaultNameDE, DefaultIconPath, emote, DefaultRaidOrgaPlusID, DefaultAbbreviation)));
        }

        [Test, Parallelizable]
        public void ProfessionDataConstructorInvalidPathTest(
            [Values(null, "", "  ", "invalid\\path\\does\\not\\exits\\void.png")] string path,
            [ValueSource(typeof(TestHelper), nameof(TestHelper.ValidDiscordEmotes))] string emote
            )
        {
            Assert.Catch(() => new Profession(DefualtEProfession, DefaultNameEN, DefaultNameDE, path, emote, DefaultRaidOrgaPlusID, DefaultAbbreviation));
        }

        [Test, Pairwise, Parallelizable]
        public void ProfessionDataEqualsTest(
            [Values] eProfession profession,
            [ValueSource(typeof(TestHelper), nameof(TestHelper.ValidDiscordEmotes))] string emote,
            [ValueSource(typeof(TestHelper), nameof(TestHelper.ValidOneWordStrings))] string oneWord
            )
        {
            Profession p1 = new Profession(DefualtEProfession, DefaultNameEN, DefaultNameDE, DefaultIconPath, emote, DefaultRaidOrgaPlusID, DefaultAbbreviation);
            Profession p2 = new Profession(profession, oneWord, DefaultNameDE, DefaultIconPath, emote, DefaultRaidOrgaPlusID, DefaultAbbreviation);
            Profession p3 = new Profession(profession, DefaultNameEN, oneWord, DefaultIconPath, emote, DefaultRaidOrgaPlusID, DefaultAbbreviation);
            Profession p4 = new Profession(profession, DefaultNameEN, DefaultNameDE, DefaultIconPath, emote, DefaultRaidOrgaPlusID, oneWord);

            foreach (var p in new Profession[] { p1, p2, p3, p4 })
            {
                Assert.That(p, Is.EqualTo(p));
                Assert.That(p, Is.Not.EqualTo(null));
                Assert.That(p, Is.Not.EqualTo(new object()));
            }

            if (profession == DefualtEProfession)
            {
                Assert.That(p1, Is.EqualTo(p2));
                Assert.That(p1, Is.EqualTo(p3));
                Assert.That(p1, Is.EqualTo(p4));
                Assert.That(p2, Is.EqualTo(p1));
                Assert.That(p3, Is.EqualTo(p1));
                Assert.That(p4, Is.EqualTo(p1));
            }
            else
            {
                Assert.That(p1, Is.Not.EqualTo(p2));
                Assert.That(p1, Is.Not.EqualTo(p3));
                Assert.That(p1, Is.Not.EqualTo(p4));
                Assert.That(p2, Is.Not.EqualTo(p1));
                Assert.That(p3, Is.Not.EqualTo(p1));
                Assert.That(p4, Is.Not.EqualTo(p1));
            }

            Assert.That(p2, Is.EqualTo(p3));
            Assert.That(p2, Is.EqualTo(p4));
            Assert.That(p3, Is.EqualTo(p4));
            Assert.That(p3, Is.EqualTo(p2));
            Assert.That(p4, Is.EqualTo(p2));
            Assert.That(p4, Is.EqualTo(p3));
        }

        [Test, Pairwise, Parallelizable]
        public void ProfessionDataOperatorTest(
            [Values] eProfession profession,
            [ValueSource(typeof(TestHelper), nameof(TestHelper.ValidDiscordEmotes))] string emote,
            [ValueSource(typeof(TestHelper), nameof(TestHelper.ValidOneWordStrings))] string oneWord
            )
        {
            Profession p1 = new Profession(DefualtEProfession, DefaultNameEN, DefaultNameDE, DefaultIconPath, emote, DefaultRaidOrgaPlusID, DefaultAbbreviation);
            Profession p2 = new Profession(profession, oneWord, DefaultNameDE, DefaultIconPath, emote, DefaultRaidOrgaPlusID, DefaultAbbreviation);
            Profession p3 = new Profession(profession, DefaultNameEN, oneWord, DefaultIconPath, emote, DefaultRaidOrgaPlusID, DefaultAbbreviation);
            Profession p4 = new Profession(profession, DefaultNameEN, DefaultNameDE, DefaultIconPath, emote, DefaultRaidOrgaPlusID, oneWord);

            foreach (var p in new Profession[] { p1, p2, p3, p4 })
            {
#pragma warning disable CS1718 // Comparison made to same variable
                Assert.That(p == p, Is.True);
#pragma warning restore CS1718 // Comparison made to same variable
                Assert.That(p == null, Is.False);
#pragma warning disable CS0253 // Possible unintended reference comparison; right hand side needs cast
                Assert.That(p == new object(), Is.False);
#pragma warning restore CS0253 // Possible unintended reference comparison; right hand side needs cast
            }

            if (profession == DefualtEProfession)
            {
                Assert.That(p1 == p2, Is.True);
                Assert.That(p1 == p3, Is.True);
                Assert.That(p1 == p4, Is.True);
                Assert.That(p2 == p1, Is.True);
                Assert.That(p3 == p1, Is.True);
                Assert.That(p4 == p1, Is.True);
                Assert.That(p1 != p2, Is.False);
                Assert.That(p1 != p3, Is.False);
                Assert.That(p1 != p4, Is.False);
                Assert.That(p2 != p1, Is.False);
                Assert.That(p3 != p1, Is.False);
                Assert.That(p4 != p1, Is.False);
            }
            else
            {
                Assert.That(p1 != p2, Is.True);
                Assert.That(p1 != p3, Is.True);
                Assert.That(p1 != p4, Is.True);
                Assert.That(p2 != p1, Is.True);
                Assert.That(p3 != p1, Is.True);
                Assert.That(p4 != p1, Is.True);
                Assert.That(p1 == p2, Is.False);
                Assert.That(p1 == p3, Is.False);
                Assert.That(p1 == p4, Is.False);
                Assert.That(p2 == p1, Is.False);
                Assert.That(p3 == p1, Is.False);
                Assert.That(p4 == p1, Is.False);
            }

            Assert.That(p2 == p3, Is.True);
            Assert.That(p2 == p4, Is.True);
            Assert.That(p3 == p4, Is.True);
            Assert.That(p3 == p2, Is.True);
            Assert.That(p4 == p2, Is.True);
            Assert.That(p4 == p3, Is.True);
            Assert.That(p2 != p3, Is.False);
            Assert.That(p2 != p4, Is.False);
            Assert.That(p3 != p4, Is.False);
            Assert.That(p3 != p2, Is.False);
            Assert.That(p4 != p2, Is.False);
            Assert.That(p4 != p3, Is.False);
        }

        [Test, Pairwise, Parallelizable]
        public void ProfessionDataGetHashCodeTest(
            [Values] eProfession profession,
            [ValueSource(typeof(TestHelper), nameof(TestHelper.ValidDiscordEmotes))] string emote,
            [ValueSource(typeof(TestHelper), nameof(TestHelper.ValidOneWordStrings))] string oneWord
            )
        {
            Profession p1 = new Profession(DefualtEProfession, DefaultNameEN, DefaultNameDE, DefaultIconPath, emote, DefaultRaidOrgaPlusID, DefaultAbbreviation);
            Profession p2 = new Profession(profession, oneWord, DefaultNameDE, DefaultIconPath, emote, DefaultRaidOrgaPlusID, DefaultAbbreviation);
            Profession p3 = new Profession(profession, DefaultNameEN, oneWord, DefaultIconPath, emote, DefaultRaidOrgaPlusID, DefaultAbbreviation);
            Profession p4 = new Profession(profession, DefaultNameEN, DefaultNameDE, DefaultIconPath, emote, DefaultRaidOrgaPlusID, oneWord);

            foreach (var p in new Profession[] { p1, p2, p3, p4 })
            {
                Assert.That(p.GetHashCode(), Is.EqualTo(p.GetHashCode()));
            }

            if (profession == DefualtEProfession)
            {
                Assert.That(p1.GetHashCode(), Is.EqualTo(p2.GetHashCode()));
                Assert.That(p1.GetHashCode(), Is.EqualTo(p3.GetHashCode()));
                Assert.That(p1.GetHashCode(), Is.EqualTo(p4.GetHashCode()));
            }
            else
            {
                Assert.That(p1.GetHashCode(), Is.Not.EqualTo(p2.GetHashCode()));
                Assert.That(p1.GetHashCode(), Is.Not.EqualTo(p3.GetHashCode()));
                Assert.That(p1.GetHashCode(), Is.Not.EqualTo(p4.GetHashCode()));
            }

            Assert.That(p2.GetHashCode(), Is.EqualTo(p3.GetHashCode()));
            Assert.That(p2.GetHashCode(), Is.EqualTo(p4.GetHashCode()));
            Assert.That(p3.GetHashCode(), Is.EqualTo(p4.GetHashCode()));
        }

        private void ValidateProfessionObject(Profession prof,
            eProfession profession = DefualtEProfession, string nameEN = DefaultNameEN, string nameDE = DefaultNameDE,
            string emote = DefaultEmote, int raidOrgaPlusID = DefaultRaidOrgaPlusID, string abbreviation = DefaultAbbreviation) =>
            ValidateProfessionObject(prof, DefaultIconPath, profession, nameEN, nameDE, emote, raidOrgaPlusID, abbreviation);

        private void ValidateProfessionObject(Profession prof, string iconPath,
            eProfession profession = DefualtEProfession, string nameEN = DefaultNameEN, string nameDE = DefaultNameDE,
            string emote = DefaultEmote, int raidOrgaPlusID = DefaultRaidOrgaPlusID, string abbreviation = DefaultAbbreviation)
        {
            Assert.That(prof.ProfessionEnum, Is.EqualTo(profession));
            Assert.That(prof.NameEN, Is.EqualTo(nameEN));
            Assert.That(prof.NameDE, Is.EqualTo(nameDE));
            Assert.That(prof.IconPath, Is.EqualTo(iconPath));
            Assert.That(prof.Icon, Is.Not.Null);
            Assert.That(prof.Emote, Is.EqualTo(emote));
            Assert.DoesNotThrow(() => LogUploader.Tools.GP.ValidateDiscordEmote(emote));
            Assert.That(prof.RaidOrgaPlusID, Is.EqualTo(raidOrgaPlusID));
            Assert.That(prof.Abbreviation, Is.EqualTo(abbreviation));
        }

        [Test]
        public void ProfessionRepositoryDefaultCreateTest()
        {
            Assert.DoesNotThrow(() => new ProfessionRepository());
        }

        [Test, Parallelizable]
        public void ProfessionRepositoryDefaultEmptyTest([Values] eProfession profession)
        {
            ProfessionRepository repo = new ProfessionRepository();
            TestHelper.ValidateArugumentException(Assert.Catch<ArgumentException>(() => repo.Get(profession)));
        }

        [Test, Parallelizable]
        public void ProfessionRepositoryInsertTest([Values] eProfession retrived)
        {
            ProfessionRepository repo = new ProfessionRepository();
            Profession prof = new Profession(DefualtEProfession, DefaultNameEN, DefaultNameDE, DefaultIconPath, DefaultEmote, DefaultRaidOrgaPlusID, DefaultAbbreviation);
            Assert.DoesNotThrow(() => repo.Add(prof));
            if (DefualtEProfession == retrived)
                Assert.DoesNotThrow(() => repo.Get(retrived));
            else
                TestHelper.ValidateArugumentException(Assert.Catch<ArgumentException>(() => repo.Get(retrived)));
        }

        [Test, Parallelizable]
        public void ProfessionRepositoryInsertDuplicateTest([Values] eProfession profession)
        {
            ProfessionRepository repo = new ProfessionRepository();
            Profession profA = new Profession(profession, "ProfA", DefaultNameDE, DefaultIconPath, DefaultEmote, DefaultRaidOrgaPlusID, DefaultAbbreviation);
            Profession profB = new Profession(profession, "ProfB", DefaultNameDE, DefaultIconPath, DefaultEmote, DefaultRaidOrgaPlusID, DefaultAbbreviation);
            Assert.DoesNotThrow(() => repo.Add(profA));
            TestHelper.ValidateArugumentException(Assert.Catch<ArgumentException>(() => repo.Add(profB)));
            Assert.That(profA, Is.EqualTo(repo.Get(profession)));
            Assert.That(profA.NameEN, Is.EqualTo(repo.Get(profession).NameEN));
        }

        [Test]
        public void ProfessionRepositoryCanStoreAllTest()
        {
            ProfessionRepository repo = new ProfessionRepository();

            foreach (eProfession profession in LogUploader.Tools.EnumHelper.GetValues<eProfession>())
            {
                Profession prof = new Profession(profession, DefaultNameEN + profession, DefaultNameDE + profession, DefaultIconPath, DefaultEmote, DefaultRaidOrgaPlusID + (int) profession, DefaultAbbreviation + profession);
                Assert.DoesNotThrow(() => repo.Add(prof));
            }

            foreach (eProfession profession in LogUploader.Tools.EnumHelper.GetValues<eProfession>())
            {
                Assert.DoesNotThrow(() => repo.Get(profession));
            }
        }

        [Test]
        public void ProfessionRepositoryGetTest()
        {
            ProfessionRepository repo = new ProfessionRepository();
            Profession a = new Profession(eProfession.Scourge, "ena", "dea", DefaultIconPath, DefaultEmote, 1, "aba");
            Profession b = new Profession(eProfession.Reaper, "enb", "deb", DefaultIconPath, DefaultEmote, 2, "abb");
            Profession c = new Profession(eProfession.Necromancer, "enc", "dec", DefaultIconPath, DefaultEmote, 3, "abc");
            Profession d = new Profession(eProfession.Chronomancer, "end", "ded", DefaultIconPath, DefaultEmote, 4, "abd");
            Profession e = new Profession(eProfession.Mirage, "ene", "dee", DefaultIconPath, DefaultEmote, 5, "abe");
            Profession f = new Profession(eProfession.Daredevil, "enf", "def", DefaultIconPath, DefaultEmote, 6, "abf");
            Profession[] profs = new Profession[] { a, b, c, d, e, f };

            foreach (Profession profession in profs)
            {
                repo.Add(profession);
            }

            foreach (Profession profession in profs)
            {
                Assert.That(profession, Is.EqualTo(repo.Get(profession.ProfessionEnum)));
                Assert.That(profession, Is.EqualTo(repo.Get(profession.NameEN)));
                Assert.That(profession, Is.EqualTo(repo.Get(profession.NameDE)));
                Assert.That(profession, Is.EqualTo(repo.Get(profession.RaidOrgaPlusID)));
                Assert.That(profession, Is.EqualTo(repo.GetByAbbreviation(profession.Abbreviation)));
            }
        }

        [Test]
        public void ProfessionRepositoryGetFailTest()
        {
            ProfessionRepository repo = new ProfessionRepository();
            Profession profession = new Profession(eProfession.Scourge, "ena", "dea", DefaultIconPath, DefaultEmote, 1, "aba");
            
            repo.Add(profession);

            TestHelper.ValidateArugumentException(Assert.Catch<ArgumentException>(() => repo.Get(eProfession.Chronomancer)));
            TestHelper.ValidateArugumentException(Assert.Catch<ArgumentException>(() => repo.Get("notAdded")));
            TestHelper.ValidateArugumentException(Assert.Catch<ArgumentException>(() => repo.Get("notAdded")));
            TestHelper.ValidateArugumentException(Assert.Catch<ArgumentException>(() => repo.Get(0)));
            TestHelper.ValidateArugumentException(Assert.Catch<ArgumentException>(() => repo.GetByAbbreviation("notAdded")));
        }
    }
}
