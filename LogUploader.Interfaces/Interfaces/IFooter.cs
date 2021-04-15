namespace LogUploader.Tools.Discord
{
    public interface IFooter
    {
        string IconURL { get; set; }
        string Text { get; set; }

        string ToString();
    }
}