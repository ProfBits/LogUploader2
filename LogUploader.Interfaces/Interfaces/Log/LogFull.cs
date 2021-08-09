using System.Collections.Generic;

namespace LogUploader.Interfaces.Interfaces
{
    interface LogFull : LogBasic
    {
        IReadOnlyList<LogTarget> Targets { get; }
        IReadOnlyList<LogPlayer> Players { get; }
    }
}

