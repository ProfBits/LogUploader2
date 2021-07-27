using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            if (ex.InnerException is ArgumentException inner) ValidateArugumentException(inner);
        }

        internal static string[] InvalidDiscordEmotes { get => new string[] {
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
    }
}
