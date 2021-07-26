
using LogUploader.Data;

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace LogUploader.Tools.Database
{
    internal abstract class AbstractLogDatabaseConnector : ILogDatabaseConnector
    {
        internal string DatabaseFilePaht { get; }
        internal string ConnectionString { get; }
        public abstract Version ToolVersion { get; }
        public abstract Version FileVersion { get; }
        public abstract bool IsUpgrageRequired { get; }

        protected AbstractLogDatabaseConnector(string path)
        {
            DatabaseFilePaht = path ?? throw new ArgumentNullException($"{nameof(path)} path to databes file cannot be null");
            ConnectionString = $@"Data Source=""{DatabaseFilePaht}""; Version=3;Connect Timeout=30";
        }

        public abstract Task Upgrade(IProgress<ProgressMessage> progress, CancellationToken ct);
        public abstract List<ICachedLog> GetAllLogs();
        public abstract List<ICachedLog> GetCountOfLogs();
        public abstract void InsertLog(ICachedLog log);
        public abstract void InsertLog(IEnumerable<ICachedLog> logs);
        public abstract Task InsertLogAsync(IEnumerable<ICachedLog> logs, IProgress<double> progress = null, CancellationToken ct = default);
        public abstract void UpdateLog(ICachedLog log);
        public abstract void DeleteLog(ICachedLog log);
        public abstract void LoadAdditionalData(ICachedLog log);
        public abstract void StoreAdditionalData(ICachedLog log);
    }

}
