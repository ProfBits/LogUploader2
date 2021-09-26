namespace LogUploader.Tools.EliteInsights.Json
{
     internal abstract class AbstractJson<TPlayer, TTarget, TPhase, TMechanik> : IInternalLog
        where TPlayer : IInternalPlayer
        where TTarget : IInternalTarget
        where TPhase : IInternalPhase
        where TMechanik : IInternalMechanic
    {
        public string eliteInsightsVersion { get; set; }
        public int triggerID { get; set; }
        public string fightName { get; set; }
        public string arcVersion { get; set; }
        public string recordedBy { get; set; }
        public string timeStart { get; set; }
        public string timeEnd { get; set; }
        public string duration { get; set; }
        public bool success { get; set; }
        public TTarget[] targets { get; set; }
        public TPlayer[] players { get; set; }
        public TPhase[] phases { get; set; }
        public TMechanik[] mechanics { get; set; }
    }
    
     internal abstract class AbstractTarget : IInternalTarget
    {
        public int id { get; set; }
        public int totalHealth { get; set; }
        public int finalHealth { get; set; }
        public string name { get; set; }
    }

     internal abstract class AbstractDps : IInternalDps
    {
        public int dps { get; set; }
        public int condiDps { get; set; }
        public int powerDps { get; set; }
    }

     internal abstract class AbstractPlayer<TDpsTargets, TDpsAll, TBuffUptimes, TGroupBuffs, TSqudBuffs, TStatsAll> : IInternalPlayer
        where TDpsTargets : IInternalDps
        where TDpsAll : IInternalDps
        where TBuffUptimes : IInternalBuffs
        where TGroupBuffs : IInternalBuffs
        where TSqudBuffs : IInternalBuffs
        where TStatsAll : IInternalStatsAll
    {
        public string account { get; set; }
        public int group { get; set; }
        public string profession { get; set; }
        public string[] weapons { get; set; }
        public TDpsTargets[][] dpsTargets { get; set; }
        public string name { get; set; }
        public TDpsAll[] dpsAll { get; set; }
        public TBuffUptimes[] buffUptimes { get; set; }
        public TGroupBuffs[] groupBuffs { get; set; }
        public TSqudBuffs[] squadBuffs { get; set; }
        public TStatsAll[] statsAll { get; set; }
    }

    internal abstract class AbstractStatsAll : IInternalStatsAll
    {
        public float stackDist { get; set; }
    }

    internal abstract class AbstractBuffs<TBuffData> : IInternalBuffs
        where TBuffData : IInternalBuffData
    {
        public int id { get; set; }
        public TBuffData[] buffData { get; set; }
    }

     internal abstract class AbstractBuffData : IInternalBuffData
    {
        public float uptime { get; set; }
        public float generation { get; set; }
    }
 
     internal abstract class AbstractPhase : IInternalPhase
    {
        public int start { get; set; }
        public int end { get; set; }
        public string name { get; set; }
        public int[] subPhases { get; set; }
    }

     internal abstract class AbstractMechanic<TMechanikData> : IInternalMechanic
        where TMechanikData : IInternalMechanicData
    {
        public TMechanikData[] mechanicsData { get; set; }
        public string name { get; set; }
    }

     internal abstract class AbstractMechanicData : IInternalMechanicData
    {
        public int time { get; set; }
        public string actor { get; set; }
    }

}
