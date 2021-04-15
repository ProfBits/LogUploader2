using LogUploader.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogUploader.Tools.Discord
{
    public interface IDiscordPostGen
    {
        List<IWebHookData> Generate(IEnumerable<ICachedLog> logs, string userName, string avatarURL);
    }
}
