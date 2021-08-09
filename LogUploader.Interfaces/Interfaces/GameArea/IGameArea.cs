using System;

using LogUploader.Localisation;

namespace LogUploader.Interfaces
{
    public interface IGameArea : INamedObject,  IAvatar, IEquatable<IGameArea>
    {
        string ShortName { get; }
        string ShortNameDE { get; }
        string ShortNameEN { get; }

        bool Equals(object obj);
        int GetHashCode();
        string GetShortName(eLanguage lang);
    }
}