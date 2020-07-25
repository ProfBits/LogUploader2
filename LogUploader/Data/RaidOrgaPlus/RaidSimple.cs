using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogUploader.Data.RaidOrgaPlus
{
    class RaidSimple
    {
        public long TerminID { get; set; }
        public long RaidID { get; set; }
        public DateTime Start { get; }
        public DateTime End { get; }
        public string Name { get; }
    }
}
