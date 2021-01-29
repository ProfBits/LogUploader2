using LogUploader.Interfaces;
using System;

namespace LogUploader.Helper.JobQueue
{
    public class JobStartedEventArgs<T> : EventArgs
    {
        public JobStartedEventArgs(IJob<T> job, int queueLength) : base()
        {
            Job = job;
            QueueLength = queueLength;
        }

        public IJob<T> Job { get; }
        public int QueueLength { get; }
    }
}
