using System;
using System.Collections.Generic;

namespace LogUploader.Data.Repositories
{
    internal abstract class EnemyRepository<T> : EnemyProvider<T> where T : Enemy
    {
        internal abstract IMultiKeyBaseDictionary<int, string, string, T> BaseData { get; }

        public T Get(int id)
        {
            return BaseData.Get(id);
        }

        public T Get(string name)
        {
            try
            {
                return BaseData.Get(key2: name);
            }
            catch (KeyNotFoundException)
            {
                return BaseData.Get(key3: name);
            }
        }

        internal abstract void Add(T enemy);
    }
}