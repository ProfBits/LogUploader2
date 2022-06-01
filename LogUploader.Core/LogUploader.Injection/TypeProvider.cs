namespace LogUploader.Injection;

internal interface ITypeProvider
{
    IEnumerable<Type> GetTypes();
}

internal class AssemblyTypeProvider : ITypeProvider
{
    private readonly IAssemblyProvider AssemblyProvider;

    public AssemblyTypeProvider(IAssemblyProvider assemblyProvider)
    {
        AssemblyProvider = assemblyProvider;
    }

    public IEnumerable<Type> GetTypes()
    {
        return AssemblyProvider.GetAssembies().SelectMany(assam => assam.GetTypes());
    }
}
