using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LogUploader.Data;
using LogUploader.Data.Repositories;

using NUnit.Framework;

namespace LogUploader.Test.Data
{
    [Parallelizable]
    public class MiscDataTest
    {
        private const string EMOTE_KILL = ":kill:";
        private const string EMOTE_WIPE = ":wipe:";

        [Test, Pairwise]
        public void MiscDataConstructorTest(
            [ValueSource(typeof(TestHelper), nameof(TestHelper.ValidDiscordEmotes))] string killEmote, 
            [ValueSource(typeof(TestHelper), nameof(TestHelper.ValidDiscordEmotes))] string wipeEmote
            )
        {
            MiscData miscData = new MiscData(killEmote, wipeEmote);
            Assert.That(miscData.EmoteKill, Is.EqualTo(killEmote));
            Assert.That(miscData.EmoteWipe, Is.EqualTo(wipeEmote));
        }

        [Test, Pairwise]
        public void MiscDataConstructorInvaldiKillEmotArgTest(
            [ValueSource(typeof(TestHelper), nameof(TestHelper.InvalidDiscordEmotes))] string killEmote,
            [ValueSource(typeof(TestHelper), nameof(TestHelper.ValidDiscordEmotes))] string wipeEmote
            )
        {
            ArgumentException ex = Assert.Catch<ArgumentException>(() => new MiscData(killEmote, wipeEmote));
            TestHelper.ValidateArugumentException(ex);
        }

        [Test, Pairwise]
        public void MiscDataConstructorInvaldiWipeEmoteArgTest(
            [ValueSource(typeof(TestHelper), nameof(TestHelper.ValidDiscordEmotes))] string killEmote,
            [ValueSource(typeof(TestHelper), nameof(TestHelper.InvalidDiscordEmotes))] string wipeEmote
            )
        {
            ArgumentException ex = Assert.Catch<ArgumentException>(() => new MiscData(killEmote, wipeEmote));
            TestHelper.ValidateArugumentException(ex);
        }

        [Test]
        public void MiscDataEqualsTest()
        {
            MiscData a = new MiscData(EMOTE_KILL, EMOTE_WIPE);
            MiscData b = new MiscData(EMOTE_KILL, EMOTE_WIPE);
            MiscData c = new MiscData(":kill2:", EMOTE_WIPE);
            MiscData d = new MiscData(EMOTE_KILL, ":wipe2:");
            MiscData e = new MiscData(":kill2:", ":wipe2:");
            MiscData f = new MiscData(EMOTE_KILL, EMOTE_WIPE);

            Assert.That(a, Is.EqualTo(a));
            Assert.That(a, Is.EqualTo(b));
            Assert.That(b, Is.EqualTo(f));
            Assert.That(a, Is.EqualTo(f));
            Assert.That(a, Is.Not.EqualTo(c));
            Assert.That(a, Is.Not.EqualTo(d));
            Assert.That(a, Is.Not.EqualTo(e));
            Assert.That(a, Is.Not.EqualTo(null));
            Assert.That(a, Is.Not.EqualTo(new object()));
        }

        [Test]
        public void MiscDataGetHashCodeTest()
        {
            MiscData a = new MiscData(EMOTE_KILL, EMOTE_WIPE);
            MiscData b = new MiscData(EMOTE_KILL, EMOTE_WIPE);
            MiscData c = new MiscData(":kill2:", EMOTE_WIPE);
            MiscData d = new MiscData(EMOTE_KILL, ":wipe2:");
            MiscData e = new MiscData(":kill2:", ":wipe2:");
            MiscData f = new MiscData(EMOTE_KILL, EMOTE_WIPE);

            Assert.That(a.GetHashCode(), Is.EqualTo(a.GetHashCode()));
            Assert.That(a.GetHashCode(), Is.EqualTo(b.GetHashCode()));
            Assert.That(b.GetHashCode(), Is.EqualTo(f.GetHashCode()));
            Assert.That(a.GetHashCode(), Is.EqualTo(f.GetHashCode()));
            Assert.That(a.GetHashCode(), Is.Not.EqualTo(c.GetHashCode()));
            Assert.That(a.GetHashCode(), Is.Not.EqualTo(d.GetHashCode()));
            Assert.That(a.GetHashCode(), Is.Not.EqualTo(e.GetHashCode()));
        }

        [Test]
        public void MiscDataRepositorySetKillTest([ValueSource(typeof(TestHelper), nameof(TestHelper.ValidDiscordEmotes))] string emote)
        {
            MiscRepository repo = new MiscRepository();
            string temp = repo.WipeEmote;
            repo.KillEmote = emote;
            Assert.That(((MiscProvider)repo).KillEmote, Is.EqualTo(emote));
            Assert.That(temp, Is.EqualTo(repo.WipeEmote));
        }

        [Test]
        public void MiscDataRepositorySetWipeTest([ValueSource(typeof(TestHelper), nameof(TestHelper.ValidDiscordEmotes))] string emote)
        {
            MiscRepository repo = new MiscRepository();
            string temp = repo.KillEmote;
            repo.WipeEmote = emote;
            Assert.That(((MiscProvider)repo).WipeEmote, Is.EqualTo(emote));
            Assert.That(temp, Is.EqualTo(repo.KillEmote));
        }

        [Test]
        public void MiscDataRepositorySetKillInvalidHasNoEffectTest([ValueSource(typeof(TestHelper), nameof(TestHelper.InvalidDiscordEmotes))] string emote)
        {
            MiscRepository repo = new MiscRepository();
            string tempK = repo.KillEmote;
            string tempW = repo.WipeEmote;
            TestHelper.ValidateArugumentException(Assert.Catch<ArgumentException>(() => repo.KillEmote = emote));
            Assert.That(tempK, Is.EqualTo(repo.KillEmote));
            Assert.That(tempW, Is.EqualTo(repo.WipeEmote));
        }

        [Test]
        public void MiscDataRepositorySetWipeInvalidHasNoEffectTest([ValueSource(typeof(TestHelper), nameof(TestHelper.InvalidDiscordEmotes))] string emote)
        {
            MiscRepository repo = new MiscRepository();
            string tempK = repo.KillEmote;
            string tempW = repo.WipeEmote;
            TestHelper.ValidateArugumentException(Assert.Catch<ArgumentException>(() => repo.WipeEmote = emote));
            Assert.That(tempK, Is.EqualTo(repo.KillEmote));
            Assert.That(tempW, Is.EqualTo(repo.WipeEmote));
        }
    }

}
