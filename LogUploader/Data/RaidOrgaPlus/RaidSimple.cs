﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogUploader.Data.RaidOrgaPlus
{
    public class RaidSimple
    {
        public long TerminID { get; set; }
        public long RaidID { get; set; }
        public DateTime Start { get; }
        public DateTime End { get; }
        public string Name { get; }
        public virtual string DisplayName { get => $"{Name} {Start.ToString("dd'.'MM' 'HH':'mm")}"; }
        
        //TODO is there a better way to bind
        //public RaidSimple Self { get => this; }

        protected RaidSimple() { }

        public RaidSimple(long terminID, long raidID, DateTime day, TimeSpan start, TimeSpan end, string name)
        {
            TerminID = terminID;
            RaidID = raidID;
            Start = day.Add(start);
            End = day.Add(end);
            Name = name;
        }

        internal static RaidSimple GetNoAccount()
        {
            return new RaidSimpleTemplate(Languages.Language.Data.MiscRaidOrgaPlusNoAccount);
        }

        internal static RaidSimple LogInFaild()
        {
            throw new NotImplementedException(Languages.Language.Data.MiscRaidOrgaPlusLoginErr);
        }

        internal static RaidSimple NoTermine()
        {
            throw new NotImplementedException(Languages.Language.Data.MiscRaidOrgaPlusNoRaid);
        }
    }

    public class RaidSimpleTemplate : RaidSimple
    {
        public override string DisplayName { get; }

        public RaidSimpleTemplate(string message)
        {
            DisplayName = message;
        }
    }
}
