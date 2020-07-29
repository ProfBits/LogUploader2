using Extensiones;
using LogUploader.Data;
using LogUploader.Helper;
using LogUploader.Languages;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogUploader.Helper.DiscordPostGen
{
    class DetaildGenerator : DiscordPostGenerator
    {
        protected override WebHookData.Field GenerateField(CachedLog log)
        {
            if (Settings.OnlyPostUploaded && string.IsNullOrWhiteSpace(log.Link))
                return null;
            var name = $"{log.Date.ToString("HH\\:mm")}";
            var value = $"{Language.Data.SuccsessFail(log.Succsess)}";
            if (log.DataCorrected)
            {
                value += $"\n";
                if (log.IsCM)
                    value += $"\nCM";
                value += $"\n{Language.Data.MiscDiscordPostGenDuration}: {log.Duration.ToString(Language.Current == eLanguage.DE ? "mm':'ss','fff" : "mm':'ss'.'fff")}";
                value += $"\n{Language.Data.MiscDiscordPostGenHpLeft}: {log.RemainingHealth.ToString("0.00'%'")}";
                value += $"\n{Language.Data.MiscDiscordPostGenGroupDPS}: {log.Players.Select(p => p.DPS).Sum()}";
                value += $"\n{Language.Data.MiscDiscordPostGenTopDPS}: {log.Players.Max(p2 => p2.DPS)} ({log.Players.Where(p => p.DPS == log.Players.Max(p2 => p2.DPS)).First().AccountName})";
            }
            if (!string.IsNullOrWhiteSpace(log.Link))
                value += $"\n[dps.report]({ log.Link})";
            return new WebHookData.Field(name, value, true);
        }

        protected override Color GetColor(IEnumerable<CachedLog> logs)
        {
            return logs.Any(log => log.Succsess) ? ColorSucc : ColorFail;
        }

        protected override KeyValueList<Grouping, IEnumerable<ParsedData>> GroupFields(IEnumerable<ParsedData> data)
        {
            var res = new KeyValueList<Grouping, IEnumerable<ParsedData>>();

            Grouping currentGroup = null;
            var currentLogs = new List<ParsedData>();

            foreach (var log in data.ToList().OrderBy(d => d.Log.Date))
            {
                if (currentGroup == null)
                    currentGroup = new Grouping(Boss.getByID(log.Log.BossID));

                if (!currentGroup.Equals(Boss.getByID(log.Log.BossID)))
                {
                    if (currentLogs.Count > 0)
                    {
                        if (currentLogs.All(l => l.Log.IsCM))
                            currentGroup.PostFix = "CM";
                        res.Add(currentGroup, currentLogs);
                    }
                    currentGroup = new Grouping(Boss.getByID(log.Log.BossID));
                    currentLogs = new List<ParsedData>();
                }

                currentLogs.Add(log);
            }

            if (currentGroup == null)
                return res;

            if (currentLogs.All(l => l.Log.IsCM))
                currentGroup.PostFix = "CM";
            res.Add(currentGroup, currentLogs);
            return res;
        }
    }
}
