using System;

namespace LogUploader.Interfaces
{
    public interface IAddEnemy : IEquatable<IAddEnemy>
    {
        bool IsInteresting { get; }

        bool Equals(object obj);
        int GetHashCode();
    }
}