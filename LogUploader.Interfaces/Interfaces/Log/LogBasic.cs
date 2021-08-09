using System;

namespace LogUploader.Interfaces
{
    public interface LogBasic : Log
    {
        TimeSpan Duration { get; }
        bool Uploaded { get; }
        bool Parsed { get; }
        bool Succcess { get; }
        bool IsCm { get; }
        float RemainingHealth { get; }
        bool UpgradeAvailable { get; }
    }
}

