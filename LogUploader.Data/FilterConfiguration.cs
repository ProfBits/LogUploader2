using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogUploader.Data
{
    public class FilterConfiguration
    {
        public bool BossEnabled { get; }
        public string BossName { get; }
        public bool HPEnabled { get; }
        public string HpRelation { get; }
        public double HpValue { get; }
        public bool DurationEnabled { get; }
        public string DurationRelation { get; }
        public TimeSpan DurationValue { get; }
        public bool SuccsessEnabled { get; }
        public bool Succsess { get; }
        public bool DateFromEnabled { get; }
        public DateTime DateFrom { get; }
        public bool DateToEnabled { get; }
        public DateTime DateTo { get; }

        public FilterConfiguration(bool bossEnabled, string bossName, bool hPEnabled, string hpRelation, double hpValue, bool durationEnabled, string durationRelation, TimeSpan durationValue, bool succsessEnabled, bool succsess, bool dateFromEnabled, DateTime dateFrom, bool dateToEnabled, DateTime dateTo)
        {
            BossEnabled = bossEnabled;
            BossName = bossName;
            HPEnabled = hPEnabled;
            HpRelation = hpRelation;
            HpValue = hpValue;
            DurationEnabled = durationEnabled;
            DurationRelation = durationRelation;
            DurationValue = durationValue;
            SuccsessEnabled = succsessEnabled;
            Succsess = succsess;
            DateFromEnabled = dateFromEnabled;
            DateFrom = dateFrom;
            DateToEnabled = dateToEnabled;
            DateTo = dateTo;
        }
    }
}
