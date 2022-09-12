namespace LogUploader.Injection
{
    public interface IServiceRegistrator
    {
        public Task Load(IServiceCollection serviceCollection);
    }
}
