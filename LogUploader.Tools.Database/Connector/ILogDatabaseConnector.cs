using LogUploader.Data;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using System;

namespace LogUploader.Tools.Database
{
    public interface ILogDatabaseConnector : ILogDatabaseStatusCommands, ILogDatabaseUpgradeCommands, ILogDatabaseGetCommands, ILogDatabaseInsertCommands,
                                             ILogDatabaseUpdateCommands, ILogDatabaseDeleteCommands, ILogDatabaseJsonExtensionCommands
    { }

    public interface ILogDatabaseStatusCommands
    {
        Version ToolVersion { get; }
        Version FileVersion { get; }
        bool IsUpgrageRequired { get; }
    }

    public interface ILogDatabaseUpgradeCommands
    {
        Task Upgrade(IProgress<ProgressMessage> progress, CancellationToken ct);
    }

    public interface ILogDatabaseGetCommands
    {
        List<ICachedLog> GetAllLogs();
        List<ICachedLog> GetCountOfLogs();
    }

    public interface ILogDatabaseInsertCommands
    {
        void InsertLog(ICachedLog log);
        void InsertLog(IEnumerable<ICachedLog> logs);
        Task InsertLogAsync(IEnumerable<ICachedLog> logs, IProgress<double> progress = null, CancellationToken ct = default);
    }

    public interface ILogDatabaseUpdateCommands
    {
        void UpdateLog(ICachedLog log);
    }

    public interface ILogDatabaseDeleteCommands
    {
        void DeleteLog(ICachedLog log);
    }

    public interface ILogDatabaseJsonExtensionCommands
    {
        void LoadAdditionalData(ICachedLog log);
        void StoreAdditionalData(ICachedLog log);
    }

    //Future of additional data
    //public interface ILogDatabaseExtendedDataCommands
    //{
    //    IExtendedLog LoadAdditionalData(ICachedLog log);
    //    void StoreAdditionalData(ICachedLog log, ILogExtensionData);
    //}
}
