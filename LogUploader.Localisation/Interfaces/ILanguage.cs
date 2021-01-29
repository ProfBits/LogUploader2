using System.Globalization;

namespace LogUploader.Localisation
{
    public interface ILanguage
    {
        CultureInfo Culture { get; }

        string Succsess { get; }
        string Fail { get; }

        string ProxySettings { get; }
        string ProxyUse { get; }
        string ProxyHostename { get; }
        string ProxyPort { get; }
        string ProxyUser { get; }
        string ProxyPassword { get; }

        string ColHeaderBoss { get; }
        string ColHeaderDate { get; }
        string ColHeaderSize { get; }
        string ColHeaderDataCorrected { get; }
        string ColHeaderDuration { get; }
        string ColHeaderSuccess { get; }
        string ColHeaderHpLeft { get; }
        string ColHeaderCM { get; }
        string ColHeaderParsed { get; }
        string ColHeaderUploaded { get; }
        string FilterHeader { get; }
        string FilterBoss { get; }
        string FilterHPLeft { get; }
        string FilterDuration { get; }
        string FilterDate { get; }
        string FilterFrom { get; }
        string FilterTo { get; }
        string FilterSuccess { get; }
        string FilterKill { get; }
        string FilterWipe { get; }
        string FilterToday { get; }
        string FilterReset { get; }
        string DetailsHeader { get; }
        string DetailsBoss { get; }
        string DetailsDate { get; }
        string DetailsSize { get; }
        string DetailsCorrected { get; }
        string DetailsDuration { get; }
        string DetailsSuccess { get; }
        string DetailsHpLeft { get; }
        string DetailsCM { get; }
        string DetailsParsed { get; }
        string DetailsUploaded { get; }
        string DetailsOpenLocal { get; }
        string DetailsOpenRemote { get; }
        string DetailsAccName { get; }
        string DetailsSubGroup { get; }
        string DetailsDPS { get; }
        string ActionsHeader { get; }
        string ActionsParseLocal { get; }
        string ActionsOpenLocal { get; }
        string ActionsUpload { get; }
        string ActionsOpenRemote { get; }
        string ActionsParseAndUpload { get; }
        string ActionsCopyLinks { get; }
        string ActionsCopied { get; }
        string ActionsPostToDiscord { get; }
        string ActionsViewInExplorer { get; }
        string SettingsHeader { get; }
        string SettingsAutoParse { get; }
        string SettingsAutoUpload { get; }
        string SettingsAbout { get; }
        string SettingsSettings { get; }
        string FooterElements { get; }
        string FooterSelected { get; }
        string FooterShown { get; }
        string FooterProcessing { get; }
        string FooterParsing { get; }
        string FooterUploading { get; }


        string AboutTitle { get; }
        string AboutProjectPage { get; }
        string AboutViewLicense { get; }
        string AboutSpecialThanks { get; }
        string AboutBetaTesters { get; }
        string AboutCopyright { get; }
        string AboutView3rdParty { get; }
        string AboutForSpellCheck { get; }
        string AboutVersion { get; }
        string AboutViewPatchnotes { get; }

        string LicenseTitle { get; }
        string LicenseFor { get; }
        string LicenseBy { get; }

        string LicensesTitle { get; }
        string LicensesClose { get; }

        string SoftwareItemProject { get; }
        string SoftwareItemViewLicense { get; }

        string InitTitle { get; }
        string InitArcPaht { get; }
        string InitBrowse { get; }
        string InitLanguage { get; }
        string InitCancle { get; }
        string InitStart { get; }
        string InitCancelSetupTitel { get; }
        string InitCancelSetupText { get; }
        string InitInvalidPathTitel { get; }
        string InitInvalidPathText { get; }


        string ConfigTitle { get; }
        string ConfigGeneralTitle { get; }
        string ConfigGeneralArcPaht { get; }
        string ConfigGeneralBrowse { get; }
        string ConfigGeneralLanguage { get; }
        string ConfigGeneralAllowBeta { get; }
        string ConfigDpsReportTitle { get; }
        string ConfigDpsReportToken { get; }
        string ConfigDpsReportGetToken { get; }
        string ConfigDpsReportProxy { get; }
        string ConfigCopyTitle { get; }
        string ConfigCopyBoss { get; }
        string ConfigCopySuccess { get; }
        string ConfigCopyInline { get; }
        string ConfigCopySpace { get; }
        string ConfigCopyEmotes { get; }
        string ConfigDiscordTitle { get; }
        string ConfigDiscordWebHookName { get; }
        string ConfigDiscordWebHookLink { get; }
        string ConfigDiscordWebHookFormat { get; }
        string ConfigDiscordWebHookAvatar { get; }
        string ConfigDiscordWebHookDelete { get; }
        string ConfigDiscordCount { get; }
        string ConfigDiscordNoHooks { get; }
        string ConfigDiscordAdd { get; }
        string ConfigDiscordOnlyUploaded { get; }
        string ConfigDiscordNameAsUsername { get; }
        string ConfigEiTitle { get; }
        string ConfigEiCombatReplay { get; }
        string ConfigEiLightTheme { get; }
        string ConfigEiAutoUpdate { get; }
        string ConfigEiUpdate { get; }
        string ConfigDefault { get; }
        string ConfigCancel { get; }
        string ConfigSave { get; }
        string ConfigDefaultMsgTitel { get; }
        string ConfigDefaultMsgText { get; }
        string ConfigDiscardMsgTitel { get; }
        string ConfigDiscardMsgText { get; }
        string ConfigRoPlusTitle { get; }
        string ConfigRoPlusUser { get; }
        string ConfigRoPlusPwd { get; }
        string ConfigRoPlusNote { get; }
        string ConfigExport { get; }
        string ConfigImport { get; }
        string ConfigExportFileFilter { get; }
        string ConfigExportSaveTitle { get; }
        string ConfigImportOpenTitle { get; }
        string ConfigExportPwdPromptTitle { get; }
        string ConfigExportPwdPromptNewText { get; }
        string ConfigExportPwdPromptText { get; }
        string ConfigExportPwdFailTitle { get; }
        string ConfigExportPwdFailText { get; }
        string ConfigExportMessageTitle { get; }
        string ConfigExportMessageFail { get; }
        string ConfigExportMessageSucc { get; }
        string ConfigImportMessageTitle { get; }
        string ConfigImportMessageFail { get; }
        string ConfigImportMessageSucc { get; }

        string NewTitle { get; }
        string NewHeading { get; }
        string NewClose { get; }


        string PlayerToCorrectPlayer { get; }
        string PlayerToCorrectAccount { get; }
        string PlayerToCorrectMember { get; }
        string PlayerToCorrectHelper { get; }
        string PlayerToCorrectLFG { get; }

        string CorrectPlayer { get; }
        string CorrectPlayerTitle { get; }
        string CorrectPlayerDone { get; }


        string MiscGenericProcessing { get; }
        string MiscNoWebhookMsgTitel { get; }
        string MiscNoWebhookMsgText { get; }
        string MiscDiscordPostGenDuration { get; }
        string MiscDiscordPostGenHpLeft { get; }
        string MiscDiscordPostGenGroupDPS { get; }
        string MiscDiscordPostGenTopDPS { get; }
        string MiscDiscordPostGenNoLink { get; }
        string MiscDiscordPostErrTitle { get; }
        string MiscDiscordPostErrMsg { get; }
        string MiscDetailsMultibleBosses { get; }
        string MiscRaidOrgaPlusNoAccount { get; }
        string MiscRaidOrgaPlusLoginErr { get; }
        string MiscRaidOrgaPlusNoRaid { get; }
    }
}
