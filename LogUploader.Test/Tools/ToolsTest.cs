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
        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            Mocks.MockSetup.SetupLogger();
        }

        [SetUp]
        [TearDown]
        public void CleanUp()
        {
            Mocks.MockSetup.ClearLogMessages();
        }

        public void TestExecuteHelperCatchesVoid()
        {
            Assert.DoesNotThrow(() => ExecuteHelper.ExecuteSecure(() => throw new NotImplementedException("exMmsgTest")));
            Assert.IsTrue(Mocks.MockSetup.GetLatestLogMessages().Any(m => m.Contains("exMmsgTest")));
            Assert.IsTrue(Mocks.MockSetup.GetLatestLogMessages().Any(m => m.Contains("NotImplementedException")));
        }

        [Test]
        public void TestExecuteHelperRunsVoid()
        {
            Assert.DoesNotThrow(() => ExecuteHelper.ExecuteSecure(() => { }));
            Assert.AreEqual("", string.Join("\n", Mocks.MockSetup.GetLatestLogMessages()));
        }

        [Test]
        public void TestExecuteHelperCatchesReturn()
        {
            Assert.DoesNotThrow(() => ExecuteHelper.ExecuteSecure<int>(() => throw new NotImplementedException("exMmsgTest")));
            Assert.IsTrue(Mocks.MockSetup.GetLatestLogMessages().Any(m => m.Contains("exMmsgTest")));
            Assert.IsTrue(Mocks.MockSetup.GetLatestLogMessages().Any(m => m.Contains("NotImplementedException")));
        }

        [Test]
        public void TestExecuteHelperRunsReturn()
        {
            Assert.DoesNotThrow(() => ExecuteHelper.ExecuteSecure(() => 0));
            Assert.AreEqual("", string.Join("\n", Mocks.MockSetup.GetLatestLogMessages()));
        }

        [Test]
        public void ExecuteHelperGuiCallbackNotNull()
        {
           Assert.Catch<ArgumentNullException>(() => ExecuteHelper.GetExceptionUI = null);
        }
    }
}
