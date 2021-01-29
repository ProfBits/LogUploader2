using System;

namespace LogUploader.Tools.Logger
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
