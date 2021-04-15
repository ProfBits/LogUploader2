namespace LogUploader.Tools.Discord
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