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
    internal class PerWingWithEmotes : PerWingGen
    {
        protected override Field GenerateField(ICachedLog log)
        {
            if (Settings.OnlyPostUploaded && string.IsNullOrWhiteSpace(log.Link))
                return null;
            string name = $"{log.Date:HH\\:mm}";
            string value = $"{(log.Succsess ? StaticData.Misc.KillEmote : StaticData.Misc.WipeEmote)} {StaticData.Bosses.Get(log.BossID).DiscordEmote}";
            if (log.IsCM)
                value += $" CM";
            if (log.DataCorrected)
                value += $" - {log.Duration.ToString(Language.Current == eLanguage.DE ? "mm':'ss','fff" : "mm':'ss'.'fff")}";
            if (!string.IsNullOrWhiteSpace(log.Link))
                value += $"\n[dps.report]({ log.Link})";
            var field = new Field(name, value, true);
            return field;
        }

        protected override Grouping GetGrouping(IEnumerable<ParsedData> data)
        {
            if (data.Select(log => StaticData.Bosses.Get(log.Log.BossID)).Distinct().Count() == 1)
                return new Grouping(StaticData.Bosses.Get(data.First().Log.BossID), StaticData.Bosses.Get(data.First().Log.BossID).Prefix(StaticData.Bosses.Get(data.First().Log.BossID).DiscordEmote + " "));
            return new Grouping((GameArea)StaticData.Bosses.Get(data.First().Log.BossID).Area);
        }
    }
}
