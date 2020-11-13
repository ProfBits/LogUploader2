using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogUploader.Data.Settings
{
    public interface IGeneralSettings
    {
        string ArcLogsPath { get; set; }
        string UserToken { get; set; }
        bool FirstBoot { get; set; }
        eLanguage Language { get; set; }
        bool EnableAutoParse { get; set; }
        bool EnableAutoUpload { get; set; }
        string WhatsNewShown { get; set; }
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
    public interface IWebHookSettings
    {
        string WebHookDBStr { get; set; }
        long CurrentWebHook { get; set; }
        eDiscordPostFormat DiscordPostFormat { get; set; }
        bool OnlyPostUploaded { get; }
        bool NameAsDiscordUser { get; }
    }
    public interface IEliteInsightsSettings
    {
        bool AutoUpdateEI { get; set; }
        bool CreateCombatReplay { get; set; }
        bool LightTheme { get; set; }
    }

    public interface IRaidOrgaPlusSettings
    {
        string RaitOrgaPlusUser { get; set; }
        string RaidOrgaPlusPassword { get; set; }
        bool RaidOrgaPlusAccoutSet { get; }
    }
}
