using LogUploader.Injection;

namespace LogUploader.IO;

public class IoRegistrator : IServiceRegistrator
{
    public async Task Load(IServiceCollection serviceCollection)
    {
        await Task.Run(() =>
        {
            serviceCollection.Add<IFileIO>(FileIOFactory.GetInstance());
        });
    }
}
