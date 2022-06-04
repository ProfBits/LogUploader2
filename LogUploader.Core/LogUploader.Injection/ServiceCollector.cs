using System.Reflection;

using LogUploader.Logging;

namespace LogUploader.Injection;

public class ServiceCollector : IServiceCollector
{
    private readonly ITypeProvider TypeProvider;
    private readonly ILogger logger;

    internal ServiceCollector(ITypeProvider typeProvider, ILogger logger)
    {
        TypeProvider = typeProvider;
        this.logger = logger;
    }

    public static IServiceCollector Create(ILogger logger)
    {
        return new ServiceCollector(new AssemblyTypeProvider(new TestAssemblieIgnoringProvider()), logger);
    }

    public async Task<IServiceCollection> CollectServicesAsync(IServiceCollection serviceCollection)
    {
        if (serviceCollection is null)
        {
            throw new ArgumentNullException(nameof(serviceCollection));
        }

        var defTask = Task.Run(() => TypeProvider.GetTypes().AsParallel().Where(t => t.IsInterface).Where(t => t.GetCustomAttribute<ServiceAttribute>() is not null).ToList());
        var implTask = Task.Run(() => TypeProvider.GetTypes().AsParallel().Where(t => !t.IsAbstract).ToList());

        var definitions = await defTask;
        var impls = await implTask;

        await Task.Run(() => definitions.AsParallel().ForAll((t) => FindDeclarationAndRegister(t, impls, serviceCollection)));

        return serviceCollection;
    }

    private void FindDeclarationAndRegister(Type t, IEnumerable<Type> possibleImpls, IServiceCollection serviceCollection)
    {
        var impls = possibleImpls.AsParallel().Where(possibleImpls => possibleImpls.IsAssignableTo(t)).ToList();
        if (impls.Count == 0)
        {
            logger.Warning("No implementation for {ServiceDeclaration} found.", new object?[] { t.FullName });
            return;
        }
        if (impls.Count > 1)
        {
            logger.Warning("Multiple implementations for {ServiceDeclaration} found, using {UsedType}.\nImplementations are {AllImplementations}", new object?[] { t.FullName, impls.First().FullName, impls });
        }
        logger.Debug("Load service {Service} from {ServiceImplementatation}", new object?[] { t.FullName, impls.First().FullName });
        serviceCollection.Add(t, impls.First());
    }
}
