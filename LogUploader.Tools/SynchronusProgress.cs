using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogUploader.Tools
{
    public class SynchronusProgress<T> : IProgress<T>
    {
        private Action<T> ReportCallback { get; }

        public SynchronusProgress(Action<T> reportCallback)
        {
            ReportCallback = reportCallback ?? throw new ArgumentNullException(nameof(reportCallback), "ReportCallback cannot be null");
        }

        public void Report(T value)
        {
            ReportCallback(value);
        }
    }
}
