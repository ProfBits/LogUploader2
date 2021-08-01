namespace LogUploader.Data
{
    public interface AreaProvider<T> where T : New.GameArea
    {
        string Name { get; }
        string NameEN { get; }
        string NameDE { get; }
        string ShortName { get; }
        string ShortNameEN { get; }
        string ShortNameDE { get; }

        T Get();
    }
}
