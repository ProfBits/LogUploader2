namespace LogUploader.Tools.Discord
{
    public interface IAuthor
    {
        string IconURL { get; set; }
        string Name { get; set; }
        string URL { get; set; }

        string ToString();
    }
}