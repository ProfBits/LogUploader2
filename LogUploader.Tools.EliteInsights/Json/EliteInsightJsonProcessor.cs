using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Newtonsoft.Json.Linq;

using LogUploader.Interfaces;
using System.IO;
using Newtonsoft.Json;

namespace LogUploader.Tools.EliteInsights.Json
{
    public static class EliteInsightJsonProcessor
    {
        internal static readonly IEiProcessor[] Processors = new IEiProcessor[]
        {
            //new EiProcessor_2_35_0_0(),
            //new EiProcessor_2_33_0_0(),
            //new EiProcessor_2_29_0_0(),
            new EiProcessor_Legacy()
        };

        internal static IEiProcessor GetEiProcessor(Version version)
        {
            return Processors.FirstOrDefault(p => p.IsVersionSupported(version));
        }

        public static LogFull Process(Stream json, IProgress<double> progress, CancellationToken ct = default)
        {
            progress?.Report(0);

            if (json?.CanSeek != true) throw new InvalidOperationException("the stream must be seekeable");

            Version version = GetJsonVersion(json, new SynchronusProgress<double>(p => progress?.Report(p * 0.05)), ct);

            IEiProcessor processor = GetEiProcessor(version);

            progress?.Report(0.06);

            json.Seek(0, SeekOrigin.Begin);

            progress?.Report(0.07);
            if (ct.IsCancellationRequested) throw new OperationCanceledException("Canceled by external token");

            LogFull parsed = processor.Process(json, new SynchronusProgress<double>(p => progress?.Report((p * 0.92) + 0.07)), ct);

            Task.Run(GC.Collect);

            progress?.Report(1);

            return parsed;
        }

        internal static LogFull ProcessStream(Stream json, IEiProcessor processor, IProgress<double> progress = null, CancellationToken cancellationToken = default)
        {
            //TODO null check
            //TODO requ pos 0
            return processor.Process(json, progress, cancellationToken);
        }

        internal static Version GetJsonVersion(Stream json, IProgress<double> progress = null, CancellationToken cancellationToken = default)
        {
            const double PROGRESS_PERCENT_STEP = 0.01;
            string versionStr = null;

            //TODO requ pos 0

            using (var ps = progress is null ? null : new ProgressStream(json))
            using (var sr = new StreamReader(ps ?? json, Encoding.UTF8))
            using (var jr = new JsonTextReader(sr))
            {
                long lastPos = long.MinValue;
                long bytePerStep = (long)(json.Length * PROGRESS_PERCENT_STEP);

                if (!(ps is null))
                    ps.BytesRead += (sender, e) => {
                        if (e.StreamPosition - bytePerStep > lastPos)
                            progress.Report((double)e.StreamPosition / e.StreamLength * 0.98);
                    };

                if (cancellationToken.IsCancellationRequested) throw new OperationCanceledException("Canceled by external token");

                while (jr.Read())
                {
                    bool currentIsVersionToken =
                        jr.TokenType == JsonToken.PropertyName
                        && (jr.Value?.Equals("eliteInsightsVersion") ?? false);

                    if (currentIsVersionToken)
                    {
                        versionStr = GetStringValueFromNextToken(versionStr, jr);
                        if (!string.IsNullOrEmpty(versionStr)) // Found it -> Done and stop looping
                            break;
                    }

                    if (cancellationToken.IsCancellationRequested) throw new OperationCanceledException("Canceled by external token");
                }
            }

            progress?.Report(1);

            if (string.IsNullOrEmpty(versionStr))
            {
                return new Version(-1, 0, 0, 0);
            }
            return new Version(versionStr);
        }

        private static string GetStringValueFromNextToken(string versionStr, JsonTextReader jr)
        {
            if (jr.Read())
            {
                if (jr.TokenType == JsonToken.String)
                {
                    versionStr = (string)jr.Value;
                }
            }

            return versionStr;
        }
    }

    internal interface IEiProcessor
    {
        bool IsVersionSupported(Version version);
        LogFull Process(Stream data, IProgress<double> progress = null, CancellationToken cancellationToken = default);
    }
    internal abstract class AbstractEiProcessor<T> : IEiProcessor
    {
        protected abstract Version MinVersion { get; }
        protected virtual Version MaxVersion { get; } = new Version(int.MaxValue, int.MaxValue, int.MaxValue, int.MaxValue);

        public virtual bool IsVersionSupported(Version version)
        {
            return MinVersion <= version && version < MaxVersion;
        }

        public virtual LogFull Process(Stream data, IProgress<double> progress = null, CancellationToken cancellationToken = default)
        {
            T parsed = Parse(data, new SynchronusProgress<double>(p => progress?.Report(p * 0.8)));
            LogFull res = Convert(parsed, new SynchronusProgress<double>(p => progress?.Report((p * 0.18) + 0.81)));
            progress?.Report(1);
            return res;
        }

        internal abstract LogFull Convert(T parsed, IProgress<double> progress = null, CancellationToken cancellationToken = default);

        internal virtual T Parse(Stream data, IProgress<double> progress = null)
        {
            const double reportStep = 0.01;

            using (var ps = new ProgressStream(data))
            using (var sr = new StreamReader(data, Encoding.UTF8))
            using (var jr = new JsonTextReader(sr))
            {
                long lastReport = long.MinValue;
                long absolutStep = (long)(data.Length * reportStep);
                ps.BytesRead += (s, args) =>
                {
                    if (lastReport <= args.StreamPosition - absolutStep)
                    {
                        lastReport = args.StreamPosition;
                        progress?.Report((double)args.StreamPosition / args.StreamLength);
                    }
                };

                return new JsonSerializer().Deserialize<T>(jr);
            }
        }
    }

    //internal class EiProcessor_2_35_0_0 : AbstractEiProcessor
    //{
    //    /* 
    //     * Supports EliteInsightVersion 2.35.0.0 onwards
    //     * Noteable features:
    //     *  - Added IsFake to JsonActor
    //     *  - Added EnemyPlayer to JsonNPC
    //     *  - Added FriendlyNPC to JsonPlayer
    //     */
    //    protected override Version MinVersion { get; } = new Version(2, 35, 0, 0);
    //
    //    public override LogFull Process(JObject log, IProgress<double> progress = null)
    //    {
    //        throw new NotImplementedException();
    //    }
    //}
    //
    //internal class EiProcessor_2_33_0_0 : AbstractEiProcessor
    //{
    //    /* 
    //     * Supports EliteInsightVersion 2.33.0.0 onwards
    //     * Noteable features:
    //     *  - Added friendly NPC concept. Saul (on Deimos) and Desmina (on River of Souls) will now appear as such
    //     */
    //    protected override Version MinVersion { get; } = new Version(2, 33, 0, 0);
    //    protected override Version MaxVersion { get; } = new Version(2, 35, 0, 0);
    //
    //    public override LogFull Process(JObject log, IProgress<double> progress = null)
    //    {
    //        throw new NotImplementedException();
    //    }
    //}
    //
    //internal class EiProcessor_2_29_0_0 : AbstractEiProcessor
    //{
    //    /* 
    //     * Supports EliteInsightVersion 2.29.0.0 onwards
    //     * Noteable features:
    //     *  - Added support for breakbar damage events
    //     */
    //    protected override Version MinVersion { get; } = new Version(2, 29, 0, 0);
    //    protected override Version MaxVersion { get; } = new Version(2, 33, 0, 0);
    //
    //    public override LogFull Process(JObject log, IProgress<double> progress = null)
    //    {
    //        throw new NotImplementedException();
    //    }
    //}
}
