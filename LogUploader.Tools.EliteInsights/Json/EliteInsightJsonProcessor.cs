using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Newtonsoft.Json.Linq;

using LogUploader.Interfaces;

namespace LogUploader.Tools.EliteInsights.Json
{
    public static class EliteInsightJsonProcessor
    {
        internal static readonly IEiProcessor[] Processors = new IEiProcessor[]
        {
            new EiProcessor_2_35_0_0(),
            new EiProcessor_2_33_0_0(),
            new EiProcessor_2_29_0_0(),
            new EiProcessor_Legacy()
        };

        internal static IEiProcessor GetEiProcessor(Version version)
        {
            return Processors.FirstOrDefault(p => p.IsVersionSupported(version));
        }

        public static LogFull Process(string json, IProgress<double> progress, CancellationToken ct = default)
        {
            progress?.Report(0);
            
            JObject parsed = JObject.Parse(json);
            
            progress?.Report(0.5);
            
            string eiVersion = (string)parsed["eliteInsightsVersion"];
            Version version = new Version(eiVersion);
            IEiProcessor processor = GetEiProcessor(version);
            
            progress?.Report(0.6);
            
            LogFull result = processor.Process(parsed, new SynchronusProgress<double>(p => progress.Report((p * 0.39) + 0.6)));
            
            progress?.Report(0.99);
            
            parsed = null;
            Task.Run(GC.Collect);
            
            progress?.Report(1);
            
            return result;
        }
    }

    internal interface IEiProcessor
    {
        bool IsVersionSupported(Version version);
        LogFull Process(JObject log, IProgress<double> progress = null);
    }
    internal abstract class AbstractEiProcessor : IEiProcessor
    {
        protected abstract Version MinVersion { get; }
        protected virtual Version MaxVersion { get; } = new Version(int.MaxValue, int.MaxValue, int.MaxValue, int.MaxValue);

        public virtual bool IsVersionSupported(Version version)
        {
            return MinVersion <= version && version < MaxVersion;
        }
        public abstract LogFull Process(JObject log, IProgress<double> progress = null);
    }

    internal class EiProcessor_2_35_0_0 : AbstractEiProcessor
    {
        /* 
         * Supports EliteInsightVersion 2.35.0.0 onwards
         * Noteable features:
         *  - Added IsFake to JsonActor
         *  - Added EnemyPlayer to JsonNPC
         *  - Added FriendlyNPC to JsonPlayer
         */
        protected override Version MinVersion { get; } = new Version(2, 35, 0, 0);

        public override LogFull Process(JObject log, IProgress<double> progress = null)
        {
            throw new NotImplementedException();
        }
    }

    internal class EiProcessor_2_33_0_0 : AbstractEiProcessor
    {
        /* 
         * Supports EliteInsightVersion 2.33.0.0 onwards
         * Noteable features:
         *  - Added friendly NPC concept. Saul (on Deimos) and Desmina (on River of Souls) will now appear as such
         */
        protected override Version MinVersion { get; } = new Version(2, 33, 0, 0);
        protected override Version MaxVersion { get; } = new Version(2, 35, 0, 0);

        public override LogFull Process(JObject log, IProgress<double> progress = null)
        {
            throw new NotImplementedException();
        }
    }

    internal class EiProcessor_2_29_0_0 : AbstractEiProcessor
    {
        /* 
         * Supports EliteInsightVersion 2.29.0.0 onwards
         * Noteable features:
         *  - Added support for breakbar damage events
         */
        protected override Version MinVersion { get; } = new Version(2, 29, 0, 0);
        protected override Version MaxVersion { get; } = new Version(2, 33, 0, 0);

        public override LogFull Process(JObject log, IProgress<double> progress = null)
        {
            throw new NotImplementedException();
        }
    }

    internal class EiProcessor_Legacy : AbstractEiProcessor
    {
        /* 
         * Supports EliteInsightVersion older than 2.29.0.0
         */
        protected override Version MinVersion { get; } = new Version(0, 0, 0, 0);
        protected override Version MaxVersion { get; } = new Version(2, 29, 0, 0);

        public override LogFull Process(JObject log, IProgress<double> progress = null)
        {
            throw new NotImplementedException();
        }
    }
}
