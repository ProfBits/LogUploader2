using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogUploader.Data.RaidOrgaPlus
{
    public enum Role : byte
    {
        Empty = 0,
        Power = 1,
        Condi = 2,
        Tank = 3,
        Heal = 4,
        Utility = 5,
        Banner = 6,
        Special = 7,
        Kiter = 8,
    }
}
