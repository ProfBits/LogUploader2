using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LogUploader.Data;

namespace LogUploader.Tools.Database
{
    public class DBLog : IDBLog
    {
        private static readonly DateTime TIME_Zero = new DateTime(2000, 1, 1);
        private string evtcPath;
        private string htmlPath;
        private string jsonPath;
        private string link;
        private float remainingHealth = 100;

        public DBLog()
        {
        }

        public DBLog(int bossID, string evtcPath, string jsonPath, string htmlPath, string link, int sizeKb, DateTime date)
        {
            BossID = bossID;
            EvtcPath = evtcPath;
            JsonPath = jsonPath;
            HtmlPath = htmlPath;
            Link = link;
            SizeKb = sizeKb;
            Date = date;
        }

        public DBLog(int bossID, string evtcPath, string jsonPath, string htmlPath, string link, int sizeKb, DateTime date, TimeSpan duration, bool dataCorrected, bool succsess, bool isCM, float remainingHealth)
        {
            BossID = bossID;
            EvtcPath = evtcPath;
            JsonPath = jsonPath;
            HtmlPath = htmlPath;
            Link = link;
            SizeKb = sizeKb;
            Date = date;
            Duration = duration;
            DataCorrected = dataCorrected;
            Succsess = succsess;
            IsCM = isCM;
            RemainingHealth = remainingHealth;
        }

        public DBLog(int iD, int bossID, string evtcPath, string jsonPath, string htmlPath, string link, int sizeKb, DateTime date, TimeSpan duration, bool dataCorrected, bool succsess, bool isCM, float remainingHealth)
        {
            ID = iD;
            BossID = bossID;
            EvtcPath = evtcPath;
            JsonPath = jsonPath;
            HtmlPath = htmlPath;
            Link = link;
            SizeKb = sizeKb;
            Date = date;
            Duration = duration;
            DataCorrected = dataCorrected;
            Succsess = succsess;
            IsCM = isCM;
            RemainingHealth = remainingHealth;
        }

        public int ID { get; set; } = -1;

        public int BossID { get; set; }
        public string BossName { get => StaticData.Bosses.Get(BossID).Name; }

        public string EvtcPath
        {
            get => evtcPath;
            set
            {
                evtcPath = value;
                HasEvtc = !string.IsNullOrEmpty(value);
            }
        }
        public string JsonPath
        {
            get => jsonPath;
            set
            {
                jsonPath = value;
                HasJson = !string.IsNullOrEmpty(value);
            }
        }
        public string HtmlPath
        {
            get => htmlPath;
            set
            {
                htmlPath = value;
                HasHtml = !string.IsNullOrEmpty(value);
            }
        }
        public string Link
        {
            get => link;
            set
            {
                link = value;
                HasLink = !string.IsNullOrEmpty(value);
            }
        }

        public int SizeKb { get; set; }

        public long TimeStamp
        {
            get => Date.Subtract(TIME_Zero).Ticks;
            set => Date = TIME_Zero.AddTicks(value);
        }
        public DateTime Date { get; set; }


        public int DurationMs
        {
            get => (int)Duration.TotalMilliseconds;
            set => Duration = TimeSpan.FromMilliseconds(value);
        }
        public TimeSpan Duration { get; set; } = new TimeSpan(0, 0, 0);

        public byte Flags
        {
            get
            {
                int res = Succsess ? 1 << 0 : 0;
                res |= IsCM ? 1 << 1 : 0;
                res |= DataCorrected ? 1 << 2 : 0x0;
                return (byte)res;
            }
            set
            {
                Succsess = (value & (1 << 0)) > 0;
                IsCM = (value & (1 << 1)) > 0;
                DataCorrected = (value & (1 << 2)) > 0;
            }
        }

        public bool DataCorrected { get; set; } = false;
        public bool Succsess { get; set; } = false;
        public bool IsCM { get; set; } = false;

        public float RemainingHealth { get => remainingHealth; set => remainingHealth = (float)Math.Round(value, 2); }
        public bool HasEvtc { get; private set; }
        public bool HasJson { get; private set; }
        public bool HasHtml { get; private set; }
        public bool HasLink { get; private set; }


        #region convert Helper
        public static DateTime GetDateTime(long timeStamp)
        {
            var log = new DBLog();
            log.TimeStamp = timeStamp;
            return log.Date;
        }

        public static long GetTimeStamp(DateTime dateTime)
        {
            var log = new DBLog();
            log.Date = dateTime;
            return log.TimeStamp;
        }
        public static TimeSpan GetDuration(int durationMs)
        {
            var log = new DBLog();
            log.DurationMs = durationMs;
            return log.Duration;
        }

        public static int GetDurationMs(TimeSpan duration)
        {
            var log = new DBLog();
            log.Duration = duration;
            return log.DurationMs;
        }
        #endregion
    }
}
