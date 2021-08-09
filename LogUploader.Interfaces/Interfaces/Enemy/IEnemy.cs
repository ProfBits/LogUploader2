using System;

namespace LogUploader.Interfaces
{
    public interface IEnemy : INamedObject, IEquatable<IEnemy>
    {
        IGameArea Area { get; }
        int ID { get; }

        bool Equals(object obj);
        int GetHashCode();
    }
}