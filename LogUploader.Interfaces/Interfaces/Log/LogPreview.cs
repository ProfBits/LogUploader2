using System.Collections.Generic;

namespace LogUploader.Interfaces
{
    public interface LogPreview : LogBasic
    {
        IReadOnlyList<LogPreviewPlayer> Players { get; }
    }
}

