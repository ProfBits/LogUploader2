using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using LogUploader.Data;

using NUnit.Framework;
using NUnit.Framework.Internal;

namespace LogUploader.Test
{
    internal class TestHelper
    {
        internal static void ValidateArugumentException(ArgumentException ex)
        {
            Assert.That(ex.ParamName, Is.Not.Null, "The ArugmentException.ParamName should be set");
            Assert.That(string.IsNullOrWhiteSpace(ex.ParamName), Is.False, "The ArugmentException.ParamName should be set to a none empty string");
            Assert.That(ex.ParamName.Split(null).Length, Is.EqualTo(1), "The ArugmentException.ParamName should only contain a single identifier");
            Assert.That(ex.Message, Is.Not.Null, "The ArugmentException.Message should be set");
            Assert.That(string.IsNullOrWhiteSpace(ex.ParamName), Is.False, "The ArugmentException.Message should be set to a none empty string");
            if (ex is ArgumentOutOfRangeException rangeEx)
            {
                Assert.That(rangeEx.ActualValue, Is.Not.Null, "The actual value of an ArgumentOutOfRangeException should be set and not null.");
            }
            if (ex.InnerException is ArgumentException inner) ValidateArugumentException(inner);
        }

        internal static IEnumerable<Type> GetAllSubClassesOfType(Type baseType)
        {
            foreach (Assembly asm in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (Type type in GetValidSubTypes(asm, baseType))
                {
                    yield return type;
                }
            }
        }

        private static IEnumerable<Type> GetValidSubTypes(Assembly asm, Type baseType)
        {
            foreach (Type type in asm.GetTypes())
            {
                if (type.IsSubclassOf(baseType) && !type.IsAbstract)
                    yield return type;
            }
        }

        public static void ValidateProgressReports(List<double> progressReports)
        {
            Assert.That(progressReports, Is.Not.Null);
            Assert.That(progressReports, Has.Count.GreaterThan(2));
            Assert.That(progressReports.Min(), Is.EqualTo(0d));
            Assert.That(progressReports.Max(), Is.EqualTo(1d));
            if (progressReports.Zip(progressReports.Skip(1), (a, b) => a >= b).Any(e => e))
            {
                var firstError = progressReports.Zip(progressReports.Skip(1), (a, b) => (a, b)).First(e => e.a >= e.b);
                Assert.Fail($"Progress sequence is not strctly ascending. Fist error is {firstError.a} before {firstError.b}");
            }
        }

        internal static string[] InvalidDiscordEmotes
        {
            get => new string[] {
            null,
            "",
            "  ",
            "\n\t\0\"",
            " :emote:",
            ":emote: ",
            " :emote: ",
            " :abc12: ",
            "<::999999999999999999>",
            "<:xxx:9999999999999999990>",
            "<:xxx:99999999999999999>",
            "<:xxx:9999999a9999999999>",
            "< :xxx:999999999999999999>",
            "<: xxx:999999999999999999>",
            "<:xxx :999999999999999999>",
            "<:xxx: 999999999999999999>",
            "<:xxx:999999999999999999 >"};
        }

        internal static string[] ValidDiscordEmotes
        {
            get => new string[] { ":emote:", "<:dgh:999999999999999999>" };
        }

        internal static string[] InvalidOneWordStrings
        {
            get => new string[] {
                null,
                "",
                " ",
                "  \t\n\r",
                "two words",
                " spaceAtFront",
                "spceAtBack ",
                " spaceAround ",
                " space AtFront",
                "spce AtBack ",
                " space Around "
            };
        }

        internal static string[] ValidOneWordStrings
        {
            get => new string[] { "one", "WordWithNumbers12345" };
        }

        internal static string[] InvalidMulitWordStrings
        {
            get => new string[] {
                null,
                "",
                " ",
                "  \t\n\r",
                " spaceAtFront",
                " space At Front",
                " spaceAround ",
                " space Around ",
                "spceAtBack ",
                "spce At Back "
            };
        }

        internal static string[] ValidMulitWordStrings
        {
            get => new string[] {
                "one",
                "WordWithNumbers12345",
                "two words",
                "three words here"
            };
        }

        internal static Profession CreateProfession(eProfession profession)
        {
            string DefaultIconPath = TestSetup.GetPathToTestFiles("static", "profIcon.png");

            return new Profession(profession, $"{profession}EN", $"{profession}DE", DefaultIconPath, $":{profession}:", (int)profession, $"{profession}".Substring(0, 3));
        }



        [Test]
        public void TestSynchronProgressCreateTest()
        {
            Assert.That(() => new SynchronProgress<double>(p => { }), Throws.Nothing);

            TestHelper.ValidateArugumentException(Assert.Catch<ArgumentException>(() => new SynchronProgress<string>(null)));
        }

        [Test]
        public void TestSynchronProgressReportTest()
        {
            double reported = 0;
            IProgress<double> progress = new SynchronProgress<double>(p => reported = p);

            progress.Report(1);
            Assert.That(reported, Is.EqualTo(1));

            reported = 0;
            IProgress<double> progressInterseption = new SynchronProgress<double>(p => progress.Report(p));
            progress.Report(2);
            Assert.That(reported, Is.EqualTo(2));
        }

        [Test]
        public void TestValidateProgressReportsNullParamTest()
        {
            AssertAssertFail(() => ValidateProgressReports(null));
        }

        [Test]
        public void TestValidateProgressReportsCountTest()
        {
            AssertAssertFail(() => ValidateProgressReports(null));

            List<double> emptyList = new List<double>();
            AssertAssertFail(() => ValidateProgressReports(emptyList));

            List<double> only2Values = new List<double>() { 0, 1 };
            AssertAssertFail(() => ValidateProgressReports(only2Values));
        }

        [Test]
        public void TestValidateProgressReportsRequireZeroTest()
        {
            List<double> req0 = new List<double>() { 0.5, 0.75, 1 };
            AssertAssertFail(() => ValidateProgressReports(req0));
        }

        [Test]
        public void TestValidateProgressReportsRequireOneTest()
        {
            List<double> req1 = new List<double>() { 0, 0.25, 0.5 };
            AssertAssertFail(() => ValidateProgressReports(req1));
        }

        [Test]
        public void TestValidateProgressReportsZeroToOneRangeTest()
        {

            List<double> tooSmall = new List<double>() { -1, 0, 0.5, 1 };
            AssertAssertFail(() => ValidateProgressReports(tooSmall));
            List<double> tooSmall2 = new List<double>() { double.MinValue, 0, 0.5, 1 };
            AssertAssertFail(() => ValidateProgressReports(tooSmall2));

            List<double> tooBig = new List<double>() { 0, 0.5, 1, 2 };
            AssertAssertFail(() => ValidateProgressReports(tooBig));
            List<double> tooBig2 = new List<double>() { 0, 0.5, 1, double.MaxValue };
            AssertAssertFail(() => ValidateProgressReports(tooBig2));
        }

        [Test]
        public void TestValidateProgressReportsOrderTest()
        {
            List<double> tooWrongOrder = new List<double>() { 1, 0.5, 0 };
            AssertAssertFail(() => ValidateProgressReports(tooWrongOrder));
            List<double> tooWrongOrder2 = new List<double>() { 0, 1, 0.5 };
            AssertAssertFail(() => ValidateProgressReports(tooWrongOrder2));
            List<double> tooWrongOrder3 = new List<double>() { 0, 0.5, 0.5, 1 };
            AssertAssertFail(() => ValidateProgressReports(tooWrongOrder3));
        }
        
        [Test]
        public void TestValidateProgressReportsOnValidDataTest()
        {
            List<double> correct = new List<double>() { 0, 0.5, 1 };
            Assert.That(SafelyCatchAnNUnitException(() => ValidateProgressReports(correct)), Is.Null);
        }

        private void AssertAssertFail(Action action, NUnit.Framework.Interfaces.TestStatus expectedTestStatus = NUnit.Framework.Interfaces.TestStatus.Failed)
        {
            Exception ex = SafelyCatchAnNUnitException(action);
            if (ex is AssertionException assertEx)
            {
                Assert.That(assertEx.ResultState.Status, Is.EqualTo(expectedTestStatus));
            }
            else if (ex is null)
            {
                Assert.Fail($"Expected to catch an {nameof(AssertionException)}");
            }
            else
            {
                throw ex;
            }
        }

        //Taken from https://github.com/nunit/nunit/issues/2758#issuecomment-385498560 - Thanks
        public static Exception SafelyCatchAnNUnitException(Action code)
        {
            Exception caughtException = null;

            using (new TestExecutionContext.IsolatedContext())
            {
                try
                {
                    code();
                }
                catch (Exception ex)
                {
                    caughtException = ex;
                }

                return caughtException;
            }
        }
    }
}
