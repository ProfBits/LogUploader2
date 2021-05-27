using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LogUploader.Tools.Logging;

using NUnit.Framework;

namespace LogUploader.Test.Tools
{
    public class LoggerTest
    {
        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            Mocks.MockSetup.InjectMoks();
        }

        [SetUp]
        public void Setup()
        {
            Mocks.MockSetup.ResetAll();
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            Mocks.MockSetup.ResetAll();
        }

        [TearDown]
        public void TearDwon()
        {
            Mocks.MockSetup.ResetAll();
        }

        [Test]
        public void InitLogging()
        {
            Logger.Init("1.0.0", eLogLevel.DEBUG);
            Assert.AreEqual(1, Mocks.MockDirectoryIO.Instance.GetFiles(Logger.FULL_LOG_DIR).Count());
        }

        [Test]
        public void LogLevelSwitchLoggs()
        {
            Logger.Init("1.0.0", eLogLevel.NORMAL);
            var logfile = Mocks.MockDirectoryIO.Instance.GetFiles(Logger.FULL_LOG_DIR).First();

            foreach (eLogLevel lvl in Enum.GetValues(typeof(eLogLevel)))
            {
                Mocks.MockFileIO.Instance.WriteAllText(logfile, "", Encoding.UTF8);
                Logger.LogLevel = lvl;
                Assert.GreaterOrEqual(Mocks.MockFileIO.Instance.ReadAllLines(logfile, Encoding.UTF8).Length, 1,
                    $"Log Message for loglevle switch to {lvl} missing");
            }
        }


        [Test]
        public void LogDebug()
        {
            Logger.Init("1.0.0", eLogLevel.DEBUG);
            var logfile = Mocks.MockDirectoryIO.Instance.GetFiles(Logger.FULL_LOG_DIR).First();
            eLogLevel[] sholdLog = new eLogLevel[] { eLogLevel.DEBUG };
            eLogLevel[] sholdNotLog = new eLogLevel[] { eLogLevel.SILETN, eLogLevel.ERROR, eLogLevel.WARN, eLogLevel.MINIMAL, eLogLevel.NORMAL, eLogLevel.VERBOSE };

            //Ensure All levles are included
            Assert.AreEqual(Enum.GetValues(typeof(eLogLevel)).Length, sholdLog.Length + sholdNotLog.Length);
            Assert.IsTrue(sholdLog.All(e => Enum.GetValues(typeof(eLogLevel)).Cast<eLogLevel>().Contains(e)));
            Assert.IsTrue(sholdNotLog.All(e => Enum.GetValues(typeof(eLogLevel)).Cast<eLogLevel>().Contains(e)));

            foreach (eLogLevel lvl in sholdLog)
            {
                Logger.LogLevel = lvl;
                Mocks.MockFileIO.Instance.WriteAllText(logfile, "", Encoding.UTF8);
                Logger.Debug("Message");
                Assert.GreaterOrEqual(Mocks.MockFileIO.Instance.ReadAllLines(logfile, Encoding.UTF8).Length, 1);
            }
            foreach (eLogLevel lvl in sholdLog)
            {
                Logger.LogLevel = lvl;
                Mocks.MockFileIO.Instance.WriteAllText(logfile, "", Encoding.UTF8);
                Logger.Debug("Message");
                Assert.GreaterOrEqual(Mocks.MockFileIO.Instance.ReadAllLines(logfile, Encoding.UTF8).Length, 1);
            }
        }

        [Test]
        public void LogVerbose()
        {
            Logger.Init("1.0.0", eLogLevel.DEBUG);
            var logfile = Mocks.MockDirectoryIO.Instance.GetFiles(Logger.FULL_LOG_DIR).First();
            eLogLevel[] sholdLog = new eLogLevel[] { eLogLevel.DEBUG, eLogLevel.VERBOSE };
            eLogLevel[] sholdNotLog = new eLogLevel[] { eLogLevel.SILETN, eLogLevel.ERROR, eLogLevel.WARN, eLogLevel.MINIMAL, eLogLevel.NORMAL };

            //Ensure All levles are included
            Assert.AreEqual(Enum.GetValues(typeof(eLogLevel)).Length, sholdLog.Length + sholdNotLog.Length);
            Assert.IsTrue(sholdLog.All(e => Enum.GetValues(typeof(eLogLevel)).Cast<eLogLevel>().Contains(e)));
            Assert.IsTrue(sholdNotLog.All(e => Enum.GetValues(typeof(eLogLevel)).Cast<eLogLevel>().Contains(e)));

            foreach (eLogLevel lvl in sholdLog)
            {
                Logger.LogLevel = lvl;
                Mocks.MockFileIO.Instance.WriteAllText(logfile, "", Encoding.UTF8);
                Logger.Verbose("Message");
                Assert.GreaterOrEqual(Mocks.MockFileIO.Instance.ReadAllLines(logfile, Encoding.UTF8).Length, 1);
            }
            foreach (eLogLevel lvl in sholdLog)
            {
                Logger.LogLevel = lvl;
                Mocks.MockFileIO.Instance.WriteAllText(logfile, "", Encoding.UTF8);
                Logger.Verbose("Message");
                Assert.GreaterOrEqual(Mocks.MockFileIO.Instance.ReadAllLines(logfile, Encoding.UTF8).Length, 1);
            }
        }

        [Test]
        public void LogMessage()
        {
            Logger.Init("1.0.0", eLogLevel.DEBUG);
            var logfile = Mocks.MockDirectoryIO.Instance.GetFiles(Logger.FULL_LOG_DIR).First();
            eLogLevel[] sholdLog = new eLogLevel[] { eLogLevel.DEBUG, eLogLevel.VERBOSE, eLogLevel.NORMAL };
            eLogLevel[] sholdNotLog = new eLogLevel[] { eLogLevel.SILETN, eLogLevel.ERROR, eLogLevel.WARN, eLogLevel.MINIMAL };

            //Ensure All levles are included
            Assert.AreEqual(Enum.GetValues(typeof(eLogLevel)).Length, sholdLog.Length + sholdNotLog.Length);
            Assert.IsTrue(sholdLog.All(e => Enum.GetValues(typeof(eLogLevel)).Cast<eLogLevel>().Contains(e)));
            Assert.IsTrue(sholdNotLog.All(e => Enum.GetValues(typeof(eLogLevel)).Cast<eLogLevel>().Contains(e)));

            foreach (eLogLevel lvl in sholdLog)
            {
                Logger.LogLevel = lvl;
                Mocks.MockFileIO.Instance.WriteAllText(logfile, "", Encoding.UTF8);
                Logger.Message("Message");
                Assert.GreaterOrEqual(Mocks.MockFileIO.Instance.ReadAllLines(logfile, Encoding.UTF8).Length, 1);
            }
            foreach (eLogLevel lvl in sholdLog)
            {
                Logger.LogLevel = lvl;
                Mocks.MockFileIO.Instance.WriteAllText(logfile, "", Encoding.UTF8);
                Logger.Message("Message");
                Assert.GreaterOrEqual(Mocks.MockFileIO.Instance.ReadAllLines(logfile, Encoding.UTF8).Length, 1);
            }
        }

        [Test]
        public void LogWarn()
        {
            Logger.Init("1.0.0", eLogLevel.DEBUG);
            var logfile = Mocks.MockDirectoryIO.Instance.GetFiles(Logger.FULL_LOG_DIR).First();
            eLogLevel[] sholdLog = new eLogLevel[] { eLogLevel.DEBUG, eLogLevel.VERBOSE, eLogLevel.NORMAL, eLogLevel.MINIMAL, eLogLevel.WARN };
            eLogLevel[] sholdNotLog = new eLogLevel[] { eLogLevel.SILETN, eLogLevel.ERROR };

            //Ensure All levles are included
            Assert.AreEqual(Enum.GetValues(typeof(eLogLevel)).Length, sholdLog.Length + sholdNotLog.Length);
            Assert.IsTrue(sholdLog.All(e => Enum.GetValues(typeof(eLogLevel)).Cast<eLogLevel>().Contains(e)));
            Assert.IsTrue(sholdNotLog.All(e => Enum.GetValues(typeof(eLogLevel)).Cast<eLogLevel>().Contains(e)));

            foreach (eLogLevel lvl in sholdLog)
            {
                Logger.LogLevel = lvl;
                Mocks.MockFileIO.Instance.WriteAllText(logfile, "", Encoding.UTF8);
                Logger.Warn("Message");
                Assert.GreaterOrEqual(Mocks.MockFileIO.Instance.ReadAllLines(logfile, Encoding.UTF8).Length, 1);
            }
            foreach (eLogLevel lvl in sholdLog)
            {
                Logger.LogLevel = lvl;
                Mocks.MockFileIO.Instance.WriteAllText(logfile, "", Encoding.UTF8);
                Logger.Warn("Message");
                Assert.GreaterOrEqual(Mocks.MockFileIO.Instance.ReadAllLines(logfile, Encoding.UTF8).Length, 1);
            }
        }

        [Test]
        public void LogError()
        {
            Logger.Init("1.0.0", eLogLevel.DEBUG);
            var logfile = Mocks.MockDirectoryIO.Instance.GetFiles(Logger.FULL_LOG_DIR).First();
            eLogLevel[] sholdLog = new eLogLevel[] { eLogLevel.DEBUG, eLogLevel.VERBOSE, eLogLevel.NORMAL, eLogLevel.MINIMAL, eLogLevel.WARN, eLogLevel.ERROR };
            eLogLevel[] sholdNotLog = new eLogLevel[] { eLogLevel.SILETN};

            //Ensure All levles are included
            Assert.AreEqual(Enum.GetValues(typeof(eLogLevel)).Length, sholdLog.Length + sholdNotLog.Length);
            Assert.IsTrue(sholdLog.All(e => Enum.GetValues(typeof(eLogLevel)).Cast<eLogLevel>().Contains(e)));
            Assert.IsTrue(sholdNotLog.All(e => Enum.GetValues(typeof(eLogLevel)).Cast<eLogLevel>().Contains(e)));

            foreach (eLogLevel lvl in sholdLog)
            {
                Logger.LogLevel = lvl;
                Mocks.MockFileIO.Instance.WriteAllText(logfile, "", Encoding.UTF8);
                Logger.Error("Message");
                Assert.GreaterOrEqual(Mocks.MockFileIO.Instance.ReadAllLines(logfile, Encoding.UTF8).Length, 1);
            }
            foreach (eLogLevel lvl in sholdLog)
            {
                Logger.LogLevel = lvl;
                Mocks.MockFileIO.Instance.WriteAllText(logfile, "", Encoding.UTF8);
                Logger.Error("Message");
                Assert.GreaterOrEqual(Mocks.MockFileIO.Instance.ReadAllLines(logfile, Encoding.UTF8).Length, 1);
            }
        }

        [Test]
        public void LogExceptionLevel()
        {
            Logger.Init("1.0.0", eLogLevel.DEBUG);
            var logfile = Mocks.MockDirectoryIO.Instance.GetFiles(Logger.FULL_LOG_DIR).First();
            eLogLevel[] sholdLog = new eLogLevel[] { eLogLevel.DEBUG, eLogLevel.VERBOSE, eLogLevel.NORMAL, eLogLevel.MINIMAL, eLogLevel.WARN, eLogLevel.ERROR };
            eLogLevel[] sholdNotLog = new eLogLevel[] { eLogLevel.SILETN };

            //Ensure All levles are included
            Assert.AreEqual(Enum.GetValues(typeof(eLogLevel)).Length, sholdLog.Length + sholdNotLog.Length);
            Assert.IsTrue(sholdLog.All(e => Enum.GetValues(typeof(eLogLevel)).Cast<eLogLevel>().Contains(e)));
            Assert.IsTrue(sholdNotLog.All(e => Enum.GetValues(typeof(eLogLevel)).Cast<eLogLevel>().Contains(e)));

            foreach (eLogLevel lvl in sholdLog)
            {
                Logger.LogLevel = lvl;
                Mocks.MockFileIO.Instance.WriteAllText(logfile, "", Encoding.UTF8);
                Logger.LogException(new NotImplementedException("Test"));
                Assert.GreaterOrEqual(Mocks.MockFileIO.Instance.ReadAllLines(logfile, Encoding.UTF8).Length, 1);
            }
            foreach (eLogLevel lvl in sholdLog)
            {
                Logger.LogLevel = lvl;
                Mocks.MockFileIO.Instance.WriteAllText(logfile, "", Encoding.UTF8);
                Logger.LogException(new NotImplementedException("Test"));
                Assert.GreaterOrEqual(Mocks.MockFileIO.Instance.ReadAllLines(logfile, Encoding.UTF8).Length, 1);
            }
        }

        [Test]
        public void LogInnerException()
        {
            Logger.Init("1.0.0", eLogLevel.DEBUG);
            var logfile = Mocks.MockDirectoryIO.Instance.GetFiles(Logger.FULL_LOG_DIR).First();
            var ex = new Exception("outer", new NotImplementedException("midlevel", new ArgumentNullException("inner")));
            string[] shouldBeContaind = new string[] { "outer", "midlevel", "inner", "Exception", "NotImplementedException", "ArgumentNullException" };

            Logger.LogException(ex);
            foreach (var e in shouldBeContaind)
            {
                var LogFileContent = Mocks.MockFileIO.Instance.ReadAllLines(logfile, Encoding.UTF8);
                Assert.IsTrue(LogFileContent.Any(m => m.Contains(e)), $"\"{e}\" is missing in the log");
            }

        }
    }
}
