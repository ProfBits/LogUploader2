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
        private readonly List<string> Messages = new List<string>();

        [TearDown]
        public void TearDwon()
        {
            Messages.Clear();
        }

        private void AppendCallback(string _, string message, Encoding encoding)
        {
            Messages.Add(message);
        }

        [Test]
        public void LogLevelSwitchLoggs()
        {
            Logger.TestInit("1.0.0", eLogLevel.NORMAL, AppendCallback);

            foreach (eLogLevel lvl in Enum.GetValues(typeof(eLogLevel)))
            {
                Messages.Clear();
                Logger.LogLevel = lvl;
                Assert.GreaterOrEqual(Messages.Count(), 1);
            }
        }


        [Test]
        public void LogDebug()
        {
            Logger.TestInit("1.0.0", eLogLevel.DEBUG, AppendCallback);
            Messages.Clear();
            eLogLevel[] sholdLog = new eLogLevel[] { eLogLevel.DEBUG };
            eLogLevel[] sholdNotLog = new eLogLevel[] { eLogLevel.SILETN, eLogLevel.ERROR, eLogLevel.WARN, eLogLevel.MINIMAL, eLogLevel.NORMAL, eLogLevel.VERBOSE };

            //Ensure All levles are included
            Assert.AreEqual(Enum.GetValues(typeof(eLogLevel)).Length, sholdLog.Length + sholdNotLog.Length);
            Assert.IsTrue(sholdLog.All(e => Enum.GetValues(typeof(eLogLevel)).Cast<eLogLevel>().Contains(e)));
            Assert.IsTrue(sholdNotLog.All(e => Enum.GetValues(typeof(eLogLevel)).Cast<eLogLevel>().Contains(e)));

            foreach (eLogLevel lvl in sholdLog)
            {
                Logger.LogLevel = lvl;
                Messages.Clear();
                Logger.Debug("Message");
                Assert.GreaterOrEqual(Messages.Count(), 1);
            }
            foreach (eLogLevel lvl in sholdLog)
            {
                Logger.LogLevel = lvl;
                Messages.Clear();
                Logger.Debug("Message");
                Assert.GreaterOrEqual(Messages.Count(), 1);
            }
        }

        [Test]
        public void LogVerbose()
        {
            Logger.TestInit("1.0.0", eLogLevel.DEBUG, AppendCallback);
            Messages.Clear();
            eLogLevel[] sholdLog = new eLogLevel[] { eLogLevel.DEBUG, eLogLevel.VERBOSE };
            eLogLevel[] sholdNotLog = new eLogLevel[] { eLogLevel.SILETN, eLogLevel.ERROR, eLogLevel.WARN, eLogLevel.MINIMAL, eLogLevel.NORMAL };

            //Ensure All levles are included
            Assert.AreEqual(Enum.GetValues(typeof(eLogLevel)).Length, sholdLog.Length + sholdNotLog.Length);
            Assert.IsTrue(sholdLog.All(e => Enum.GetValues(typeof(eLogLevel)).Cast<eLogLevel>().Contains(e)));
            Assert.IsTrue(sholdNotLog.All(e => Enum.GetValues(typeof(eLogLevel)).Cast<eLogLevel>().Contains(e)));

            foreach (eLogLevel lvl in sholdLog)
            {
                Logger.LogLevel = lvl;
                Messages.Clear();
                Logger.Verbose("Message");
                Assert.GreaterOrEqual(Messages.Count(), 1);
            }
            foreach (eLogLevel lvl in sholdLog)
            {
                Logger.LogLevel = lvl;
                Messages.Clear();
                Logger.Verbose("Message");
                Assert.GreaterOrEqual(Messages.Count(), 1);
            }
        }

        [Test]
        public void LogMessage()
        {
            Logger.TestInit("1.0.0", eLogLevel.DEBUG, AppendCallback);
            Messages.Clear();
            eLogLevel[] sholdLog = new eLogLevel[] { eLogLevel.DEBUG, eLogLevel.VERBOSE, eLogLevel.NORMAL };
            eLogLevel[] sholdNotLog = new eLogLevel[] { eLogLevel.SILETN, eLogLevel.ERROR, eLogLevel.WARN, eLogLevel.MINIMAL };

            //Ensure All levles are included
            Assert.AreEqual(Enum.GetValues(typeof(eLogLevel)).Length, sholdLog.Length + sholdNotLog.Length);
            Assert.IsTrue(sholdLog.All(e => Enum.GetValues(typeof(eLogLevel)).Cast<eLogLevel>().Contains(e)));
            Assert.IsTrue(sholdNotLog.All(e => Enum.GetValues(typeof(eLogLevel)).Cast<eLogLevel>().Contains(e)));

            foreach (eLogLevel lvl in sholdLog)
            {
                Logger.LogLevel = lvl;
                Messages.Clear();
                Logger.Message("Message");
                Assert.GreaterOrEqual(Messages.Count(), 1);
            }
            foreach (eLogLevel lvl in sholdLog)
            {
                Logger.LogLevel = lvl;
                Messages.Clear();
                Logger.Message("Message");
                Assert.GreaterOrEqual(Messages.Count(), 1);
            }
        }

        [Test]
        public void LogWarn()
        {
            Logger.TestInit("1.0.0", eLogLevel.DEBUG, AppendCallback);
            Messages.Clear();
            eLogLevel[] sholdLog = new eLogLevel[] { eLogLevel.DEBUG, eLogLevel.VERBOSE, eLogLevel.NORMAL, eLogLevel.MINIMAL, eLogLevel.WARN };
            eLogLevel[] sholdNotLog = new eLogLevel[] { eLogLevel.SILETN, eLogLevel.ERROR };

            //Ensure All levles are included
            Assert.AreEqual(Enum.GetValues(typeof(eLogLevel)).Length, sholdLog.Length + sholdNotLog.Length);
            Assert.IsTrue(sholdLog.All(e => Enum.GetValues(typeof(eLogLevel)).Cast<eLogLevel>().Contains(e)));
            Assert.IsTrue(sholdNotLog.All(e => Enum.GetValues(typeof(eLogLevel)).Cast<eLogLevel>().Contains(e)));

            foreach (eLogLevel lvl in sholdLog)
            {
                Logger.LogLevel = lvl;
                Messages.Clear();
                Logger.Warn("Message");
                Assert.GreaterOrEqual(Messages.Count(), 1);
            }
            foreach (eLogLevel lvl in sholdLog)
            {
                Logger.LogLevel = lvl;
                Messages.Clear();
                Logger.Warn("Message");
                Assert.GreaterOrEqual(Messages.Count(), 1);
            }
        }

        [Test]
        public void LogError()
        {
            Logger.TestInit("1.0.0", eLogLevel.DEBUG, AppendCallback);
            Messages.Clear();
            eLogLevel[] sholdLog = new eLogLevel[] { eLogLevel.DEBUG, eLogLevel.VERBOSE, eLogLevel.NORMAL, eLogLevel.MINIMAL, eLogLevel.WARN, eLogLevel.ERROR };
            eLogLevel[] sholdNotLog = new eLogLevel[] { eLogLevel.SILETN};

            //Ensure All levles are included
            Assert.AreEqual(Enum.GetValues(typeof(eLogLevel)).Length, sholdLog.Length + sholdNotLog.Length);
            Assert.IsTrue(sholdLog.All(e => Enum.GetValues(typeof(eLogLevel)).Cast<eLogLevel>().Contains(e)));
            Assert.IsTrue(sholdNotLog.All(e => Enum.GetValues(typeof(eLogLevel)).Cast<eLogLevel>().Contains(e)));

            foreach (eLogLevel lvl in sholdLog)
            {
                Logger.LogLevel = lvl;
                Messages.Clear();
                Logger.Error("Message");
                Assert.GreaterOrEqual(Messages.Count(), 1);
            }
            foreach (eLogLevel lvl in sholdLog)
            {
                Logger.LogLevel = lvl;
                Messages.Clear();
                Logger.Error("Message");
                Assert.GreaterOrEqual(Messages.Count(), 1);
            }
        }

        [Test]
        public void LogExceptionLevel()
        {
            Logger.TestInit("1.0.0", eLogLevel.DEBUG, AppendCallback);
            Messages.Clear();
            eLogLevel[] sholdLog = new eLogLevel[] { eLogLevel.DEBUG, eLogLevel.VERBOSE, eLogLevel.NORMAL, eLogLevel.MINIMAL, eLogLevel.WARN, eLogLevel.ERROR };
            eLogLevel[] sholdNotLog = new eLogLevel[] { eLogLevel.SILETN };

            //Ensure All levles are included
            Assert.AreEqual(Enum.GetValues(typeof(eLogLevel)).Length, sholdLog.Length + sholdNotLog.Length);
            Assert.IsTrue(sholdLog.All(e => Enum.GetValues(typeof(eLogLevel)).Cast<eLogLevel>().Contains(e)));
            Assert.IsTrue(sholdNotLog.All(e => Enum.GetValues(typeof(eLogLevel)).Cast<eLogLevel>().Contains(e)));

            foreach (eLogLevel lvl in sholdLog)
            {
                Logger.LogLevel = lvl;
                Messages.Clear();
                Logger.LogException(new NotImplementedException("Test"));
                Assert.GreaterOrEqual(Messages.Count(), 1);
            }
            foreach (eLogLevel lvl in sholdLog)
            {
                Logger.LogLevel = lvl;
                Messages.Clear();
                Logger.LogException(new NotImplementedException("Test"));
                Assert.GreaterOrEqual(Messages.Count(), 1);
            }
        }

        [Test]
        public void LogInnerException()
        {
            Logger.TestInit("1.0.0", eLogLevel.DEBUG, AppendCallback);
            Messages.Clear();
            var ex = new Exception("outer", new NotImplementedException("midlevel", new ArgumentNullException("inner")));
            string[] shouldBeContaind = new string[] { "outer", "midlevel", "inner", "Exception", "NotImplementedException", "ArgumentNullException" };

            Logger.LogException(ex);
            foreach (var e in shouldBeContaind)
            {
                Assert.IsTrue(Messages.Any(m => m.Contains(e)), $"\"{e}\" is missing in the log");
            }

        }
    }
}
