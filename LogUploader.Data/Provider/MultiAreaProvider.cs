using System.Collections.Generic;

namespace LogUploader.Data.Repositories
{
    public interface MultiAreaProvider<T> : IEnumerable<T>
    {
        T Get(int id);
        bool Exists(int number);
    }
}