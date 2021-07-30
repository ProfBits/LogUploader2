using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NUnit.Framework;

using LogUploader.Data;
using LogUploader.Tools;
using LogUploader.Data.Repositories;
using LogUploader.Data.GameAreas;

namespace LogUploader.Test.Data
{
    internal abstract class AbstractEnemyRepositoryTest<T> where T : Enemy
    {
        protected virtual string DEFAULT_NAME_EN { get => "Enemy"; }
        protected virtual string DEFAULT_NAME_DE { get => "Gegner"; }

        protected (int id, string nameEN, string nameDE, TestGameArea gameArea) GetNubmerElement(int number)
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
        }

        internal abstract EnemyRepository<T> CreateEmptyRepository();
        internal abstract EnemyRepository<T> CreatePreFilledRepository(int number);
        public abstract T CreateNumberedEnemy(int number);

        [Test]
        public void CreateEmptyRepositoryDoesNotThrow()
        {
            Assert.That(() => CreateEmptyRepository(), Throws.Nothing);
            Assert.That(CreateEmptyRepository(), Is.Not.Null);
        }

        [Test]
        public void CreatePreFilledRepositoryDoesNotThrow([Values(1, 2, 5)] int number)
        {
            Assert.That(() => CreatePreFilledRepository(number), Throws.Nothing);
            var repo = CreatePreFilledRepository(number);
            Assert.That(repo, Is.Not.Null);
            EnemyProvider<T> provider = repo;
            
            for (int i = 0; i < number; i++)
            {
                Assert.That(() => provider.Get(i), Throws.Nothing);
                T enemy = provider.Get(i);
                ValidateNumberedEnemy(enemy, GetNubmerElement(i));
            }
        }
        
        [Test]
        public void AddElementToRepository()
        {
            var repo = CreateEmptyRepository();
            T enemy = CreateNumberedEnemy(1);
            Assert.That(() => repo.Add(enemy), Throws.Nothing);
            Assert.That(repo.Get(enemy.ID), Is.EqualTo(enemy));
        }

        [Test]
        public void CannotAddNullToRepository()
        {
            var repo = CreateEmptyRepository();
            TestHelper.ValidateArugumentException(Assert.Catch<ArgumentException>(() => repo.Add(null)));
        }

        [Test]
        public void GetElementByBasicData([Values(0, 1, 2)] int num)
        {
            const int NUM_OF_ELEMENTS = 3;
            var expected = GetNubmerElement(num);
            var repo = CreatePreFilledRepository(NUM_OF_ELEMENTS);
            EnemyProvider<T> provider = repo;

            ValidateNumberedEnemy(provider.Get(expected.id), expected);
            ValidateNumberedEnemy(provider.Get(expected.nameEN), expected);
            ValidateNumberedEnemy(provider.Get(expected.nameDE), expected);
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


        public override Boss CreateNumberedEnemy(int number)
        {
            (int id, string nameEN, string nameDE, TestGameArea gameArea) = GetNubmerElement(number);
            string folderNameEN = GetNumberdString("folderEN", number);
            string folderNameDE = GetNumberdString("folderDE", number);
            string avatarURL = GetNumberdString("avatarUrl", number);
            string discordEmote = GetNumberdString("discordEmote", number);
            string eIName = GetNumberdString("eiName", number);

            return new Boss(id, nameEN, nameDE, folderNameEN, folderNameDE, gameArea, avatarURL, discordEmote, eIName, number);
        }

        internal override EnemyRepository<Boss> CreateEmptyRepository()
        {
            return new BossRepository();
        }

        internal override EnemyRepository<Boss> CreatePreFilledRepository(int number)
        {
            BossRepository repo = new BossRepository();
            for (int i = 0; i < number; i++)
            {
                repo.Add(CreateNumberedEnemy(i));
            }
            return repo;
        }
    }

    internal class AddEnemyRepositoryTest : AbstractEnemyRepositoryTest<AddEnemy>
    {
        public override AddEnemy CreateNumberedEnemy(int number)
        {
            (int id, string nameEN, string nameDE, TestGameArea area) = GetNubmerElement(number);

            return new AddEnemy(id, nameEN, nameDE, area, false);
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
                repo.Add(CreateNumberedEnemy(i));
            }
            return repo;
        }
    }
}
