using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LogUploader.Data;
using LogUploader.Interfaces;

namespace LogUploader.Tools.EliteInsights.Data
{
    internal class LogFullEi : LogBasicEi, LogFull
    {
        internal LogFullEi(List<LogTargetEi> targetsData, List<LogPlayerEi> playersData, LogBasicEi log) : base(log)
        {
            TargetsData = targetsData ?? throw new ArgumentNullException(nameof(targetsData));
            PlayersData = playersData ?? throw new ArgumentNullException(nameof(playersData));
        }

        internal List<LogTargetEi> TargetsData { get; }
        internal List<LogPlayerEi> PlayersData { get; }
        public IReadOnlyList<LogTarget> Targets { get => TargetsData; }
        public IReadOnlyList<LogPlayer> Players { get => PlayersData; }
    }

    internal class LogBasicEi : LogEi, LogBasic
    {
        internal LogBasicEi(LogBasicEi log) : this((log.Duration, log.Uploaded, log.Parsed, log.Succcess, log.IsCm, log.RemainingHealth, log.UpgradeAvailable), log) { }

        internal LogBasicEi((TimeSpan Duration, bool Uploaded, bool Parsed, bool Succcess, bool IsCm, float RemainingHealth, bool UpgradeAvailable) basicData, Log log) : base(log)
        {
            if (basicData.Duration.TotalSeconds < 0) throw new ArgumentOutOfRangeException(nameof(basicData.Duration), basicData.Duration, "Duration cannot be a negative time span.");
            Duration = basicData.Duration;
            Uploaded = basicData.Uploaded;
            Parsed = basicData.Parsed;
            Succcess = basicData.Succcess;
            IsCm = basicData.IsCm;
            if (!(0 <= basicData.RemainingHealth && basicData.RemainingHealth <= 100))
                throw new ArgumentOutOfRangeException(nameof(basicData.RemainingHealth), basicData.RemainingHealth, "RemainingHealth must be a value in range 0 <= x <= 100.");
            RemainingHealth = basicData.RemainingHealth;
            UpgradeAvailable = basicData.UpgradeAvailable;
        }

        public TimeSpan Duration { get; }
        public bool Uploaded { get; }
        public bool Parsed { get; }
        public bool Succcess { get; }
        public bool IsCm { get; }
        public float RemainingHealth { get; }
        public bool UpgradeAvailable { get; }
    }

    internal class LogEi : Log
    {
        internal LogEi(Log log) : this((log.Boss as Boss ?? StaticData.Bosses.Get(log.Boss.ID), log.Date, log.SizeKb, log.EvtcExists)) { }
        internal LogEi((Boss BossData, DateTime Date, int SizeKb, bool EvtcExists) logData)
        {
            BossData = logData.BossData ?? throw new ArgumentNullException(nameof(logData.BossData));
            Date = logData.Date;
            if (logData.SizeKb < 0) throw new ArgumentOutOfRangeException(nameof(logData.SizeKb), logData.SizeKb, "File size cannot be less than 0");
            SizeKb = logData.SizeKb;
            EvtcExists = logData.EvtcExists;
        }

        internal Boss BossData { get; }
        public IBoss Boss { get => BossData; }
        public DateTime Date { get; }
        public int SizeKb { get; }
        public bool EvtcExists { get; }
    }

    internal class LogTargetEi : LogTarget
    {
        internal LogTargetEi(int iD, int maxHealth, int remainingHealth, int firstAware, int lastAware)
        {
            if (iD < 0) throw new ArgumentOutOfRangeException(nameof(iD), iD, "iD has to be positive");
            ID = iD;
            if (maxHealth < 0) throw new ArgumentOutOfRangeException(nameof(maxHealth), maxHealth, "maxHealth has to be positive");
            if (maxHealth < remainingHealth) throw new ArgumentException("remainingHealth has to be less or equal to maxHealth", nameof(remainingHealth));
            MaxHealth = maxHealth;
            if (remainingHealth < 0) throw new ArgumentOutOfRangeException(nameof(remainingHealth), remainingHealth, "remainingHealth has to be positive");
            RemainingHealth = remainingHealth;
            if (firstAware < 0) throw new ArgumentOutOfRangeException(nameof(firstAware), firstAware, "firstAware has to be positive");
            FirstAware = firstAware;
            if (lastAware < 0) throw new ArgumentOutOfRangeException(nameof(lastAware), lastAware, "lastAware has to be positive");
            LastAware = lastAware;
        }

        public int ID { get; }
        public int MaxHealth { get; }
        public int RemainingHealth { get; }
        public int FirstAware { get; }
        public int LastAware { get; }
    }

    internal class LogPlayerEi : LogPlayer
    {
        internal LogPlayerEi((string AccountName, string CharakterName, Profession Profession, byte SubGroup) basic, LogPhaseEi fullFightData, List<LogPhaseEi> phasesData)
        {
            ProfessionData = basic.Profession ?? throw new ArgumentNullException(nameof(basic.Profession));
            FullFightData = fullFightData ?? throw new ArgumentNullException(nameof(fullFightData));
            PhasesData = phasesData ?? throw new ArgumentNullException(nameof(phasesData));
            AccountName = basic.AccountName ?? throw new ArgumentNullException(nameof(basic.AccountName));
            CharakterName = basic.CharakterName ?? throw new ArgumentNullException(nameof(basic.CharakterName));
            SubGroup = basic.SubGroup;
        }

        internal Profession ProfessionData { get; }
        internal LogPhaseEi FullFightData { get; }
        internal List<LogPhaseEi> PhasesData { get; }

        public string AccountName { get; }
        public string CharakterName { get; }
        public IProfession Profession { get => ProfessionData; }
        public byte SubGroup { get; }
        public LogPhase FullFight { get => FullFightData; }
        public IReadOnlyList<LogPhase> Phases { get => PhasesData; }
    }

    internal class LogPhaseEi : LogPhase
    {
        internal LogPhaseEi(LogDpsEi dpsAllData, Dictionary<int, LogDpsEi> dpsTargetData, LogBuffsEi buffsData)
        {
            DpsAllData = dpsAllData ?? throw new ArgumentNullException(nameof(dpsAllData));
            if (dpsTargetData is null) throw new ArgumentNullException(nameof(dpsTargetData));
            if (dpsTargetData.Any(e => e.Value is null)) throw new ArgumentNullException(nameof(dpsTargetData), "some element in the dpsTargetData is null");
            DpsTargetData = dpsTargetData;
            BuffsData = buffsData ?? throw new ArgumentNullException(nameof(buffsData));
        }

        internal LogDpsEi DpsAllData { get; }
        internal Dictionary<int, LogDpsEi> DpsTargetData { get; }
        internal LogBuffsEi BuffsData { get; }
        public LogDps DpsAll { get => DpsAllData; }
        public IReadOnlyDictionary<int, LogDps> DpsTarget { get => DpsTargetData.ToDictionary(e => e.Key, e => (LogDps)e.Value); }
        public LogBuffs Buffs { get => BuffsData; }
    }

    internal class LogBuffsEi : LogBuffs
    {
        internal LogBuffsEi((float Quickness, float Alacrety) group, (float Quickness, float Alacrety, float Might, float Fury, float Protection, float Regeneration) squad, (float Strength, float Tactics) squadBanner)
        {
            if (group.Quickness < 0) throw new ArgumentOutOfRangeException(nameof(group.Quickness), group.Quickness, "Group Quickness creation cannot be less than zero");
            GroupQuickness = group.Quickness;
            if (group.Alacrety < 0) throw new ArgumentOutOfRangeException(nameof(group.Alacrety), group.Alacrety, "Group Alacrety creation cannot be less than zero");
            GroupAlacrety = group.Alacrety;
            if (squad.Quickness < 0) throw new ArgumentOutOfRangeException(nameof(squad.Quickness), squad.Quickness, "Squad Quickness creation cannot be less than zero");
            SquadQuickness = squad.Quickness;
            if (squad.Alacrety < 0) throw new ArgumentOutOfRangeException(nameof(squad.Alacrety), squad.Alacrety, "Squad Alacrety creation cannot be less than zero");
            SquadAlacrety = squad.Alacrety;
            if (squad.Might < 0) throw new ArgumentOutOfRangeException(nameof(squad.Might), squad.Might, "Squad Might creation cannot be less than zero");
            SquadMight = squad.Might;
            if (squad.Fury < 0) throw new ArgumentOutOfRangeException(nameof(squad.Fury), squad.Fury, "Squad Fury creation cannot be less than zero");
            SquadFury = squad.Fury;
            if (squad.Protection < 0) throw new ArgumentOutOfRangeException(nameof(squad.Protection), squad.Protection, "Squad Protection creation cannot be less than zero");
            SquadProtection = squad.Protection;
            if (squad.Regeneration < 0) throw new ArgumentOutOfRangeException(nameof(squad.Regeneration), squad.Regeneration, "Squad Regeneration creation cannot be less than zero");
            SquadRegeneration = squad.Regeneration;
            if (squadBanner.Strength < 0) throw new ArgumentOutOfRangeException(nameof(squadBanner.Strength), squadBanner.Strength, "Squad StrengthBanner creation cannot be less than zero");
            SquadBannerStrength = squadBanner.Strength;
            if (squadBanner.Tactics < 0) throw new ArgumentOutOfRangeException(nameof(squadBanner.Tactics), squadBanner.Tactics, "Squad TacticsBanner creation cannot be less than zero");
            SquadBannerTactics = squadBanner.Tactics;
        }

        public float GroupQuickness { get; }
        public float GroupAlacrety { get; }
        public float SquadQuickness { get; }
        public float SquadAlacrety { get; }
        public float SquadMight { get; }
        public float SquadFury { get; }
        public float SquadProtection { get; }
        public float SquadRegeneration { get; }
        public float SquadBannerStrength { get; }
        public float SquadBannerTactics { get; }
    }

    internal class LogDpsEi : LogDps
    {
        internal LogDpsEi((int All, int Power, int Condi) dps, int cC)
        {
            if (dps.All < 0) throw new ArgumentOutOfRangeException(nameof(dps.All), dps.All, "Dps all cannot be less than 0");
            All = dps.All;
            if (dps.Power < 0) throw new ArgumentOutOfRangeException(nameof(dps.Power), dps.Power, "Power dps cannot be less than 0");
            Power = dps.Power;
            if (dps.Condi < 0) throw new ArgumentOutOfRangeException(nameof(dps.Condi), dps.Condi, "Condi dps cannot be less than 0");
            Condi = dps.Condi;
            if (cC < 0) throw new ArgumentOutOfRangeException(nameof(cC), cC, "CC cannot be less than 0");
            CC = cC;
        }

        public int All { get; }
        public int Power { get; }
        public int Condi { get; }
        public int CC { get; }
    }
}
