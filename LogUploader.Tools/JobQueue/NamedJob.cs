using LogUploader.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogUploader.Tools.JobQueue
{
    public class NamedJob<T> : INamedJob<T>
    {
        private static int JobID = 0;
        public int ID { get; } = JobID++;
        public string Name { get; }

        private readonly Func<T> Job;

        public NamedJob(string name, Func<T> job)
        {
            Name = name;
            Job = job;
        }

        public T DoWork()
        {
            return Job();
        }
    }
}
