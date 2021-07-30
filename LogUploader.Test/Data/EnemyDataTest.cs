using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NUnit.Framework;

using LogUploader.Data;
using LogUploader.Data.GameAreas;

namespace LogUploader.Test.Data
{
    public abstract class AbstractEnemyDataTest<T> where T : Enemy
    {
        protected const string DEFAULT_NAME_EN = "EnemyNameEN";
        protected const string DEFAULT_NAME_DE = "EnemyNameDE";
        internal readonly TestGameArea DEFAULT_AREA = new TestGameArea($"{nameof(T)}Area", 1);

        internal abstract T CreateNumberedEnemyObject(int number);
        internal abstract T CreateNumberedEnemyObject(int number, string nameEN, string nameDE, GameArea gameArea);


        [Test]
        public void CreateEnemyObjectTest()
        {
            Assert.That(() => CreateNumberedEnemyObject(1), Throws.Nothing);
        }

        [Test]
        public void CreateEnemyWithNullBasicTest()
        {
            TestHelper.ValidateArugumentException(Assert.Catch<ArgumentNullException>(() => CreateNumberedEnemyObject(1, null, DEFAULT_NAME_DE, DEFAULT_AREA)));
            TestHelper.ValidateArugumentException(Assert.Catch<ArgumentNullException>(() => CreateNumberedEnemyObject(1, DEFAULT_NAME_EN, null, DEFAULT_AREA)));
            TestHelper.ValidateArugumentException(Assert.Catch<ArgumentNullException>(() => CreateNumberedEnemyObject(1, DEFAULT_NAME_EN, DEFAULT_NAME_DE, null)));
            TestHelper.ValidateArugumentException(Assert.Catch<ArgumentNullException>(() => CreateNumberedEnemyObject(1, null, null, DEFAULT_AREA)));
            TestHelper.ValidateArugumentException(Assert.Catch<ArgumentNullException>(() => CreateNumberedEnemyObject(1, DEFAULT_NAME_EN, null, null)));
            TestHelper.ValidateArugumentException(Assert.Catch<ArgumentNullException>(() => CreateNumberedEnemyObject(1, null, DEFAULT_NAME_DE, null)));
            TestHelper.ValidateArugumentException(Assert.Catch<ArgumentNullException>(() => CreateNumberedEnemyObject(1, null, null, null)));
        }

        [Test]
        public void CreateEnemyInvalidArgumentsTest([ValueSource(typeof(TestHelper), nameof(TestHelper.InvalidMulitWordStrings))] string invalidName)
        {
            TestHelper.ValidateArugumentException(Assert.Catch<ArgumentException>(() => CreateNumberedEnemyObject(-1, DEFAULT_NAME_EN, DEFAULT_NAME_DE, DEFAULT_AREA), "BossID should be >= 0."));
            TestHelper.ValidateArugumentException(Assert.Catch<ArgumentException>(() => CreateNumberedEnemyObject(1, invalidName, DEFAULT_NAME_DE, DEFAULT_AREA)));
            TestHelper.ValidateArugumentException(Assert.Catch<ArgumentException>(() => CreateNumberedEnemyObject(1, DEFAULT_NAME_EN, invalidName, DEFAULT_AREA)));
        }

        [Test]
        public void EnemyConstructorValueAssigmentTest()
        {
            const int number = 1;
            var exptected = GetNumberedBaseData(number);
            T enemy = CreateNumberedEnemyObject(number);
            Assert.That(enemy.ID, Is.EqualTo(exptected.id));
            Assert.That(enemy.NameEN, Is.EqualTo(exptected.nameEN));
            Assert.That(enemy.NameDE, Is.EqualTo(exptected.nameDE));
            Assert.That(enemy.Area, Is.EqualTo(exptected.gameArea));
        }

        public abstract void ExtendedEnemyConstructorValueAssigmentTest();

        internal (int id, string nameEN, string nameDE, TestGameArea gameArea) GetNumberedBaseData(int number)
        {
            return (number, $"{DEFAULT_NAME_EN}_{number}", $"{DEFAULT_NAME_DE}_{number}", DEFAULT_AREA);
        }

    }

    public class AddEnemyDataTest : AbstractEnemyDataTest<AddEnemy>
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

        [Test]
        public override void ExtendedEnemyConstructorValueAssigmentTest()
        {

            const int number = 1;
            AddEnemy enemy = CreateNumberedEnemyObject(number);
            Assert.That(enemy.IsInteresting, Is.EqualTo(DEFAULT_INTRESTRING));
        }

    }

    public class BossDataTest : AbstractEnemyDataTest<Boss>
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

        [Test]
        public void CreateBossWithNullExtendedTest()
        {
            TestHelper.ValidateArugumentException(Assert.Catch<ArgumentNullException>(() => CreateNumberedEnemyObjectFree(gameArea: DEFAULT_AREA, folderNameEN: null)));
            TestHelper.ValidateArugumentException(Assert.Catch<ArgumentNullException>(() => CreateNumberedEnemyObjectFree(gameArea: DEFAULT_AREA, folderNameDE: null)));
            TestHelper.ValidateArugumentException(Assert.Catch<ArgumentNullException>(() => CreateNumberedEnemyObjectFree(gameArea: DEFAULT_AREA, avatarUrl: null)));
            TestHelper.ValidateArugumentException(Assert.Catch<ArgumentNullException>(() => CreateNumberedEnemyObjectFree(gameArea: DEFAULT_AREA, emote: null)));
            TestHelper.ValidateArugumentException(Assert.Catch<ArgumentNullException>(() => CreateNumberedEnemyObjectFree(gameArea: DEFAULT_AREA, eiName: null)));
        }

        [Test]
        public void CreateBossInvalidArgumentsExtendedTest([ValueSource(typeof(TestHelper), nameof(TestHelper.InvalidMulitWordStrings))] string invalidName)
        {
            TestHelper.ValidateArugumentException(Assert.Catch<ArgumentException>(() => CreateNumberedEnemyObjectFree(gameArea: DEFAULT_AREA, folderNameEN: invalidName)));
            TestHelper.ValidateArugumentException(Assert.Catch<ArgumentException>(() => CreateNumberedEnemyObjectFree(gameArea: DEFAULT_AREA, folderNameDE: invalidName)));
            TestHelper.ValidateArugumentException(Assert.Catch<ArgumentException>(() => CreateNumberedEnemyObjectFree(gameArea: DEFAULT_AREA, avatarUrl: invalidName)));
            TestHelper.ValidateArugumentException(Assert.Catch<ArgumentException>(() => CreateNumberedEnemyObjectFree(gameArea: DEFAULT_AREA, eiName: invalidName)));
        }

        [Test]
        public void CreateBossInvalidEmoteTest([ValueSource(typeof(TestHelper), nameof(TestHelper.InvalidDiscordEmotes))] string invalidEmote)
        {
            TestHelper.ValidateArugumentException(Assert.Catch<ArgumentException>(() => CreateNumberedEnemyObjectFree(gameArea: DEFAULT_AREA, emote: invalidEmote)));
        }

        [Test]
        public void CreateBossValidEmoteTest([ValueSource(typeof(TestHelper), nameof(TestHelper.ValidDiscordEmotes))] string validEmote)
        {
            Assert.That(() => CreateNumberedEnemyObjectFree(gameArea: DEFAULT_AREA, emote: validEmote), Throws.Nothing);
        }

        [Test]
        public override void ExtendedEnemyConstructorValueAssigmentTest()
        {
            const int number = 1;
            var exptected = GetNumberedExtededData(number);
            Boss boss = CreateNumberedEnemyObject(number: number);
            Assert.That(boss.FolderNameEN, Is.EqualTo(exptected.folderNameEN));
            Assert.That(boss.FolderNameDE, Is.EqualTo(exptected.folderNameDE));
            Assert.That(boss.EIName, Is.EqualTo(exptected.eiName));
            Assert.That(boss.AvatarURL, Is.EqualTo(exptected.avatarUrl));
            Assert.That(boss.DiscordEmote, Is.EqualTo(exptected.emote));
        }
    }
}
