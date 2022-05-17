namespace LogUploader.Injection;

public interface IServiceDescriptor
{   
    IServiceHandel CreateHandel();
}

internal class TransientServiceDescriptor : IServiceDescriptor
{
    public TransientServiceDescriptor(Type declaration, Type implementation)
    {
        Declaration = declaration;
        Implementation = implementation;
    }

    public Type Declaration { get; }
    public Type Implementation { get; }

    public IServiceHandel CreateHandel()
    {
        return new TransientServiceHandel(Declaration, Implementation);
    }
}

internal class SingeltonServiceDescriptor : IServiceDescriptor
{
    public SingeltonServiceDescriptor(Type declaration, Type implementation)
    {
        Declaration = declaration;
        Implementation = implementation;
    }

    public Type Declaration { get; }
    public Type Implementation { get; }

    public IServiceHandel CreateHandel()
    {
        return new SingeltonServiceHandel(Declaration, Implementation);
    }
}

internal class PreInitSingeltonDescriptor : IServiceDescriptor
{
    public PreInitSingeltonDescriptor(Type type, object instance)
    {
        Declaration = type;
        Instance = instance;
    }

    public Type Declaration { get; }
    public object Instance { get; }

    public IServiceHandel CreateHandel()
    {
        return new PreInitSingeltonHandle(Declaration, Instance);
    }
}
