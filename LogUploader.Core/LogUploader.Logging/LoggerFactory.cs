using System.IO.Compression;

using Serilog;
using Serilog.Formatting.Compact;
using Serilog.Sinks.File.Archive;

using LogUploader.IO;

namespace LogUploader.Logging;

public static class LoggerFactory
{
    public static ILogger CreateLogger(IFileIO fileIO)
    {
#if DEBUG
        var logLevel = Serilog.Events.LogEventLevel.Verbose;
#else
        var logLevel = Serilog.Events.LogEventLevel.Information;
#endif

        var logFileName = fileIO.GetPath(RootFolder.Logs, $"{DateTime.Now:yyyy'-'MM'-'dd'_'HH'-'mm'-'ss}_{logLevel}.log.zjson");

        Log.Logger = new LoggerConfiguration()
             .WriteTo.Async(w => w.File(new CompactJsonFormatter(), logFileName, restrictedToMinimumLevel: logLevel, hooks: new ArchiveHooks(CompressionLevel.SmallestSize)))
#if DEBUG
             .WriteTo.Debug(outputTemplate: "[{Timestamp:yyyy-MM-dd HH:mm:ss.fff} {Level:u3}] <{ThreadId}><{ThreadName}> {Message:lj}{NewLine}{Exception}", restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Verbose)
#endif
             .Enrich.WithThreadId()
             .Enrich.WithThreadName()

             .MinimumLevel.Verbose()
             .CreateLogger();
        return GetInstance();
    }

    internal static ILogger GetInstance()
    {
        return new Logger(Log.Logger);
    }

    public static void StopLogger()
    {
        Log.CloseAndFlush();
    }
}
