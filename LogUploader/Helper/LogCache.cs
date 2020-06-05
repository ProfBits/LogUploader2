using LogUploader.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogUploader.Helper
{
    static class LogCache
    {
        private static LinkedList<CachedLog> Cache = new LinkedList<CachedLog>();

        private static int maxSize = 128;

        public static int MaxSize { get => maxSize; set => maxSize = value > 0 ? value : maxSize; }

        public static void Add(CachedLog log)
        {
            if (Cache.Contains(log))
                Cache.Remove(log);
            Cache.AddFirst(log);
            if (Cache.Count > maxSize)
                Cache.RemoveLast();
        }
        public static void Addend(CachedLog log)
        {
            if (Cache.Contains(log))
                Cache.Remove(log);
            if (Cache.Count > maxSize)
                Cache.RemoveLast();
            Cache.AddLast(log);
        }

        public static void Remove(int id)
        {
            Remove(getLog(id));
        }

        public static void Remove(CachedLog log)
        {
            Cache.Remove(log);
        }

        public static CachedLog getLog(int id)
        {
            return Cache.Where(log => log.ID == id).FirstOrDefault();
        }
    }
}
