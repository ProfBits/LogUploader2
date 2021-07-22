using LogUploader.Localisation;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace LogUploader.Tools.Settings
{
    public interface IGeneralSettings
    {
        string ArcLogsPath { get; set; }
        string UserToken { get; set; }
        bool FirstBoot { get; set; }
        string Language { get; set; }
        bool EnableAutoParse { get; set; }
        bool EnableAutoUpload { get; set; }
        string WhatsNewShown { get; set; }
        bool AllowPrerelases { get; set; }
    }
    public interface ICopyLinksSettings
    {
        bool EncounterName { get; set; }
        bool EncounterSuccess { get; set; }
        bool Inline { get; set; }
        bool EmptyLineBetween { get; set; }
        bool UseGnDiscordEmotes { get; set; }
    }

    public interface IProxySettings
    {
        bool UseProxy { get; set; }
        string ProxyAddress { get; set; }
        int ProxyPort { get; set; }
        string ProxyUsername { get; set; }
        string ProxyPassword { get; set; }
    }
}
