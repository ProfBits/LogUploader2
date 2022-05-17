using System.Xml.Linq;

namespace LogUploader.Injection;

public class ServiceCollection : IServiceCollection
{
    private Dictionary<Type, IServiceDescriptor> ServiceData = new Dictionary<Type, IServiceDescriptor>();

    private void Add(Type declaration, IServiceDescriptor descriptor)
    {
        ServiceData.Add(declaration, descriptor);
    }

    private void ThrowIfAlreadExists(Type declaration)
    {
        if (ServiceData.ContainsKey(declaration) || declaration.Equals(typeof(IServiceProvider)))
        {
            throw new DuplicatedServiceException(declaration);
        }
    }

    public void Add(Type declaration, Type implementation)
    {
        ThrowIfAlreadExists(declaration);
        Add(declaration, GetServiceDescriptor(declaration, implementation));

    }

    public void Add<TDeclaration, TImplementation>() where TImplementation : TDeclaration
    {
        Add(typeof(TDeclaration), typeof(TImplementation));
    }

    private static IServiceDescriptor GetServiceDescriptor(Type declaration, Type implementation)
    {
        ServiceAttribute serviceAttribute = GetServiceAttribute(declaration);
        return serviceAttribute.ServiceType switch
        {
            ServiceType.Transient => new TransientServiceDescriptor(declaration, implementation),
            ServiceType.Singelton => new SingeltonServiceDescriptor(declaration, implementation),
            _ => throw new UnkownServiceTypeException(serviceAttribute.ServiceType, declaration)
        };
    }

    private static ServiceAttribute GetServiceAttribute(Type declaration)
    {
        var serviceAttributes = declaration.GetCustomAttributes(typeof(ServiceAttribute), false).Cast<ServiceAttribute>().ToArray();

        if (serviceAttributes.Length != 1)
        {
            throw new NotAServiceException(declaration);
        }
        return serviceAttributes[0];
    }

    private bool Contains(Type type)
    {
        if (type.Equals(typeof(IServiceProvider)))
        {
            return true;
        }
        return ServiceData.ContainsKey(type);
    }

    public bool Contains<T>()
    {
        return Contains(typeof(T));
    }

    public IServiceProvider BuildProvider()
    {
        return ServiceBuilder.Build(ServiceData.Values);
    }

    public void Add<T>(T instance)
    {
        Add(typeof(T), instance);
    }

    private void Add(Type declaration, object? instance)
    {
        ThrowIfAlreadExists(declaration);
        var attribute = GetServiceAttribute(declaration);
        if (attribute.ServiceType != ServiceType.Singelton)
        {
            throw new ServiceTypeNotCopatipleWithPreInitException(declaration);
        }
        if (instance is null)
        {
            throw new ServiceInstanceNullException(declaration);
        }
        ServiceData.Add(declaration, new PreInitSingeltonDescriptor(declaration, instance));
    }
}
