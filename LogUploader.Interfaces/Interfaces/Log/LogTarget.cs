namespace LogUploader.Interfaces.Interfaces
{
    interface LogTarget
    {
        string Name { get; }
        byte SubGroup { get; }
        int Dps { get; }
    }
}

