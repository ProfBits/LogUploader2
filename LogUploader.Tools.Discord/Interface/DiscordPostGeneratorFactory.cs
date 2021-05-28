using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LogUploader.Tools.Settings;

namespace LogUploader.Tools.Discord
{
    public static class DiscordPostGeneratorFactory
    {
        public static void SetSettings(IWebHookSettings Settings)
        {
            DiscordPostGenerator.Settings = Settings;
        }

        public static IDiscordPostGen Get(eDiscordPostFormat format)
        {
            switch (format)
            {
                case eDiscordPostFormat.PerBoss:
                    return new PerBossGenerator();
                case eDiscordPostFormat.PerTryDetaild:
                    return new DetaildGenerator();
                case eDiscordPostFormat.PerAreaEmotes:
                    return new PerWingWithEmotes();
                case eDiscordPostFormat.CompactWithEmotes:
                    return new CompactWithEmotesGenerator();
                case eDiscordPostFormat.CompactWithClasses:
                    return new CompactWithClasesGenerator();
                case eDiscordPostFormat.PerAreaClasses:
                    return new PerWingWithClassesGenerator();
                case eDiscordPostFormat.PerArea:
                default:
                    return new PerWingGen();
            }
        }
    }
}
