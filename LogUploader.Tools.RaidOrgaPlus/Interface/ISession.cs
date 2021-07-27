using System;

namespace LogUploader.Tools.RaidOrgaPlus.Data
{
    public interface ISession
    {
        DateTime Created { get; }
        TimeSpan Timeout { get; }
        string Token { get; }
        string UserAgent { get; }
        bool Valid { get; }

        bool Equals(object obj);
        int GetHashCode();
        string ToString();
    }
}