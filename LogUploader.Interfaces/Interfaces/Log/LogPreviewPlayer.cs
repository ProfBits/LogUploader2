using LogUploader.Data;

namespace LogUploader.Interfaces
{
    public interface LogPreviewPlayer
    {
        string Name { get; }
        IProfession Profession { get; }
        byte SubGroup { get; }
        int Dps { get; }
    }
}

