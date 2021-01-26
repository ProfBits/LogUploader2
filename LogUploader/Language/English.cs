using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogUploader.Languages
{
    public class English : ILanguage
    {
        public CultureInfo Culture { get => new CultureInfo("en-us"); }

        public string Succsess { get => "Success"; }
        public string Fail { get => "Fail"; }

        public string ColHeaderBoss { get => "Boss"; }
        public string ColHeaderDate { get => "Date"; }
        public string ColHeaderSize { get => "Size"; }
        public string ColHeaderDataCorrected { get => "Data checked"; }
        public string ColHeaderDuration { get => "Duration"; }
        public string ColHeaderSuccess { get => "Kill"; }
        public string ColHeaderHpLeft { get => "% left"; }
        public string ColHeaderCM { get => "CM"; }
        public string ColHeaderParsed { get => "Parsed"; }
        public string ColHeaderUploaded { get => "Uploaded"; }
        public string FilterHeader { get => "Filter"; }
        public string FilterBoss { get => "Boss"; }
        public string FilterHPLeft { get => "HP left"; }
        public string FilterDuration { get => "Duration"; }
        public string FilterDate { get => "Date"; }
        public string FilterFrom { get => "from"; }
        public string FilterTo { get => "to"; }
        public string FilterSuccess { get => "Success"; }
        public string FilterKill { get => "Kill"; }
        public string FilterWipe { get => "Wipe"; }
        public string FilterToday { get => "Today"; }
        public string FilterReset { get => "Reset"; }
        public string DetailsHeader { get => "Details"; }
        public string DetailsBoss { get => "Boss:"; }
        public string DetailsDate { get => "Date:"; }
        public string DetailsSize { get => "Size:"; }
        public string DetailsCorrected { get => "Corrected:"; }
        public string DetailsDuration { get => "Duration:"; }
        public string DetailsSuccess { get => "Success:"; }
        public string DetailsHpLeft { get => "HP left:"; }
        public string DetailsCM { get => "CM:"; }
        public string DetailsParsed { get => "Parsed:"; }
        public string DetailsUploaded { get => "Uploaded:"; }
        public string DetailsOpenLocal { get => "open"; }
        public string DetailsOpenRemote { get => "open"; }
        public string DetailsAccName { get => "Account"; }
        public string DetailsSubGroup { get => "SG"; }
        public string DetailsDPS { get => "DPS"; }
        public string ActionsHeader { get => "Actions"; }
        public string ActionsParseLocal { get => "Parse local"; }
        public string ActionsOpenLocal { get => "Open local"; }
        public string ActionsUpload { get => "Upload"; }
        public string ActionsOpenRemote { get => "dps.report"; }
        public string ActionsParseAndUpload { get => "Parse local and upload"; }
        public string ActionsCopyLinks { get => "Copy links"; }
        public string ActionsCopied { get => "copied"; }
        public string ActionsPostToDiscord { get => "Post to Discord"; }
        public string ActionsViewInExplorer { get => "View in Explorer"; }
        public string SettingsHeader { get => "Settings"; }
        public string SettingsAutoParse { get => "Auto parse new logs"; }
        public string SettingsAutoUpload { get => "Auto upload new logs"; }
        public string SettingsAbout { get => "About"; }
        public string SettingsSettings { get => "Settings ..."; }
        public string FooterElements { get => "Elements"; }
        public string FooterSelected { get => "Selected"; }
        public string FooterShown { get => "Shown"; }
        public string FooterProcessing { get => "Processing"; }
        public string FooterParsing { get => "Parsing"; }
        public string FooterUploading { get => "Uploading"; }

        public string ProxySettings { get => "Proxy Settings"; }
        public string ProxyUse { get => "Use Proxy"; }
        public string ProxyHostename { get => "Hostname"; }
        public string ProxyPort { get => "Port"; }
        public string ProxyUser { get => "Username"; }
        public string ProxyPassword { get => "Password"; }

        public string AboutTitle { get => "About"; }
        public string AboutProjectPage { get => "Projectpage github.com"; }
        public string AboutViewLicense { get => "View License"; }
        public string AboutSpecialThanks { get => "Special Thanks to:"; }
        public string AboutBetaTesters { get => "Beta Testers:"; }
        public string AboutCopyright { get => "Copyright:"; }
        public string AboutView3rdParty { get => "View 3rd Party Libraries, Software"; }
        public string AboutForSpellCheck { get => "for Spell Checking"; }
        public string AboutVersion { get => "Version"; }
        public string AboutViewPatchnotes { get => "View Patchnotes"; }

        public string LicenseTitle { get => "License"; }
        public string LicenseFor { get => "for"; }
        public string LicenseBy { get => "by"; }

        public string LicensesTitle { get => "3rd Party Software"; }
        public string LicensesClose { get => "Close"; }

        public string SoftwareItemProject { get => "Project"; }
        public string SoftwareItemViewLicense { get => "view License"; }


        public string InitTitle { get => "First Setup"; }
        public string InitArcPaht { get => @"Arc Logs Path:"; }
        public string InitBrowse { get => "Browse ..."; }
        public string InitLanguage { get => "Language:"; }
        public string InitCancle { get => "Cancel"; }
        public string InitStart { get => "Start"; }
        public string InitCancelSetupText { get => "Really abort the setup?"; }
        public string InitCancelSetupTitel { get => "Confirm cancel"; }
        public string InitInvalidPathTitel { get => "Invalid path"; }
        public string InitInvalidPathText { get => @"...\arcdps.cbtlogs\ path is invalid!"; }


        public string ConfigTitle { get => "Settings"; }
        public string ConfigGeneralTitle { get => "General"; }
        public string ConfigGeneralArcPaht { get => "Arc Log Folder Path"; }
        public string ConfigGeneralBrowse { get => "Browse..."; }
        public string ConfigGeneralLanguage { get => "Language"; }
        public string ConfigGeneralAllowBeta { get => "Allow Beta Updates"; }
        public string ConfigDpsReportTitle { get => "dps.report"; }
        public string ConfigDpsReportToken { get => "User Token:"; }
        public string ConfigDpsReportGetToken { get => "Generate Token:"; }
        public string ConfigDpsReportProxy { get => "Proxy Settings..."; }
        public string ConfigCopyTitle { get => "Copy Links"; }
        public string ConfigCopyBoss { get => "Include encounter"; }
        public string ConfigCopySuccess { get => "Include success"; }
        public string ConfigCopyInline { get => "Link in same line as encounter"; }
        public string ConfigCopySpace { get => "Empty line between logs"; }
        public string ConfigCopyEmotes { get => "Include RisingLight Discord Emotes"; }
        public string ConfigDiscordTitle { get => "Discord Webhooks"; }
        public string ConfigDiscordWebHookName { get => "Name:"; }
        public string ConfigDiscordWebHookLink { get => "Link:"; }
        public string ConfigDiscordWebHookFormat { get => "Format:"; }
        public string ConfigDiscordWebHookAvatar { get => "Avatar URL:"; }
        public string ConfigDiscordWebHookDelete { get => "Delete"; }
        public string ConfigDiscordNoHooks { get => "No WebHooks configured\n\nAdd a new one!"; }
        public string ConfigDiscordCount { get => "Count"; }
        public string ConfigDiscordAdd { get => "Add"; }
        public string ConfigDiscordOnlyUploaded { get => "Only post Uploaded"; }
        public string ConfigDiscordNameAsUsername { get => "Name as Discord Username"; }
        public string ConfigEiTitle { get => "EliteInsights"; }
        public string ConfigEiCombatReplay { get => "Generate combat replay"; }
        public string ConfigEiLightTheme { get => "Use light theme"; }
        public string ConfigEiAutoUpdate { get => "Auto update"; }
        public string ConfigEiUpdate { get => "Update / Reinstall"; }
        public string ConfigDefault { get => "Default"; }
        public string ConfigCancel { get => "Cancel"; }
        public string ConfigSave { get => "Save"; }
        public string ConfigDefaultMsgTitel { get => "Reset Settings"; }
        public string ConfigDefaultMsgText { get => "Restore the Default settings?\nThis will permanently delete your current settings!"; }
        public string ConfigDiscardMsgTitel { get => "Discard Changes?"; }
        public string ConfigDiscardMsgText { get => "Continue without saving?"; }
        public string ConfigRoPlusTitle { get => "RaidOrga+"; }
        public string ConfigRoPlusUser { get => "User"; }
        public string ConfigRoPlusPwd { get => "Password"; }
        public string ConfigRoPlusNote { get => "Note:\nOnly raids, where you are also Raidlead, will be shown"; }
        public string ConfigExport { get => "Export"; }
        public string ConfigImport { get => "Import"; }
        public string ConfigExportFileFilter { get => "LogUploaderSettings files|*.lus|All files|*.*"; }
        public string ConfigExportSaveTitle { get => "Select file to export to"; }
        public string ConfigImportOpenTitle { get => "Select file to import from"; }
        public string ConfigExportPwdPromptTitle { get => "Password"; }
        public string ConfigExportPwdPromptText { get => "Enter Password:"; }
        public string ConfigExportPwdPromptNewText { get => "Enter password (empty is no password):"; }
        public string ConfigExportPwdFailTitle { get => "Password Error"; }
        public string ConfigExportPwdFailText { get => "Invalid Password"; }
        public string ConfigExportMessageTitle { get => "Export result"; }
        public string ConfigExportMessageFail { get => "Export faild"; }
        public string ConfigExportMessageSucc { get => "Export complete"; }
        public string ConfigImportMessageTitle { get => "Import result"; }
        public string ConfigImportMessageFail { get => "Import failed"; }
        public string ConfigImportMessageSucc { get => "Import complete"; }

        public string NewTitle { get => "What's new in "; }
        public string NewHeading { get => "Patchnotes"; }
        public string NewClose { get => "Close"; }


        public string PlayerToCorrectPlayer { get => "Player"; }
        public string PlayerToCorrectAccount { get => "Account"; }
        public string PlayerToCorrectMember { get => "Member"; }
        public string PlayerToCorrectHelper { get => "Helper"; }
        public string PlayerToCorrectLFG { get => "LFG"; }

        public string CorrectPlayer { get => "Correct Players"; }
        public string CorrectPlayerTitle { get => "Assigen Players"; }
        public string CorrectPlayerDone { get => "Done"; }

        public string MiscGenericProcessing { get => "Processing"; }
        public string MiscNoWebhookMsgTitel { get => "Setup Webhooks"; }
        public string MiscNoWebhookMsgText { get => "You have currently no Webhooks set up.\nGo to Discord Webhooks in Settings and create a new one"; }
        public string MiscDiscordPostGenDuration { get => "Duration"; }
        public string MiscDiscordPostGenHpLeft { get => "HP left"; }
        public string MiscDiscordPostGenGroupDPS { get => "Group DPS"; }
        public string MiscDiscordPostGenTopDPS { get => "Top DPS"; }
        public string MiscDiscordPostGenNoLink { get => "no Link"; }
        public string MiscDiscordPostErrTitle { get => "Discord posting Error"; }
        public string MiscDiscordPostErrMsg
        {
            get
            {
                return "Unable to post to Webhook %s.\n" +
                    $"Make sure\n" +
                    $"- the address of the WebHook is correct\n" +
                    $"- if an avatar is specified to check its link\n" +
                    $"- you are connected to the internet";
            }
        }
        public string MiscDetailsMultibleBosses { get => "Multiple"; }
        public string MiscRaidOrgaPlusNoAccount { get => "No account configured"; }
        public string MiscRaidOrgaPlusLoginErr { get => "Login faild"; }
        public string MiscRaidOrgaPlusNoRaid { get => "No raid with leader permission"; }
    }
}
