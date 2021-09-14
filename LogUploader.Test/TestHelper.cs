using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using LogUploader.Data;

using NUnit.Framework;

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
    }
}
