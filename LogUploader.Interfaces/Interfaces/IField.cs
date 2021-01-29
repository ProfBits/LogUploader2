namespace LogUploader.Tools.Discord.DiscordPostGen
{
    public interface IField
    {
        bool Inline { get; set; }
        string Name { get; set; }
        string Value { get; set; }

        bool ShouldSerializeInline();
        string ToString();
    }
}