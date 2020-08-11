using LogUploader.Helper;
using LogUploader.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogUploader.Data.Settings
{
    class SettingsData : IGeneralSettings, ICopyLinksSettings, IProxySettings, IWebHookSettings, IEliteInsightsSettings, IRaidOrgaPlusSettings
    {
        #region General Settings

        public string ArcLogsPath { get; set; }
        public string UserToken { get; set; }
        public bool FirstBoot { get; set; }
        public eLanguage Language { get; set; }
        public bool EnableAutoParse { get; set; }
        public bool EnableAutoUpload { get; set; }
        public string WhatsNewShown { get; set; }

        #endregion
        #region CopyLink

        public bool EncounterName { get; set; }
        public bool EncounterSuccess { get; set; }
        public bool Inline { get; set; }
        public bool EmptyLineBetween { get; set; }
        public bool UseGnDiscordEmotes { get; set; }

        #endregion
        #region Proxy

        public bool UseProxy { get; set; }
        public string ProxyAddress { get; set; }
        public int ProxyPort { get; set; }
        public string ProxyUsername { get; set; }
        public string ProxyPassword { get; set; }

        #endregion
        #region WebHook

        public string WebHookDBStr { get => WebHookDB.GetSaveString(); set => WebHookDB = new WebHookDB(value); }
        public long CurrentWebHook { get; set; }
        public eDiscordPostFormat DiscordPostFormat { get; set; }
        public WebHookDB WebHookDB { get; set; }
        public bool OnlyPostUploaded { get; set; }
        public bool NameAsDiscordUser { get; set; }

        #endregion
        #region Ei

        public bool AutoUpdateEI { get; set; }
        public bool CreateCombatReplay { get; set; }
        public bool LightTheme { get; set; }

        #endregion
        #region RO+

        public string RaitOrgaPlusUser { get; set; }
        public string RaidOrgaPlusPassword { get; set; }
        public bool RaidOrgaPlusAccoutSet { get => !string.IsNullOrWhiteSpace(RaitOrgaPlusUser) || !string.IsNullOrWhiteSpace(RaidOrgaPlusPassword); }

        #endregion

        public SettingsData(Properties.Settings settings)
        {
            SetGeneral(settings.ArcLogsPath, settings.UserToken, settings.FirstBoot, settings.Language, settings.EnableAutoParse, settings.EnableAutoUpload, settings.WhatsNewShown);
            SetCopyLinks(settings.EncounterName, settings.EncounterSuccsess, settings.inline, settings.EmptyLineBetween, settings.UseGnDiscordEmotes);
            SetProxy(settings.UseProxy, settings.ProxyAddress, settings.ProxyPort, settings.ProxyUsername, settings.ProxyPassword);
            SetWebHook(settings.WebHookDB, settings.CurrentWebHook, settings.DiscordPostFormat, settings.OnlyPostUploaded, settings.NameAsDiscordUser);
            SetEliteInsights(settings.AutoUpdateEI, settings.EiCreateCombatReplay, settings.EiUseLightTheme);
            SetRaidOrgaPlus(settings.RopUser, settings.RopPwd);
        }

        #region SetPerInterface

        private void SetGeneral(string arcLogsPath, string userToken, bool firstBoot, string language, bool eap, bool eau, string whatsNewShown)
        {
            ArcLogsPath = arcLogsPath;
            UserToken = SettingsHelper.UnprotectString(userToken);
            FirstBoot = firstBoot;
            EnableAutoParse = eap;
            EnableAutoUpload = eau;
            WhatsNewShown = whatsNewShown;
            try
            {
                Language = (eLanguage)Enum.Parse(typeof(eLanguage), language);
            }
            catch (Exception e) when (e is ArgumentException || e is InvalidCastException)
            {
                Language = eLanguage.EN;
            }
        }

        private void SetCopyLinks(bool encounterName, bool encounterSuccess, bool inline, bool emptyLineBetween, bool useGnDiscordEmotes)
        {
            EncounterName = encounterName;
            EncounterSuccess = encounterSuccess;
            Inline = inline;
            EmptyLineBetween = emptyLineBetween;
            UseGnDiscordEmotes = useGnDiscordEmotes;
        }

        private void SetProxy(bool useProxy, string proxyAddress, int proxyPort, string proxyUsername, string proxyPassword)
        {
            UseProxy = useProxy;
            ProxyAddress = proxyAddress;
            ProxyPort = proxyPort;
            ProxyUsername = proxyUsername;
            ProxyPassword = SettingsHelper.UnprotectString(proxyPassword);
        }

        private void SetWebHook(string webHookDB, long currentWebHook, string discordPostFormat, bool onlyPostUploaded, bool nameAsDiscordUser)
        {
            WebHookDBStr = webHookDB;
            CurrentWebHook = currentWebHook;
            try
            {
                DiscordPostFormat = (eDiscordPostFormat)Enum.Parse(typeof(eDiscordPostFormat), discordPostFormat);
            }
            catch (Exception e) when (e is ArgumentException || e is InvalidCastException)
            {
                DiscordPostFormat = eDiscordPostFormat.PerArea;
            }
            OnlyPostUploaded = onlyPostUploaded;
            NameAsDiscordUser = nameAsDiscordUser;
        }

        private void SetEliteInsights(bool autoUpdateEI, bool createCombatReplay, bool lightTheme)
        {
            AutoUpdateEI = autoUpdateEI;
            CreateCombatReplay = createCombatReplay;
            LightTheme = lightTheme;
        }

        private void SetRaidOrgaPlus(string ropUser, string ropPwd)
        {
            RaitOrgaPlusUser = ropUser;
            RaidOrgaPlusPassword = SettingsHelper.UnprotectString(ropPwd);
        }


        #endregion

        public Properties.Settings ApplyTo(Properties.Settings settings)
        {
            ApplyGeneral(settings);
            ApplyCopyLinks(settings);
            ApplyProxy(settings);
            ApplyWebHook(settings);
            ApplyEliteInsights(settings);
            ApplyRaidOrgaPlus(settings);
            return settings;
        }

        #region ApplyPerInterface

        private void ApplyGeneral(Properties.Settings settings)
        {
            settings.ArcLogsPath = ArcLogsPath;
            settings.UserToken = SettingsHelper.ProtectString(UserToken);
            settings.FirstBoot = FirstBoot;
            settings.Language = Language.ToString();
            settings.EnableAutoParse = EnableAutoParse;
            settings.EnableAutoUpload = EnableAutoUpload;
            settings.WhatsNewShown = WhatsNewShown;
        }

        private void ApplyCopyLinks(Properties.Settings settings)
        {
            settings.EncounterName = EncounterName;
            settings.EncounterSuccsess = EncounterSuccess;
            settings.inline = Inline;
            settings.EmptyLineBetween = EmptyLineBetween;
            settings.UseGnDiscordEmotes = UseGnDiscordEmotes;
        }

        private void ApplyProxy(Properties.Settings settings)
        {
            settings.UseProxy = UseProxy;
            settings.ProxyAddress = ProxyAddress;
            settings.ProxyPort = ProxyPort;
            settings.ProxyUsername = ProxyUsername;
            settings.ProxyPassword = SettingsHelper.ProtectString(ProxyPassword);
        }

        private void ApplyWebHook(Properties.Settings settings)
        {
            settings.WebHookDB = WebHookDBStr;
            settings.CurrentWebHook = CurrentWebHook;
            settings.DiscordPostFormat = DiscordPostFormat.ToString();
            settings.OnlyPostUploaded = OnlyPostUploaded;
            settings.NameAsDiscordUser = NameAsDiscordUser;
        }

        private void ApplyEliteInsights(Properties.Settings settings)
        {
            settings.AutoUpdateEI = AutoUpdateEI;
            settings.EiCreateCombatReplay = CreateCombatReplay;
            settings.EiUseLightTheme = LightTheme;
        }

        private void ApplyRaidOrgaPlus(Properties.Settings settings)
        {
            settings.RopUser = RaitOrgaPlusUser;
            settings.RopPwd = SettingsHelper.ProtectString(RaidOrgaPlusPassword);
        }

        #endregion

        #region Equals/GetHashCodoe

        public override bool Equals(object obj)
        {
            return obj is SettingsData data &&
                   ArcLogsPath == data.ArcLogsPath &&
                   UserToken == data.UserToken &&
                   FirstBoot == data.FirstBoot &&
                   Language == data.Language &&
                   EncounterName == data.EncounterName &&
                   EncounterSuccess == data.EncounterSuccess &&
                   Inline == data.Inline &&
                   EmptyLineBetween == data.EmptyLineBetween &&
                   UseGnDiscordEmotes == data.UseGnDiscordEmotes &&
                   UseProxy == data.UseProxy &&
                   ProxyAddress == data.ProxyAddress &&
                   ProxyPort == data.ProxyPort &&
                   ProxyUsername == data.ProxyUsername &&
                   ProxyPassword == data.ProxyPassword &&
                   WebHookDB.ToString() == data.WebHookDB.ToString() &&
                   CurrentWebHook == data.CurrentWebHook &&
                   DiscordPostFormat == data.DiscordPostFormat &&
                   OnlyPostUploaded == data.OnlyPostUploaded &&
                   NameAsDiscordUser == data.NameAsDiscordUser &&
                   AutoUpdateEI == data.AutoUpdateEI &&
                   CreateCombatReplay == data.CreateCombatReplay &&
                   LightTheme == data.LightTheme;
        }

        public override int GetHashCode()
        {
            var hashCode = 1707943953;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(ArcLogsPath);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(UserToken);
            hashCode = hashCode * -1521134295 + FirstBoot.GetHashCode();
            hashCode = hashCode * -1521134295 + Language.GetHashCode();
            hashCode = hashCode * -1521134295 + EncounterName.GetHashCode();
            hashCode = hashCode * -1521134295 + EncounterSuccess.GetHashCode();
            hashCode = hashCode * -1521134295 + Inline.GetHashCode();
            hashCode = hashCode * -1521134295 + EmptyLineBetween.GetHashCode();
            hashCode = hashCode * -1521134295 + UseGnDiscordEmotes.GetHashCode();
            hashCode = hashCode * -1521134295 + UseProxy.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(ProxyAddress);
            hashCode = hashCode * -1521134295 + ProxyPort.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(ProxyUsername);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(ProxyPassword);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(WebHookDB.ToString());
            hashCode = hashCode * -1521134295 + CurrentWebHook.GetHashCode();
            hashCode = hashCode * -1521134295 + DiscordPostFormat.GetHashCode();
            hashCode = hashCode * -1521134295 + OnlyPostUploaded.GetHashCode();
            hashCode = hashCode * -1521134295 + NameAsDiscordUser.GetHashCode();
            hashCode = hashCode * -1521134295 + AutoUpdateEI.GetHashCode();
            hashCode = hashCode * -1521134295 + CreateCombatReplay.GetHashCode();
            hashCode = hashCode * -1521134295 + LightTheme.GetHashCode();
            return hashCode;
        }

        public static bool operator ==(SettingsData left, SettingsData right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(SettingsData left, SettingsData right)
        {
            return !(left == right);
        }


        #endregion
    }
}
