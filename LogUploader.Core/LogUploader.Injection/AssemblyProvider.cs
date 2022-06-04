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
            .Where(assam => !(assam.GetName().Name?.StartsWith("Mono.") ?? false))
            .Where(assam => !(assam.GetName().Name?.StartsWith("NuGet.") ?? false))
            .Where(assam => !(assam.GetName().Name?.StartsWith("Newtonsoft.Json") ?? false))
            .Where(assam => !(assam.GetName().Name?.StartsWith("nunit.") ?? false))
            .Where(assam => !(assam.GetName().Name?.StartsWith("NUnit3.") ?? false))
            .Where(assam => !(assam.GetName().Name?.Equals("testhost") ?? false))
            .Where(assam => !(assam.GetName().Name?.StartsWith("testcentric.") ?? false))
            .Where(assam => !(assam.GetName().Name?.Equals("DynamicProxyGenAssembly2") ?? false))
            .Where(assam => !(assam.GetName().Name?.Equals("Castle.Core") ?? false))
            .Where(assam => !(assam.GetName().Name?.Equals("Moq") ?? false))
            ;
    }
}