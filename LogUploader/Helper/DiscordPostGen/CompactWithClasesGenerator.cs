using LogUploader.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogUploader.Helper.DiscordPostGen
{
    class CompactWithClasesGenerator : CompactWithEmotesGenerator
    {
        protected override WebHookData.Field GenerateField(CachedLog log)
        {
            if (Settings.OnlyPostUploaded && string.IsNullOrWhiteSpace(log.Link))
                return null;
            string name = log.Succsess ? MiscData.EmoteRaidKill : MiscData.EmoteRaidWipe;
            name += Boss.getByID(log.BossID).DiscordEmote;
            name += " " + log.BossName;
            if (log.IsCM)
                name += " CM";

            string value;
            if (!string.IsNullOrWhiteSpace(log.Link))
                value = $"[dps.report]({log.Link})";
            else
                value = Languages.Language.Data.MiscDiscordPostGenNoLink;


            if (log.DataCorrected)
            {
                var subGroups = log.PlayersNew.GroupBy(p => p.Group).OrderBy(g => g.Key);
                foreach (var grop in subGroups)
                {
                    value += "\n";
                    foreach (var player in grop.OrderBy(p => p.Profession.Name))
                    {
                        value += player.Profession.Emote;
                    }
                }
            }

            return new WebHookData.Field(name, value, true);
        }
    }
}
