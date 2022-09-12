namespace LogUploader.Injection;

public interface ILoadedService
{
    string Name() => GetType().Name;

    Task Load(CancellationToken ct, IProgress<IProgressMessage>? progress = null);
}

//public interface IBackgroundService : IUnlaodedService
//{
//    Task Run(CancellationToken ct);
//}
//
//public interface IUnlaodedService
//{
//    Task Unload(IProgress<IProgressMessage>? progress = null);
//}
