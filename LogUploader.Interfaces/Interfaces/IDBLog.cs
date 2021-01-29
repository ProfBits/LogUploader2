using System;

namespace LogUploader.Data
{
    public interface IDBLog
    {
        int BossID { get; set; }
        string BossName { get; }
        bool DataCorrected { get; set; }
        DateTime Date { get; set; }
        TimeSpan Duration { get; set; }
        int DurationMs { get; set; }
        string EvtcPath { get; set; }
        byte Flags { get; set; }
        bool HasEvtc { get; }
        bool HasHtml { get; }
        bool HasJson { get; }
        bool HasLink { get; }
        string HtmlPath { get; set; }
        int ID { get; set; }
        bool IsCM { get; set; }
        string JsonPath { get; set; }
        string Link { get; set; }
        float RemainingHealth { get; set; }
        int SizeKb { get; set; }
        bool Succsess { get; set; }
        long TimeStamp { get; set; }
    }
}