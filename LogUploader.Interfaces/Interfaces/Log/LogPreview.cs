using System.Collections.Generic;

namespace LogUploader.Interfaces.Interfaces
{
    interface LogPreview : LogBasic
    {
        IReadOnlyList<LogPreviewPlayer> Players { get; }
        bool EvtcExists { get; }
        bool UpgradeAvailable { get; }
    }
}

