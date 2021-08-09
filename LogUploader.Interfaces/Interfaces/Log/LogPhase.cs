using System.Collections.Generic;

namespace LogUploader.Interfaces.Interfaces
{
    interface LogPhase
    {
        LogDps DpsAll { get; }
        IReadOnlyDictionary<int, LogDps> DpsTarget { get; }
        LogBuffs Buffs { get; }
    }
}

