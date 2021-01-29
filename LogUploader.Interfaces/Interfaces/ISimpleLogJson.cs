using System.Collections.Generic;

namespace LogUploader.Data
{
    public interface ISimpleLogJson
    {
        List<ISimplePlayer> Players { get; }
        string RecordedBy { get; }
        List<ISimmpleTarget> Targets { get; }
        int Version { get; }

        string ToString();
    }
}