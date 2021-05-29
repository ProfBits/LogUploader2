using Extensiones;
using LogUploader.Data;
using LogUploader.Helper;
using LogUploader.Localisation;

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogUploader.Tools.Discord
{
    internal class DetaildGenerator : DiscordPostGenerator
    {
        protected override Field GenerateField(ICachedLog log)
        {
            if (Settings.OnlyPostUploaded && string.IsNullOrWhiteSpace(log.Link))
                return null;
            var name = $"{log.Date:HH\\:mm}";
            var value = $"{(log.Succsess ? Language.Data.Succsess : Language.Data.Fail)}";
            if (log.DataCorrected)
            {
                value += $"\n";
                if (log.IsCM)
                    value += $"\nCM";
                value += $"\n{Language.Data.MiscDiscordPostGenDuration}: {log.Duration.ToString(Language.Current == eLanguage.DE ? "mm':'ss','fff" : "mm':'ss'.'fff")}";
                value += $"\n{Language.Data.MiscDiscordPostGenHpLeft}: {log.RemainingHealth:0.00'%'}";
                value += $"\n{Language.Data.MiscDiscordPostGenGroupDPS}: {log.PlayersNew.Select(p => p.DpsAll).Sum()}";
                value += $"\n{Language.Data.MiscDiscordPostGenTopDPS}: {log.PlayersNew.Max(p2 => p2.DpsAll)} ({log.PlayersNew.Where(p => p.DpsAll == log.PlayersNew.Max(p2 => p2.DpsAll)).First().Account})";
            }
            if (!string.IsNullOrWhiteSpace(log.Link))
                value += $"\n[dps.report]({ log.Link})";
            return new Field(name, value, true);
        }

        protected override Color GetColor(IEnumerable<ICachedLog> logs)
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
                    currentGroup = new Grouping(Boss.GetByID(log.Log.BossID));

                if (!currentGroup.Equals(Boss.GetByID(log.Log.BossID)))
                {
                    if (currentLogs.Count > 0)
                    {
                        if (currentLogs.All(l => l.Log.IsCM))
                            currentGroup.PostFix = "CM";
                        res.Add(currentGroup, currentLogs);
                    }
                    currentGroup = new Grouping(Boss.GetByID(log.Log.BossID));
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
