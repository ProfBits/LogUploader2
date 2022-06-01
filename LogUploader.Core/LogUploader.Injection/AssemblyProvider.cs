using System.Reflection;

namespace LogUploader.Injection;

internal interface IAssemblyProvider
{
    IEnumerable<Assembly> GetAssembies();
}

internal class TestAssemblieIgnoringProvider : IAssemblyProvider
{
    public IEnumerable<Assembly> GetAssembies()
    {
        return AppDomain.CurrentDomain.GetAssemblies()
            .Where(assam => !(assam.GetName().Name?.EndsWith(".Test") ?? false))
            .Where(assam => !(assam.GetName().Name?.StartsWith("System.") ?? false))
            .Where(assam => !(assam.GetName().Name?.StartsWith("Microsoft.") ?? false))
            ;
    }
}