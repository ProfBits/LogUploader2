using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using LogUploader.Data.GameAreas;
using LogUploader.Localisation;

namespace LogUploader.Data.Repositories
{
    internal abstract class EnemyRepository<T> : EnemyProvider<T> where T : Enemy
    {
        public int Count { get => BaseData.Count; }
        internal abstract IMultiKeyBaseDictionary<int, string, string, T> BaseData { get; }

        public bool Exists(int id)
        {
            return BaseData.ContainsKey(id);
        }

        public bool Exists(string name)
        {
            return BaseData.ContainsKey(key2: name) || BaseData.ContainsKey(key3: name);
        }

        public bool Exists(string name, eLanguage lang)
        {
            switch (lang)
            {
                case eLanguage.DE:
                    return BaseData.ContainsKey(key3: name);
                case eLanguage.EN:
                default:
                    return BaseData.ContainsKey(key2: name);
            }
        }

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

        public List<T> Get(GameArea area)
        {
            return this.Where(e => e.Area.Equals(area)).ToList();
        }

        public abstract IEnumerator<T> GetEnumerator();
        internal abstract void Add(T enemy);

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}