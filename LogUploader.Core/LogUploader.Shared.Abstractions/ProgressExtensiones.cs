using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogUploader
{
    public static class ProgressExtensiones
    {
        public static IProgress<double> Split(this IProgress<double> progress, double from, double to)
        {
            var delta = to - from;
            return new SynchronusProgress<double>(p => progress.Report((p * delta) + from));
        }

        public static IProgress<IProgressMessage> Split(this IProgress<IProgressMessage> progress, double from, double to)
        {
            var delta = to - from;
            return new SynchronusProgress<IProgressMessage>(p => progress.Report(new ProgressMessage(p.Message, (p.Progress * delta) + from)));
        }

        public static IProgress<IProgressMessage> Split(this IProgress<IProgressMessage> progress, string prefix, double from, double to)
        {
            var delta = to - from;
            return new SynchronusProgress<IProgressMessage>(p => progress.Report(new ProgressMessage(prefix + p.Message, (p.Progress * delta) + from)));
        }

        public static IProgress<double> WithStaticMessage(this IProgress<IProgressMessage> progress, string message, double from, double to)
        {
            var delta = to - from;
            return new SynchronusProgress<double>(p => progress.Report(new ProgressMessage(message, (p * delta) + from)));
        }

        public static IEnumerable<(T, int)> WithIndex<T>(this IEnumerable<T> enumerable) {
            int i = -1;
            foreach (var e in enumerable)
                yield return (e, ++i);
        }
    }
}
