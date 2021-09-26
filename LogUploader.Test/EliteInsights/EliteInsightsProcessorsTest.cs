using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NUnit.Framework;

using LogUploader.Tools.EliteInsights.Json;
using LogUploader.Interfaces;
using Throws = LogUploader.Test.Constraints.Throws;

namespace LogUploader.Test.EliteInsights
{
    internal abstract class AbstractEliteInsightsProcessorTest
    {
        protected string GetFullPahtForFile(string jsonName)
        {
            string path = TestSetup.GetPathToTestFiles($"static{System.IO.Path.DirectorySeparatorChar}EiJson", jsonName + ".json");
            Assert.That(path, Does.Exist);
            return path;
        }

        protected abstract IEiProcessor GetProcesor();

        protected abstract IValidator GetValidator(string paht);

        public abstract void CanProcessJsonsTest(string jsons);

        protected void CanProcessJsonsTestImpl(string json)
        {
            string path = GetFullPahtForFile(json);

            var processor = GetProcesor();
            object result = null;

            using (var fs = System.IO.File.OpenRead(GetFullPahtForFile(json)))
            {
                Assert.That(() => result = processor.Process(fs), Throws.Nothing);
            }

            Assert.That(result, Is.Not.Null);
        }

        public abstract void ProgressReportTest();
        protected void ProgressReportTestImpl(string json)
        {
            string path = GetFullPahtForFile(json);

            var processor = GetProcesor();

            List<double> progressReports = new List<double>();

            using (var fs = System.IO.File.OpenRead(GetFullPahtForFile(json)))
            {
                _ = processor.Process(fs, new SynchronProgress<double>(p => progressReports.Add(p)));
            }

            TestHelper.ValidateProgressReports(progressReports);

        }

        public abstract void ProcessedCorrectlyTest(string json);
        public void ProcessedCorrectlyTestImpl(string json)
        {
            string path = GetFullPahtForFile(json);

            var processor = GetProcesor();
            var validator = GetValidator(json);
            Assert.That(validator, Is.Not.Null);
            if (validator.Skip) Assert.Warn($"Skipped {nameof(ProcessedCorrectlyTest)} for {nameof(json)} = \"{json}\"");

            LogFull result = null;

            using (var fs = System.IO.File.OpenRead(GetFullPahtForFile(json)))
            {
                result = processor.Process(fs);
                Assume.That(result, Is.Not.Null);
            }

            validator.Validate(result);
        }

        public void ArgumentNullValidationTest()
        {
            IEiProcessor eiProcessor = GetProcesor();
            Assert.That(() => eiProcessor.Process(null), Throws.ValidateArgumentNullException);
        }
    }

    internal class LegacyEiProcessorTest : AbstractEliteInsightsProcessorTest
    {
        internal static string[] JSONS
        {
            get => new string[]
            {
                "ei2_10_0_0.json.small",
                "ei2_11_0_0.json.small",
                "ei2_12_0_0.json.small",
                "ei2_13_0_0.json.small",
                "ei2_14_1_0.json.small",
                "ei2_15_0_0.json.small"
            };
        }

        [Test]
        public override void CanProcessJsonsTest([ValueSource(nameof(JSONS))] string jsons)
        {
            CanProcessJsonsTestImpl(jsons);
        }

        [Test]
        public override void ProcessedCorrectlyTest(string json)
        {
            Assert.Warn("not implemented");
        }

        [Test]
        public override void ProgressReportTest()
        {
            ProgressReportTestImpl(JSONS.Last());
        }

        protected override IEiProcessor GetProcesor()
        {
            return new EiProcessor_Legacy();
        }

        protected override IValidator GetValidator(string paht)
        {
            return new SkipValidator();
        }
    }
}
