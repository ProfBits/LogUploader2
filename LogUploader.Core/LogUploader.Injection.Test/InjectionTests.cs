namespace LogUploader.Injection.Test;

internal class InjectionTests
{
    [Test]
    public void ServiceCollectionShouldAddTransientTest()
    {
        IServiceCollection sc = new ServiceCollection();

        Assert.That(() => sc.Add<ITestTransient, TestTransient>(), Throws.Nothing);
    }

    [Test]
    public void ServiceCollectionCannotAddTwiceTest()
    {
        IServiceCollection sc = new ServiceCollection();

        sc.Add<ITestTransient, TestTransient>();
        Assert.That(() => sc.Add<ITestTransient, TestTransient>(), Throws.TypeOf<DuplicatedServiceException>()
            .And.Property(nameof(DuplicatedServiceException.AffectedService)).EqualTo(typeof(ITestTransient))
            .And.Message.Contains(typeof(ITestTransient).FullName));
    }

    [Test]
    public void ServiceCollectionContainsTrueTest()
    {
        IServiceCollection sc = new ServiceCollection();

        sc.Add<ITestTransient, TestTransient>();

        Assert.That(sc.Contains<ITestTransient>(), Is.True);
    }

    [Test]
    public void ServiceCollectionContainsFalseTest()
    {
        IServiceCollection sc = new ServiceCollection();

        sc.Add<ITestTransient, TestTransient>();

        Assert.That(sc.Contains<object>(), Is.False);
    }

    [Test]
    public void ServiceCollectionEmptyContainsTest()
    {
        IServiceCollection sc = new ServiceCollection();

        Assert.That(sc.Contains<ITestTransient>(), Is.False);
    }

    [Test]
    public void ServiceCollectionEmptyBuildProviderTest()
    {
        IServiceCollection sc = new ServiceCollection();

        IServiceProvider sp = sc.BuildProvider();
    }

    [Test]
    public void ServiceCollectionBuildProviderOneServiceTest()
    {
        IServiceCollection sc = new ServiceCollection();

        sc.Add<ITestTransient, TestTransient>();

        IServiceProvider sp = sc.BuildProvider();

        Assert.That(sp, Is.Not.Null);
    }

    [Test]
    public void ServiceProviderTransiantServiceTest()
    {
        IServiceCollection sc = new ServiceCollection();

        sc.Add<ITestTransient, TestTransient>();

        IServiceProvider sp = sc.BuildProvider();
        ITestTransient actual = sp.Create<ITestTransient>();

        Assert.That(actual, Is.Not.Null);
        Assert.That(actual, Is.InstanceOf<TestTransient>());
    }

    [Test]
    public void ServiceProviderTransiantServiceTwiceTest()
    {
        IServiceCollection sc = new ServiceCollection();

        sc.Add<ITestTransient, TestTransient>();

        IServiceProvider sp = sc.BuildProvider();
        ITestTransient actual1 = sp.Create<ITestTransient>();
        ITestTransient actual2 = sp.Create<ITestTransient>();

        Assert.That(actual2, Is.Not.SameAs(actual1));
    }

    [Test]
    public void ServiceProviderOneSingeltonTest()
    {
        IServiceCollection sc = new ServiceCollection();

        sc.Add<ITestSingelton, TestSingelton>();

        IServiceProvider sp = sc.BuildProvider();

        var actual = sp.Create<ITestSingelton>();

        Assert.That(actual, Is.Not.Null);
        Assert.That(actual, Is.TypeOf<TestSingelton>());
    }

    [Test]
    public void ServiceProviderSingeltonTwiceIsTheSameTest()
    {
        IServiceCollection sc = new ServiceCollection();

        sc.Add<ITestSingelton, TestSingelton>();

        IServiceProvider sp = sc.BuildProvider();

        var actual1 = sp.Create<ITestSingelton>();
        var actual2 = sp.Create<ITestSingelton>();

        Assert.That(actual2, Is.SameAs(actual1));
    }

    [Test]
    public void ServiceProviderCreateSingeltonWithDependecyTest()
    {
        IServiceCollection sc = new ServiceCollection();

        sc.Add<ITestSingelton, TestSingelton>();
        sc.Add<ITestSingeltenDependend, TestSingeltenDependend>();

        IServiceProvider sp = sc.BuildProvider();

        var actual = sp.Create<ITestSingeltenDependend>();

        Assert.That(actual, Is.Not.Null);
        Assert.That(actual.TestSingelton, Is.Not.Null);
        Assert.That(actual, Is.TypeOf<TestSingeltenDependend>());
        Assert.That(actual.TestSingelton, Is.TypeOf<TestSingelton>());
    }

    [Test]
    public void ServiceProviderCreateSingeltonWithDependecyTwiceTest()
    {
        IServiceCollection sc = new ServiceCollection();

        sc.Add<ITestSingelton, TestSingelton>();
        sc.Add<ITestSingeltenDependend, TestSingeltenDependend>();

        IServiceProvider sp = sc.BuildProvider();

        var actual1 = sp.Create<ITestSingeltenDependend>();
        var actual2 = sp.Create<ITestSingeltenDependend>();

        Assert.That(actual2, Is.SameAs(actual1));
    }

    [Test]
    public void ServiceProviderCreateTransientWithDependecyTest()
    {
        IServiceCollection sc = new ServiceCollection();

        sc.Add<ITestSingelton, TestSingelton>();
        sc.Add<ITestSingeltenDependend, TestSingeltenDependend>();
        sc.Add<ITestTransient, TestTransient>();
        sc.Add<ITestTransientDependend, TestTransientDependend>();

        IServiceProvider sp = sc.BuildProvider();

        var actual = sp.Create<ITestTransientDependend>();

        Assert.Multiple(() =>
        {
            Assert.That(actual, Is.Not.Null);
            Assert.That(actual.TestTransient, Is.Not.Null);
            Assert.That(actual.TestSingeltenDependend, Is.Not.Null);
            Assert.That(actual, Is.TypeOf<TestTransientDependend>());
            Assert.That(actual.TestTransient, Is.TypeOf<TestTransient>());
            Assert.That(actual.TestSingeltenDependend, Is.TypeOf<TestSingeltenDependend>());
        });
    }

    [Test]
    public void ServiceProviderCreateTransientWithDependecyTwiceTest()
    {
        IServiceCollection sc = new ServiceCollection();

        sc.Add<ITestSingelton, TestSingelton>();
        sc.Add<ITestSingeltenDependend, TestSingeltenDependend>();
        sc.Add<ITestTransient, TestTransient>();
        sc.Add<ITestTransientDependend, TestTransientDependend>();

        IServiceProvider sp = sc.BuildProvider();

        var actual1 = sp.Create<ITestTransientDependend>();
        var actual2 = sp.Create<ITestTransientDependend>();

        Assert.Multiple(() =>
        {
            Assert.That(actual2, Is.Not.SameAs(actual1));
            Assert.That(actual2.TestSingeltenDependend, Is.SameAs(actual1.TestSingeltenDependend));
            Assert.That(actual2.TestTransient, Is.Not.SameAs(actual1.TestTransient));
        });
    }

    [Test]
    public void ServiceProviderCyclicRefercneShouldBeDetectedTest()
    {
        IServiceCollection sc = new ServiceCollection();

        sc.Add<ITestCycleA1, TestCycleA1>();
        sc.Add<ITestCycleA2, TestCycleA2>();

        Assert.That(() => sc.BuildProvider(), Throws.TypeOf<CyclicServcieReferenceException>()
            .And.Property(nameof(CyclicServcieReferenceException.Services)).Contains(typeof(ITestCycleA1))
            .And.Property(nameof(CyclicServcieReferenceException.Services)).Contains(typeof(ITestCycleA2))
            .And.Message.Contains(typeof(ITestCycleA1).FullName)
            .And.Message.Contains(typeof(ITestCycleA2).FullName));
    }

    [Test]
    public void ServiceProviderSelfCyclicRefercneShouldBeDetectedTest()
    {
        IServiceCollection sc = new ServiceCollection();

        sc.Add<ITestCycleSelf, TestCycleSelf>();

        Assert.That(() => sc.BuildProvider(), Throws.TypeOf<CyclicServcieReferenceException>()
            .And.Property(nameof(CyclicServcieReferenceException.Services)).Contains(typeof(ITestCycleSelf))
            .And.Message.Contains(typeof(ITestCycleSelf).FullName));
    }

    [Test]
    public void BuildServiceProviderNoMarkedConstructorTest()
    {
        IServiceCollection sc = new ServiceCollection();

        sc.Add<ITestSingelton, TestSingeltenNoConstructor>();

        Assert.That(() => sc.BuildProvider(), Throws.TypeOf<NoConstroctorMarkedForInjectionException>()
            .And.Property(nameof(NoConstroctorMarkedForInjectionException.AffectedService)).EqualTo(typeof(TestSingeltenNoConstructor))
            .And.Message.Contains(typeof(TestSingeltenNoConstructor).FullName));
    }

    [Test]
    public void BuildServiceProviderMultipleMarkedConstructorTest()
    {
        IServiceCollection sc = new ServiceCollection();

        sc.Add<ITestTransient, TestTransientMultiConstructor>();

        Assert.That(() => sc.BuildProvider(), Throws.TypeOf<MultipleConstroctorMarkedForInjectionException>()
            .And.Property(nameof(MultipleConstroctorMarkedForInjectionException.AffectedService)).EqualTo(typeof(TestTransientMultiConstructor))
            .And.Message.Contains(typeof(TestTransientMultiConstructor).FullName));
    }

    [Test]
    public void ServiceCollectionAddNoServiceTest()
    {
        IServiceCollection sc = new ServiceCollection();

        Assert.That(() => sc.Add<ISomeClass, SomeClass>(), Throws.TypeOf<NotAServiceException>()
            .And.Property(nameof(NotAServiceException.AffectedService)).EqualTo(typeof(ISomeClass))
            .And.Message.Contains(typeof(ISomeClass).FullName));
    }

    [Test]
    public void ServiceCollectionAddUnkownServiceTypeTest()
    {
        IServiceCollection sc = new ServiceCollection();

        Assert.That(() => sc.Add<IUnkownService, UnkownService>(), Throws.TypeOf<UnkownServiceTypeException>()
            .And.Property(nameof(UnkownServiceTypeException.ServiceType)).EqualTo((ServiceType)(int.MaxValue / 2))
            .And.Property(nameof(UnkownServiceTypeException.AffectedService)).EqualTo(typeof(IUnkownService))
            .And.Message.Contains(((ServiceType)(int.MaxValue / 2)).ToString())
            .And.Message.Contains(typeof(IUnkownService).FullName));
    }

    [Test]
    public void ServiceCollectionAddPreInitSingeltonTest()
    {
        IServiceCollection sc = new ServiceCollection();
        var instance = new TestSingelton();

        Assert.That(() => sc.Add<ITestSingelton>(instance), Throws.Nothing);
        Assert.That(sc.Contains<ITestSingelton>(), Is.True);
    }

    [Test]
    public void ServiceCollectionCanCreateServiceProviderWithPreInitSingeltonTest()
    {
        IServiceCollection sc = new ServiceCollection();
        var instance = new TestSingelton();
        sc.Add<ITestSingelton>(instance);

        var sp = sc.BuildProvider();
        Assert.That(sp, Is.Not.Null);
    }

    [Test]
    public void ServiceProviderReturnsPreInitSingeltonInstanceTest()
    {
        IServiceCollection sc = new ServiceCollection();
        var instance = new TestSingelton();
        sc.Add<ITestSingelton>(instance);
        var sp = sc.BuildProvider();

        var actual = sp.Create<ITestSingelton>();
        Assert.That(actual, Is.SameAs(instance));
    }

    [Test]
    public void ServiceProviderReturnsPreInitSingeltonInstanceTwiceTest()
    {
        IServiceCollection sc = new ServiceCollection();
        var instance = new TestSingelton();
        sc.Add<ITestSingelton>(instance);
        sc.Contains<ITestSingelton>();
        var sp = sc.BuildProvider();
        var expected = sp.Create<ITestSingelton>();

        var actual = sp.Create<ITestSingelton>();
        Assert.That(actual, Is.SameAs(expected));
    }

    [Test]
    public void ServiceCollectionAddPreInitSingeltonNullInstanceTest()
    {
        IServiceCollection sc = new ServiceCollection();

#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
        Assert.That(() => sc.Add<ITestSingelton>(null), Throws.TypeOf<ServiceInstanceNullException>()
            .And.Property(nameof(ServiceInstanceNullException.AffectedService)).EqualTo(typeof(ITestSingelton))
            .And.Message.Contains(typeof(ITestSingelton).FullName));
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
    }


    [Test]
    public void ServiceCollectionCannotAddTransiantWithPreInitTestTest()
    {
        IServiceCollection sc = new ServiceCollection();

        Assert.That(() => sc.Add<ITestTransient>(new TestTransient()), Throws.TypeOf<ServiceTypeNotCopatipleWithPreInitException>()
            .And.Property(nameof(ServiceTypeNotCopatipleWithPreInitException.AffectedService)).EqualTo(typeof(ITestTransient))
            .And.Message.Contains(typeof(ITestTransient).FullName));
    }

    [Test]
    public void ServiceProviderReturnsLoadedServicesInOrderTest()
    {
        var sc = new ServiceCollection();
        sc.Add<ITestDependedLoadedService, TestDependedLoadedService>();
        sc.Add<ITestLoadedService, TestLoadedService>();
        sc.Add<ITestSingelton, TestSingelton>();

        var sp = sc.BuildProvider();

        var LoadedServices = sp.GetLoadedServices().ToArray();

        Assert.That(LoadedServices, Has.Length.EqualTo(2));
        Assert.That(LoadedServices[0], Is.TypeOf<TestLoadedService>());
        Assert.That(LoadedServices[1], Is.TypeOf<TestDependedLoadedService>());
    }

    [Test]
    public void LoadedTestShouldBeLateInitTestTest()
    {
        var sc = new ServiceCollection();
        sc.Add<ITestLoadedDependendServiceForLateInit, TestLoadedDependendServiceForLateInit>();
        sc.Add<ITestLoadedServiceForLateInit, TestLoadedServiceForLateInit>();

        var sp = sc.BuildProvider();
        Assert.Multiple(() =>
        {
            Assert.That(TestLoadedServiceForLateInit.ConstructorCalled, Is.False);
            Assert.That(TestLoadedDependendServiceForLateInit.ConstructorCalled, Is.False);
        });
        var loadedServices = sp.GetLoadedServices();
        var enumerator = loadedServices.GetEnumerator();
        Assert.That(enumerator.MoveNext(), Is.True);
        var first = enumerator.Current;

        Assert.Multiple(() =>
        {
            Assert.That(first, Is.Not.Null);
            Assert.That(first, Is.TypeOf<TestLoadedServiceForLateInit>());
            Assert.That(TestLoadedServiceForLateInit.ConstructorCalled, Is.True);
            Assert.That(TestLoadedDependendServiceForLateInit.ConstructorCalled, Is.False);
        });

        Assert.That(enumerator.MoveNext(), Is.True);
        var second = enumerator.Current;

        Assert.Multiple(() =>
        {
            Assert.That(second, Is.Not.Null);
            Assert.That(second, Is.TypeOf<TestLoadedDependendServiceForLateInit>());
            Assert.That(TestLoadedServiceForLateInit.ConstructorCalled, Is.True);
            Assert.That(TestLoadedDependendServiceForLateInit.ConstructorCalled, Is.True);
        });
    }

    [Test]
    public void TransiantServicesCannotBeLoadedTest()
    {
        var sc = new ServiceCollection();
        sc.Add<ILoadedTransient, LoadedTransient>();

        Assert.That(() => sc.BuildProvider(), Throws.TypeOf<ServiceTypeNotCopatipleWithLaodedServiceException>()
            .And.Property(nameof(ServiceTypeNotCopatipleWithLaodedServiceException.AffectedService)).EqualTo(typeof(ILoadedTransient))
            .And.Message.Contains(typeof(ILoadedTransient).FullName));
    }

    [Test]
    public void ServiceProviderCountTest()
    {
        var sc = new ServiceCollection();
        sc.Add<ITestSingelton, TestSingelton>();
        sc.Add<ITestTransient, TestTransient>();
        sc.Add<IServiceForCounting, ServiceForCounting>();

        var sp = sc.BuildProvider();

        var actual = sp.Count;

        Assert.That(actual, Is.EqualTo(4));
        Assert.That(ServiceForCounting.ConstructorCalled, Is.False);
    }

    [Test]
    public void GetLoadedCountDoesNotConstructAnyObjectTest()
    {
        var sc = new ServiceCollection();
        sc.Add<ILoadedServiceForCounting, LoadedServiceForCounting>();

        var sp = sc.BuildProvider();

        var actual = sp.CountOfLoadedServices;

        Assert.That(actual, Is.EqualTo(1));
        Assert.That(LoadedServiceForCounting.ConstructorCalled, Is.False);
    }

    [Test]
    public void ServiceProviderShouldRetrunItselfTest()
    {
        var sc = new ServiceCollection();
        var sp = sc.BuildProvider();

        var actual = sp.Create<IServiceProvider>();

        Assert.That(actual, Is.SameAs(sp));
    }

    [Test]
    public void ServiceCollectionCannotAddIServiceProverTest()
    {
        var sc = new ServiceCollection();

        Assert.That(() => sc.Add<IServiceProvider, ServiceProvider>(), Throws.TypeOf<DuplicatedServiceException>()
            .And.Property(nameof(DuplicatedServiceException.AffectedService)).EqualTo(typeof(IServiceProvider))
            .And.Message.Contains(typeof(IServiceProvider).FullName));
    }

    [Test]
    public void EmptyServiceCollectionContainsIServiceProverTest()
    {
        var sc = new ServiceCollection();

        var actual = sc.Contains<IServiceProvider>();

        Assert.That(actual, Is.True);
    }

    [Test]
    public void ProviderHandelShouldThrowWhenInitNotCompleted()
    {
        var ph = new ProviderHandle();

        Assert.That(() => ph.CreateService(), Throws.TypeOf<InvalidOperationException>());
    }

    [Test]
    public void ShouldThrowOnBuildProviderWithUnkownReference()
    {
        var sc = new ServiceCollection();
        sc.Add<ITestSingelton, NoneServiceConstructorParam>();

        Assert.That(() => sc.BuildProvider(), Throws.InstanceOf<DependencyNotFoundException>()
            .And.Property(nameof(DependencyNotFoundException.AffectedService)).EqualTo(typeof(ITestSingelton))
            .And.Property(nameof(DependencyNotFoundException.MissingDependency)).EqualTo(typeof(ISomeClass))
            .And.Message.Contains(typeof(ITestSingelton).FullName)
            .And.Message.Contains(typeof(ISomeClass).FullName));
    }
}

internal class TestTransient : ITestTransient
{
    [InjectionConstructor]
    internal TestTransient()
    {

    }
}

[Service(ServiceType.Transient)]
internal interface ITestTransient
{
}

internal class TestTransientMultiConstructor : ITestTransient
{
    [InjectionConstructor]
    private TestTransientMultiConstructor()
    {

    }

    [InjectionConstructor]
    private TestTransientMultiConstructor(ITestSingelton _)
    {

    }
}

internal class TestSingeltenNoConstructor : ITestSingelton
{   
}

internal class TestSingelton : ITestSingelton
{
    [InjectionConstructor]
    internal TestSingelton()
    {

    }
}

[Service(ServiceType.Singelton)]
internal interface ITestSingelton
{

}

internal class TestSingeltenDependend : ITestSingeltenDependend
{
    [InjectionConstructor]
    public TestSingeltenDependend(ITestSingelton testSingelton)
    {
        TestSingelton = testSingelton;
    }

    public ITestSingelton TestSingelton { get; }
}

[Service(ServiceType.Singelton)]
internal interface ITestSingeltenDependend
{
    ITestSingelton TestSingelton { get; }
}

internal class TestTransientDependend : ITestTransientDependend
{
    [InjectionConstructor]
    public TestTransientDependend(ITestSingeltenDependend testSingeltenDependend, ITestTransient testTransient)
    {
        TestSingeltenDependend = testSingeltenDependend;
        TestTransient = testTransient;
    }

    public ITestSingeltenDependend TestSingeltenDependend { get; }
    public ITestTransient TestTransient { get; }
}

[Service(ServiceType.Transient)]
internal interface ITestTransientDependend
{
    ITestSingeltenDependend TestSingeltenDependend { get; }
    ITestTransient TestTransient { get; }
}

internal class TestCycleSelf : ITestCycleSelf
{
    [InjectionConstructor]
    private TestCycleSelf(ITestCycleSelf _)
    {

    }
}

[Service(ServiceType.Singelton)]
internal interface ITestCycleSelf
{

}

internal class TestCycleA1 : ITestCycleA1
{
    [InjectionConstructor]
    private TestCycleA1(ITestCycleA2 _)
    {

    }
}

[Service(ServiceType.Singelton)]
internal interface ITestCycleA1
{
}

internal class TestCycleA2 : ITestCycleA2
{
    [InjectionConstructor]
    private TestCycleA2(ITestCycleA1 _)
    {

    }
}

[Service(ServiceType.Singelton)]
internal interface ITestCycleA2
{

}

internal class UnkownService : IUnkownService
{
    [InjectionConstructor]
    private UnkownService() { }
}

[Service((ServiceType)(int.MaxValue / 2))]
internal interface IUnkownService
{
    
}

internal class TestLoadedService : ITestLoadedService
{
    [InjectionConstructor]
    internal TestLoadedService() { }

    public async Task Load(CancellationToken ct, IProgress<IProgressMessage>? _ = null)
    {
        await Task.Delay(0);
    }
}

[Service(ServiceType.Singelton)]
internal interface ITestLoadedService : ILoadedService
{

}

internal class TestDependedLoadedService : ITestDependedLoadedService
{
    [InjectionConstructor]
    internal TestDependedLoadedService(ITestLoadedService a, ITestSingelton _)
    {
        TestLoadedService = a;
    }

    public ITestLoadedService TestLoadedService { get; }

    public async Task Load(CancellationToken ct, IProgress<IProgressMessage>? _ = null)
    {
        await Task.Delay(0);
    }
}

[Service(ServiceType.Singelton)]
internal interface ITestDependedLoadedService : ILoadedService
{

}

internal class TestLoadedServiceForLateInit : ITestLoadedServiceForLateInit
{
    [InjectionConstructor]
    private TestLoadedServiceForLateInit()
    {
        ConstructorCalled = true;
    }

    public static bool ConstructorCalled { get; private set; } = false;
    public static bool LoadCalled { get; private set; } = false;

    public async Task Load(CancellationToken ct, IProgress<IProgressMessage>? _ = null)
    {
        LoadCalled = true;
        await Task.Delay(0);
    }
}

[Service(ServiceType.Singelton)]
internal interface ITestLoadedServiceForLateInit : ILoadedService
{
}

internal class TestLoadedDependendServiceForLateInit : ITestLoadedDependendServiceForLateInit
{
    [InjectionConstructor]
    public TestLoadedDependendServiceForLateInit(ITestLoadedServiceForLateInit otherService)
    {
        OtherService = otherService;
        ConstructorCalled = true;
    }

    public ITestLoadedServiceForLateInit OtherService { get; }
    public static bool ConstructorCalled { get; private set; } = false;
    public static bool LoadCalled { get; private set; } = false;

    public async Task Load(CancellationToken ct, IProgress<IProgressMessage>? progress = null)
    {
        LoadCalled = true;
        await Task.Delay(0);
    }
}

[Service(ServiceType.Singelton)]
internal interface ITestLoadedDependendServiceForLateInit : ILoadedService
{
    ITestLoadedServiceForLateInit OtherService { get; }

}

internal class LoadedTransient : ILoadedTransient
{
    [InjectionConstructor]
    private LoadedTransient() { }

    public Task Load(CancellationToken ct, IProgress<IProgressMessage>? progress = null)
    {
        throw new InvalidOperationException();
    }
}

[Service(ServiceType.Transient)]
internal interface ILoadedTransient : ILoadedService
{

}

internal class LoadedServiceForCounting : ILoadedServiceForCounting
{
    [InjectionConstructor]
    private LoadedServiceForCounting()
    {
        ConstructorCalled = true;
    }

    public static bool ConstructorCalled { get; private set; } = false;

    public Task Load(CancellationToken ct, IProgress<IProgressMessage>? progress = null)
    {
        throw new InvalidOperationException();
    }
}

[Service(ServiceType.Singelton)]
internal interface ILoadedServiceForCounting : ILoadedService
{

}

internal class ServiceForCounting : IServiceForCounting
{
    [InjectionConstructor]
    private ServiceForCounting()
    {
        ConstructorCalled = true;
    }

    public static bool ConstructorCalled { get; private set; } = false;
}

[Service(ServiceType.Singelton)]
internal interface IServiceForCounting
{

}

internal class SomeClass : ISomeClass
{

}

internal interface ISomeClass
{

}

internal class NoneServiceConstructorParam : ITestSingelton
{
    [InjectionConstructor]
    public NoneServiceConstructorParam(ISomeClass _)
    {

    }
}