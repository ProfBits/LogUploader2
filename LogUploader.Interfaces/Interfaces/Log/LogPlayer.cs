using System.Collections.Generic;

namespace LogUploader.Interfaces.Interfaces
{
    interface LogPlayer
    {
        string AccountName { get; }
        string CharakterName { get; }
        byte SubGroup { get; }
        LogPhase FullFight { get; }
        IReadOnlyList<LogPhase> PhaseData { get; }
    }
}

