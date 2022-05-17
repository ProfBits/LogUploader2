namespace LogUploader.Injection;

public interface IServiceProvider
{
    T Create<T>();
    IEnumerable<ILoadedService> GetLoadedServices();
    int CountOfLoadedServices { get; }

    int Count { get; }
}
