using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NUnit.Framework;

using LogUploader.Interfaces;
using LogUploader.Tools.EliteInsights.Data;

namespace LogUploader.Test.Data.Logs
{
    public abstract class LogDataTest<T> where T : Log
    {
        protected virtual IBoss Boss { get; } = new LogUploader.Data.Boss();
        protected virtual DateTime Date { get; } = DateTime.Now;
        protected virtual int SizeKb { get; } = 41;
        protected virtual bool EvtcExists { get; } = true;

        public virtual T GetLog() => GetLog(Boss, Date, SizeKb, EvtcExists);
        public abstract T GetLog(IBoss boss, DateTime date, int sizeKb, bool evtcExists);

        [OneTimeSetUp]
        public void OneTimeSetUp() => AbstractOneTimeSetUp();
        protected abstract void AbstractOneTimeSetUp();

        [Test]
        public void LogDataConstructorAsssigenTest()
        {
            Assert.That(GetLog, Throws.Nothing);

            T log = GetLog();
            ValidateDefaultLog(log);
        }

        protected virtual void ValidateDefaultLog(T log)
        {
            Assert.That(log.Boss, Is.EqualTo(Boss));
            Assert.That(log.Date, Is.EqualTo(Date));
            Assert.That(log.SizeKb, Is.EqualTo(SizeKb));
            Assert.That(log.EvtcExists, Is.EqualTo(EvtcExists));
        }

        [Test]
        public void LogDataConstructorInvalidArgumentsTest()
        {
            TestHelper.ValidateArugumentException(Assert.Catch<ArgumentNullException>(() => GetLog(null, Date, SizeKb, EvtcExists)));
            TestHelper.ValidateArugumentException(Assert.Catch<ArgumentOutOfRangeException>(() => GetLog(Boss, Date, -1, EvtcExists)));
        }
    }

    public abstract class LogBasicDataTest<T> : LogDataTest<T> where T : LogBasic
    {
        protected virtual TimeSpan Duration { get; } = TimeSpan.FromMilliseconds(420000);
        protected virtual bool Uploaded { get; } = true;
        protected virtual bool Parsed { get; } = true;
        protected virtual bool Succcess { get; } = true;
        protected virtual bool IsCm { get; } = true;
        protected virtual float RemainingHealth { get; } = 3.14f;
        protected virtual bool UpgradeAvailable { get; } = true;

        public override T GetLog(IBoss boss, DateTime date, int sizeKb, bool evtcExists)
            => GetLogBasic(boss, date, sizeKb, evtcExists, Duration, Uploaded, Parsed,
                Succcess, IsCm, RemainingHealth, UpgradeAvailable);
        public override T GetLog() => base.GetLog();
        public abstract T GetLogBasic(IBoss boss, DateTime date, int sizeKb, bool evtcExists,
            TimeSpan duration, bool uploaded, bool parsed, bool succcess, bool isCm,
            float remainingHealth, bool upgradeAvailable);

        protected override void ValidateDefaultLog(T log)
        {
            base.ValidateDefaultLog(log);
            Assert.That(log.Duration, Is.EqualTo(Duration));
            Assert.That(log.Uploaded, Is.EqualTo(Uploaded));
            Assert.That(log.Parsed, Is.EqualTo(Parsed));
            Assert.That(log.Succcess, Is.EqualTo(Succcess));
            Assert.That(log.IsCm, Is.EqualTo(IsCm));
            Assert.That(log.RemainingHealth, Is.EqualTo(RemainingHealth));
            Assert.That(log.UpgradeAvailable, Is.EqualTo(UpgradeAvailable));
        }

        [Test]
        public void LogBasicDataConstructorInvalidArgumentsTest()
        {
            TestHelper.ValidateArugumentException(Assert.Catch<ArgumentNullException>(() => GetLog(null, Date, SizeKb, EvtcExists)));
            TestHelper.ValidateArugumentException(Assert.Catch<ArgumentOutOfRangeException>(() => GetLog(Boss, Date, -1, EvtcExists)));
            TestHelper.ValidateArugumentException(Assert.Catch<ArgumentOutOfRangeException>(() => GetLogBasic(Boss, Date, SizeKb, EvtcExists, new TimeSpan(0, -1, 0), Uploaded, Parsed, Succcess, IsCm, RemainingHealth, UpgradeAvailable)));
            TestHelper.ValidateArugumentException(Assert.Catch<ArgumentOutOfRangeException>(() => GetLogBasic(Boss, Date, SizeKb, EvtcExists, Duration, Uploaded, Parsed, Succcess, IsCm, 100.1f, UpgradeAvailable)));
            TestHelper.ValidateArugumentException(Assert.Catch<ArgumentOutOfRangeException>(() => GetLogBasic(Boss, Date, SizeKb, EvtcExists, Duration, Uploaded, Parsed, Succcess, IsCm, -1f, UpgradeAvailable)));
            TestHelper.ValidateArugumentException(Assert.Catch<ArgumentOutOfRangeException>(() => GetLogBasic(Boss, Date, SizeKb, EvtcExists, Duration, Uploaded, Parsed, Succcess, IsCm, float.NaN, UpgradeAvailable)));
            TestHelper.ValidateArugumentException(Assert.Catch<ArgumentOutOfRangeException>(() => GetLogBasic(Boss, Date, SizeKb, EvtcExists, Duration, Uploaded, Parsed, Succcess, IsCm, 0f / 0f, UpgradeAvailable)));
            TestHelper.ValidateArugumentException(Assert.Catch<ArgumentOutOfRangeException>(() => GetLogBasic(Boss, Date, SizeKb, EvtcExists, Duration, Uploaded, Parsed, Succcess, IsCm, float.MaxValue, UpgradeAvailable)));
            TestHelper.ValidateArugumentException(Assert.Catch<ArgumentOutOfRangeException>(() => GetLogBasic(Boss, Date, SizeKb, EvtcExists, Duration, Uploaded, Parsed, Succcess, IsCm, float.MinValue, UpgradeAvailable)));
        }
    }

    public abstract class LogPreviewDataTest<T> : LogBasicDataTest<T> where T : LogPreview
    {
        public abstract LogPreviewPlayer Player1 { get; }
        public abstract LogPreviewPlayer Player2 { get; }

        public override T GetLogBasic(IBoss boss, DateTime date, int sizeKb, bool evtcExists,
            TimeSpan duration, bool uploaded, bool parsed, bool succcess, bool isCm,
            float remainingHealth, bool upgradeAvailable)
            => GetLogPreview(boss, date, sizeKb, evtcExists,
            duration, uploaded, parsed, succcess, isCm,
            remainingHealth, upgradeAvailable, new LogPreviewPlayer[] { Player1, Player2 });
        public override T GetLog() => base.GetLog();
        public abstract T GetLogPreview(IBoss boss, DateTime date, int sizeKb, bool evtcExists,
            TimeSpan duration, bool uploaded, bool parsed, bool succcess, bool isCm,
            float remainingHealth, bool upgradeAvailable, IEnumerable<LogPreviewPlayer> players);

        protected override void ValidateDefaultLog(T log)
        {
            Assume.That(Player1, Is.Not.EqualTo(Player2));

            base.ValidateDefaultLog(log);

            Assert.That(log.Players, Is.Not.Null);
            Assert.That(log.Players, Has.Count.EqualTo(2));
            Assert.That(log.Players, Contains.Item(Player1));
            Assert.That(log.Players, Contains.Item(Player2));
        }

        [Test]
        public void LogPreviewDataConstructorInvalidArgumentsTest()
        {
            TestHelper.ValidateArugumentException(Assert.Catch<ArgumentNullException>(() => GetLog(null, Date, SizeKb, EvtcExists)));
            TestHelper.ValidateArugumentException(Assert.Catch<ArgumentOutOfRangeException>(() => GetLog(Boss, Date, -1, EvtcExists)));
            TestHelper.ValidateArugumentException(Assert.Catch<ArgumentOutOfRangeException>(() => GetLogBasic(Boss, Date, SizeKb, EvtcExists, new TimeSpan(0, -1, 0), Uploaded, Parsed, Succcess, IsCm, RemainingHealth, UpgradeAvailable)));
            TestHelper.ValidateArugumentException(Assert.Catch<ArgumentOutOfRangeException>(() => GetLogBasic(Boss, Date, SizeKb, EvtcExists, Duration, Uploaded, Parsed, Succcess, IsCm, 100.1f, UpgradeAvailable)));
            TestHelper.ValidateArugumentException(Assert.Catch<ArgumentOutOfRangeException>(() => GetLogBasic(Boss, Date, SizeKb, EvtcExists, Duration, Uploaded, Parsed, Succcess, IsCm, -1f, UpgradeAvailable)));
            TestHelper.ValidateArugumentException(Assert.Catch<ArgumentNullException>(() => GetLogPreview(Boss, Date, SizeKb, EvtcExists, Duration, Uploaded, Parsed, Succcess, IsCm, RemainingHealth, UpgradeAvailable, null)));
            TestHelper.ValidateArugumentException(Assert.Catch<ArgumentNullException>(() => GetLogPreview(Boss, Date, SizeKb, EvtcExists, Duration, Uploaded, Parsed, Succcess, IsCm, RemainingHealth, UpgradeAvailable, new LogPreviewPlayer[] { null, Player2 })));
            TestHelper.ValidateArugumentException(Assert.Catch<ArgumentNullException>(() => GetLogPreview(Boss, Date, SizeKb, EvtcExists, Duration, Uploaded, Parsed, Succcess, IsCm, RemainingHealth, UpgradeAvailable, new LogPreviewPlayer[] { Player1, null })));
            TestHelper.ValidateArugumentException(Assert.Catch<ArgumentNullException>(() => GetLogPreview(Boss, Date, SizeKb, EvtcExists, Duration, Uploaded, Parsed, Succcess, IsCm, RemainingHealth, UpgradeAvailable, new LogPreviewPlayer[] { null, null })));
        }
    }

    public abstract class LogFullDataTest<T> : LogBasicDataTest<T> where T : LogFull
    {
        public abstract LogPlayer Player1 { get; }
        public abstract LogPlayer Player2 { get; }
        public abstract LogTarget Target1 { get; }
        public abstract LogTarget Target2 { get; }

        public override T GetLogBasic(IBoss boss, DateTime date, int sizeKb, bool evtcExists,
            TimeSpan duration, bool uploaded, bool parsed, bool succcess, bool isCm,
            float remainingHealth, bool upgradeAvailable)
            => GetLogFull(boss, date, sizeKb, evtcExists,
            duration, uploaded, parsed, succcess, isCm,
            remainingHealth, upgradeAvailable, new LogPlayer[] { Player1, Player2 },
            new LogTarget[] { Target1, Target2 });
        public override T GetLog() => base.GetLog();
        public abstract T GetLogFull(IBoss boss, DateTime date, int sizeKb, bool evtcExists,
            TimeSpan duration, bool uploaded, bool parsed, bool succcess, bool isCm,
            float remainingHealth, bool upgradeAvailable, IEnumerable<LogPlayer> players
            , IEnumerable<LogTarget> targets);

        protected override void ValidateDefaultLog(T log)
        {
            Assume.That(Player1, Is.Not.EqualTo(Player2));
            Assume.That(Target1, Is.Not.EqualTo(Target2));

            base.ValidateDefaultLog(log);

            Assert.That(log.Players, Is.Not.Null);
            Assert.That(log.Players, Has.Count.EqualTo(2));
            Assert.That(log.Players, Contains.Item(Player1));
            Assert.That(log.Players, Contains.Item(Player2));

            Assert.That(log.Targets, Is.Not.Null);
            Assert.That(log.Targets, Has.Count.EqualTo(2));
            Assert.That(log.Targets, Contains.Item(Target1));
            Assert.That(log.Targets, Contains.Item(Target2));
        }

        [Test]
        public void LogPreviewDataConstructorInvalidArgumentsTest()
        {
            TestHelper.ValidateArugumentException(Assert.Catch<ArgumentOutOfRangeException>(() => GetLog(null, Date, SizeKb, EvtcExists)));
            TestHelper.ValidateArugumentException(Assert.Catch<ArgumentOutOfRangeException>(() => GetLog(Boss, Date, -1, EvtcExists)));
            TestHelper.ValidateArugumentException(Assert.Catch<ArgumentOutOfRangeException>(() => GetLogBasic(Boss, Date, SizeKb, EvtcExists, new TimeSpan(0, -1, 0), Uploaded, Parsed, Succcess, IsCm, RemainingHealth, UpgradeAvailable)));
            TestHelper.ValidateArugumentException(Assert.Catch<ArgumentOutOfRangeException>(() => GetLogBasic(Boss, Date, SizeKb, EvtcExists, Duration, Uploaded, Parsed, Succcess, IsCm, 100.1f, UpgradeAvailable)));
            TestHelper.ValidateArugumentException(Assert.Catch<ArgumentOutOfRangeException>(() => GetLogBasic(Boss, Date, SizeKb, EvtcExists, Duration, Uploaded, Parsed, Succcess, IsCm, -1f, UpgradeAvailable)));
            TestHelper.ValidateArugumentException(Assert.Catch<ArgumentNullException>(() => GetLogFull(Boss, Date, SizeKb, EvtcExists, Duration, Uploaded, Parsed, Succcess, IsCm, RemainingHealth, UpgradeAvailable, null, new LogTarget[] { Target1, Target2 })));
            TestHelper.ValidateArugumentException(Assert.Catch<ArgumentNullException>(() => GetLogFull(Boss, Date, SizeKb, EvtcExists, Duration, Uploaded, Parsed, Succcess, IsCm, RemainingHealth, UpgradeAvailable, new LogPlayer[] { null, Player2 }, new LogTarget[] { Target1, Target2 })));
            TestHelper.ValidateArugumentException(Assert.Catch<ArgumentNullException>(() => GetLogFull(Boss, Date, SizeKb, EvtcExists, Duration, Uploaded, Parsed, Succcess, IsCm, RemainingHealth, UpgradeAvailable, new LogPlayer[] { Player1, null }, new LogTarget[] { Target1, Target2 })));
            TestHelper.ValidateArugumentException(Assert.Catch<ArgumentNullException>(() => GetLogFull(Boss, Date, SizeKb, EvtcExists, Duration, Uploaded, Parsed, Succcess, IsCm, RemainingHealth, UpgradeAvailable, new LogPlayer[] { null, null }, new LogTarget[] { Target1, Target2 })));
            TestHelper.ValidateArugumentException(Assert.Catch<ArgumentNullException>(() => GetLogFull(Boss, Date, SizeKb, EvtcExists, Duration, Uploaded, Parsed, Succcess, IsCm, RemainingHealth, UpgradeAvailable, new LogPlayer[] { Player1, Player2 }, null)));
            TestHelper.ValidateArugumentException(Assert.Catch<ArgumentNullException>(() => GetLogFull(Boss, Date, SizeKb, EvtcExists, Duration, Uploaded, Parsed, Succcess, IsCm, RemainingHealth, UpgradeAvailable, new LogPlayer[] { Player1, Player2 }, null)));
            TestHelper.ValidateArugumentException(Assert.Catch<ArgumentNullException>(() => GetLogFull(Boss, Date, SizeKb, EvtcExists, Duration, Uploaded, Parsed, Succcess, IsCm, RemainingHealth, UpgradeAvailable, new LogPlayer[] { Player1, Player2 }, new LogTarget[] { null, Target2 })));
            TestHelper.ValidateArugumentException(Assert.Catch<ArgumentNullException>(() => GetLogFull(Boss, Date, SizeKb, EvtcExists, Duration, Uploaded, Parsed, Succcess, IsCm, RemainingHealth, UpgradeAvailable, new LogPlayer[] { Player1, Player2 }, new LogTarget[] { Target1, null })));
            TestHelper.ValidateArugumentException(Assert.Catch<ArgumentNullException>(() => GetLogFull(Boss, Date, SizeKb, EvtcExists, Duration, Uploaded, Parsed, Succcess, IsCm, RemainingHealth, UpgradeAvailable, new LogPlayer[] { Player1, Player2 }, new LogTarget[] { null, null })));
        }
    }

    public class DbLogBasicDataTest : LogBasicDataTest<LogBasic>
    {
        public override LogBasic GetLogBasic(IBoss boss, DateTime date, int sizeKb, bool evtcExists, TimeSpan duration, bool uploaded, bool parsed, bool succcess, bool isCm, float remainingHealth, bool upgradeAvailable)
        {
            throw new NotImplementedException();
        }

        protected override void AbstractOneTimeSetUp()
        {
            Assume.That(false, "DbLogBasicDataTest not implemented yet");
        }
    }

    public class DbLogPreviewDataTest : LogPreviewDataTest<LogPreview>
    {
        public override LogPreviewPlayer Player1 { get; }
        public override LogPreviewPlayer Player2 { get; }

        public override LogPreview GetLogPreview(IBoss boss, DateTime date, int sizeKb, bool evtcExists, TimeSpan duration, bool uploaded, bool parsed, bool succcess, bool isCm, float remainingHealth, bool upgradeAvailable, IEnumerable<LogPreviewPlayer> players)
        {
            throw new NotImplementedException();
        }

        protected override void AbstractOneTimeSetUp()
        {
            Assume.That(false, "DbLogPreviewDataTest not implemented yet");
        }
    }

    public class DbLogFullDataTest : LogFullDataTest<LogFull>
    {
        public override LogPlayer Player1 { get; }
        public override LogPlayer Player2 { get; }
        public override LogTarget Target1 { get; }
        public override LogTarget Target2 { get; }

        public override LogFull GetLogFull(IBoss boss, DateTime date, int sizeKb, bool evtcExists, TimeSpan duration, bool uploaded, bool parsed, bool succcess, bool isCm, float remainingHealth, bool upgradeAvailable, IEnumerable<LogPlayer> players, IEnumerable<LogTarget> targets)
        {
            throw new NotImplementedException();
        }

        protected override void AbstractOneTimeSetUp()
        {
            Assume.That(false, "DbLogFullDataTest not implemented yet");
        }
    }

    internal class EiLogBasicDataTest : LogBasicDataTest<LogBasicEi>
    {
        public override LogBasicEi GetLogBasic(IBoss boss, DateTime date, int sizeKb, bool evtcExists, TimeSpan duration, bool uploaded, bool parsed, bool succcess, bool isCm, float remainingHealth, bool upgradeAvailable)
        {
            Assert.That(boss, Is.InstanceOf<LogUploader.Data.Boss>().Or.Null);
            return new LogBasicEi(new LogBasicEi((duration, uploaded, parsed, succcess, IsCm, remainingHealth, upgradeAvailable),
                new LogEi((boss as LogUploader.Data.Boss, date, sizeKb, evtcExists))));
        }

        protected override void AbstractOneTimeSetUp()
        {
            Assume.That(true, "EiLogBasicDataTest implemented now");
        }
    }

    public class EiLogPreviewDataTest : LogPreviewDataTest<LogPreview>
    {
        public override LogPreviewPlayer Player1 { get; }
        public override LogPreviewPlayer Player2 { get; }

        public override LogPreview GetLogPreview(IBoss boss, DateTime date, int sizeKb, bool evtcExists, TimeSpan duration, bool uploaded, bool parsed, bool succcess, bool isCm, float remainingHealth, bool upgradeAvailable, IEnumerable<LogPreviewPlayer> players)
        {
            throw new NotImplementedException();
        }

        protected override void AbstractOneTimeSetUp()
        {
            Assume.That(false, "EiLogPreviewDataTest not implemented yet");
        }
    }

    public class EiLogFullDataTest : LogFullDataTest<LogFull>
    {
        public override LogPlayer Player1 { get; }
        public override LogPlayer Player2 { get; }
        public override LogTarget Target1 { get; }
        public override LogTarget Target2 { get; }

        public override LogFull GetLogFull(IBoss boss, DateTime date, int sizeKb, bool evtcExists, TimeSpan duration, bool uploaded, bool parsed, bool succcess, bool isCm, float remainingHealth, bool upgradeAvailable, IEnumerable<LogPlayer> players, IEnumerable<LogTarget> targets)
        {
            throw new NotImplementedException();
        }

        protected override void AbstractOneTimeSetUp()
        {
            Assume.That(false, "EiLogFullDataTest not implemented yet");
        }
    }

    #region AbstractLogPreviewPlayerTest

    public abstract class AbstractLogPreviewPlayerTest<T> where T : LogPreviewPlayer
    {
        public abstract T CreatePreviewPlayer(string name, LogUploader.Data.IProfession profession, byte subGroup, int dps);

        [OneTimeSetUp]
        public void OneTimeSetup() => AbstractOneTimeSetup();
        protected abstract void AbstractOneTimeSetup();

        [Test]
        public void LogPreviewPlayerConstructorAssigenTest([ValueSource(typeof(TestHelper), nameof(TestHelper.ValidMulitWordStrings))] string validName)
        {
            LogUploader.Data.IProfession profession = TestHelper.CreateProfession(eProfession.Scourge);
            const byte subGroup = 1;
            const int dps = 10410;

            T prevPlayer = CreatePreviewPlayer(validName, profession, subGroup, dps);

            Assert.That(prevPlayer.Name, Is.EqualTo(validName));
            Assert.That(prevPlayer.Profession, Is.EqualTo(profession));
            Assert.That(prevPlayer.SubGroup, Is.EqualTo(subGroup));
            Assert.That(prevPlayer.Dps, Is.EqualTo(dps));
        }

        [Test]
        public void LogPreviewPlayerConstructorInvalidNameTest([ValueSource(typeof(TestHelper), nameof(TestHelper.InvalidMulitWordStrings))] string invalidName)
        {
            LogUploader.Data.IProfession profession = TestHelper.CreateProfession(eProfession.Scourge);
            const byte subGroup = 1;
            const int dps = 10410;

            T prevPlayer = CreatePreviewPlayer(invalidName, profession, subGroup, dps);
        }

        [Test]
        public void LogPreviewPlayerConstructorNullArgsTest()
        {
            const string name = "prevPlayer";
            LogUploader.Data.IProfession profession = TestHelper.CreateProfession(eProfession.Scourge);
            const byte subGroup = 1;
            const int dps = 10410;

            T prevPlayer = CreatePreviewPlayer(name, profession, subGroup, dps);

            TestHelper.ValidateArugumentException(Assert.Catch<ArgumentNullException>(() => CreatePreviewPlayer(null, profession, subGroup, dps)));
            TestHelper.ValidateArugumentException(Assert.Catch<ArgumentNullException>(() => CreatePreviewPlayer(name, null, subGroup, dps)));
        }
    }

    public class DbLogPreviewPlayerTest : AbstractLogPreviewPlayerTest<LogPreviewPlayer>
    {
        public override LogPreviewPlayer CreatePreviewPlayer(string name, LogUploader.Data.IProfession profession, byte subGroup, int dps)
        {
            throw new NotImplementedException();
        }

        protected override void AbstractOneTimeSetup()
        {
            Assume.That(false, "DbLogPreviewPlayerTest not Implemented yet");
        }

    }

    public class EiLogPreviewPlayerTest : AbstractLogPreviewPlayerTest<LogPreviewPlayer>
    {
        public override LogPreviewPlayer CreatePreviewPlayer(string name, LogUploader.Data.IProfession profession, byte subGroup, int dps)
        {
            throw new NotImplementedException();
        }

        protected override void AbstractOneTimeSetup()
        {
            Assume.That(false, "EiLogPreviewPlayerTest not Implemented yet");
        }

    }

    #endregion
    #region AbstractLogPlayerTest

    public abstract class AbstractLogPlayerTest<T> where T : LogPlayer
    {
        public abstract T CreateLogPlayer(string accountName, string charackterName, LogUploader.Data.IProfession profession, byte subgroup, LogPhase fullFight, IEnumerable<LogPhase> Phasees);
        public abstract LogPhase CreateLogPhase();

        protected abstract void AbstractOneTimeSetUp();
        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            AbstractOneTimeSetUp();
            Assume.That(CreateLogPhase(), Is.Not.EqualTo(CreateLogPhase()));
        }

        [Test]
        public void LogPlayerConstructorAssigenTest()
        {
            const string accountName = "account.12324";
            const string charakterName = "charakter xyz";
            LogUploader.Data.IProfession profession = TestHelper.CreateProfession(eProfession.Scourge);
            const byte subGroup = 41;
            LogPhase fullFight = CreateLogPhase();
            LogPhase phase1 = CreateLogPhase();
            LogPhase phase2 = CreateLogPhase();

            T player = CreateLogPlayer(accountName, charakterName, profession, subGroup, fullFight, new LogPhase[] { phase1, phase2 });
            ValidatePlayer(accountName, charakterName, profession, subGroup, fullFight, phase1, phase2, player);
        }

        private static void ValidatePlayer(string accountName, string charakterName, LogUploader.Data.IProfession profession, byte subGroup, LogPhase fullFight, LogPhase phase1, LogPhase phase2, T player)
        {
            Assert.That(player.AccountName, Is.EqualTo(accountName));
            Assert.That(player.CharakterName, Is.EqualTo(charakterName));
            Assert.That(player.Profession, Is.EqualTo(profession));
            Assert.That(player.SubGroup, Is.EqualTo(subGroup));
            Assert.That(player.FullFight, Is.EqualTo(fullFight));
            Assert.That(player.Phases, Is.Not.Null);
            Assert.That(player.Phases, Is.Not.Empty);
            Assert.That(player.Phases, Has.Count.EqualTo(2));
            Assert.That(player.Phases, Contains.Item(phase1));
            Assert.That(player.Phases, Contains.Item(phase2));
        }

        [Test]
        public void LogPlayerConstructorAssigenValidAccountNameTest([ValueSource(typeof(TestHelper), nameof(TestHelper.ValidOneWordStrings))] string accountName)
        {
            const string charakterName = "charakter xyz";
            LogUploader.Data.IProfession profession = TestHelper.CreateProfession(eProfession.Scourge);
            const byte subGroup = 41;
            LogPhase fullFight = CreateLogPhase();
            LogPhase phase1 = CreateLogPhase();
            LogPhase phase2 = CreateLogPhase();

            T player = CreateLogPlayer(accountName, charakterName, profession, subGroup, fullFight, new LogPhase[] { phase1, phase2 });
            ValidatePlayer(accountName, charakterName, profession, subGroup, fullFight, phase1, phase2, player);
        }

        [Test]
        public void LogPlayerConstructorAssigenValidCharakterNmaeTest([ValueSource(typeof(TestHelper), nameof(TestHelper.ValidMulitWordStrings))] string charakterName)
        {
            const string accountName = "account.12324";
            LogUploader.Data.IProfession profession = TestHelper.CreateProfession(eProfession.Scourge);
            const byte subGroup = 41;
            LogPhase fullFight = CreateLogPhase();
            LogPhase phase1 = CreateLogPhase();
            LogPhase phase2 = CreateLogPhase();

            T player = CreateLogPlayer(accountName, charakterName, profession, subGroup, fullFight, new LogPhase[] { phase1, phase2 });
            ValidatePlayer(accountName, charakterName, profession, subGroup, fullFight, phase1, phase2, player);
        }

        [Test]
        public void LogPlayerConstructorInvalidAccountNameTest([ValueSource(typeof(TestHelper), nameof(TestHelper.InvalidOneWordStrings))] string accountName)
        {
            const string charakterName = "charakter xyz";
            LogUploader.Data.IProfession profession = TestHelper.CreateProfession(eProfession.Scourge);
            const byte subGroup = 41;
            LogPhase fullFight = CreateLogPhase();
            LogPhase phase1 = CreateLogPhase();
            LogPhase phase2 = CreateLogPhase();

            TestHelper.ValidateArugumentException(Assert.Catch<ArgumentException>(() =>
                CreateLogPlayer(accountName, charakterName, profession, subGroup, fullFight, new LogPhase[] { phase1, phase2 })));
        }

        [Test]
        public void LogPlayerConstructorInvalidCharakterNameTest([ValueSource(typeof(TestHelper), nameof(TestHelper.InvalidMulitWordStrings))] string charakterName)
        {
            const string accountName = "account.12324";
            LogUploader.Data.IProfession profession = TestHelper.CreateProfession(eProfession.Scourge);
            const byte subGroup = 41;
            LogPhase fullFight = CreateLogPhase();
            LogPhase phase1 = CreateLogPhase();
            LogPhase phase2 = CreateLogPhase();

            TestHelper.ValidateArugumentException(Assert.Catch<ArgumentException>(() =>
                CreateLogPlayer(accountName, charakterName, profession, subGroup, fullFight, new LogPhase[] { phase1, phase2 })));
        }

        [Test]
        public void LogPlayerConstructorNullArgumentTest()
        {
            const string accountName = "account.12324";
            const string charakterName = "charakter xyz";
            LogUploader.Data.IProfession profession = TestHelper.CreateProfession(eProfession.Scourge);
            const byte subGroup = 41;
            LogPhase fullFight = CreateLogPhase();
            LogPhase phase1 = CreateLogPhase();
            LogPhase phase2 = CreateLogPhase();

            TestHelper.ValidateArugumentException(Assert.Catch<ArgumentNullException>(() =>
                CreateLogPlayer(null, charakterName, profession, subGroup, fullFight, new LogPhase[] { phase1, phase2 })));
            TestHelper.ValidateArugumentException(Assert.Catch<ArgumentNullException>(() =>
                CreateLogPlayer(accountName, null, profession, subGroup, fullFight, new LogPhase[] { phase1, phase2 })));
            TestHelper.ValidateArugumentException(Assert.Catch<ArgumentNullException>(() =>
                CreateLogPlayer(accountName, charakterName, null, subGroup, fullFight, new LogPhase[] { phase1, phase2 })));
            TestHelper.ValidateArugumentException(Assert.Catch<ArgumentNullException>(() =>
                CreateLogPlayer(accountName, charakterName, profession, subGroup, null, new LogPhase[] { phase1, phase2 })));
            TestHelper.ValidateArugumentException(Assert.Catch<ArgumentNullException>(() =>
                CreateLogPlayer(accountName, charakterName, profession, subGroup, fullFight, null)));
            TestHelper.ValidateArugumentException(Assert.Catch<ArgumentNullException>(() =>
                CreateLogPlayer(accountName, charakterName, profession, subGroup, fullFight, new LogPhase[] { null, phase2 })));
            TestHelper.ValidateArugumentException(Assert.Catch<ArgumentNullException>(() =>
                CreateLogPlayer(accountName, charakterName, profession, subGroup, fullFight, new LogPhase[] { phase1, null })));
            TestHelper.ValidateArugumentException(Assert.Catch<ArgumentNullException>(() =>
                CreateLogPlayer(accountName, charakterName, profession, subGroup, fullFight, new LogPhase[] { null, null })));
        }

    }

    public class DbLogPlayerTest : AbstractLogPlayerTest<LogPlayer>
    {
        public override LogPhase CreateLogPhase()
        {
            throw new NotImplementedException();
        }

        public override LogPlayer CreateLogPlayer(string accountName, string charackterName, LogUploader.Data.IProfession profession, byte subgroup, LogPhase fullFight, IEnumerable<LogPhase> Phasees)
        {
            throw new NotImplementedException();
        }

        protected override void AbstractOneTimeSetUp()
        {
            Assume.That(false, "DbLogPlayerTest not Implemented yet");
        }
    }

    public class EiLogPlayerTest : AbstractLogPlayerTest<LogPlayer>
    {
        public override LogPhase CreateLogPhase()
        {
            throw new NotImplementedException();
        }

        public override LogPlayer CreateLogPlayer(string accountName, string charackterName, LogUploader.Data.IProfession profession, byte subgroup, LogPhase fullFight, IEnumerable<LogPhase> Phasees)
        {
            throw new NotImplementedException();
        }

        protected override void AbstractOneTimeSetUp()
        {
            Assume.That(false, "EiLogPlayerTest not Implemented yet");
        }
    }

    #endregion
    #region AbstractLogTargetTest

    public abstract class AbstractLogTargetTest<T> where T : LogTarget
    {
        public static int ID { get; } = 987;
        public static int MaxHealth { get; } = 654;
        public static int RemainingHealth { get; } = 543;
        public static int FirstAware { get; } = 159;
        public static int LastAware { get; } = 951;

        public abstract T CreateLogTarget(int id, int maxHealth, int remainingHealth, int firstAware, int lastAware);
        public T CreateLogTarget() => CreateLogTarget(ID, MaxHealth, RemainingHealth, FirstAware, LastAware);

        [OneTimeSetUp]
        public void OneTimeSetup() => AbstractOneTimeSetup();
        protected abstract void AbstractOneTimeSetup();

        public static IEnumerable<(int id, int maxHealth, int remainingHealth, int firstAware, int lastAware)> InvalidAarguments()
        {
            yield return (-1, MaxHealth, RemainingHealth, FirstAware, LastAware);
            yield return (ID, -1, RemainingHealth, FirstAware, LastAware);
            yield return (ID, MaxHealth, -1, FirstAware, LastAware);
            yield return (ID, MaxHealth, RemainingHealth, -1, LastAware);
            yield return (ID, MaxHealth, RemainingHealth, FirstAware, -1);
            yield return (-1, -1, -1, -1, -1);
        }

        public static IEnumerable<(int id, int maxHealth, int remainingHealth, int firstAware, int lastAware)> ValidAarguments()
        {
            yield return (0, MaxHealth, RemainingHealth, FirstAware, LastAware);
            yield return (ID, 0, 0, FirstAware, LastAware);
            yield return (ID, MaxHealth, 0, FirstAware, LastAware);
            yield return (ID, MaxHealth, RemainingHealth, 0, LastAware);
            yield return (ID, MaxHealth, RemainingHealth, FirstAware, 0);
            yield return (0, 0, 0, 0, 0);
        }

        [Test]
        public void LogTargetConstructorAssigenTest()
        {
            T logTarget = CreateLogTarget();

            Assert.That(logTarget.ID, Is.EqualTo(ID));
            Assert.That(logTarget.MaxHealth, Is.EqualTo(MaxHealth));
            Assert.That(logTarget.RemainingHealth, Is.EqualTo(RemainingHealth));
            Assert.That(logTarget.FirstAware, Is.EqualTo(FirstAware));
            Assert.That(logTarget.LastAware, Is.EqualTo(LastAware));
        }

        [Test]
        public void LogTargetConstructorValidArgumentTest([ValueSource(nameof(ValidAarguments))]
        (int id, int maxHealth, int remainingHealth, int firstAware, int lastAware) args)
        {
            (int id, int maxHealth, int remainingHealth, int firstAware, int lastAware) = args;

            Assert.That(() => CreateLogTarget(id, maxHealth, remainingHealth, firstAware, lastAware), Throws.Nothing);
        }

        [Test]
        public void LogTargetConstructorInvalidArgumentTest([ValueSource(nameof(InvalidAarguments))]
        (int id, int maxHealth, int remainingHealth, int firstAware, int lastAware) args)
        {
            (int id, int maxHealth, int remainingHealth, int firstAware, int lastAware) = args;

            TestHelper.ValidateArugumentException(Assert.Catch<ArgumentOutOfRangeException>(() => CreateLogTarget(id, maxHealth, remainingHealth, firstAware, lastAware)));
        }

        [Test]
        public void LogTargetConstructorMaxLessThanRemainingTest()
        {
            TestHelper.ValidateArugumentException(Assert.Catch<ArgumentException>(() => CreateLogTarget(ID, 200, 404, FirstAware, LastAware)));
        }

    }
    public class DbLogTargetTest : AbstractLogTargetTest<LogTarget>
    {
        protected override void AbstractOneTimeSetup()
        {
            Assume.That(false, "DbLogTargetTest not Implemented yet");
        }

        public override LogTarget CreateLogTarget(int id, int maxHealth, int remainingHealth, int firstAware, int lastAware)
        {
            throw new NotImplementedException();
        }
    }

    internal class EiLogTargetTest : AbstractLogTargetTest<LogTargetEi>
    {
        protected override void AbstractOneTimeSetup()
        {
            Assume.That(true, "EiLogTargetTest Implemented now");
        }

        public override LogTargetEi CreateLogTarget(int id, int maxHealth, int remainingHealth, int firstAware, int lastAware)
        {
            return new LogTargetEi(id, maxHealth, remainingHealth, firstAware, lastAware);
        }
    }

    #endregion
    #region AbstractLogPhaseTest

    public abstract class AbstractLogPhaseTest<T> where T : LogPhase
    {
        protected virtual T CreateLogPhase(LogDps logDps, LogBuffs logBuffs, IEnumerable<(int tagetId, LogDps targetDps)> dpsTargets)
            => CreateLogPhase(logDps, logBuffs, dpsTargets.Select(e => new Tuple<int, LogDps>(e.tagetId, e.targetDps)));
        protected abstract T CreateLogPhase(LogDps logDps, LogBuffs logBuffs, IEnumerable<Tuple<int, LogDps>> dpsTargets);
        protected abstract LogBuffs CreateBuffs();
        protected abstract LogDps CreateDps(int num = 0);

        (int tagetId, LogDps targetDps) GetTargetDps(int id) => (id, CreateDps(id));

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            AbstractOneTimeSetUp();
            Assume.That(CreateBuffs(), Is.Not.EqualTo(CreateBuffs()));
            Assume.That(CreateDps(), Is.Not.EqualTo(CreateDps()));
            Assume.That(GetTargetDps(1).targetDps, Is.Not.EqualTo(GetTargetDps(2).targetDps));
            Assume.That(GetTargetDps(1).tagetId, Is.EqualTo(GetTargetDps(1).tagetId));
            Assume.That(GetTargetDps(41).targetDps.All, Is.EqualTo(GetTargetDps(41).targetDps.All));
        }

        protected abstract void AbstractOneTimeSetUp();

        [Test]
        public void LogPhaseConstructorAssigenTest()
        {
            LogDps dps = CreateDps();
            LogBuffs buffs = CreateBuffs();
            (int tagetId, LogDps targetDps) target1 = GetTargetDps(1);
            (int tagetId, LogDps targetDps) target2 = GetTargetDps(2);
            T phase = CreateLogPhase(dps, buffs, new (int tagetId, LogDps targetDps)[] { target1, target2 });

            Assert.That(phase.DpsAll, Is.EqualTo(dps));
            Assert.That(phase.Buffs, Is.EqualTo(buffs));
            Assert.That(phase.DpsTarget, Contains.Key(target1.tagetId));
            Assert.That(phase.DpsTarget[target1.tagetId].All, Is.EqualTo(target1.targetDps.All));
            Assert.That(phase.DpsTarget, Contains.Key(target2.tagetId));
            Assert.That(phase.DpsTarget[target2.tagetId].All, Is.EqualTo(target2.targetDps.All));
        }

        [Test]
        public void LogPhaseConstructorNullArgumentTest()
        {
            LogDps dps = CreateDps();
            LogBuffs buffs = CreateBuffs();
            (int tagetId, LogDps targetDps) target1 = GetTargetDps(1);
            (int tagetId, LogDps targetDps) target2 = GetTargetDps(2);
            T phase = CreateLogPhase(dps, buffs, new (int tagetId, LogDps targetDps)[] { target1, target2 });

            TestHelper.ValidateArugumentException(Assert.Catch<ArgumentNullException>(() => CreateLogPhase(null, buffs, new (int tagetId, LogDps targetDps)[] { target1, target2 })));
            TestHelper.ValidateArugumentException(Assert.Catch<ArgumentNullException>(() => CreateLogPhase(dps, null, new (int tagetId, LogDps targetDps)[] { target1, target2 })));
            TestHelper.ValidateArugumentException(Assert.Catch<ArgumentNullException>(() => CreateLogPhase(dps, buffs, (IEnumerable<Tuple<int, LogDps>>)null)));
            TestHelper.ValidateArugumentException(Assert.Catch<ArgumentNullException>(() => CreateLogPhase(dps, buffs, new Tuple<int, LogDps>[] { new Tuple<int, LogDps>(target1.tagetId, null), new Tuple<int, LogDps>(target2.tagetId, target2.targetDps) })));
            TestHelper.ValidateArugumentException(Assert.Catch<ArgumentNullException>(() => CreateLogPhase(dps, buffs, new Tuple<int, LogDps>[] { new Tuple<int, LogDps>(target1.tagetId, target1.targetDps), new Tuple<int, LogDps>(target2.tagetId, null) })));
            TestHelper.ValidateArugumentException(Assert.Catch<ArgumentNullException>(() => CreateLogPhase(dps, buffs, new Tuple<int, LogDps>[] { new Tuple<int, LogDps>(target1.tagetId, null), new Tuple<int, LogDps>(target2.tagetId, null) })));
        }

    }

    public class DbLogPhaseTest : AbstractLogPhaseTest<LogPhase>
    {

        protected override void AbstractOneTimeSetUp()
        {
            Assume.That(false, "DbLogPhaseTest not Implemented yet");
        }

        protected override LogBuffs CreateBuffs()
        {
            throw new NotImplementedException();
        }

        protected override LogDps CreateDps(int num = 0)
        {
            throw new NotImplementedException();
        }

        protected override LogPhase CreateLogPhase(LogDps logDps, LogBuffs logBuffs, IEnumerable<Tuple<int, LogDps>> dpsTargets)
        {
            throw new NotImplementedException();
        }
    }

    internal class EiLogPhaseTest : AbstractLogPhaseTest<LogPhaseEi>
    {

        protected override void AbstractOneTimeSetUp()
        {
            Assume.That(true, "EiLogPhaseTest Implemented now");
        }

        protected override LogBuffs CreateBuffs()
        {
            return new LogBuffsEi((0f, 0f), (0f, 0f, 0f, 0f, 0f, 0f), (0f, 0f));
        }

        protected override LogDps CreateDps(int num = 0)
        {
            return new LogDpsEi((num, num, num), num);
        }

        protected override LogPhaseEi CreateLogPhase(LogDps logDps, LogBuffs logBuffs, IEnumerable<Tuple<int, LogDps>> dpsTargets)
        {
            Assume.That(logDps, Is.TypeOf<LogDpsEi>().Or.Null);
            Assume.That(logBuffs, Is.TypeOf<LogBuffsEi>().Or.Null);
            if (!(dpsTargets is null))
            {
                foreach (var element in dpsTargets)
                {
                    Assume.That(element?.Item2, Is.TypeOf<LogDpsEi>().Or.Null);
                }
            }

            return new LogPhaseEi(logDps as LogDpsEi, dpsTargets?.ToDictionary(e => e.Item1, e => e.Item2 as LogDpsEi), logBuffs as LogBuffsEi);
        }
    }

    #endregion
    #region AbstractLogDpsTest

    public abstract class AbstractLogDpsTest<T> where T : LogDps
    {
        public static int All   { get; } = 987;
        public static int Power { get; } = 654;
        public static int Condi { get; } = 654;
        public static int CC    { get; } = 159;

        public abstract T CreateLogDps(int all, int power, int condi, int cc);
        public T CreateLogDps() => CreateLogDps(All, Power, Condi, CC);

        [OneTimeSetUp]
        public void OneTimeSetup() => AbstractOneTimeSetup();
        protected abstract void AbstractOneTimeSetup();

        public static IEnumerable<(int all, int power, int condi, int cc)> InvalidAarguments()
        {
            yield return (-1, Power, Condi, CC);
            yield return (All, -1, Condi, CC);
            yield return (All, Power, -1, CC);
            yield return (All, Power, Condi, -1);
            yield return (-1, -1, -1, -1);
        }

        public static IEnumerable<(int all, int power, int condi, int cc)> ValidAarguments()
        {
            yield return (0, Power, Condi, CC);
            yield return (All, 0, Condi, CC);
            yield return (All, Power, 0, CC);
            yield return (All, Power, Condi, 0);
            yield return (0, 0, 0, 0);
        }

        [Test]
        public void LogDpsConstructorAssigenTest()
        {
            T logDps = CreateLogDps();

            Assert.That(logDps.All, Is.EqualTo(All));
            Assert.That(logDps.Power, Is.EqualTo(Power));
            Assert.That(logDps.Condi, Is.EqualTo(Condi));
            Assert.That(logDps.CC, Is.EqualTo(CC));
        }

        [Test]
        public void LogDpsConstructorValidArgumentTest([ValueSource(nameof(ValidAarguments))]
        (int all, int power, int condi, int cc) args)
        {
            (int all, int power, int condi, int cc) = args;

            Assert.That(() => CreateLogDps(all, power, condi, cc), Throws.Nothing);
        }

        [Test]
        public void LogDpsConstructorInvalidArgumentTest([ValueSource(nameof(InvalidAarguments))]
        (int all, int power, int condi, int cc) args)
        {
            (int all, int power, int condi, int cc) = args;

            TestHelper.ValidateArugumentException(Assert.Catch<ArgumentOutOfRangeException>(() => CreateLogDps(all, power, condi, cc)));
        }

    }

    public class DbLogDpsTest : AbstractLogDpsTest<LogDps>
    {
        protected override void AbstractOneTimeSetup()
        {
            Assume.That(false, "DbLogDpsTest not Implemented yet");
        }

        public override LogDps CreateLogDps(int all, int power, int condi, int cc)
        {
            throw new NotImplementedException();
        }
    }

    internal class EiLogDpsTest : AbstractLogDpsTest<LogDpsEi>
    {
        protected override void AbstractOneTimeSetup()
        {
            Assume.That(true, "EiLogDpsTest Implemented now");
        }

        public override LogDpsEi CreateLogDps(int all, int power, int condi, int cc)
        {
            return new LogDpsEi((all, power, condi), cc);
        }
    }


    #endregion
    #region AbstractLogBuffsTest

    public abstract class AbstractLogBuffsTest<T> where T : LogBuffs
    {
        float GroupQuickness { get; } = 1.23f;
        float GroupAlacrety { get; } = 4.56f;
        float SquadQuickness { get; } = 7.89f;
        float SquadAlacrety { get; } = 0.12f;
        float SquadMight { get; } = 3.45f;
        float SquadFury { get; } = 6.78f;
        float SquadProtection { get; } = 9.01f;
        float SquadRegeneration { get; } = 2.34f;
        float SquadBannerStrength { get; } = 5.67f;
        float SquadBannerTactics { get; } = 8.90f;

        public abstract T CreateLogBuffs(float groupQuickness, float groupAlacrety, float squadQuickness, float squadAlacrety, float squadMight, float squadFury, float squadProtection, float squadRegeneration, float squadBannerStrength, float squadBannerTactics);
        public T CreateLogBuffs() => CreateLogBuffs(
            GroupQuickness,
            GroupAlacrety,
            SquadQuickness,
            SquadAlacrety,
            SquadMight,
            SquadFury,
            SquadProtection,
            SquadRegeneration,
            SquadBannerStrength,
            SquadBannerTactics
            );

        [Test]
        public void LogBuffsConstructorAssigeTest()
        {
            T logBuffs = CreateLogBuffs();

            Assert.That(logBuffs.GroupQuickness, Is.EqualTo(GroupQuickness));
            Assert.That(logBuffs.GroupAlacrety, Is.EqualTo(GroupAlacrety));
            Assert.That(logBuffs.SquadQuickness, Is.EqualTo(SquadQuickness));
            Assert.That(logBuffs.SquadAlacrety, Is.EqualTo(SquadAlacrety));
            Assert.That(logBuffs.SquadMight, Is.EqualTo(SquadMight));
            Assert.That(logBuffs.SquadFury, Is.EqualTo(SquadFury));
            Assert.That(logBuffs.SquadProtection, Is.EqualTo(SquadProtection));
            Assert.That(logBuffs.SquadRegeneration, Is.EqualTo(SquadRegeneration));
            Assert.That(logBuffs.SquadBannerStrength, Is.EqualTo(SquadBannerStrength));
            Assert.That(logBuffs.SquadBannerTactics, Is.EqualTo(SquadBannerTactics));
        }

        public static IEnumerable<(float groupQuickness, float groupAlacrety,
            float squadQuickness, float squadAlacrety, float squadMight,
            float squadFury, float squadProtection, float squadRegeneration,
            float squadBannerStrength, float squadBannerTactics)> GetInvalidCombinations()
        {
            yield return (-11f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f);
            yield return (1.5f, -11f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f);
            yield return (1.5f, 1.5f, -11f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f);
            yield return (1.5f, 1.5f, 1.5f, -11f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f);
            yield return (1.5f, 1.5f, 1.5f, 1.5f, -11f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f);
            yield return (1.5f, 1.5f, 1.5f, 1.5f, 1.5f, -11f, 1.5f, 1.5f, 1.5f, 1.5f);
            yield return (1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, -11f, 1.5f, 1.5f, 1.5f);
            yield return (1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, -11f, 1.5f, 1.5f);
            yield return (1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, -11f, 1.5f);
            yield return (1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, -11f);
            yield return (-11f, -11f, -11f, -11f, -11f, -11f, -11f, -11f, -11f, -11f);
        }

        public static IEnumerable<(float groupQuickness, float groupAlacrety,
            float squadQuickness, float squadAlacrety, float squadMight,
            float squadFury, float squadProtection, float squadRegeneration,
            float squadBannerStrength, float squadBannerTactics)> GetValidCombinations()
        {
            yield return (0.0f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f);
            yield return (1.5f, 0.0f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f);
            yield return (1.5f, 1.5f, 0.0f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f);
            yield return (1.5f, 1.5f, 1.5f, 0.0f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f);
            yield return (1.5f, 1.5f, 1.5f, 1.5f, 0.0f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f);
            yield return (1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 0.0f, 1.5f, 1.5f, 1.5f, 1.5f);
            yield return (1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 0.0f, 1.5f, 1.5f, 1.5f);
            yield return (1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 0.0f, 1.5f, 1.5f);
            yield return (1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 0.0f, 1.5f);
            yield return (1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 0.0f);
            yield return (0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f);
        }

        [OneTimeSetUp]
        public void OneTimeSetup() => AbstractOneTimeSetup();
        protected abstract void AbstractOneTimeSetup();

        [Test]
        public void LogBuffsConstructorInvalidArgumentTest([ValueSource(nameof(GetInvalidCombinations))]
               (float groupQuickness, float groupAlacrety,
               float squadQuickness, float squadAlacrety, float squadMight,
               float squadFury, float squadProtection, float squadRegeneration,
               float squadBannerStrength, float squadBannerTactics) args)
        {
            (float groupQuickness, float groupAlacrety,
               float squadQuickness, float squadAlacrety, float squadMight,
               float squadFury, float squadProtection, float squadRegeneration,
               float squadBannerStrength, float squadBannerTactics) = args;
            
            Action createCall = () => CreateLogBuffs(groupQuickness, groupAlacrety,
            squadQuickness, squadAlacrety, squadMight, squadFury, squadProtection, squadRegeneration,
            squadBannerStrength, squadBannerTactics);

            TestHelper.ValidateArugumentException(Assert.Catch<ArgumentOutOfRangeException>(() => createCall()));
        }

        [Test]
        public void LogBuffsConstructorValidArgumentTest([ValueSource(nameof(GetValidCombinations))]
               (float groupQuickness, float groupAlacrety,
               float squadQuickness, float squadAlacrety, float squadMight,
               float squadFury, float squadProtection, float squadRegeneration,
               float squadBannerStrength, float squadBannerTactics) args)
        {
            (float groupQuickness, float groupAlacrety,
               float squadQuickness, float squadAlacrety, float squadMight,
               float squadFury, float squadProtection, float squadRegeneration,
               float squadBannerStrength, float squadBannerTactics) = args;
            
            Action createCall = () => CreateLogBuffs(groupQuickness, groupAlacrety,
            squadQuickness, squadAlacrety, squadMight, squadFury, squadProtection, squadRegeneration,
            squadBannerStrength, squadBannerTactics);

            Assert.That(createCall, Throws.Nothing);
        }
    }

    public class DbLogBuffsTest : AbstractLogBuffsTest<LogBuffs>
    {
        protected override void AbstractOneTimeSetup()
        {
            Assume.That(false, "EiLogBuffsTest not Implemented yet");
        }

        public override LogBuffs CreateLogBuffs(float groupQuickness, float groupAlacrety, float squadQuickness, float squadAlacrety, float squadMight, float squadFury, float squadProtection, float squadRegeneration, float squadBannerStrength, float squadBannerTactics)
        {
            throw new NotImplementedException();
        }
    }

    internal class EiLogBuffsTest : AbstractLogBuffsTest<LogBuffsEi>
    {
        protected override void AbstractOneTimeSetup()
        {
            Assume.That(true, "EiLogBuffsTest Implemented now");
        }

        public override LogBuffsEi CreateLogBuffs(float groupQuickness, float groupAlacrety, float squadQuickness, float squadAlacrety, float squadMight, float squadFury, float squadProtection, float squadRegeneration, float squadBannerStrength, float squadBannerTactics)
        {
            return new LogBuffsEi((groupQuickness, groupAlacrety),
                (squadQuickness, squadAlacrety, squadMight, squadFury, squadProtection, squadRegeneration),
                (squadBannerStrength, squadBannerTactics));
        }
    }

    #endregion
    #region LogTest

    public class DbLogTest : LogDataTest<Log>
    {
        public override Log GetLog(IBoss boss, DateTime date, int sizeKb, bool evtcExists)
        {
            throw new NotImplementedException();
        }

        protected override void AbstractOneTimeSetUp()
        {
            Assume.That(false, "DbLogTest not Implemented yet");
        }
    }

    internal class EiLogTest : LogDataTest<LogEi>
    {
        public override LogEi GetLog(IBoss boss, DateTime date, int sizeKb, bool evtcExists)
        {
            Assume.That(boss, Is.InstanceOf<LogUploader.Data.Boss>().Or.Null);
            return new LogEi(new LogEi((boss as LogUploader.Data.Boss, date, sizeKb, evtcExists)));
        }
    
        protected override void AbstractOneTimeSetUp()
        {
            Assume.That(true, "EiLogTest implemented now");
        }
    }

    #endregion
}
