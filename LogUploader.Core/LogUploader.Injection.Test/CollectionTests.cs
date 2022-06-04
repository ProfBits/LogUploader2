using LogUploader.Logging;

using Moq;

namespace LogUploader.Injection.Test;

internal class CollectionTests
{
    [Test]
    public void ServiceCollectorCreateShouldNotThrow()
    {
        Assert.That(() => ServiceCollector.Create(new Mock<ILogger>().Object), Throws.Nothing);
    }

    [Test]
    public void ServiceCollectorCreateShouldNotReturnNull()
    {   
        Assert.That(ServiceCollector.Create(new Mock<ILogger>().Object), Is.Not.Null);
    }

    [Test]
    public void ServiceCollectorCollectoShouldNotThrow()
    {
        var collector = ServiceCollector.Create(new Mock<ILogger>().Object);
        var sc = new ServiceCollection();

        Assert.That(async () => await collector.CollectServicesAsync(sc), Throws.Nothing);
    }

    [Test]
    public void ServiceCollectorCollectoShouldThrowIfCollectionIsNull()
    {
        var collector = ServiceCollector.Create(new Mock<ILogger>().Object);

#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
        Assert.That(async () => await collector.CollectServicesAsync(null), Throws.ArgumentNullException);
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
    }

    [Test]
    public async Task FilledServiceCollectionShouldBeBuildeable()
    {
        var collector = ServiceCollector.Create(new Mock<ILogger>().Object);
        IServiceCollection sc = new ServiceCollection();

        sc = await collector.CollectServicesAsync(sc);

        Assert.That(sc.BuildProvider, Throws.Nothing);
    }

    [Test]
    public async Task FilledServiceCollectionShouldNotContainAnyTestCalsses()
    {
        var collector = ServiceCollector.Create(new Mock<ILogger>().Object);
        IServiceCollection sc = new ServiceCollection();

        sc = await collector.CollectServicesAsync(sc);

        Assert.That(sc.Contains<ISericeInTest>, Is.False);
    }

    [Test]
    public async Task FilledServiceCollectionShouldFindImplementations()
    {
        var typeProvider = new Mock<ITypeProvider>();
        typeProvider.Setup(tp => tp.GetTypes()).Returns(new[] { typeof(ServiceInTest), typeof(ISericeInTest) });

        var collector = new ServiceCollector(typeProvider.Object, new Mock<ILogger>().Object);
        IServiceCollection sc = new ServiceCollection();

        sc = await collector.CollectServicesAsync(sc);

        Assert.That(sc.Contains<ISericeInTest>, Is.True);
    }

    [Test]
    public async Task FilledServiceCollectionShouldWarnIfMultipleImplementations()
    {
        //TODO loggin assert

        var typeProvider = new Mock<ITypeProvider>();
        typeProvider.Setup(tp => tp.GetTypes()).Returns(new[] { typeof(ServiceInTest), typeof(ServiceInTest2), typeof(ISericeInTest) });

        var collector = new ServiceCollector(typeProvider.Object, new Mock<ILogger>().Object);
        IServiceCollection sc = new ServiceCollection();

        sc = await collector.CollectServicesAsync(sc);

        Assert.That(sc.Contains<ISericeInTest>, Is.True);
    }

    [Test]
    public async Task FilledServiceCollectionShouldWarnIfNoImplementation()
    {
        //TODO loggin assert

        var typeProvider = new Mock<ITypeProvider>();
        typeProvider.Setup(tp => tp.GetTypes()).Returns(new[] { typeof(ISericeInTest) });

        var collector = new ServiceCollector(typeProvider.Object, new Mock<ILogger>().Object);
        IServiceCollection sc = new ServiceCollection();

        sc = await collector.CollectServicesAsync(sc);

        Assert.That(sc.Contains<ISericeInTest>, Is.False);
    }
}

[Service(ServiceType.Singelton)]
internal interface ISericeInTest
{

}

internal class ServiceInTest : ISericeInTest
{

}

internal class ServiceInTest2 : ISericeInTest
{

}
