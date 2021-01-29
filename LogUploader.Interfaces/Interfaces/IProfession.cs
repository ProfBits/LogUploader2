using System;
using System.Drawing;

namespace LogUploader.Data
{
    public interface IProfession : IEquatable<IProfession>
    {
        string Emote { get; }
        Image Icon { get; }
        string IconPath { get; }
        eProfession ProfessionEnum { get; }
        int RaidOrgaPlusID { get; }

        bool Equals(eProfession other);
        bool Equals(object obj);
        int GetHashCode();
        string ToString();
    }
}