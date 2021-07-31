using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NUnit.Framework;

using LogUploader.Data;
using LogUploader.Tools;
using LogUploader.Data.Repositories;
using LogUploader.Data.GameAreas;
using LogUploader.Localisation;
using Newtonsoft.Json.Linq;

namespace LogUploader.Test.Data
{
    internal abstract class AbstractEnemyRepositoryTest<T> where T : Enemy
    {
        protected const string DEFAULT_NAME_EN = "Enemy";
        protected const string DEFAULT_NAME_DE = "Gegner";

        protected (int id, string nameEN, string nameDE, TestGameArea gameArea) GetNumberedBaseData(int number)
        {
            return (number, GetNumberdString(DEFAULT_NAME_EN, number), GetNumberdString(DEFAULT_NAME_DE, number), new TestGameArea(nameof(T), number));
        }

        protected string GetNumberdString(string str, int number)
        {
            return $"{str}_{number}";
        }

        [OneTimeSetUp]
        public virtual void OneTimeSetUp()
        {
            Assume.That(() => GP.ValidateStringOneWord(DEFAULT_NAME_EN), Throws.Nothing);
            Assume.That(() => GP.ValidateStringOneWord(DEFAULT_NAME_DE), Throws.Nothing);
            Assume.That(DEFAULT_NAME_EN, Is.Not.EqualTo(DEFAULT_NAME_DE));
        }

        internal abstract EnemyRepository<T> CreateEmptyRepository();
        internal abstract EnemyRepository<T> CreatePreFilledRepository(int number);
        internal abstract T CreateNumberedEnemyObject(int number);
        internal abstract T CreateNumberedEnemyObject(int number, string nameEN, string nameDE, GameArea gameArea);

        [Test]
        public void CreateEmptyRepositoryDoesNotThrowTest()
        {
            Assert.That(() => CreateEmptyRepository(), Throws.Nothing);
            Assert.That(CreateEmptyRepository(), Is.Not.Null);
        }

        [Test]
        public void CreatePreFilledRepositoryDoesNotThrowTest([Values(1, 2, 5)] int number)
        {
            Assert.That(() => CreatePreFilledRepository(number), Throws.Nothing);
            var repo = CreatePreFilledRepository(number);
            Assert.That(repo, Is.Not.Null);
            EnemyProvider<T> provider = repo;
            
            for (int i = 0; i < number; i++)
            {
                Assert.That(() => provider.Get(i), Throws.Nothing);
                T enemy = provider.Get(i);
                ValidateNumberedEnemy(enemy, GetNumberedBaseData(i));
            }
        }
        
        [Test]
        public void AddElementToRepositoryTest()
        {
            var repo = CreateEmptyRepository();
            T enemy = CreateNumberedEnemyObject(1);
            Assert.That(() => repo.Add(enemy), Throws.Nothing);
            Assert.That(repo.Get(enemy.ID), Is.EqualTo(enemy));
        }

        [Test]
        public void CannotAddNullToRepositoryTest()
        {
            var repo = CreateEmptyRepository();
            TestHelper.ValidateArugumentException(Assert.Catch<ArgumentException>(() => repo.Add(null)));
        }

        [Test]
        public void GetElementByBasicDataTest([Values(0, 1, 2)] int num)
        {
            const int NUM_OF_ELEMENTS = 3;
            var expected = GetNumberedBaseData(num);
            var repo = CreatePreFilledRepository(NUM_OF_ELEMENTS);
            EnemyProvider<T> provider = repo;

            ValidateNumberedEnemy(provider.Get(expected.id), expected);
            ValidateNumberedEnemy(provider.Get(expected.nameEN), expected);
            ValidateNumberedEnemy(provider.Get(expected.nameDE), expected);
        }

        [Test]
        public void GetEnmiesByAreaData()
        {
            GameArea area = new TestGameArea("Area1", 1);
            GameArea area2 = new TestGameArea("Area2", 2);
            var repo = CreateEmptyRepository();
            var enemy1 = CreateNumberedEnemyObject(1, "en1", "de1", area);
            var enemy2 = CreateNumberedEnemyObject(2, "en2", "de2", area);
            repo.Add(enemy1);
            repo.Add(enemy2);
            EnemyProvider<T> provider = repo;

            Assert.That(() => provider.Get(area), Throws.Nothing);
            Assert.That(provider.Get(area), Is.Not.Null);
            Assert.That(provider.Get(area), Has.Exactly(2).Items);
            Assert.That(provider.Get(area), Does.Contain(enemy1));
            Assert.That(provider.Get(area), Does.Contain(enemy2));

            Assert.That(() => provider.Get(area2), Throws.Nothing);
            Assert.That(provider.Get(area2), Is.Not.Null);
            Assert.That(provider.Get(area2), Is.Empty);

            Assert.That(() => provider.Get((GameArea)null), Throws.Nothing);
            Assert.That(provider.Get((GameArea)null), Is.Not.Null);
            Assert.That(provider.Get((GameArea)null), Is.Empty);
        }

        [Test]
        public void ExistsTest()
        {
            const int NUM_OF_ELEMENTS = 1;
            var expected = GetNumberedBaseData(NUM_OF_ELEMENTS - 1);
            var repo = CreatePreFilledRepository(NUM_OF_ELEMENTS);
            EnemyProvider<T> provider = repo;

            Assert.That(provider.Exists(expected.id), Is.True);
            Assert.That(provider.Exists(expected.nameEN), Is.True);
            Assert.That(provider.Exists(expected.nameDE), Is.True);
            Assert.That(provider.Exists(expected.nameEN, eLanguage.EN), Is.True);
            Assert.That(provider.Exists(expected.nameDE, eLanguage.DE), Is.True);
        }

        [Test]
        public void ExistsNotTest()
        {
            const int NUM_OF_ELEMENTS = 1;
            var expected = GetNumberedBaseData(NUM_OF_ELEMENTS + 1);
            var repo = CreatePreFilledRepository(NUM_OF_ELEMENTS);
            ExistsNotAsserts(expected, repo);
        }

        [Test]
        public void ExistNotEmptyTest()
        {
            var expected = GetNumberedBaseData(1);
            var repo = CreateEmptyRepository();
            ExistsNotAsserts(expected, repo);
        }

        [Test]
        public void EnumreableEnemyRepositoryTest([Values(0, 1, 5)] int numberOfElements)
        {
            var repo = CreatePreFilledRepository(numberOfElements);
            Assert.That(repo.GetEnumerator(), Is.Not.Null);
            Assert.That(repo.Count(), Is.EqualTo(numberOfElements));
            for (int i = 0; i < numberOfElements; i++)
            {
                Assert.That(repo, Does.Contain(CreateNumberedEnemyObject(i)));
            }
        }

        [Test]
        public void EnemyRepositoryCountTest([Values(0, 1, 5)] int numberOfElements)
        {
            var repo = CreatePreFilledRepository(numberOfElements);
            Assert.That(repo.Count, Is.EqualTo(numberOfElements));
        }


        private static void ExistsNotAsserts((int id, string nameEN, string nameDE, TestGameArea gameArea) expected, EnemyProvider<T> provider)
        {
            Assert.That(() => provider.Exists(-1), Throws.Nothing);
            Assert.That(() => provider.Exists(null), Throws.Nothing);

            Assert.That(() => provider.Exists(expected.id), Throws.Nothing);
            Assert.That(() => provider.Exists(expected.nameEN), Throws.Nothing);
            Assert.That(() => provider.Exists(expected.nameDE), Throws.Nothing);
            Assert.That(provider.Exists(expected.id), Is.False);
            Assert.That(provider.Exists(expected.nameEN), Is.False);
            Assert.That(provider.Exists(expected.nameDE), Is.False);
            Assert.That(provider.Exists(expected.nameEN, eLanguage.DE), Is.False);
            Assert.That(provider.Exists(expected.nameDE, eLanguage.EN), Is.False);
        }

        protected void ValidateNumberedEnemy(T enemy, (int id, string nameEN, string nameDE, TestGameArea gameArea) values)
        {
            Assert.That(enemy.ID, Is.EqualTo(values.id));
            Assert.That(enemy.NameEN, Is.EqualTo(values.nameEN));
            Assert.That(enemy.NameDE, Is.EqualTo(values.nameDE));
            Assert.That(enemy.Area, Is.EqualTo(values.gameArea));
        }
    }


    internal class BossRepositoryTest : AbstractEnemyRepositoryTest<Boss>
    {
        private const string DEFAULT_FOLDER_NAME_EN = "foderEN";
        private const string DEFAULT_FOLDER_NAME_DE = "foderDE";
        private const string DEFAULT_EI = "eiName";
        private const string DEFAULT_AVATAR_URL = "avatarUrl";
        private const string DEFAULT_EMOTE = ":emote:";

        private (string folderNameEN, string folderNameDE, string eiName, string avatarUrl, string emote) GetNumberedExtededData(int number)
        {
            string folderNameEN = $"{DEFAULT_FOLDER_NAME_EN}_{number}";
            string folderNameDE = $"{DEFAULT_FOLDER_NAME_DE}_{number}";
            string eiName = $"{DEFAULT_EI}_{number}";
            string avatarUrl = $"{DEFAULT_AVATAR_URL}_{number}";
            string emote = DEFAULT_EMOTE;
            return (folderNameEN, folderNameDE, eiName, avatarUrl, emote);
        }

        internal override Boss CreateNumberedEnemyObject(int number)
        {
            (int id, string nameEN, string nameDE, TestGameArea gameArea) = GetNumberedBaseData(number);
            return CreateNumberedEnemyObject(id, nameEN, nameDE, gameArea);
        }

        internal override Boss CreateNumberedEnemyObject(int number, string nameEN, string nameDE, GameArea gameArea)
        {
            (string folderNameEN, string folderNameDE, string eiName, string avatarUrl, string emote) = GetNumberedExtededData(number);
            return CreateNumberedEnemyObjectFree(number, nameEN, nameDE, folderNameEN, folderNameDE, gameArea, avatarUrl, emote, eiName, number);
        }

        private Boss CreateNumberedEnemyObjectFree(int id = 1, string nameEN = DEFAULT_NAME_EN, string nameDE = DEFAULT_NAME_DE,
            string folderNameEN = DEFAULT_FOLDER_NAME_EN, string folderNameDE = DEFAULT_FOLDER_NAME_DE, GameArea gameArea = null,
            string avatarUrl = DEFAULT_AVATAR_URL, string emote = DEFAULT_EMOTE, string eiName = DEFAULT_EI, int ropID = 0)
        {
            return new Boss(id, nameEN, nameDE, folderNameEN, folderNameDE, gameArea, avatarUrl, emote, eiName, ropID);
        }

        internal override EnemyRepository<Boss> CreateEmptyRepository() => CreateEmptyBossRepository();
        private BossRepository CreateEmptyBossRepository()
        {
            return new BossRepository();
        }

        internal override EnemyRepository<Boss> CreatePreFilledRepository(int number) => CreatePreFilledBossRepository(number);
        private BossRepository CreatePreFilledBossRepository(int number)
        {
            BossRepository repo = new BossRepository();
            for (int i = 0; i < number; i++)
            {
                repo.Add(CreateNumberedEnemyObject(i));
            }
            return repo;
        }


        [Test]
        public void GetBossByExtendedDataTest([Values(0, 1, 2)] int num)
        {
            const int NUM_OF_ELEMENTS = 3;
            var expected = GetNumberedExtededData(num);
            var repo = CreatePreFilledBossRepository(NUM_OF_ELEMENTS);
            BossProvider provider = repo;

            ValidateNumberedBoss(provider.GetByFolderName(expected.folderNameDE), expected);
            ValidateNumberedBoss(provider.GetByFolderName(expected.folderNameEN), expected);
            ValidateNumberedBoss(provider.GetByFolderName(expected.folderNameDE, eLanguage.DE), expected);
            ValidateNumberedBoss(provider.GetByFolderName(expected.folderNameEN, eLanguage.EN), expected);
            ValidateNumberedBoss(provider.GetByEiName(expected.eiName), expected);
        }

        [Test]
        public void GetBossByRaidOrgaPlusIDTest()
        {
            var expected = CreateNumberedEnemyObject(1);
            var ropIDDupe = CreateNumberedEnemyObjectFree(2, gameArea: new TestGameArea("BossRepoRopIDTest", 1), ropID: expected.RaidOrgaPlusID);
            var repo = CreateEmptyBossRepository();
            repo.Add(expected);
            BossProvider provider = repo;

            Assert.That(provider.GetByRaidOrgaPlusID(expected.ID), Is.EqualTo(expected));
            Assert.That(() => provider.GetByRaidOrgaPlusID(expected.ID + 1), Throws.InstanceOf<System.Collections.Generic.KeyNotFoundException>());
            repo.Add(ropIDDupe);
            TestHelper.ValidateArugumentException(Assert.Catch<ArgumentException>(() => provider.GetByRaidOrgaPlusID(expected.ID)));
        }

        [Test]
        public void GetBossByeBossTest()
        {
            eBosses contained = eBosses.ValeGuardian;
            eBosses notContained = eBosses.Gorseval;
            Assume.That(contained, Is.Not.EqualTo(notContained));
            
            Boss expected = CreateNumberedEnemyObject((int)contained);
            BossRepository repo = CreateEmptyBossRepository();
            repo.Add(expected);
            BossProvider provider = repo;

            Assert.That(provider.Get(contained), Is.EqualTo(expected));
            Assert.That(() => provider.Get(notContained), Throws.InstanceOf<System.Collections.Generic.KeyNotFoundException>());

        }

        [Test, Category(TestCategory.Regression)]
        public void GetSunquaAiFakeIDOverride([Values(23255, 23256)] int fakeAiId)
        {
            eBosses Ai = eBosses.Ai;

            Boss expected = CreateNumberedEnemyObject((int)Ai);
            BossRepository repo = CreateEmptyBossRepository();
            repo.Add(expected);
            BossProvider provider = repo;

            Assert.That(provider.Get(fakeAiId), Is.EqualTo(expected));

            Boss fakeAi = CreateNumberedEnemyObject(fakeAiId);
            TestHelper.ValidateArugumentException(Assert.Catch<ArgumentOutOfRangeException>(() => repo.Add(fakeAi)));


            Assert.That(provider.Exists(fakeAiId), Is.True);
        }


        [Test, Category(TestCategory.Regression)]
        public void GetBrokenKingFolderNameDEOverride([Values("Gebrochener König", "Bezwungener König")] string bokenKingFolderNamesDE)
        {
            eBosses BrokenKing = eBosses.BrokenKing;

            Boss expected = CreateNumberedEnemyObject((int)BrokenKing);
            BossRepository repo = CreateEmptyBossRepository();
            repo.Add(expected);
            BossProvider provider = repo;

            Assert.That(provider.GetByFolderName(bokenKingFolderNamesDE), Is.EqualTo(expected));
            Assert.That(provider.GetByFolderName(bokenKingFolderNamesDE, eLanguage.DE), Is.EqualTo(expected));
        }

        private void ValidateNumberedBoss(Boss boss, (string folderNameEN, string folderNameDE, string eiName, string avatarUrl, string emote) values)
        {
            Assert.That(boss.FolderNameEN, Is.EqualTo(values.folderNameEN));
            Assert.That(boss.FolderNameDE, Is.EqualTo(values.folderNameDE));
            Assert.That(boss.EIName, Is.EqualTo(values.eiName));
            Assert.That(boss.AvatarURL, Is.EqualTo(values.avatarUrl));
            Assert.That(boss.DiscordEmote, Is.EqualTo(values.emote));
        }

        private void ValidateNumberedBoss(Boss boss,
            (string folderNameEN, string folderNameDE, string eiName, string avatarUrl, string emote) values,
            (int id, string nameEN, string nameDE, TestGameArea gameArea) basicValues)
        {
            ValidateNumberedEnemy(boss, basicValues);
            ValidateNumberedBoss(boss, values);
        }
    }

    internal class AddEnemyRepositoryTest : AbstractEnemyRepositoryTest<AddEnemy>
    {
        private const bool DEFAULT_INTRESTRING = true;

        internal override AddEnemy CreateNumberedEnemyObject(int number)
        {
            (int id, string nameEN, string nameDE, TestGameArea gameArea) = GetNumberedBaseData(number);
            return CreateNumberedEnemyObject(id, nameEN, nameDE, gameArea);
        }

        internal override AddEnemy CreateNumberedEnemyObject(int number, string nameEN, string nameDE, GameArea gameArea)
        {
            bool isIntresting = DEFAULT_INTRESTRING;
            return new AddEnemy(number, nameEN, nameDE, gameArea, isIntresting);
        }

        internal override EnemyRepository<AddEnemy> CreateEmptyRepository()
        {
            return new AddEnemyRepository();
        }

        internal override EnemyRepository<AddEnemy> CreatePreFilledRepository(int number)
        {
            AddEnemyRepository repo = new AddEnemyRepository();
            for (int i = 0; i < number; i++)
            {
                repo.Add(CreateNumberedEnemyObject(i));
            }
            return repo;
        }
    }
}
