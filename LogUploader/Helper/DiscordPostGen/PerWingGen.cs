using LogUploader.Data;
using LogUploader.Helper;
using LogUploader.Languages;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Extensiones.Linq;
using LogUploader.Data.GameAreas;

namespace LogUploader.Helper.DiscordPostGen
{
    class PerWingGen : DiscordPostGenerator
    {
        protected override WebHookData.Field GenerateField(CachedLog log)
        {
            if (Settings.OnlyPostUploaded && string.IsNullOrWhiteSpace(log.Link))
                return null;
            string name = $"{(log.Succsess ? Language.Data.Succsess : Language.Data.Fail)} - {log.Date.ToString("HH\\:mm")}";
            string value = $"{log.BossName}";
            if (log.IsCM)
                value += $" CM";
            if (log.DataCorrected)
                value += $" - {log.Duration.ToString("mm':'ss")}";
            if (!string.IsNullOrWhiteSpace(log.Link))
                value += $"\n[dps.report]({ log.Link})";
            var field = new WebHookData.Field(name, value, true);
            return field;
        }
        protected virtual WebHookData.Field GenerateSingleBossField(CachedLog log, DPSReport remot = null)
        {
            if (Settings.OnlyPostUploaded && string.IsNullOrWhiteSpace(log.Link))
                return null;
            string name = $"{log.Date.ToString("HH\\:mm")}";
            string value = $"{(log.Succsess ? Language.Data.Succsess : Language.Data.Fail)}";
            if (log.IsCM)
                value = $"CM " + value;
            if (log.DataCorrected)
                value += $" - {log.Duration.ToString("mm':'ss")}";
            if (!string.IsNullOrWhiteSpace(log.Link))
                value += $"\n[dps.report]({ log.Link})";
            var field = new WebHookData.Field(name, value, true);
            return field;
        }

        protected override Color GetColor(IEnumerable<CachedLog> logs)
        {
            var groups = logs.GroupBy(log => Boss.getByID(log.BossID)).Select(group => group.Any(log => log.Succsess));

            var succsess = groups.Any(log => log);
            var fail = groups.Any(log => !log);

            if (succsess && !fail)
                return ColorSucc;
            else if (!succsess && fail)
                return ColorFail;
            return ColorMixed;
        }

        protected override KeyValueList<Grouping, IEnumerable<ParsedData>> GroupFields(IEnumerable<ParsedData> data)
        {
            var res = new KeyValueList<Grouping, IEnumerable<ParsedData>>();

            Grouping currentGroup = null;
            var currentLogs = new List<ParsedData>();

            foreach (var log in data.ToList().OrderBy(d => d.Log.Date))
            {
                if (currentGroup == null)
                    currentGroup = new Grouping(Boss.getByID(log.Log.BossID).Area);

                if (!currentGroup.Equals(Boss.getByID(log.Log.BossID).Area))
                {
                    if (currentLogs.Count > 0)
                    {
                        currentGroup = GetGrouping(currentLogs);
                        if (currentLogs.All(l => l.Log.IsCM))
                            currentGroup.PostFix = "CM";
                        res.Add(currentGroup, currentLogs);
                    }
                    currentGroup = new Grouping(Boss.getByID(log.Log.BossID).Area);
                    currentLogs = new List<ParsedData>();
                }

                currentLogs.Add(log);
            }

            if (currentGroup == null)
                return res;

            currentGroup = GetGrouping(currentLogs);
            if (currentLogs.All(l => l.Log.IsCM))
                currentGroup.PostFix = "CM";
            res.Add(currentGroup, currentLogs);

            return res;
        }

        protected virtual Grouping GetGrouping(IEnumerable<ParsedData> data)
        {
            if (data.Select(log => Boss.getByID(log.Log.BossID)).Distinct().Count() == 1)
                return new Grouping(Boss.getByID(data.First().Log.BossID));
            return new Grouping(Boss.getByID(data.First().Log.BossID).Area);
        }

        protected override WebHookData.Embed GetEmbedFrame(Grouping grouping, IEnumerable<ParsedData> values)
        {
            var temp = base.GetEmbedFrame(grouping, values);
            var wing = RaidWing.RaidWings.Where(w => w.Value.Name == grouping.Name).Select(g => g.Value).FirstOrDefault();
            if (wing != null)
            {
                temp.Title = wing.ShortName + " - " + temp.Title;
                return temp;
            }
            var frac = Fractal.Fractals.Where(f => f.Value.Name == grouping.Name).Select(g => g.Value).FirstOrDefault();
            if (frac != null)
            {
                temp.Title = frac.ShortName + " - " + temp.Title;
                return temp;
            }
            var strike = Strike.StrikeMissions.Where(s => s.Value.Name == grouping.Name).Select(g => g.Value).FirstOrDefault();
            if (strike != null)
            {
                temp.Title = strike.ShortName + " - " + temp.Title;
                return temp;
            }
            return temp;
        }
    }
}
