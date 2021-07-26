using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

using NUnit.Framework;

using LogUploader.Data;
using LogUploader.Data.GameAreas;
using LogUploader.Localisation;
using LogUploader.Data.StaticDataLoader;

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
            TestDataRegistrator.TestRaidWing wing = null;
            Assert.DoesNotThrow(() => wing = TestRegistrator.TestAreas.TestGetRaidWing(1));
            BasicGameAreaAssertions(wing, "RaidEN", "RaidDE", "https://raid.area/icon.png");
        }

        [Test]
        public void DataBuilderStrikeTest()
        {
            TestDataRegistrator.TestStrike strike = null;
            Assert.DoesNotThrow(() => strike = TestRegistrator.TestAreas.TestGetStrike(1));
            BasicGameAreaAssertions(strike, "StrikeEN", "StriekDE", "https://strike.area/icon.png");
        }

        [Test]
        public void DataBuilderFractalTest()
        {
            TestDataRegistrator.TestFractal fractal = null;
            Assert.DoesNotThrow(() => fractal = TestRegistrator.TestAreas.TestGetFractal(1));
            BasicGameAreaAssertions(fractal, "FractalEN", "FractalDE", "https://fractal.area/icon.png");
        }

        [Test]
        public void DataBuilderDrmTest()
        {
            TestDataRegistrator.TestDragonResponseMission drm = null;
            Assert.DoesNotThrow(() => drm = TestRegistrator.TestAreas.TestGetDragonResponseMission(1));
            BasicGameAreaAssertions(drm, "DRMEN", "DRMDE", "https://drm.area/icon.png");
        }

        [Test]
        public void DataBuilderWvWTest()
        {
            TestDataRegistrator.TestWvW wvw = null;
            Assert.DoesNotThrow(() => wvw = TestRegistrator.TestAreas.TestGetWvW());
            BasicGameAreaAssertions(wvw, "WvWEN", "WvWDE", "https://wvw.area/icon.png");
            ExtendedGameAreaAssertions(wvw, "WvWen", "WvWde");
        }

        [Test]
        public void DataBuilderTrainingTest()
        {
            TestDataRegistrator.TestTraining training = null;
            Assert.DoesNotThrow(() => training = TestRegistrator.TestAreas.TestGetTraining());
            BasicGameAreaAssertions(training, "GolemEN", "GolemDE", "https://golem.area/icon.png");
            ExtendedGameAreaAssertions(training, "Golemen", "Golemde");
        }

        [Test]
        public void DataBuilderUnknownTest()
        {
            TestDataRegistrator.TestUnknowen unknowen = null;
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
            TestBoss(0, "B?EN", "B?DE", "Bf?en", "Bf?de", "B?EI", typeof(AbstractUnknowen), ":emote:", "https://unkown.boss/icon.png", -1);
            TestBoss(15438, "Vale Guardian", "Tal Wächter", "Vale Guardianf", "Tal-Wächterf", "Vale Guardiane", typeof(AbstractRaidWing), ":vg:", "https://vg.boss/icon.png", 1);
            TestBoss(22154, "Construct", "Konstrukt", "ConstructF", "KonstruktF", "ConstructEI", typeof(AbstractStrike), ":strike:", "https://construct.boss/icon.png", -1);
            TestBoss(23330, "Destroyer", "Zerstörer", "DestroyerF", "ZerstörerF", "DestroyerEI", typeof(AbstractDragonResponseMission), ":drm:", "https://drm.boss/icon.png", -1);
            TestBoss(1, "WvWEN", "WvWDE", "WvWENf", "WvWDEf", "WvWEI", typeof(AbstractWvW), ":wvw:", "https://wvw.boss/icon.png", -1);
            TestBoss(17021, "fractalEN", "fractalDE", "fractalENf", "fractalDEf", "fractalEI", typeof(AbstractFractal), ":fractal:", "https://fractal.boss/icon.png", -1);
            TestBoss(16202, "GolemEN", "GolemDE", "GolemENf", "GolemDEf", "GolemEI", typeof(AbstractTraining), ":golem:", "https://golem.boss/icon.png", -1);
        }

        private void TestBoss(int id, string nameEN, string nameDE, string folderNameEN, string folderNameDE,
            string eiName, Type areaType, string discordEmote, string avatarURL, int raidOrgaPlusID)
        {
            Assert.That(TestRegistrator.TestBosses.Exists(id), Is.True);
            TestDataRegistrator.TestBoss boss = (TestDataRegistrator.TestBoss)TestRegistrator.TestBosses.Get(id);
            TestBasicEnemy(boss, id, nameEN, nameDE, areaType);
            Assert.That(boss.GetFolderName(eLanguage.EN), Is.EqualTo(folderNameEN));
            Assert.That(boss.GetFolderName(eLanguage.DE), Is.EqualTo(folderNameDE));
            Assert.That(boss.EIName, Is.EqualTo(eiName));
            Assert.That(boss.DiscordEmote, Is.EqualTo(discordEmote));
            Assert.That(boss.AvatarURL, Is.EqualTo(avatarURL));
            Assert.That(boss.RaidOrgaPlusID, Is.EqualTo(raidOrgaPlusID));
        }

        [Test]
        public void DataBuilderAddEnemyTest()
        {
            Assert.That(TestRegistrator.TestAddEnemies.Adds.Count, Is.EqualTo(3));
            TestAdd(0, "Add?EN", "Add?DE", typeof(AbstractUnknowen), true);
            TestAdd(2, "IntrestingEN", "IntrestingDE", typeof(AbstractRaidWing), true);
            TestAdd(3, "UnintrestingEN", "UnintrestingDE", typeof(AbstractRaidWing), false);
        }


        private void TestAdd(int id, string nameEN, string nameDE, Type areaType, bool interesting)
        {
            Assert.That(TestRegistrator.TestAddEnemies.Exists(id), Is.True);
            TestDataRegistrator.TestAddEnemy add = (TestDataRegistrator.TestAddEnemy)TestRegistrator.TestAddEnemies.Get(id);
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
            public IAreaRegistrator SetAreas { get => TestAreas; }
            public IBosseRegistrator SetBosses { get => TestBosses; }
            public IAddEnemyRegistrator SetAddEnemies { get => TestAddEnemies; }
            public IMiscDataRegistrator SetMiscData { get => TestMiscData; }
            public IAreaRepository Areas { get => SetAreas; }
            public IBosseRepository Bosses { get => SetBosses; }
            public IAddEnemyRepository AddEnemies { get => SetAddEnemies; }
            public IMiscDataRepository MiscData { get => SetMiscData; }

            public TestDataRegistrator()
            {
                TestAreas = new TestAreaRegistrator();
                TestBosses = new TestBosseRegistrator();
                TestAddEnemies = new TestAddEnemyRegistrator();
                TestMiscData = new TestMiscDataRegistrator();
            }

            public class TestAreaRegistrator : IAreaRegistrator
            {
                private Dictionary<int, TestDragonResponseMission> drms = new Dictionary<int, TestDragonResponseMission>();
                private Dictionary<int, TestFractal> fractals = new Dictionary<int, TestFractal>();
                private Dictionary<int, TestRaidWing> wings = new Dictionary<int, TestRaidWing>();
                private Dictionary<int, TestStrike> strikes = new Dictionary<int, TestStrike>();
                private List<TestTraining> trainings = new List<TestTraining>();
                private List<TestUnknowen> unknowens = new List<TestUnknowen>();
                private List<TestWvW> wvws = new List<TestWvW>();

                public AbstractDragonResponseMission GetDragonResponseMission(int id) => TestGetDragonResponseMission(id);

                public AbstractFractal GetFractal(int id) => TestGetFractal(id);

                public AbstractRaidWing GetRaidWing(int id) => TestGetRaidWing(id);

                public AbstractStrike GetStrike(int id) => TestGetStrike(id);

                public AbstractTraining GetTraining() => TestGetTraining();

                public AbstractUnknowen GetUnkowen() => TestGetUnkowen();

                public AbstractWvW GetWvW() => TestGetWvW();

                public TestDragonResponseMission TestGetDragonResponseMission(int id)
                {
                    return drms[id];
                }

                public TestFractal TestGetFractal(int id)
                {
                    return fractals[id];
                }

                public TestRaidWing TestGetRaidWing(int id)
                {
                    return wings[id];
                }

                public TestStrike TestGetStrike(int id)
                {
                    return strikes[id];
                }

                public TestTraining TestGetTraining()
                {
                    return trainings.FirstOrDefault();
                }

                public TestUnknowen TestGetUnkowen()
                {
                    return unknowens.FirstOrDefault();
                }

                public TestWvW TestGetWvW()
                {
                    return wvws.FirstOrDefault();
                }

                public void RegisterDragonResponseMission(GameArea.BasicInfo basicInfo, int id)
                {
                    drms.Add(id, new TestDragonResponseMission(basicInfo, id));
                }

                public void RegisterFractal(GameArea.BasicInfo basicInfo, int id)
                {
                    fractals.Add(id, new TestFractal(basicInfo, id));
                }

                public void RegisterRaidWing(GameArea.BasicInfo basicInfo, int id)
                {
                    wings.Add(id, new TestRaidWing(basicInfo, id));
                }

                public void RegisterStrike(GameArea.BasicInfo basicInfo, int id)
                {
                    strikes.Add(id, new TestStrike(basicInfo, id));
                }

                public void RegisterTraining(GameArea.ExtendedInfo basicInfo)
                {
                    trainings.Add(new TestTraining(basicInfo));
                }

                public void RegisterUnkowen(GameArea.ExtendedInfo basicInfo)
                {
                    unknowens.Add(new TestUnknowen(basicInfo));
                }

                public void RegisterWvW(GameArea.ExtendedInfo basicInfo)
                {
                    wvws.Add(new TestWvW(basicInfo));
                }
            }

            public class TestDragonResponseMission : AbstractDragonResponseMission
            {
                public TestDragonResponseMission(IBasicInfo info, int number) : base(info, number)
                {
                }
            }

            public class TestFractal : AbstractFractal
            {
                public TestFractal(IBasicInfo info, int number) : base(info, number)
                {
                }
            }

            public class TestRaidWing : AbstractRaidWing
            {
                public TestRaidWing(IBasicInfo info, int number) : base(info, number)
                {
                }
            }

            public class TestStrike : AbstractStrike
            {
                public TestStrike(IBasicInfo info, int number) : base(info, number)
                {
                }
            }

            public class TestTraining : AbstractTraining
            {
                public TestTraining(IExtendedInfo info) : base(info)
                {
                }
            }

            public class TestUnknowen : AbstractUnknowen
            {
                public TestUnknowen(IExtendedInfo info) : base(info)
                {
                }
            }

            public class TestWvW : AbstractWvW
            {
                public TestWvW(IExtendedInfo info) : base(info)
                {
                }
            }

            public class TestBosseRegistrator : IBosseRegistrator
            {
                public Dictionary<int, TestBoss> Bosses = new Dictionary<int, TestBoss>();

                public bool Exists(int id)
                {
                    return Bosses.ContainsKey(id);
                }

                public AbstractBoss Get(int id)
                {
                    return Bosses[id];
                }

                public void Register(Enemy.BasicInfo info, string FolderNameEN, string FolderNameDE, string avatarURL, string discordEmote, string eIName, int raidOrgaPlusID)
                {
                    Bosses.Add(info.ID, new TestBoss(info, FolderNameEN, FolderNameDE, avatarURL, discordEmote, eIName, raidOrgaPlusID));
                }

                public void Register()
                {
                    var boss = new TestBoss();
                    Bosses.Add(boss.ID, boss);
                }
            }

            public class TestBoss : AbstractBoss
            {
                public TestBoss() : base()
                { }

                public TestBoss(BasicInfo info, string FolderNameEN, string FolderNameDE, string avatarURL, string discordEmote, string eIName, int raidOrgaPlusID) :
                    base(info, FolderNameEN, FolderNameDE, avatarURL, discordEmote, eIName, raidOrgaPlusID)
                { }

                internal string GetFolderName(eLanguage lang)
                {
                    return m_FolderName.GetName(lang);
                }
            }

            public class TestAddEnemyRegistrator : IAddEnemyRegistrator
            {
                public Dictionary<int, TestAddEnemy> Adds = new Dictionary<int, TestAddEnemy>();

                public bool Exists(int id)
                {
                    return Adds.ContainsKey(id);
                }

                public AbstractAddEnemy Get(int id)
                {
                    return Adds[id];
                }

                public void Register(Enemy.BasicInfo info, bool intresting)
                {
                    Adds.Add(info.ID, new TestAddEnemy(info, intresting));
                }

                public void Register()
                {
                    var add = new TestAddEnemy();
                    Adds.Add(add.ID, add);
                }
            }

            public class TestAddEnemy : AbstractAddEnemy
            {
                public TestAddEnemy() : base()
                { }

                public TestAddEnemy(BasicInfo info, bool interesting) :
                    base(info, interesting)
                { }
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
