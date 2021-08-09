using System.Collections.Generic;

namespace LogUploader.Interfaces
{
    public interface LogFull : LogBasic
    {
        IReadOnlyList<LogTarget> Targets { get; }
        IReadOnlyList<LogPlayer> Players { get; }
    }
}

