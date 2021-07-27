using LogUploader.Data;
using LogUploader.Localisation;
using LogUploader.Tools.Discord.Data;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogUploader.Tools.Discord
{
    internal class PerWingWithClassesGenerator : PerWingGen
    {
        protected override Field GenerateField(ICachedLog log)
        {
            if (Settings.OnlyPostUploaded && string.IsNullOrWhiteSpace(log.Link))
                return null;
            string name = GenerateName(log);

            string value = GenerateDateAndDuration(log);
            value += GenerateLink(log);

            if (log.DataCorrected)
            {
                value += GenerateClassIcons(log);
            }

            return new Field(name, value, true);
        }

        private static string GenerateClassIcons(ICachedLog log)
        {
            string value = "";
            var subGroups = log.PlayersNew.GroupBy(p => p.Group).OrderBy(g => g.Key);
            foreach (var grop in subGroups)
            {
                value += "\n";
                foreach (var player in grop.OrderBy(p => ((Profession)p.Profession).Name))
                {
                    value += player.Profession.Emote;
                }
            }

            return value;
        }

        private static string GenerateLink(ICachedLog log)
        {
            if (!string.IsNullOrWhiteSpace(log.Link))
                return $"[dps.report]({log.Link})";
            return Language.Data.MiscDiscordPostGenNoLink;
        }

        private static string GenerateDateAndDuration(ICachedLog log)
        {
            string value = log.Date.ToString("HH':'mm");
            if (log.DataCorrected)
                value += $" - {log.Duration.Minutes}m {log.Duration.Seconds}s";
            return value + "\n";
        }

        private static string GenerateName(ICachedLog log)
        {
            string name = log.Succsess ? MiscData.EmoteRaidKill : MiscData.EmoteRaidWipe;
            name += Boss.GetByID(log.BossID).DiscordEmote;
            name += " " + log.BossName;
            if (log.IsCM)
                name += " CM";
            return name;
        }
    }
}
