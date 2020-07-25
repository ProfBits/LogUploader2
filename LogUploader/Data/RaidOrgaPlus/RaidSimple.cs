using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogUploader.Data.RaidOrgaPlus
{
    public class RaidSimple
    {
        public RaidSimple(long terminID, long raidID, DateTime day, TimeSpan start, TimeSpan end, string name)
        {
            TerminID = terminID;
            RaidID = raidID;
            Start = day.Add(start);
            End = day.Add(end);
            Name = name;
        }

        public long TerminID { get; set; }
        public long RaidID { get; set; }
        public DateTime Start { get; }
        public DateTime End { get; }
        public string Name { get; }
    }
}
