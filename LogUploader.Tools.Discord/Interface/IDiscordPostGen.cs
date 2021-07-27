using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LogUploader.Data;
using LogUploader.Tools.Discord.Data;

namespace LogUploader.Tools.Discord
{
    public interface IDiscordPostGen
    {
        List<WebHookData> Generate(IEnumerable<ICachedLog> logs, string userName, string avatarURL);
    }
}
