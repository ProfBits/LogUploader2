using System.Collections.Generic;

using LogUploader.Data;

namespace LogUploader.Interfaces
{
    public interface LogPlayer
    {
        string AccountName { get; }
        string CharakterName { get; }
        IProfession Profession { get; }
        byte SubGroup { get; }
        LogPhase FullFight { get; }
        IReadOnlyList<LogPhase> Phases { get; }
    }
}

