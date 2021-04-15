using System;

namespace LogUploader.Tools.JobQueue
{
    public class JobDoneEventArgs<T> : EventArgs
    {
        public JobDoneEventArgs(T result, int remainingQueueLength) : base()
        {
            Result = result;
            RemainingQueueLength = remainingQueueLength;
        }

        public T Result { get; }
        public int RemainingQueueLength { get; }
    }
}
