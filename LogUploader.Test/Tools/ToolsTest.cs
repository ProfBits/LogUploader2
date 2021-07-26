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
            int res = ExecuteHelper.ExecuteSecure(() => 42);
            Assert.AreEqual(42, res);
            Assert.AreEqual("", string.Join("\n", Mocks.MockSetup.GetLatestLogMessages()));
        }

        [Test]
        public void ExecuteHelperGuiCallbackNotNull()
        {
           Assert.Catch<ArgumentNullException>(() => ExecuteHelper.GetExceptionUI = null);
        }

        [Test]
        public void JsonHandlingCreateTest()
        {
            string testFilePaht = TestSetup.GetPathToTestFiles("temp", "jsonHandlingCreateTest.json");
            if (System.IO.File.Exists(testFilePaht))
                System.IO.File.Delete(testFilePaht);

            JsonHandling.WriteJsonFile(testFilePaht, "");
            FileAssert.Exists(testFilePaht);

            System.IO.File.Delete(testFilePaht);
        }

        [Test]
        public void JsonHandlingCreateAndReadTest()
        {
            string testFilePaht = TestSetup.GetPathToTestFiles("temp", "jsonHandlingCreateAndReadTest.json");
            string testContent = "{\"test\":42}";
            if (System.IO.File.Exists(testFilePaht))
                System.IO.File.Delete(testFilePaht);

            JsonHandling.WriteJsonFile(testFilePaht, testContent);
            FileAssert.Exists(testFilePaht);
            Assert.AreEqual(testContent, JsonHandling.ReadJsonFile(testFilePaht));

            System.IO.File.Delete(testFilePaht);
        }

        [Test]
        public void JsonHandlingOverrideAndReadTest()
        {
            string testFilePaht = TestSetup.GetPathToTestFiles("temp", "jsonHandlingOverrideAndReadTest.json");
            string testContent = "{\"test\":42}";
            if (System.IO.File.Exists(testFilePaht))
                System.IO.File.Delete(testFilePaht);

            JsonHandling.WriteJsonFile(testFilePaht, "");
            JsonHandling.WriteJsonFile(testFilePaht, testContent);
            FileAssert.Exists(testFilePaht);
            Assert.AreEqual(testContent, JsonHandling.ReadJsonFile(testFilePaht));

            System.IO.File.Delete(testFilePaht);
        }

        [Test]
        public void JsonHandlingCreateAndReadSpecialCharactresTest()
        {
            string testFilePaht = TestSetup.GetPathToTestFiles("temp", "jsonHandlingCreateAndReadSpecialCharactresTest.json");
            string testContent = "{\"test\":\"äöüÄÖÜß^\"}";
            if (System.IO.File.Exists(testFilePaht))
                System.IO.File.Delete(testFilePaht);

            JsonHandling.WriteJsonFile(testFilePaht, testContent);
            FileAssert.Exists(testFilePaht);
            Assert.AreEqual(testContent, JsonHandling.ReadJsonFile(testFilePaht));

            System.IO.File.Delete(testFilePaht);
        }

        [Test]
        public void EnumHelperTest()
        {
            IEnumerable<TestEnumHelper> res = EnumHelper.GetValues<TestEnumHelper>();
            Assert.AreEqual(3, res.Count());
            CollectionAssert.AllItemsAreUnique(res);
            CollectionAssert.Contains(res, TestEnumHelper.VarA);
            CollectionAssert.Contains(res, TestEnumHelper.VarB);
            CollectionAssert.Contains(res, TestEnumHelper.VarC);
        }

        private enum TestEnumHelper
        {
            VarA,
            VarB,
            VarC
        }

        [Test]
        public void UnixTimeStampToDateTime([Values(0, 60, -60, 3600, -3600, 36000, -36000, 36500, -36500)] int offset)
        {
            DateTime exptected = new DateTime(2021, 7, 26, 8, 57, 29).Add(new TimeSpan(0, 0, offset));
            var actuel = GP.UnixTimeStampToDateTime(1627282649 + offset);
            Assert.AreEqual(exptected, actuel);
        }
    }
}
