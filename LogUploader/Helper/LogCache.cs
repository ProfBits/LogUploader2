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
            lock (Cache)
            {
                if (Cache.Contains(log))
                    Cache.Remove(log);
                Cache.AddFirst(log);
                if (Cache.Count > maxSize)
                    Cache.RemoveLast();
            }
        }
        public static void AddEnd(CachedLog log)
        {
            lock (Cache)
            {
                if (Cache.Contains(log))
                    Cache.Remove(log);
                if (Cache.Count > maxSize)
                    Cache.RemoveLast();
                Cache.AddLast(log);
            }
        }

        public static void Remove(int id)
        {
            Remove(GetLog(id));
        }

        public static void Remove(CachedLog log)
        {
            lock (Cache)
                Cache.Remove(log);
        }

        public static CachedLog GetLog(int id)
        {
            lock (Cache)
               return Cache.Where(log => log.ID == id).FirstOrDefault();
        }
    }
}
