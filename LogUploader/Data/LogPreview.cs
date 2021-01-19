using LogUploader.GUIs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LogUploader.Data
{
    class LogPreview
    {
        public LogPreview(CachedLog log, IEnumerable<PlayerData> players) : this(
            log.BossName,
            log.SizeKb,
            log.RemainingHealth,
            log.Date,
            false,
            log.Duration,
            log.DataCorrected ? CheckState.Checked : CheckState.Unchecked,
            log.IsCM ? CheckState.Checked : CheckState.Unchecked,
            log.Succsess ? CheckState.Checked : CheckState.Unchecked,
            !string.IsNullOrWhiteSpace(log.HtmlPath) ? CheckState.Checked : CheckState.Unchecked,
            false,
            log.HtmlPath,
            !string.IsNullOrWhiteSpace(log.Link) ? CheckState.Checked : CheckState.Unchecked,
            log.Link,
            players
            )
        { }

        public LogPreview(string name, int sizeKb, float hPLeft, DateTime date, bool isMaxDuration, TimeSpan maxDuratin, CheckState corrected, CheckState isCM, CheckState success, CheckState hasHtmlCb, bool multiSelect, string hTML, CheckState hasLinkCb, string link, IEnumerable<PlayerData> players)
        {
            Name = name;
            SizeKb = sizeKb;
            HPLeft = hPLeft;
            Date = date;
            this.IsMaxDuration = isMaxDuration;
            MaxDuratin = maxDuratin;
            Corrected = corrected;
            IsCM = isCM;
            Success = success;
            HasHtmlCb = hasHtmlCb;
            MultiSelect = multiSelect;
            HTML = hTML;
            HasLinkCb = hasLinkCb;
            Link = link;
            Players = players;
        }

        public string Name { get; } = "";
        public int SizeKb { get; }
        public float HPLeft { get; }
        public DateTime Date { get; }
        public bool IsMaxDuration { get; }
        public TimeSpan MaxDuratin { get; }
        public CheckState Corrected { get; } = CheckState.Indeterminate;
        public CheckState IsCM { get; } = CheckState.Indeterminate;
        public CheckState Success { get; } = CheckState.Indeterminate;
        public CheckState HasHtmlCb { get; } = CheckState.Indeterminate;
        public bool HasHtml { get => HasHtmlCb != CheckState.Unchecked; }
        public bool MultiSelect { get; }
        public string HTML { get; } = "";
        public CheckState HasLinkCb { get; } = CheckState.Indeterminate;
        public bool HasLink { get => HasLinkCb != CheckState.Unchecked; }
        public string Link { get; } = "";
        public IEnumerable<PlayerData> Players { get; }
    }
}
