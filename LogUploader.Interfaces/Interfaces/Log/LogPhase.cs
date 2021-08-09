using System.Collections.Generic;

namespace LogUploader.Interfaces
{
    public interface LogPhase
    {
        LogDps DpsAll { get; }
        IReadOnlyDictionary<int, LogDps> DpsTarget { get; }
        LogBuffs Buffs { get; }
    }
}

