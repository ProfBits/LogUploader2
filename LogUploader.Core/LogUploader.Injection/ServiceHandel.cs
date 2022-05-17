namespace LogUploader.Injection;

public interface IServiceHandel
{
    Type Declaration { get; }
    IServiceHandel[] Dependencies { get; }
    int Order { get; set; }
    bool IsLoadedService { get; }

    object CreateService();
    void Link(Dictionary<Type, IServiceHandel> allServices);
}

internal abstract class AbstractServiceHandel : IServiceHandel
{
    protected AbstractServiceHandel(Type declaration)
    {
        Declaration = declaration;
        IsLoadedService = declaration.IsAssignableTo(typeof(ILoadedService));
    }

    protected static System.Reflection.ConstructorInfo GetConstructorInfoForType(Type type)
    {
        var allConstructors = type.GetConstructors(
            System.Reflection.BindingFlags.Instance
            | System.Reflection.BindingFlags.Public
            | System.Reflection.BindingFlags.NonPublic
            );
        var relevantConstructors = allConstructors.Where(ci => ci.CustomAttributes.Any(attrib => attrib.AttributeType.Equals(typeof(InjectionConstructorAttribute)))).ToArray();

        if (relevantConstructors.Length == 1)
        {
            return relevantConstructors[0];
        }
        else if (relevantConstructors.Length <= 0)
        {
            throw new NoConstroctorMarkedForInjectionException(type);
        }
        else
        {
            throw new MultipleConstroctorMarkedForInjectionException(type);
        }

    }

    public Type Declaration { get; }
    public IServiceHandel[] Dependencies { get; private set; } = Array.Empty<IServiceHandel>();
    public int Order { get; set; }
    public bool IsLoadedService { get; }

    protected abstract Type[] GetRequiredTypes();
    public abstract object CreateService();

    public void Link(Dictionary<Type, IServiceHandel> allServices)
    {
        Type[] requiredTypes = GetRequiredTypes();
        var missingDependency = requiredTypes.FirstOrDefault(t => !allServices.ContainsKey(t));
        if (missingDependency is not null)
        {
            throw new DependencyNotFoundException(Declaration, missingDependency);
        }
        this.Dependencies = requiredTypes.Select(d => allServices[d]).ToArray();
    }
}

internal class TransientServiceHandel : AbstractServiceHandel
{
    private System.Reflection.ConstructorInfo ConstructorInfo;

    public TransientServiceHandel(Type declaration, Type implementation) : base(declaration)
    {
        if (IsLoadedService)
        {
            throw new ServiceTypeNotCopatipleWithLaodedServiceException(declaration);
        }

        ConstructorInfo = GetConstructorInfoForType(implementation);
    }

    public override object CreateService()
    {
        return ConstructorInfo.Invoke(Dependencies.Select(d => d.CreateService()).ToArray());
    }

    protected override Type[] GetRequiredTypes()
    {
        return ConstructorInfo.GetParameters().Select(pi => pi.ParameterType).ToArray();
    }
}


internal class SingeltonServiceHandel : AbstractServiceHandel
{
    private System.Reflection.ConstructorInfo ConstructorInfo;
    private object? instance = null;

    public SingeltonServiceHandel(Type declaration, Type implementation) : base(declaration)
    {
        ConstructorInfo = GetConstructorInfoForType(implementation);
    }

    public override object CreateService()
    {
        if (instance == null)
        {
            instance = ConstructorInfo.Invoke(Dependencies.Select(d => d.CreateService()).ToArray());
        }
        return instance;
    }

    protected override Type[] GetRequiredTypes()
    {
        return ConstructorInfo.GetParameters().Select(pi => pi.ParameterType).ToArray();
    }
}

internal class PreInitSingeltonHandle : AbstractServiceHandel
{
    private readonly object instance;

    public PreInitSingeltonHandle(Type declaration, object instance) : base(declaration)
    {
        this.instance = instance;
    }

    public override object CreateService()
    {
        return instance;
    }

    protected override Type[] GetRequiredTypes()
    {
        return Array.Empty<Type>();
    }
}

internal class ProviderHandle : AbstractServiceHandel
{
    public ProviderHandle() : base(typeof(IServiceProvider))
    {
    }

    public IServiceProvider? Insatnce { get; set; } = null;

    public override object CreateService()
    {
        return Insatnce ?? throw new InvalidOperationException("ProviderHandle init was not completed");
    }

    protected override Type[] GetRequiredTypes()
    {
        return Array.Empty<Type>();
    }
}