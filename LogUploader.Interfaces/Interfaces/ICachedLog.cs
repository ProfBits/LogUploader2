using Newtonsoft.Json.Linq;

using System;
using System.Collections.Generic;

namespace LogUploader.Data
{
    public interface ICachedLog
    {
        int BossID { get; set; }
        string BossName { get; }
        bool DataCorrected { get; set; }
        int DataVersion { get; }
        DateTime Date { get; set; }
        TimeSpan Duration { get; set; }
        string EvtcPath { get; set; }
        string HtmlPath { get; set; }
        int ID { get; set; }
        bool IsCM { get; set; }
        string JsonPath { get; set; }
        string Link { get; set; }
        string LinkAvailable { get; }
        string LocalAvailable { get; }
        IReadOnlyList<ISimplePlayer> PlayersNew { get; }
        string RecordedBy { get; }
        float RemainingHealth { get; set; }
        int SizeKb { get; set; }
        bool Succsess { get; set; }
        List<ISimmpleTarget> Targets { get; }

        void ApplySimpleLog(ISimpleLogJson data);
        bool Equals(object obj);
        IDBLog GetDBLog();
        int GetHashCode();
        void UpdateDpsReport(JObject data);
        void UpdateEi(JObject data);
        void UpdateEi(string json);
    }
}