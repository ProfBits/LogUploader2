using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogUploader.Data
{
    static class MiscData
    {
        private static string emoteRaidKill = "";
        private static string emoteRaidWipe = "";

        public static string EmoteRaidKill { get => emoteRaidKill; set => emoteRaidKill = $":{value.Trim(':')}:"; }
        public static string EmoteRaidWipe { get => emoteRaidWipe; set => emoteRaidWipe = $":{value.Trim(':')}:"; }
    }
}
