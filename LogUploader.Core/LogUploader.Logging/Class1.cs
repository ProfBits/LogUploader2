using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Serilog;
using Serilog.Formatting.Compact;
using Serilog.Sinks.File.Archive;

namespace LogUploader.Logging
{
    public class Class1
    {
        public static void test()
        {
#if DEBUG
            var logLevel = Serilog.Events.LogEventLevel.Verbose;
#else
            var logLevel = Serilog.Events.LogEventLevel.Information;
#endif

            Log.Logger = new LoggerConfiguration()
                 .WriteTo.Async(w => w.File(new CompactJsonFormatter(), "log.json", restrictedToMinimumLevel: logLevel, hooks: new ArchiveHooks(CompressionLevel.SmallestSize)))
#if DEBUG
                 .WriteTo.Debug(outputTemplate: "[{Timestamp:yyyy-MM-dd HH:mm:ss.fff} {Level:u3}] <{ThreadId}><{ThreadName}> {Message:lj}{NewLine}{Exception}", restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Verbose)
#endif
                 .Enrich.WithThreadId()
                 .Enrich.WithThreadName()

                 .MinimumLevel.Verbose()
                 .CreateLogger();

            Log.Logger.Debug("Hello Debug");

            var contexted = Log.Logger.ForContext<Class1>();
            contexted.Debug("from context");

            Log.CloseAndFlush();
        }
    }
}
