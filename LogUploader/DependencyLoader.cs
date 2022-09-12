using LogUploader.Injection;
using LogUploader.Logging;

using Serilog.Core;

namespace LogUploader;

internal class DependencyLoader
{
    private static readonly Func<IServiceCollection, ILogger, Task>[] RegistratorsHandels = new Func<IServiceCollection, ILogger, Task>[] {
        (sc, logger) => ExecuteRegistrator<IO.Registrator>(sc, logger),
        (sc, logger) => ExecuteRegistrator<Logging.Registrator>(sc, logger),
        (sc, logger) => ExecuteRegistrator<Localization.Registrator>(sc, logger),
        (sc, logger) => ExecuteRegistrator<Gui.Main.Registrator>(sc, logger)
    };

    public async Task Load(IServiceCollection serviceCollection, ILogger logger, IProgress<IProgressMessage>? progress, CancellationToken ct)
    {
        var loadTasks = RegistratorsHandels.Select(handel => handel(serviceCollection, logger)).ToArray();
        var numberOfTasks = loadTasks.Length;
        for (int i = 0; i < numberOfTasks; i++)
        {
            progress?.Report(new ProgressMessage("Collection modules", (double)i / numberOfTasks));
            if (ct.IsCancellationRequested) break;
            await loadTasks[i];
        }
    }

    private static async Task ExecuteRegistrator<T>(IServiceCollection serviceCollection, ILogger logger) where T : IServiceRegistrator, new()
    {
        logger.Verbose("Executing serviceregistrator {ServiceRegistrator}", new object?[] { typeof(T).FullName });
        var registrator = new T();
        var loadTask = registrator.Load(serviceCollection);
        await loadTask;

        LogOutcome(logger, loadTask, typeof(T));
    }

    private static void LogOutcome(ILogger logger, Task loadTask, Type registrator)
    {
        if (loadTask.IsFaulted)
        {
            const string MessageTemplate = "Serviceregistrator {ServiceRegistrator} failed";
            if (loadTask.Exception is null)
            {
                logger.Warning(MessageTemplate, new object?[] { registrator.FullName });
            }
            else
            {
                logger.Warning(loadTask.Exception, MessageTemplate, new object?[] { registrator.FullName });
            }
        }
        else
        {
            logger.Verbose("Serviceregistrator {ServiceRegistrator} finished", new object?[] { registrator.FullName });
        }
    }
}
