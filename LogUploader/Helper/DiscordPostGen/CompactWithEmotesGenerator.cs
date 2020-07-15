using LogUploader.Data;
using LogUploader.Helpers;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogUploader.Helper.DiscordPostGen
{
    public class CompactWithEmotesGenerator : DiscordPostGenerator
    {
        protected override WebHookData.Field GenerateField(CachedLog log)
        {
            string name = log.Succsess ? MiscData.EmoteRaidKill : MiscData.EmoteRaidWipe;
            name += Boss.getByID(log.BossID).DiscordEmote;
            name += " " + log.BossName;
            if (log.IsCM)
            {
                name += " CM";
            }
            string value = "";
            if (!string.IsNullOrWhiteSpace(log.Link))
            {
                value = $"[dps.report]({log.Link})";
            }
            else
            {
                //TODO localize
                value = Languages.Language.Current == eLanguage.EN ? "no Link" : "kein Link";
            }
            return new WebHookData.Field(name, value, true);
        }

        protected override Color GetColor(IEnumerable<CachedLog> logs)
        {
            var data = logs.GroupBy(log => log.BossName).Select(grp => grp.Any(log => log.Succsess));
            if (data.All(b => b))
                return ColorSucc;
            if (data.All(b => !b))
                return ColorFail;
            return ColorMixed;
        }

        protected override KeyValueList<Grouping, IEnumerable<ParsedData>> GroupFields(IEnumerable<ParsedData> data)
        {
            Grouping g;
            var dates = data.Select(log => log.Log.Date);
            var start = dates.Min();
            var end = dates.Max();
            end = end.Add(data.Where(log => log.Log.Date.Equals(end)).First().Log.Duration);

            if (start.Date.Equals(end.Date))
            {
                string en = $"Logs {start.ToString("HH':'mm")} - {end.ToString("HH':'mm")}";
                string de = $"Logs {start.ToString("HH':'mm")} - {end.ToString("HH':'mm")}";
                g = new Grouping(@"https://wiki.guildwars2.com/images/1/1f/Spirit_Vale_%28achievements%29.png", new NamedObject(en, de));
            }
            else
            {
                string en = $"Logs {start.ToString("yyyy'-'MM'-'dd' 'HH':'mm")} - {end.ToString("yyyy'-'MM'-'dd' 'HH':'mm")}";
                string de = $"Logs {start.ToString("dd'.'MM'.'yyy' 'HH':'mm")} - {end.ToString("dd'.'MM'.'yyy' 'HH':'mm")}";
                g = new Grouping(@"https://wiki.guildwars2.com/images/1/1f/Spirit_Vale_%28achievements%29.png", new NamedObject(en, de));
            }

            var list = new KeyValueList<Grouping, IEnumerable<ParsedData>>();
            list.Add(g, data);
            return list;
        }
    }
}
