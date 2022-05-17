namespace LogUploader.Injection;

internal static class ServiceBuilder
{
    public static IServiceProvider Build(IEnumerable<IServiceDescriptor> serviceDescriptors)
    {
        Dictionary<Type, IServiceHandel> serviceHandels = CreateServiceHandels(serviceDescriptors);

        var providerHandler = new ProviderHandle();
        serviceHandels.Add(providerHandler.Declaration, providerHandler);

        Dictionary<Type, IServiceHandel> linkedServiceHandels = LinkServices(serviceHandels);
        IList<IServiceHandel> orderedServiceHandels = OrderServices(linkedServiceHandels);
        var sp =  new ServiceProvider(orderedServiceHandels);
        
        providerHandler.Insatnce = sp;

        return sp;
    }

    private static IList<IServiceHandel> OrderServices(Dictionary<Type, IServiceHandel> linkedServiceHandels)
    {
        Queue<IServiceHandel> toProcess = new ();

        foreach (var handel in linkedServiceHandels.Values)
        {
            if (handel.Dependencies.Length == 0)
            {
                handel.Order = 0;
            }
            else
            {
                handel.Order = 1;
                toProcess.Enqueue(handel);
            }
        }

        var pushesSinceLastRemoval = 0;

        while (toProcess.TryDequeue(out IServiceHandel? handel))
        {
            if (handel.Order <= handel.Dependencies.Max(d => d.Order)) {
                handel.Order++;
                toProcess.Enqueue(handel);
                pushesSinceLastRemoval++;
            }
            else
            {
                pushesSinceLastRemoval = 0;
            }
            
            if (pushesSinceLastRemoval > toProcess.Count)
            {
                throw new CyclicServcieReferenceException(toProcess.Select(s => s.Declaration).ToArray());
            }
        }

        return linkedServiceHandels.Values.OrderBy(h => h.Order).ToList();
    }

    private static Dictionary<Type, IServiceHandel> LinkServices(Dictionary<Type, IServiceHandel> serviceHandels)
    {
        foreach (var handel in serviceHandels.Values)
        {
            handel.Link(serviceHandels);
        }

        return serviceHandels;
    }

    private static Dictionary<Type, IServiceHandel> CreateServiceHandels(IEnumerable<IServiceDescriptor> serviceDescriptors)
    {
        return serviceDescriptors.Select(sd => sd.CreateHandel()).ToDictionary(ks => ks.Declaration, vs => vs);
    }
}
