using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

using NUnit.Framework;

using LogUploader.Data;
using LogUploader.Localisation;
using LogUploader.Data.StaticDataLoader;
using LogUploader.Data.Repositories;

namespace LogUploader.Test.Data
{
    public class DataBuilderTest
    {
        private TestDataRegistrator TestRegistrator = new TestDataRegistrator();

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            DataBuilder.LoadDataJson(LogUploader.Tools.JsonHandling.ReadJsonFile(TestSetup.GetPathToTestFiles("static", "bossDataSimple.json")), TestRegistrator);
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            TestRegistrator = null;
        }

        [Test]
        public void DataBuilderRaidWingTest()
        {
            Raid wing = null;
            Assert.DoesNotThrow(() => wing = TestRegistrator.TestAreas.TestGetRaidWing(1));
            BasicGameAreaAssertions(wing, "RaidEN", "RaidDE", "https://raid.area/icon.png");
        }

        [Test]
        public void DataBuilderStrikeTest()
        {
            Strike strike = null;
            Assert.DoesNotThrow(() => strike = TestRegistrator.TestAreas.TestGetStrike(1));
            BasicGameAreaAssertions(strike, "StrikeEN", "StriekDE", "https://strike.area/icon.png");
        }

        [Test]
        public void DataBuilderFractalTest()
        {
            Fractal fractal = null;
            Assert.DoesNotThrow(() => fractal = TestRegistrator.TestAreas.TestGetFractal(1));
            BasicGameAreaAssertions(fractal, "FractalEN", "FractalDE", "https://fractal.area/icon.png");
        }

        [Test]
        public void DataBuilderDrmTest()
        {
            DragonResponseMission drm = null;
            Assert.DoesNotThrow(() => drm = TestRegistrator.TestAreas.TestGetDragonResponseMission(1));
            BasicGameAreaAssertions(drm, "DRMEN", "DRMDE", "https://drm.area/icon.png");
        }

        [Test]
        public void DataBuilderWvWTest()
        {
            WvW wvw = null;
            Assert.DoesNotThrow(() => wvw = TestRegistrator.TestAreas.TestGetWvW());
            BasicGameAreaAssertions(wvw, "WvWEN", "WvWDE", "https://wvw.area/icon.png");
            ExtendedGameAreaAssertions(wvw, "WvWen", "WvWde");
        }

        [Test]
        public void DataBuilderTrainingTest()
        {
            Training training = null;
            Assert.DoesNotThrow(() => training = TestRegistrator.TestAreas.TestGetTraining());
            BasicGameAreaAssertions(training, "GolemEN", "GolemDE", "https://golem.area/icon.png");
            ExtendedGameAreaAssertions(training, "Golemen", "Golemde");
        }

        [Test]
        public void DataBuilderUnknownTest()
        {
            UnkowenGameArea unknowen = null;
            Assert.DoesNotThrow(() => unknowen = TestRegistrator.TestAreas.TestGetUnkowen());
            BasicGameAreaAssertions(unknowen, "???EN", "???DE", "https://unkown.area/icon.png");
            ExtendedGameAreaAssertions(unknowen, "???en", "???de");
        }

        private void BasicGameAreaAssertions(GameArea area, string nameEN, string nameDE, string avatarURL)
        {
            Assert.That(area, Is.Not.Null);
            Assert.That(area.NameEN, Is.EqualTo(nameEN));
            Assert.That(area.NameDE, Is.EqualTo(nameDE));
            Assert.That(area.AvatarURL, Is.EqualTo(avatarURL));
        }

        private void ExtendedGameAreaAssertions(GameArea area, string shortNameEN, string shortNameDE)
        {
            Assert.That(area, Is.Not.Null);
            Assert.That(area.GetShortName(eLanguage.EN), Is.EqualTo(shortNameEN));
            Assert.That(area.GetShortName(eLanguage.DE), Is.EqualTo(shortNameDE));
        }

        [Test]
        public void DataBuilderBossTest()
        {
            Assert.That(TestRegistrator.TestBosses.Bosses.Count, Is.EqualTo(7));
            TestBoss(0, "B?EN", "B?DE", "Bf?en", "Bf?de", "B?EI", typeof(UnkowenGameArea), ":emote:", "https://unkown.boss/icon.png", -1);
            TestBoss(15438, "Vale Guardian", "Tal Wächter", "Vale Guardianf", "Tal-Wächterf", "Vale Guardiane", typeof(Raid), ":vg:", "https://vg.boss/icon.png", 1);
            TestBoss(22154, "Construct", "Konstrukt", "ConstructF", "KonstruktF", "ConstructEI", typeof(Strike), ":strike:", "https://construct.boss/icon.png", -1);
            TestBoss(23330, "Destroyer", "Zerstörer", "DestroyerF", "ZerstörerF", "DestroyerEI", typeof(DragonResponseMission), ":drm:", "https://drm.boss/icon.png", -1);
            TestBoss(1, "WvWEN", "WvWDE", "WvWENf", "WvWDEf", "WvWEI", typeof(WvW), ":wvw:", "https://wvw.boss/icon.png", -1);
            TestBoss(17021, "fractalEN", "fractalDE", "fractalENf", "fractalDEf", "fractalEI", typeof(Fractal), ":fractal:", "https://fractal.boss/icon.png", -1);
            TestBoss(16202, "GolemEN", "GolemDE", "GolemENf", "GolemDEf", "GolemEI", typeof(Training), ":golem:", "https://golem.boss/icon.png", -1);
        }

        private void TestBoss(int id, string nameEN, string nameDE, string folderNameEN, string folderNameDE,
            string eiName, Type areaType, string discordEmote, string avatarURL, int raidOrgaPlusID)
        {
            Assert.That(TestRegistrator.TestBosses.Exists(id), Is.True);
            Boss boss = (Boss)TestRegistrator.TestBosses.Get(id);
            TestBasicEnemy(boss, id, nameEN, nameDE, areaType);
            Assert.That(boss.FolderNameEN, Is.EqualTo(folderNameEN));
            Assert.That(boss.FolderNameDE, Is.EqualTo(folderNameDE));
            Assert.That(boss.EIName, Is.EqualTo(eiName));
            Assert.That(boss.DiscordEmote, Is.EqualTo(discordEmote));
            Assert.That(boss.AvatarURL, Is.EqualTo(avatarURL));
            Assert.That(boss.RaidOrgaPlusID, Is.EqualTo(raidOrgaPlusID));
        }

        [Test]
        public void DataBuilderAddEnemyTest()
        {
            Assert.That(TestRegistrator.TestAddEnemies.Adds.Count, Is.EqualTo(3));
            TestAdd(0, "Add?EN", "Add?DE", typeof(UnkowenGameArea), true);
            TestAdd(2, "IntrestingEN", "IntrestingDE", typeof(Raid), true);
            TestAdd(3, "UnintrestingEN", "UnintrestingDE", typeof(Raid), false);
        }


        private void TestAdd(int id, string nameEN, string nameDE, Type areaType, bool interesting)
        {
            Assert.That(TestRegistrator.TestAddEnemies.Exists(id), Is.True);
            AddEnemy add = (AddEnemy)TestRegistrator.TestAddEnemies.Get(id);
            TestBasicEnemy(add, id, nameEN, nameDE, areaType);
            Assert.That(add.IsInteresting, Is.EqualTo(interesting));
        }

        private void TestBasicEnemy(Enemy enemy, int id, string nameEN, string nameDE, Type areaType)
        {
            Assert.That(enemy.ID, Is.EqualTo(id));
            Assert.That(enemy.NameEN, Is.EqualTo(nameEN));
            Assert.That(enemy.NameDE, Is.EqualTo(nameDE));
            Assert.That(enemy.Area, Is.InstanceOf(areaType));
        }

        [Test]
        public void DataBuilderMiscTest()
        {
            Assert.That(TestRegistrator.TestMiscData.KillEmote, Is.EqualTo(":kill:"));
            Assert.That(TestRegistrator.TestMiscData.WipeEmote, Is.EqualTo(":wipe:"));
        }

        private class TestDataRegistrator : IGameDataRegistrator
        {
            public TestAreaRegistrator TestAreas { get; }
            public TestBosseRegistrator TestBosses { get; }
            public TestAddEnemyRegistrator TestAddEnemies { get; }
            public TestMiscDataRegistrator TestMiscData { get; }
            public IAreaRegistrator Areas { get => TestAreas; }
            public IBosseRegistrator Bosses { get => TestBosses; }
            public IAddEnemyRegistrator AddEnemies { get => TestAddEnemies; }
            public IMiscDataRegistrator MiscData { get => TestMiscData; }

            public TestDataRegistrator()
            {
                TestAreas = new TestAreaRegistrator();
                TestBosses = new TestBosseRegistrator();
                TestAddEnemies = new TestAddEnemyRegistrator();
                TestMiscData = new TestMiscDataRegistrator();
            }

            public class TestAreaRegistrator : IAreaRegistrator
            {
                private Dictionary<int, DragonResponseMission> drms = new Dictionary<int, DragonResponseMission>();
                private Dictionary<int, Fractal> fractals = new Dictionary<int, Fractal>();
                private Dictionary<int, Raid> wings = new Dictionary<int, Raid>();
                private Dictionary<int, Strike> strikes = new Dictionary<int, Strike>();
                private List<Training> trainings = new List<Training>();
                private List<UnkowenGameArea> unknowens = new List<UnkowenGameArea>();
                private List<WvW> wvws = new List<WvW>();

                public DragonResponseMission GetDragonResponseMission(int id) => TestGetDragonResponseMission(id);

                public Fractal GetFractal(int id) => TestGetFractal(id);

                public Raid GetRaidWing(int id) => TestGetRaidWing(id);

                public Strike GetStrike(int id) => TestGetStrike(id);

                public Training GetTraining() => TestGetTraining();

                public UnkowenGameArea GetUnkowen() => TestGetUnkowen();

                public WvW GetWvW() => TestGetWvW();

                public DragonResponseMission TestGetDragonResponseMission(int id)
                {
                    return drms[id];
                }

                public Fractal TestGetFractal(int id)
                {
                    return fractals[id];
                }

                public Raid TestGetRaidWing(int id)
                {
                    return wings[id];
                }

                public Strike TestGetStrike(int id)
                {
                    return strikes[id];
                }

                public Training TestGetTraining()
                {
                    return trainings.LastOrDefault();
                }

                public UnkowenGameArea TestGetUnkowen()
                {
                    return unknowens.LastOrDefault();
                }

                public WvW TestGetWvW()
                {
                    return wvws.LastOrDefault();
                }

                public void RegisterDragonResponseMission(int id, (string nameEN, string nameDE, string avatarURL) basicData)
                {
                    (string nameEN, string nameDE, string avatarURL) = basicData;
                    DragonResponseMission drm = new DragonResponseMission(id, nameEN, nameDE, avatarURL);
                    drms.Add(id, drm);
                }

                public void RegisterFractal(int id, (string nameEN, string nameDE, string avatarURL) basicData)
                {
                    (string nameEN, string nameDE, string avatarURL) = basicData;
                    Fractal fractal = new Fractal(id, nameEN, nameDE, avatarURL);
                    fractals.Add(id, fractal);
                }

                public void RegisterRaidWing(int id, (string nameEN, string nameDE, string avatarURL) basicData)
                {
                    (string nameEN, string nameDE, string avatarURL) = basicData;
                    Raid raid = new Raid(id, nameEN, nameDE, avatarURL);
                    wings.Add(id, raid);
                }

                public void RegisterStrike(int id, (string nameEN, string nameDE, string avatarURL) basicData)
                {
                    (string nameEN, string nameDE, string avatarURL) = basicData;
                    Strike strike = new Strike(id, nameEN, nameDE, avatarURL);
                    strikes.Add(id, strike);
                }

                public void RegisterTraining((string nameEN, string nameDE, string avatarURL) basicData, (string shortNameEN, string shortNameDE) extendedData)
                {
                    (string nameEN, string nameDE, string avatarURL) = basicData;
                    (string shortNameEN, string shortNameDE) = extendedData;
                    Training training = new Training(nameEN, nameDE, shortNameEN, shortNameDE, avatarURL);
                    trainings.Add(training);
                }

                public void RegisterUnkowen((string nameEN, string nameDE, string avatarURL) basicData, (string shortNameEN, string shortNameDE) extendedData)
                {
                    (string nameEN, string nameDE, string avatarURL) = basicData;
                    (string shortNameEN, string shortNameDE) = extendedData;
                    UnkowenGameArea unkowen = new UnkowenGameArea(nameEN, nameDE, shortNameEN, shortNameDE, avatarURL);
                    unknowens.Add(unkowen);
                }

                public void RegisterWvW((string nameEN, string nameDE, string avatarURL) basicData, (string shortNameEN, string shortNameDE) extendedData)
                {
                    (string nameEN, string nameDE, string avatarURL) = basicData;
                    (string shortNameEN, string shortNameDE) = extendedData;
                    WvW wvw = new WvW(nameEN, nameDE, shortNameEN, shortNameDE, avatarURL);
                    wvws.Add(wvw);
                }

                public bool ExitsRaidWing(int id)
                {
                    return wings.ContainsKey(id);
                }

                public bool ExitsStrike(int id)
                {
                    return strikes.ContainsKey(id);
                }

                public bool ExitsFractal(int id)
                {
                    return fractals.ContainsKey(id);
                }

                public bool ExitsWvW()
                {
                    return wvws.Count > 0;
                }

                public bool ExitsDragonResponseMission(int id)
                {
                    return drms.ContainsKey(id);
                }

                public bool ExitsTraining()
                {
                    return wvws.Count > 0;
                }

                public bool ExitsUnkowen()
                {
                    return wvws.Count > 0;
                }
            }

            public class TestBosseRegistrator : IBosseRegistrator
            {
                public Dictionary<int, Boss> Bosses = new Dictionary<int, Boss>();

                public bool Exists(int id)
                {
                    return Bosses.ContainsKey(id);
                }

                public Boss Get(int id)
                {
                    return Bosses[id];
                }

                public void Register((int iD, string nameEN, string nameDE, GameArea gameArea) basicInfo,
                    (string folderNameEN, string folderNameDE, string avatarURL, string discordEmote, string eIName, int raidOrgaPlusID) extendedInfo)
                {
                    (int iD, string nameEN, string nameDE, GameArea gameArea) = basicInfo;
                    (string folderNameEN, string folderNameDE, string avatarURL, string discordEmote, string eIName, int raidOrgaPlusID) = extendedInfo;
                    Boss boss = new Boss(iD, nameEN, nameDE, folderNameEN, folderNameDE, gameArea, avatarURL, discordEmote, eIName, raidOrgaPlusID);
                    Bosses.Add(iD, boss);
                }

                public void Register()
                {
                    var boss = new Boss();
                    Bosses.Add(boss.ID, boss);
                }
            }

            public class TestAddEnemyRegistrator : IAddEnemyRegistrator
            {
                public Dictionary<int, AddEnemy> Adds = new Dictionary<int, AddEnemy>();

                public bool Exists(int id)
                {
                    return Adds.ContainsKey(id);
                }

                public AddEnemy Get(int id)
                {
                    return Adds[id];
                }

                public void Register((int iD, string nameEN, string nameDE, GameArea gameArea) basicInfo, bool intresting)
                {
                    (int iD, string nameEN, string nameDE, GameArea gameArea) = basicInfo;
                    AddEnemy add = new AddEnemy(iD, nameEN, nameDE, gameArea, intresting);
                    Adds.Add(iD, add);
                }

                public void Register()
{
                    AddEnemy add = new AddEnemy();
                    Adds.Add(add.ID, add);
                }
            }

            public class TestMiscDataRegistrator : IMiscDataRegistrator
            {
                public string KillEmote { get; private set; }
                public string WipeEmote { get; private set; }

                public void RegisterKillEmote(string emote)
                {
                    KillEmote = emote;
                }

                public void RegisterWipeEmote(string emote)
                {
                    WipeEmote = emote;
                }
            }
        }
    }

    
}
