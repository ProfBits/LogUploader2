namespace LogUploader.Tools.Discord.DiscordPostGen
{
    public interface IAuthor
    {
        string IconURL { get; set; }
        string Name { get; set; }
        string URL { get; set; }

        string ToString();
    }
}