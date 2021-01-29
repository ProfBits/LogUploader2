namespace LogUploader.Tools.Discord
{
    public enum eDiscordPostFormat
    {
        [ObjectName("per Boss", "pro Boss")]
        PerBoss,
        [ObjectName("per Area", "pro Gebiet")]
        PerArea,
        [ObjectName("per Try Detaild", "pro Versuch ausführlich")]
        PerTryDetaild,
        [ObjectName("per Area with emotes", "pro Gebiet mit Emotes")]
        PerAreaEmotes,
        [ObjectName("per Area with classes", "pro Gebiet mit Klassen")]
        PerAreaClasses,
        [ObjectName("compact with emotes", "kompakt mit Emotes")]
        CompactWithEmotes,
        [ObjectName("compact with classes", "kompakt mit Klassen")]
        CompactWithClasses
    }
}
