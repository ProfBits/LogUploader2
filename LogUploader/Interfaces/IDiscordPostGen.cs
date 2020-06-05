using LogUploader.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogUploader.Interfaces
{
    interface IDiscordPostGen
    {
        List<WebHookData> Generate(IEnumerable<CachedLog> logs, string userName, string avatarURL);
    }
}
