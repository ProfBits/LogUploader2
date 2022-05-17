namespace LogUploader.Injection;

internal class ServiceProvider : IServiceProvider
{
    private Dictionary<Type, IServiceHandel> linkedServiceHandels;

    public ServiceProvider(IList<IServiceHandel> orderedServiceHandels)
    {
        linkedServiceHandels = orderedServiceHandels.ToDictionary(ks => ks.Declaration, vs => vs);
        CountOfLoadedServices = orderedServiceHandels.Count(handle => handle.IsLoadedService);
        Count = linkedServiceHandels.Count;
    }

    public int CountOfLoadedServices { get; }
    public int Count { get; }

    public T Create<T>()
    {
        return (T)linkedServiceHandels[typeof(T)].CreateService();
    }

    public IEnumerable<ILoadedService> GetLoadedServices()
    {
        return linkedServiceHandels.Values
            .Where(handle => handle.IsLoadedService)
            .OrderBy(handle => handle.Order)
            .Select(handle => handle.CreateService())
            .Cast<ILoadedService>();
    }
}
