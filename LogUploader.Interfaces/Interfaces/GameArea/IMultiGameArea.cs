using System;

namespace LogUploader.Interfaces
{
    public interface IMultiGameArea : IGameArea, IEquatable<IMultiGameArea>
    {
        int ID { get; }
    }
}