namespace LogUploader.Injection;

[Serializable]
public class InjectionException : Exception
{
    public InjectionException(Type affectedService, string message) : base(message)
    {
        AffectedService = affectedService;
    }

    public Type AffectedService { get; }
}

[Serializable]
public class DuplicatedServiceException : InjectionException
{
    public DuplicatedServiceException(Type duplicatedService) : base(duplicatedService, $"Service of type \"{duplicatedService.FullName}\" already added")
    {
    }
}


[Serializable]
public class CyclicServcieReferenceException : InjectionException
{
    public CyclicServcieReferenceException(Type[] services) : base(services[0], GetMessage(services))
    {
        Services = services;
    }

    private static string GetMessage(Type[] services)
    {
        return "Services have a cyclic dependecyies. Affected services " + String.Join(", ", services.Select(t => t.FullName));
    }

    public Type[] Services { get; }
}

[Serializable]
public class NoConstroctorMarkedForInjectionException : InjectionException
{
    public NoConstroctorMarkedForInjectionException(Type implementation) : base(implementation, $"The type {implementation.FullName} does not mark any constroctor for injection with {nameof(InjectionConstructorAttribute)}, one marked construcor expected")
    {
    }
}

[Serializable]
public class MultipleConstroctorMarkedForInjectionException : InjectionException
{
    public MultipleConstroctorMarkedForInjectionException(Type implementation) : base(implementation, $"The type {implementation.FullName} does mark multiple constroctor for injection with {nameof(InjectionConstructorAttribute)}, only one marked constructor expected")
    {
    }
}


[Serializable]
public class NotAServiceException : InjectionException
{
    public NotAServiceException(Type declaration) : base(declaration, $"The type {declaration.FullName} is not makred as a service, add the {nameof(ServiceAttribute)} to mark it as a service")
    {
    }
}


[Serializable]
public class UnkownServiceTypeException : InjectionException
{
    public UnkownServiceTypeException(ServiceType serviceType, Type affectedService) : base(affectedService, $"The servicetype \"{serviceType}\" of service \"{affectedService.FullName}\" is unkown or not supported")
    {
        ServiceType = serviceType;
    }

    public ServiceType ServiceType { get; }
}


[Serializable]
public class ServiceInstanceNullException : InjectionException
{
    public ServiceInstanceNullException(Type affectedService) : base(affectedService, $"Instance for sercive \"{affectedService}\" was null")
    {
    }
}

[Serializable]
public class ServiceTypeNotCopatipleWithPreInitException : InjectionException
{
    public ServiceTypeNotCopatipleWithPreInitException(Type affectedService)
        : base(affectedService, $"The service of type \"{affectedService.FullName}\" is not sutiable for pre init, only {nameof(ServiceType.Singelton)} is supported")
    {
    }
}

[Serializable]
public class ServiceTypeNotCopatipleWithLaodedServiceException : InjectionException
{
    public ServiceTypeNotCopatipleWithLaodedServiceException(Type affectedService)
        : base(affectedService, $"The service of type \"{affectedService.FullName}\" is not sutiable for loaded service, only {nameof(ServiceType.Singelton)} is supported")
    {
    }
}


[Serializable]
public class DependencyNotFoundException : InjectionException
{
    public DependencyNotFoundException(Type declaration, Type missingDependency)
        : base(declaration, $"Could not finde Dependency \"{missingDependency.FullName}\" for service \"{declaration.FullName}\"")
    {
        MissingDependency = missingDependency;
    }

    public Type MissingDependency { get; }
}