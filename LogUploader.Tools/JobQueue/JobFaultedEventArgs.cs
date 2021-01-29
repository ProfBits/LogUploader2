using LogUploader.Interfaces;
using System;

namespace LogUploader.Helper.JobQueue
{
    public class JobFaultedEventArgs<T> : EventArgs
    {
        public JobFaultedEventArgs(IJob<T> job, Exception exception, int remainingQueueLength) : base()
        {
            Job = job;
            RemainingQueueLength = remainingQueueLength;
            Exception = exception;
        }

        public int RemainingQueueLength { get; }
        public IJob<T> Job { get; }
        public Exception Exception { get; }
    }
}
