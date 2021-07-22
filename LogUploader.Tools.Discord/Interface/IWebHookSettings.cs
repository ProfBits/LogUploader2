using LogUploader.Tools.Discord;


namespace LogUploader.Tools.Settings
{
    public interface IWebHookSettings
    {
        string WebHookDBStr { get; set; }
        long CurrentWebHook { get; set; }
        eDiscordPostFormat DiscordPostFormat { get; set; }
        bool OnlyPostUploaded { get; }
        bool NameAsDiscordUser { get; }
    }
}
