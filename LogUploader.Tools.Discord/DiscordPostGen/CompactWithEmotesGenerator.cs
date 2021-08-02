﻿using LogUploader.Data;
using LogUploader.Helper;
using LogUploader.Localisation;
using LogUploader.Tools.Discord.Data;

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogUploader.Tools.Discord
{
    internal class CompactWithEmotesGenerator : DiscordPostGenerator
    {
        protected override Field GenerateField(ICachedLog log)
        {
            if (Settings.OnlyPostUploaded && string.IsNullOrWhiteSpace(log.Link))
                return null;
            string name = log.Succsess ? StaticData.Misc.KillEmote : StaticData.Misc.WipeEmote;
            name += StaticData.Bosses.Get(log.BossID).DiscordEmote;
            name += " " + log.BossName;
            if (log.IsCM)
                name += " CM";

            string value;
            if (!string.IsNullOrWhiteSpace(log.Link))
                value = $"[dps.report]({log.Link})";
            else
                value = Language.Data.MiscDiscordPostGenNoLink;
            
            return new Field(name, value, true);
        }

        protected override Color GetColor(IEnumerable<ICachedLog> logs)
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
                string en = $"Logs {start:HH':'mm} - {end:HH':'mm}";
                string de = $"Logs {start:HH':'mm} - {end:HH':'mm}";
                g = new Grouping(@"https://wiki.guildwars2.com/images/1/1f/Spirit_Vale_%28achievements%29.png", new NamedObject(en, de));
            }
            else
            {
                string en = $"Logs {start:yyyy'-'MM'-'dd' 'HH':'mm} - {end:yyyy'-'MM'-'dd' 'HH':'mm}";
                string de = $"Logs {start:dd'.'MM'.'yyy' 'HH':'mm} - {end:dd'.'MM'.'yyy' 'HH':'mm}";
                g = new Grouping(@"https://wiki.guildwars2.com/images/1/1f/Spirit_Vale_%28achievements%29.png", new NamedObject(en, de));
            }

            var list = new KeyValueList<Grouping, IEnumerable<ParsedData>>
            {
                { g, data }
            };
            return list;
        }
    }
}
