using LogUploader.Localisation;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogUploader.Tools.RaidOrgaPlus.Data
{
    public class RaidSimple
    {
        public long TerminID { get; }
        public long RaidID { get; }
        public DateTime Start { get; }
        public DateTime End { get; }
        public string Name { get; }
        public virtual string DisplayName { get => $"{Name} {Start:dd'.'MM' 'HH':'mm}"; }

        //TODO is there a better way to bind
        /// <summary>
        /// Required for data binding of comboboxes
        /// </summary>
        public RaidSimple Self { get => this; }

        protected RaidSimple() { }

        internal RaidSimple(long terminID, long raidID, DateTime day, TimeSpan start, TimeSpan end, string name)
        {
            TerminID = terminID;
            RaidID = raidID;
            Start = day.Add(start);
            End = day.Add(end);
            Name = name;
        }

        public static RaidSimple GetNoAccount()
        {
            return new RaidSimpleTemplate(Language.Data.MiscRaidOrgaPlusNoAccount);
        }

        public static RaidSimple GetLogInFaild()
        {
            return new RaidSimpleTemplate(Language.Data.MiscRaidOrgaPlusLoginErr);
        }

        public static RaidSimple GetNoTermine()
        {
            return new RaidSimpleTemplate(Language.Data.MiscRaidOrgaPlusNoRaid);
        }

        [Obsolete]
        public static RaidSimple GetRaidOrgaDisabled()
        {
            return new RaidSimpleTemplate("Feature Disabled");
        }
    }

    public class RaidSimpleTemplate : RaidSimple
    {
        public override string DisplayName { get; }

        internal RaidSimpleTemplate(string message) : base(-1, -1, DateTime.Today, TimeSpan.Zero, TimeSpan.Zero, "")
        {
            DisplayName = message;
        }
    }
}
