namespace LogUploader.Data.Repositories
{
    public interface MultiAreaProvider<T>
    {
        T Get(int id);
        bool Exists(int number);
    }
}