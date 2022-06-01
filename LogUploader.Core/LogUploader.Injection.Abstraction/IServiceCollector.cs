namespace LogUploader.Injection;

public interface IServiceCollector
{
    Task<IServiceCollection> CollectServicesAsync(IServiceCollection serviceCollection);
}
