using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NUnit.Framework;

using LogUploader.Tools.EliteInsights.Json;
using System.IO.Compression;
using LogUploader.Interfaces;
using Newtonsoft.Json.Linq;

namespace LogUploader.Test.EliteInsights
{
    [Explicit]
    [Category(TestCategory.LogRunning)]
    internal abstract class AbstractEliteInsightsProcessorTest
    {
        private const string VALIDATE_FILE_EXT = ".validate.json";

        protected abstract string TestJsonZipName { get; }
        protected abstract string TestTempFolderName { get; }
        internal string AbsolutePathToTestJsonsZip { get => TestSetup.GetPathToTestFiles("eiJsonZips", TestJsonZipName); }
        internal string AbsolutePathToTestTempFolder { get => TestSetup.GetPathToTestFiles("temp", TestTempFolderName); }

        protected abstract IEiProcessor GetProcesor();

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            ZipFile.ExtractToDirectory(AbsolutePathToTestJsonsZip, AbsolutePathToTestTempFolder);
        }

        [Test]
        public void CanProcessJsonsTest()
        {
            foreach (string file in System.IO.Directory.GetFiles(AbsolutePathToTestTempFolder, $"*{VALIDATE_FILE_EXT}"))
            {
                StringAssert.EndsWith(VALIDATE_FILE_EXT, file);
                string dataFile = file.Substring(0, file.Length - VALIDATE_FILE_EXT.Length) + ".json";
                FileAssert.Exists(dataFile);

                LogFull actual = ProcessJson(dataFile);
                LogFull expected = ProcessJson(file);
                Validate(actual, expected);
            }
        }

        private LogFull ProcessJson(string absPathToDataJson)
        {
            string json = LogUploader.Tools.JsonHandling.ReadJsonFile(absPathToDataJson);
            JObject parsed = JObject.Parse(json);
            return GetProcesor().Process(parsed);
        }

        private LogFull PrepeareValidation(string absPathToValidateJson)
        {
            string json = LogUploader.Tools.JsonHandling.ReadJsonFile(absPathToValidateJson);
            JObject parsed = JObject.Parse(json);
            return GetProcesor().Process(parsed);
        }

        protected virtual LogFull CreateValidation(JObject data)
        {
            Assert.Warn("CreateValidation not Implemented");
            return null;
        }

        protected virtual void Validate(LogFull actual, LogFull exptected)
        {
            Assert.Warn("Validate not Implemented");
        }
    }

    [Explicit]
    [Category(TestCategory.LogRunning)]
    internal class EiProcessor_LegacyTest : AbstractEliteInsightsProcessorTest
    {
        protected override string TestJsonZipName { get => "legacy.zip"; }
        protected override string TestTempFolderName { get => "legacy"; }

        protected override IEiProcessor GetProcesor()
        {
            return new EiProcessor_Legacy();
        }
    }

    [Explicit]
    [Category(TestCategory.LogRunning)]
    internal class EiProcessor_2_29_0_0Test : AbstractEliteInsightsProcessorTest
    {
        protected override string TestJsonZipName { get => "2_29_0_0.zip"; }
        protected override string TestTempFolderName { get => "2_29_0_0"; }

        protected override IEiProcessor GetProcesor()
        {
            return new EiProcessor_2_29_0_0();
        }
    }

    [Explicit]
    [Category(TestCategory.LogRunning)]
    internal class EiProcessor_2_33_0_0Test : AbstractEliteInsightsProcessorTest
    {
        protected override string TestJsonZipName { get => "2_33_0_0.zip"; }
        protected override string TestTempFolderName { get => "2_33_0_0"; }

        protected override IEiProcessor GetProcesor()
        {
            return new EiProcessor_2_33_0_0();
        }
    }

    [Explicit]
    [Category(TestCategory.LogRunning)]
    internal class EiProcessor_2_35_0_0Test : AbstractEliteInsightsProcessorTest
    {
        protected override string TestJsonZipName { get => "2_35_0_0.zip"; }
        protected override string TestTempFolderName { get => "2_35_0_0"; }

        protected override IEiProcessor GetProcesor()
        {
            return new EiProcessor_2_35_0_0();
        }
    }
}
