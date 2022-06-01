using System.Reflection;

namespace LogUploader.Injection;

public class ServiceCollector : IServiceCollector
{
    private readonly ITypeProvider TypeProvider;

    internal ServiceCollector(ITypeProvider typeProvider)
    {
        TypeProvider = typeProvider;
    }

    public static IServiceCollector Create()
    {
        return new ServiceCollector(new AssemblyTypeProvider(new TestAssemblieIgnoringProvider()));
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

    private static void FindDeclarationAndRegister(Type t, IEnumerable<Type> possibleImpls, IServiceCollection serviceCollection)
    {
        var impls = possibleImpls.AsParallel().Where(possibleImpls => possibleImpls.IsAssignableTo(t)).ToList();
        if (impls.Count == 0)
        {
            //Log.Warn(no impl for t found)
            return;
        }
        if (impls.Count > 1)
        {
            //Log.Warn(multiple impls for t found, using impls.first(). All imples string.join(", ", impls)
        }
        //Log.Debug(Load service t.FullName form impls.First()
        serviceCollection.Add(t, impls.First());
    }
}
