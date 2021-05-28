using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogUploader.RaidOrgaPlus.Data
{
    internal class RaidOrgaPlusCacheItem
    {
        public string AccountName { get; set; }
        public long AccountID { get; set; }

        public RaidOrgaPlusCacheItem(string accountName, long accountID)
        {
            AccountName = accountName;
            AccountID = accountID;
        }
    }
}
