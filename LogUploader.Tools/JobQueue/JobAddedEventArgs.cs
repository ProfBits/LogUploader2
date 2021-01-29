using LogUploader.Interfaces;
using System;

namespace LogUploader.Helper.JobQueue
{
    public class JobAddedEventArgs<T> : EventArgs
    {
        public JobAddedEventArgs(IJob<T> job, int newQueueLength) : base()
        {
            Job = job;
            NewQueueLength = newQueueLength;
        }

        public IJob<T> Job { get; }
        public int NewQueueLength { get; }
    }
}
