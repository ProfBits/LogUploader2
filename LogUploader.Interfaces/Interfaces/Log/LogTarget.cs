namespace LogUploader.Interfaces
{
    public interface LogTarget
    {
        int ID { get; }
        int MaxHealth { get; }
        int RemainingHealth { get; }
        int FirstAware { get; }
        int LastAware { get; }
    }
}

