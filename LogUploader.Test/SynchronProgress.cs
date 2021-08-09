using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogUploader.Test
{
    internal class SynchronProgress<T> : IProgress<T>
    {
        private readonly Action<T> handler;

        public SynchronProgress(Action<T> handler)
        {
            this.handler = handler ?? throw new ArgumentNullException(nameof(handler), "Handler cannot be null");
        }

        public void Report(T value)
        {
            handler(value);
        }
    }
}
