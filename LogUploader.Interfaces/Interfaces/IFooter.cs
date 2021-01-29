namespace LogUploader.Tools.Discord.DiscordPostGen
{
    public interface IFooter
    {
        string IconURL { get; set; }
        string Text { get; set; }

        string ToString();
    }
}