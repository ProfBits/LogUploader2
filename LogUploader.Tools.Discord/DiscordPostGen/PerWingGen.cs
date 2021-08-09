using LogUploader.Data;
using LogUploader.Helper;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Extensiones.Linq;
using LogUploader.Data;
using LogUploader.Localisation;
using LogUploader.Tools.Discord.Data;

namespace LogUploader.Tools.Discord
{
    internal class PerWingGen : DiscordPostGenerator
    {
        protected override Field GenerateField(ICachedLog log)
        {
            if (Settings.OnlyPostUploaded && string.IsNullOrWhiteSpace(log.Link))
                return null;
            string name = $"{(log.Succsess ? Language.Data.Succsess : Language.Data.Fail)} - {log.Date:HH\\:mm}";
            string value = $"{log.BossName}";
            if (log.IsCM)
                value += $" CM";
            if (log.DataCorrected)
                value += $" - {log.Duration:mm':'ss}";
            if (!string.IsNullOrWhiteSpace(log.Link))
                value += $"\n[dps.report]({ log.Link})";
            var field = new Field(name, value, true);
            return field;
        }
        protected virtual Field GenerateSingleBossField(ICachedLog log)
        {
            if (Settings.OnlyPostUploaded && string.IsNullOrWhiteSpace(log.Link))
                return null;
            string name = $"{log.Date:HH\\:mm}";
            string value = $"{(log.Succsess ? Language.Data.Succsess : Language.Data.Fail)}";
            if (log.IsCM)
                value = $"CM " + value;
            if (log.DataCorrected)
                value += $" - {log.Duration:mm':'ss}";
            if (!string.IsNullOrWhiteSpace(log.Link))
                value += $"\n[dps.report]({ log.Link})";
            var field = new Field(name, value, true);
            return field;
        }

        protected override Color GetColor(IEnumerable<ICachedLog> logs)
        {
            var groups = logs.GroupBy(log => StaticData.Bosses.Get(log.BossID)).Select(group => group.Any(log => log.Succsess));

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
                    currentGroup = new Grouping((GameArea)StaticData.Bosses.Get(log.Log.BossID).Area);

                if (!currentGroup.Equals(StaticData.Bosses.Get(log.Log.BossID).Area))
                {
                    if (currentLogs.Count > 0)
                    {
                        currentGroup = GetGrouping(currentLogs);
                        if (currentLogs.All(l => l.Log.IsCM))
                            currentGroup.PostFix = "CM";
                        res.Add(currentGroup, currentLogs);
                    }
                    currentGroup = new Grouping((GameArea)StaticData.Bosses.Get(log.Log.BossID).Area);
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
            if (data.Select(log => StaticData.Bosses.Get(log.Log.BossID)).Distinct().Count() == 1)
                return new Grouping(StaticData.Bosses.Get(data.First().Log.BossID));
            return new Grouping((GameArea)StaticData.Bosses.Get(data.First().Log.BossID).Area);
        }

        protected override Embed GetEmbedFrame(Grouping grouping, IEnumerable<ParsedData> values)
        {
            //TODO dragon response Missions
            var temp = base.GetEmbedFrame(grouping, values);
            var wing = StaticData.Areas.RaidWings.Where(w => w.Name == grouping.Name).FirstOrDefault();
            if (wing != null)
            {
                temp.Title = wing.ShortName + " - " + temp.Title;
                return temp;
            }
            var frac = StaticData.Areas.Fractals.Where(f => f.Name == grouping.Name).FirstOrDefault();
            if (frac != null)
            {
                temp.Title = frac.ShortName + " - " + temp.Title;
                return temp;
            }
            var strike = StaticData.Areas.Strikes.Where(s => s.Name == grouping.Name).FirstOrDefault();
            if (strike != null)
            {
                temp.Title = strike.ShortName + " - " + temp.Title;
                return temp;
            }
            return temp;
        }
    }
}
