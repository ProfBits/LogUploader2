using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LogUploader.Tools.Logging;

namespace LogUploader.Test.Mocks
{
    internal static class MockSetup
    {
        internal static void SetupLogger(eLogLevel loglevel = eLogLevel.DEBUG)
        {
            Logger.Init("1.0.0", loglevel);
        }

        internal static string[] GetLatestLogMessages()
        {
            return Wrapper.FileIO.ReadAllLines(Logger.LogFile, Encoding.UTF8);
        }

        internal static void ClearLogMessages()
        {
            Wrapper.FileIO.WriteAllText(Logger.LogFile, "", Encoding.UTF8);
        }
    }
}
