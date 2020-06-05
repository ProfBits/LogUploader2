using LogUploader.Interfaces;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LogUploader.Helper.JobQueue
{

    public class JobQueue<T>
    {
        private BlockingCollection<IJob<T>> jobQueue = new BlockingCollection<IJob<T>>(new ConcurrentQueue<IJob<T>>());
        private CancellationTokenSource PrivateCts = null;

        private Thread WorkerThread { get; set; }
        public bool IsStopped { get => !WorkerThread.IsAlive; }
        private CancellationToken StopToken { get; }

        public delegate void JobDoneEventHandler(object sender, JobDoneEventArgs<T> e);
        public event JobDoneEventHandler JobDone;

        protected virtual void OnJobDone(JobDoneEventArgs<T> e)
        {
            JobDone?.Invoke(this, e);
        }

        public delegate void QueueEmptyEventHandler(object sender, JobQueueEmptyEventArgs e);
        public event QueueEmptyEventHandler JobQueueEmpty;
        protected virtual void OnQueueEmpty(JobQueueEmptyEventArgs e)
        {
            JobQueueEmpty?.Invoke(this, e);
        }

        public delegate void JobAddedEventHandler(object sender, JobAddedEventArgs<T> e);
        public event JobAddedEventHandler JobAdded;
        protected virtual void OnJobAdded(JobAddedEventArgs<T> e)
        {
            JobAdded?.Invoke(this, e);
        }

        public delegate void JobStartedEventHandler(object sender, JobStartedEventArgs<T> e);
        public event JobStartedEventHandler JobStarted;
        protected virtual void OnJobStarted(JobStartedEventArgs<T> e)
        {
            JobStarted?.Invoke(this, e);
        }

        public delegate void JobFaultedEventHandler(object sender, JobFaultedEventArgs<T> e);
        public event JobFaultedEventHandler JobFaulted;
        protected virtual void OnJobFaulted(JobFaultedEventArgs<T> e)
        {
            JobFaulted?.Invoke(this, e);
        }

        public JobQueue() : this ($"JobQueue of {typeof(T).Name}")
        {
        }

        public JobQueue(string name)
        {
            PrivateCts = new CancellationTokenSource();
            StopToken = PrivateCts.Token;

            Start(name);
        }

        public JobQueue(CancellationToken stopToken) : this(stopToken, $"JobQueue of {typeof(T).Name}")
        {
        }

        public JobQueue(CancellationToken stopToken, string name)
        {
            StopToken = stopToken;

            Start(name);
        }

        private void Start(string name)
        {
            WorkerThread = new Thread(new ThreadStart(Run));
            WorkerThread.IsBackground = false;
            WorkerThread.Name = name;
            WorkerThread.CurrentCulture = System.Globalization.CultureInfo.InvariantCulture;
            WorkerThread.Start();
        }

        private void Run()
        {
            while (!StopToken.IsCancellationRequested)
            {
                IJob<T> job;
                try
                {
                    job = jobQueue.Take(StopToken);
                    ExecuteJob(job);

                    if (jobQueue.Count == 0)
                        OnQueueEmpty(new JobQueueEmptyEventArgs());
                }
                catch (OperationCanceledException)
                {
                    break;
                }
                catch (ThreadAbortException)
                {
                    jobQueue.CompleteAdding();
                    jobQueue.Dispose();
                    return;
                }
            }
            jobQueue.CompleteAdding();
        }

        private void ExecuteJob(IJob<T> job)
        {
            try
            {
                OnJobStarted(new JobStartedEventArgs<T>(job, jobQueue.Count));
                var res = job.DoWork();
                OnJobDone(new JobDoneEventArgs<T>(res, jobQueue.Count));
            }
            catch (OperationCanceledException e)
            {
                throw e;
            }
            catch (Exception e)
            {
                OnJobFaulted(new JobFaultedEventArgs<T>(job, e, jobQueue.Count));
            }
        }

        public void Add(IJob<T> job)
        {
            jobQueue.Add(job);
            OnJobAdded(new JobAddedEventArgs<T>(job, jobQueue.Count));
        }

        public void Add(Func<T> func)
        {
            Add(new GenericJob<T>(func));
        }

        private class GenericJob<R> : IJob<R>
        {
            private Func<R> job;

            public GenericJob(Func<R> func)
            {
                this.job = func;
            }

            public R DoWork() => job();
        }

        ~JobQueue()
        {
            jobQueue.CompleteAdding();
            jobQueue.Dispose();
        }
    }
}
