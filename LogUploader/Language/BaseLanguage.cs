using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogUploader.Languages
{
    public abstract class BaseLanguage : ILanguage
    {
        public abstract CultureInfo Culture { get; }
        public abstract string Succsess { get; }
        public abstract string Fail { get; }
        public abstract string ProxySettings { get; }
        public abstract string ProxyUse { get; }
        public abstract string ProxyHostename { get; }
        public abstract string ProxyPort { get; }
        public abstract string ProxyUser { get; }
        public abstract string ProxyPassword { get; }
        public abstract string ColHeaderBoss { get; }
        public abstract string ColHeaderDataCorrected { get; }
        public abstract string ColHeaderHpLeft { get; }
        public abstract string ColHeaderCM { get; }
        public abstract string ColHeaderParsed { get; }
        public abstract string FilterHeader { get; }
        public abstract string FilterBoss { get; }
        public abstract string FilterHPLeft { get; }
        public abstract string FilterDuration { get; }
        public abstract string FilterDate { get; }
        public abstract string FilterFrom { get; }
        public abstract string FilterTo { get; }
        public abstract string FilterSuccess { get; }
        public abstract string FilterKill { get; }
        public abstract string FilterWipe { get; }
        public abstract string FilterToday { get; }
        public abstract string FilterReset { get; }
        public abstract string DetailsHeader { get; }
        public abstract string DetailsBoss { get; }
        public abstract string DetailsDate { get; }
        public abstract string DetailsSize { get; }
        public abstract string DetailsCorrected { get; }
        public abstract string DetailsDuration { get; }
        public abstract string DetailsSuccess { get; }
        public abstract string DetailsHpLeft { get; }
        public abstract string DetailsCM { get; }
        public abstract string DetailsParsed { get; }
        public abstract string DetailsUploaded { get; }
        public abstract string DetailsOpenLocal { get; }
        public abstract string DetailsOpenRemote { get; }
        public abstract string DetailsAccName { get; }
        public abstract string DetailsSubGroup { get; }
        public abstract string DetailsDPS { get; }
        public abstract string ActionsHeader { get; }
        public abstract string ActionsParseLocal { get; }
        public abstract string ActionsOpenLocal { get; }
        public abstract string ActionsUpload { get; }
        public abstract string ActionsOpenRemote { get; }
        public abstract string ActionsParseAndUpload { get; }
        public abstract string ActionsCopyLinks { get; }
        public abstract string ActionsCopied { get; }
        public abstract string ActionsPostToDiscord { get; }
        public abstract string SettingsHeader { get; }
        public abstract string SettingsAutoParse { get; }
        public abstract string SettingsAutoUpload { get; }
        public abstract string SettingsAbout { get; }
        public abstract string SettingsSettings { get; }
        public abstract string FooterElements { get; }
        public abstract string FooterSelected { get; }
        public abstract string FooterShown { get; }
        public abstract string FooterProcessing { get; }
        public abstract string FooterParsing { get; }
        public abstract string FooterUploading { get; }
        public abstract string ColHeaderDate { get; }
        public abstract string ColHeaderSize { get; }
        public abstract string ColHeaderDuration { get; }
        public abstract string ColHeaderSuccess { get; }
        public abstract string ColHeaderUploaded { get; }
        public abstract string AboutTitle { get; }
        public abstract string AboutProjectPage { get; }
        public abstract string AboutViewLicense { get; }
        public abstract string AboutSpecialThanks { get; }
        public abstract string AboutBetaTesters { get; }
        public abstract string AboutCopyright { get; }
        public abstract string AboutView3rdParty { get; }
        public abstract string AboutForSpellCheck { get; }
        public abstract string LicenseTitle { get; }
        public abstract string LicenseFor { get; }
        public abstract string LicenseBy { get; }
        public abstract string LicensesTitle { get; }
        public abstract string LicensesClose { get; }
        public abstract string SoftwareItemProject { get; }
        public abstract string SoftwareItemViewLicense { get; }
        public abstract string InitTitle { get; }
        public abstract string InitArcPaht { get; }
        public abstract string InitBrowse { get; }
        public abstract string InitLanguage { get; }
        public abstract string InitCancle { get; }
        public abstract string InitStart { get; }
        public abstract string InitCancelSetupTitel { get; }
        public abstract string InitCancelSetupText { get; }
        public abstract string InitInvalidPathTitel { get; }
        public abstract string InitInvalidPathText { get; }
        public abstract string ConfigTitle { get; }
        public abstract string ConfigGeneralTitle { get; }
        public abstract string ConfigGeneralArcPaht { get; }
        public abstract string ConfigGeneralBrowse { get; }
        public abstract string ConfigGeneralLanguage { get; }
        public abstract string ConfigDpsReportTitle { get; }
        public abstract string ConfigDpsReportToken { get; }
        public abstract string ConfigDpsReportGetToken { get; }
        public abstract string ConfigDpsReportProxy { get; }
        public abstract string ConfigCopyTitle { get; }
        public abstract string ConfigCopyBoss { get; }
        public abstract string ConfigCopySuccess { get; }
        public abstract string ConfigCopyInline { get; }
        public abstract string ConfigCopySpace { get; }
        public abstract string ConfigCopyEmotes { get; }
        public abstract string ConfigDiscordTitle { get; }
        public abstract string ConfigDiscordWebHookName { get; }
        public abstract string ConfigDiscordWebHookLink { get; }
        public abstract string ConfigDiscordWebHookFormat { get; }
        public abstract string ConfigDiscordWebHookAvatar { get; }
        public abstract string ConfigDiscordWebHookDelete { get; }
        public abstract string ConfigDiscordNoHooks { get; }
        public abstract string ConfigDiscordCount { get; }
        public abstract string ConfigDiscordAdd { get; }
        public abstract string ConfigDiscordOnlyUploaded { get; }
        public abstract string ConfigDiscordNameAsUsername { get; }
        public abstract string ConfigEiTitle { get; }
        public abstract string ConfigEiCombatReplay { get; }
        public abstract string ConfigEiLightTheme { get; }
        public abstract string ConfigDefault { get; }
        public abstract string ConfigCancel { get; }
        public abstract string ConfigSave { get; }
        public abstract string ConfigDefaultMsgTitel { get; }
        public abstract string ConfigDefaultMsgText { get; }
        public abstract string ConfigDiscardMsgTitel { get; }
        public abstract string ConfigDiscardMsgText { get; }

        public abstract string MiscGenericProcessing { get; }
        public abstract string MiscNoWebhookMsgTitel { get; }
        public abstract string MiscNoWebhookMsgText { get; }
        public abstract string MiscDiscordPostGenDuration { get; }
        public abstract string MiscDiscordPostGenHpLeft { get; }
        public abstract string MiscDiscordPostGenGroupDPS { get; }
        public abstract string MiscDiscordPostGenTopDPS { get; }
        public abstract string MiscDetailsMultibleBosses { get; }

        public string SuccsessFail(bool succsess)
        {
            if (succsess)
                return Succsess;
            return Fail;
        }
    }
}
