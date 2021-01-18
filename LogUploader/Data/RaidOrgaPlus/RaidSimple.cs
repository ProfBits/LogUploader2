using System;
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
        public virtual string DisplayName { get => $"{Name} {Start:dd'.'MM' 'HH':'mm}"; }
        
        //TODO is there a better way to bind
        /// <summary>
        /// Required for data binding of comboboxes
        /// </summary>
        public RaidSimple Self { get => this; }

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

        internal static RaidSimple GetLogInFaild()
        {
            return new RaidSimpleTemplate(Languages.Language.Data.MiscRaidOrgaPlusLoginErr);
        }

        internal static RaidSimple GetNoTermine()
        {
            return new RaidSimpleTemplate(Languages.Language.Data.MiscRaidOrgaPlusNoRaid);
        }

        internal static RaidSimple GetRaidOrgaDisabled()
        {
            return new RaidSimpleTemplate("Feature Disabled");
        }
    }

    public class RaidSimpleTemplate : RaidSimple
    {
        public override string DisplayName { get; }

        public RaidSimpleTemplate(string message) : base(-1, -1, DateTime.Today, TimeSpan.Zero, TimeSpan.Zero, "")
        {
            DisplayName = message;
        }
    }
}
