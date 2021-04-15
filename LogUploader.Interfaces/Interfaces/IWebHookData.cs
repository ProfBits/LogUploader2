using System.Collections.Generic;

namespace LogUploader.Tools.Discord
{
    public interface IWebHookData
    {
        string AvaturURL { get; set; }
        string Content { get; set; }
        List<IEmbed> Embeds { get; set; }
        string Username { get; set; }

        bool ShouldSerializeEmbeds();
        string ToString();
    }
}