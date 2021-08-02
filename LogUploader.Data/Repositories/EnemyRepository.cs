using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using LogUploader.Localisation;

namespace LogUploader.Data.Repositories
{
    internal abstract class EnemyRepository<T> : EnemyProvider<T> where T : Enemy
    {
        public int Count { get => BaseData.Count; }
        internal abstract IDictionary<int, T> BaseData { get; }
        

        public virtual bool Exists(int id)
        {
            return BaseData.ContainsKey(id);
        }

        public virtual bool Exists(string name)
        {
            return BaseData.Values.Any(e => e.NameEN == name || e.NameDE == name);
        }

        public virtual bool Exists(string name, eLanguage lang)
        {
            switch (lang)
            {
                case eLanguage.DE:
                    return BaseData.Values.Any(e => e.NameDE == name);
                case eLanguage.EN:
                default:
                    return BaseData.Values.Any(e => e.NameEN == name);
            }
        }

        public virtual T Get(int id)
        {
            return BaseData.ContainsKey(id) ? BaseData[id] : BaseData[0];
        }

        public virtual T Get(string name)
        {
            return BaseData.Values.FirstOrDefault(e => e.NameEN == name || e.NameDE == name) ?? throw new KeyNotFoundException();
        }

        public virtual List<T> Get(GameArea area)
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