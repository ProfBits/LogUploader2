using System;
using System.Threading;
using System.Collections.Generic;

using LogUploader.Interfaces;

namespace LogUploader.Tools.EliteInsights.Json
{
    using LogUploader.Tools.EliteInsights.Json.V1;

    internal class EiProcessor_Legacy : AbstractEiProcessor<JsonV1>
    {
        /* 
         * Supports EliteInsightVersion older than 2.29.0.0
         */
        protected override Version MinVersion { get; } = new Version(0, 0, 0, 0);
        protected override Version MaxVersion { get; } = new Version(2, 29, 0, 0);

        internal override LogFull Convert(JsonV1 parsed, IProgress<double> progress = null, CancellationToken cancellationToken = default)
        {
            //LogBasic basics = GetBasicData(parsed);
            //List<PhaseInfo> relevantPhases = GetRelevantPhases(parsed.phases, basics);
            //List<LogTarget> targetsdata = GetTargetData(parsed.targets, relevantPhases);
            //List<LogPlayer> playerData = GetPlayerData(parsed.players, relevantPhases);
            //
            //return CreateLogFull(basics, targetsdata, playerData);
            return null;
        }

        internal virtual LogFull CreateLogFull(LogBasic basics, List<LogTarget> targetsdata, List<LogPlayer> playerData)
        {
            throw new NotImplementedException();
        }

        internal virtual List<LogPlayer> GetPlayerData(PlayerV01[] players, List<PhaseInfo> relevantPhases)
        {
            throw new NotImplementedException();
        }

        internal virtual List<LogTarget> GetTargetData(TargetV01[] targets, List<PhaseInfo> relevantPhases)
        {
            throw new NotImplementedException();
        }

        internal virtual List<PhaseInfo> GetRelevantPhases(PhaseV01[] phases, LogBasic basics)
        {
            throw new NotImplementedException();
        }

        internal virtual LogBasic GetBasicData(JsonV1 parsed)
        {
            throw new NotImplementedException();
        }

        internal class PhaseInfo { } //HACK to be implemented/removed
    }
}
//        public string[] logErrors { get; set; }
/* Changes in other json parts:
 * 2.14.0.0
 * player/Support added props
 * 
 *      public int condiCleanseSelf { get; set; }
 *      public float condiCleanseTimeSelf { get; set; }
 *      public int boonStrips { get; set; }
 *      public float boonStripsTime { get; set; }
 *      
 * 2.16.0.0
 * player
 *      public int[] activeTimes { get; set; }
 * player/stats all
 *      public float avgActiveBoons { get; set; }
 *      public float avgActiveConditions { get; set; }
 * 2.16.1.0
 *      public string fightIcon { get; set; }
 * 2.20.0.0
 *      changed to player/buffuptime/buffdata
 *       generated, overstaece , extenden... added agina contains data per player causing it
 * 2.21.0.0
 *      public int gW2Build { get; set; }
 *      public string language { get; set; }
 *      public int languageID { get; set; }
 *    player/traget
 *      public int instanceID { get; set; }
 * 2.22.0.0
 *   mechanic data
 *      public string description { get; set; }
 * 2.26.0.0
 *    stats all
 *      public float distToCom { get; set; }
 *    phases
 *      public bool breakbarPhase { get; set; }
 * 2.27.0.0
 *   stat all
 *     serval changes
 *   defense
 *     serveral changes
 * 2.28.0.0
 *   stats all add
 * 
 *      public int killed { get; set; }
 *      public int downed { get; set; }
 * 2.33.0.0
 *   player
 *     public int totalHealth { get; set; }
 *    public bool friendlyNPC { get; set; }
 * 2.34.0.0
 *   player add
 *     public bool notInSquad { get; set; }
 * 2.35.0.0
 *  target
 * 
 *      public bool enemyPlayer { get; set; }
 *      public bool isFake { get; set; }
 */

/* History
 * 2.10.0.0 - 2.15.0.0 legacy, first supported versions
 * 2.15.1.0 - 2.15.1.0 duration format changed, added milliseconds "08m 46s" to "06m 58s 895ms"
 * 2.16.0.0 -          split in buffdata type between uptime and other areas
 */

namespace LogUploader.Tools.EliteInsights.Json.V7
{
    using LogUploader.Tools.EliteInsights.Json.V6;
    using LogUploader.Tools.EliteInsights.Json.V5;
    using LogUploader.Tools.EliteInsights.Json.V4;
    using LogUploader.Tools.EliteInsights.Json.V3;
    using LogUploader.Tools.EliteInsights.Json.V2;
    using LogUploader.Tools.EliteInsights.Json.V1;


    /// <summary>
    /// Supported Versions: 2.35.0.0 - current (2.37.2)
    /// </summary>
    internal class JsonV7 : AbstractJsonV03<PlayerV06, TargetV07, PhaseV04, MechanicV01>
    {
    }

    internal class TargetV07 : TargetV01
    {
        public bool isFake { get; set; }
    }
}

namespace LogUploader.Tools.EliteInsights.Json.V6
{
    using LogUploader.Tools.EliteInsights.Json.V5;
    using LogUploader.Tools.EliteInsights.Json.V4;
    using LogUploader.Tools.EliteInsights.Json.V3;
    using LogUploader.Tools.EliteInsights.Json.V2;
    using LogUploader.Tools.EliteInsights.Json.V1;


    /// <summary>
    /// Supported Versions: 2.33.0.0 - 2.33.99.99
    /// </summary>
    internal class JsonV6 : AbstractJsonV03<PlayerV06, TargetV01, PhaseV04, MechanicV01>
    {
    }

    internal abstract class AbstractPlayerV6<TDpsTargets, TDpsAll, TBuffUptimes, TGroupBuffs, TSqudBuffs, TStatsAll>
        : AbstractPlayer<TDpsTargets, TDpsAll, TBuffUptimes, TGroupBuffs, TSqudBuffs, TStatsAll>
        where TDpsTargets : IInternalDps
        where TDpsAll : IInternalDps
        where TBuffUptimes : IInternalBuffs
        where TGroupBuffs : IInternalBuffs
        where TSqudBuffs : IInternalBuffs
        where TStatsAll : IInternalStatsAll
    {
        public bool friendlyNPC { get; set; }
    }

    internal class PlayerV06 : AbstractPlayerV6<DpsV05, DpsV05, BuffUptimessV02, BuffsGenerationV02, BuffsGenerationV02, StatsAllV01>
    {
    }
}


namespace LogUploader.Tools.EliteInsights.Json.V5
{
    using LogUploader.Tools.EliteInsights.Json.V4;
    using LogUploader.Tools.EliteInsights.Json.V3;
    using LogUploader.Tools.EliteInsights.Json.V2;
    using LogUploader.Tools.EliteInsights.Json.V1;

    /// <summary>
    /// Supported Versions: 2.29.0.0 - 2.33.99.99
    /// </summary>
    internal class JsonV5 : AbstractJsonV03<PlayerV05, TargetV01, PhaseV04, MechanicV01>
    {
    }
    
    internal class DpsV05 : DpsV01
    {
        public float breakbarDamage { get; set; }
    }

    internal class PlayerV05 : AbstractPlayer<DpsV05, DpsV05, BuffUptimessV02, BuffsGenerationV02, BuffsGenerationV02, StatsAllV01>
    {
    }
}

namespace LogUploader.Tools.EliteInsights.Json.V4
{
    using LogUploader.Tools.EliteInsights.Json.V3;
    using LogUploader.Tools.EliteInsights.Json.V2;
    using LogUploader.Tools.EliteInsights.Json.V1;

    /// <summary>
    /// Supported Versions: 2.26.0.0 - 2.28.2.0
    /// </summary>
    internal class JsonV4 : AbstractJsonV03<PlayerV02, TargetV01, PhaseV04, MechanicV01>
    {
    }


    internal class PhaseV04 : PhaseV01
    {
        public bool breakbarPhase { get; set; }
    }

}

namespace LogUploader.Tools.EliteInsights.Json.V3
{
    using LogUploader.Tools.EliteInsights.Json.V2;
    using LogUploader.Tools.EliteInsights.Json.V1;

    /// <summary>
    /// Supported Versions: 2.22.0.0 - 2.27.99.99
    /// </summary>
    internal class JsonV3 : AbstractJson<PlayerV02, TargetV01, PhaseV01, MechanicV01>
    {
    }

    internal abstract class AbstractJsonV03<TPlayer, TTarget, TPhase, TMechanik> : AbstractJson<TPlayer, TTarget, TPhase, TMechanik>
        where TPlayer : IInternalPlayer
        where TTarget : IInternalTarget
        where TPhase : IInternalPhase
        where TMechanik : IInternalMechanic
    {
        public string timeStartStd { get; set; }
        public string timeEndStd { get; set; }
        public bool isCM { get; set; }
    }
}

namespace LogUploader.Tools.EliteInsights.Json.V2
{
    using LogUploader.Tools.EliteInsights.Json.V1;

    /// <summary>
    /// Supported Versions: 2.16.0.0 - 2.21.2.0
    /// </summary>
    internal class JsonV2 : AbstractJson<PlayerV02, TargetV01, PhaseV01, MechanicV01>
    {
    }

    internal class PlayerV02 : AbstractPlayer<DpsV01, DpsV01, BuffUptimessV02, BuffsGenerationV02, BuffsGenerationV02, StatsAllV01>
    {
    }

    internal class BuffUptimessV02 : AbstractBuffs<BuffdataUptimeV02>
    {
    }

    internal class BuffsGenerationV02 : AbstractBuffs<BuffdataGenerationV02>
    {
    }

    internal class BuffdataUptimeV02 : IInternalBuffData
    {
        public float uptime { get; set; }
    }

    internal class BuffdataGenerationV02 : IInternalBuffData
    {
        public float generation { get; set; }
    }
}

namespace LogUploader.Tools.EliteInsights.Json.V1
{
    /// <summary>
    /// Supported Versions: 2.10.0.0 - 2.15.1.0
    /// </summary>
    internal class JsonV1 : AbstractJson<PlayerV01, TargetV01, PhaseV01, MechanicV01>
    {
    }

    internal class TargetV01 : AbstractTarget
    {
    }

    internal class DpsV01 : AbstractDps
    {
    }

    internal class PlayerV01 : AbstractPlayer<DpsV01, DpsV01, BuffsV01, BuffsV01, BuffsV01, StatsAllV01>
    {
    }

    internal class StatsAllV01 : AbstractStatsAll
    {
    }

    internal class BuffsV01 : AbstractBuffs<BuffdataV01>
    {
    }

    internal class BuffdataV01 : AbstractBuffData
    {
    }

    internal class PhaseV01 : AbstractPhase
    {
    }

    internal class MechanicV01 : AbstractMechanic<MechanicsdataV01>
    {
    }

    internal class MechanicsdataV01 : AbstractMechanicData
    {
    }

}