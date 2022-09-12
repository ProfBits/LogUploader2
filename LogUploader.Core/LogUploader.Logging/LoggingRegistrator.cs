using LogUploader.Injection;

namespace LogUploader.Logging;

public class LoggingRegistrator : IServiceRegistrator
{
    public async Task Load(IServiceCollection serviceCollection)
    {
        await Task.Run(() => serviceCollection.Add<ILogger>(LoggerFactory.GetInstance()));
    }
}
