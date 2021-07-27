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
    internal class CompactWithClasesGenerator : CompactWithEmotesGenerator
    {
        protected override Field GenerateField(ICachedLog log)
        {
            if (Settings.OnlyPostUploaded && string.IsNullOrWhiteSpace(log.Link))
                return null;
            string name = log.Succsess ? MiscData.EmoteRaidKill : MiscData.EmoteRaidWipe;
            name += Boss.GetByID(log.BossID).DiscordEmote;
            name += " " + log.BossName;
            if (log.IsCM)
                name += " CM";

            string value;
            if (!string.IsNullOrWhiteSpace(log.Link))
                value = $"[dps.report]({log.Link})";
            else
                value = Language.Data.MiscDiscordPostGenNoLink;


            if (log.DataCorrected)
            {
                var subGroups = log.PlayersNew.GroupBy(p => p.Group).OrderBy(g => g.Key);
                foreach (var grop in subGroups)
                {
                    value += "\n";
                    foreach (var player in grop.OrderBy(p => ((Profession)p.Profession).Name))
                    {
                        value += player.Profession.Emote;
                    }
                }
            }

            return new Field(name, value, true);
        }
    }
}
