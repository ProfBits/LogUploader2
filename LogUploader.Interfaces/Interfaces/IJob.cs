namespace LogUploader.Interfaces
{
    public interface IJob<T>
    {
        T DoWork();
    }
    public interface INamedJob<T> : IJob<T>
    {
        int ID { get; }
        string Name { get; }
    }
}
