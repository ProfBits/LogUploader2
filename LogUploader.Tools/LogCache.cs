using LogUploader.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogUploader.Tools
{
    public static class LogCache
    {
        private static readonly LinkedList<ICachedLog> Cache = new LinkedList<ICachedLog>();

        private static int maxSize = 128;

        public static int MaxSize { get => maxSize; set => maxSize = value > 0 ? value : maxSize; }

        public static void Add(ICachedLog log)
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
        public static void AddEnd(ICachedLog log)
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

        public static void Remove(ICachedLog log)
        {
            lock (Cache)
                Cache.Remove(log);
        }

        public static ICachedLog GetLog(int id)
        {
            lock (Cache)
               return Cache.Where(log => log.ID == id).FirstOrDefault();
        }
    }
}
