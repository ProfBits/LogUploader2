using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

using LogUploader.Tools;
using LogUploader.Tools.Logging;

using NUnit.Framework;

namespace LogUploader.Test.Tools
{
    class ToolsTest
    {
        private readonly List<string> Messages = new List<string>();

        private void AppendCallback(string _, string message, Encoding encoding)
        {
            Messages.Add(message);
        }

        [Test]
        public void TestExecuteHelperCatchesVoid()
        {
            Logger.TestInit("1.0.0", eLogLevel.ERROR, AppendCallback);
            Messages.Clear();

            Assert.DoesNotThrow(() => ExecuteHelper.ExecuteSecure(() => throw new NotImplementedException("exMmsgTest")));
            Assert.IsTrue(Messages.Any(m => m.Contains("exMmsgTest")));
            Assert.IsTrue(Messages.Any(m => m.Contains("NotImplementedException")));
        }

        [Test]
        public void TestExecuteHelperRunsVoid()
        {
            Logger.TestInit("1.0.0", eLogLevel.ERROR, AppendCallback);
            Messages.Clear();

            Assert.DoesNotThrow(() => ExecuteHelper.ExecuteSecure(() => { }));
            Assert.AreEqual(0, Messages.Count());
        }

        [Test]
        public void TestExecuteHelperCatchesReturn()
        {
            Logger.TestInit("1.0.0", eLogLevel.ERROR, AppendCallback);
            Messages.Clear();

            Assert.DoesNotThrow(() => ExecuteHelper.ExecuteSecure<int>(() => throw new NotImplementedException("exMmsgTest")));
            Assert.IsTrue(Messages.Any(m => m.Contains("exMmsgTest")));
            Assert.IsTrue(Messages.Any(m => m.Contains("NotImplementedException")));
        }

        [Test]
        public void TestExecuteHelperRunsReturn()
        {
            Logger.TestInit("1.0.0", eLogLevel.ERROR, AppendCallback);
            Messages.Clear();

            Assert.DoesNotThrow(() => ExecuteHelper.ExecuteSecure(() => 0));
            Assert.AreEqual(0, Messages.Count());
        }

        [Test]
        public void ExecuteHelperGuiCallbackNotNull()
        {
           Assert.Catch<ArgumentNullException>(() => ExecuteHelper.GetExceptionUI = null);
        }
    }
}
