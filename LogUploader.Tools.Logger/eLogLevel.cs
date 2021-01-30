using System;

namespace LogUploader.Tools.Logging
{
    public enum eLogLevel : int
    {
        [Obsolete]
        SILETN,
        ERROR,
        WARN,
        MINIMAL,
        NORMAL,
        VERBOSE,
        DEBUG,
    }
}
