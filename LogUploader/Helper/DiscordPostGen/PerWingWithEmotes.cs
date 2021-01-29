using LogUploader.Data;
using LogUploader.Localisation;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogUploader.Helper.DiscordPostGen
{
    class PerWingWithEmotes : PerWingGen
    {
        protected override WebHookData.Field GenerateField(CachedLog log)
        {
            if (Settings.OnlyPostUploaded && string.IsNullOrWhiteSpace(log.Link))
                return null;
            string name = $"{log.Date:HH\\:mm}";
            string value = $"{(log.Succsess ? MiscData.EmoteRaidKill : MiscData.EmoteRaidWipe)} {Boss.GetByID(log.BossID).DiscordEmote}";
            if (log.IsCM)
                value += $" CM";
            if (log.DataCorrected)
                value += $" - {log.Duration.ToString(Language.Current == eLanguage.DE ? "mm':'ss','fff" : "mm':'ss'.'fff")}";
            if (!string.IsNullOrWhiteSpace(log.Link))
                value += $"\n[dps.report]({ log.Link})";
            var field = new WebHookData.Field(name, value, true);
            return field;
        }

        protected override Grouping GetGrouping(IEnumerable<ParsedData> data)
        {
            if (data.Select(log => Boss.GetByID(log.Log.BossID)).Distinct().Count() == 1)
                return new Grouping(Boss.GetByID(data.First().Log.BossID), Boss.GetByID(data.First().Log.BossID).Prefix(Boss.GetByID(data.First().Log.BossID).DiscordEmote + " "));
            return new Grouping(Boss.GetByID(data.First().Log.BossID).Area);
        }
    }
}
