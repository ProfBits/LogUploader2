namespace LogUploader.Data
{
    public interface ISimmpleTarget
    {
        int FinalHealth { get; }
        int FirstAware { get; }
        int ID { get; }
        int LastAware { get; }
        int TotalHealth { get; }

        string ToString();
    }
}