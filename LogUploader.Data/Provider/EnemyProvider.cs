using System.Collections.Generic;

using LogUploader.Localisation;

namespace LogUploader.Data
{
    public interface EnemyProvider<T> : IEnumerable<T> where T : Enemy
    {
        int Count { get; }
        T Get(int id);
        T Get(string name);
        bool Exists(int id);
        bool Exists(string name);
        bool Exists(string nameEN, eLanguage eN);
        List<T> Get(GameArea area);
    }
}