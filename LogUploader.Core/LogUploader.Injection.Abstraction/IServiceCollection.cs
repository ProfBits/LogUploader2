namespace LogUploader.Injection;

public interface IServiceCollection
{
    void Add<TDeclaration, TImplementation>() where TImplementation : TDeclaration;
    bool Contains<T>();
    IServiceProvider BuildProvider();
    void Add<T>(T instance);
}
