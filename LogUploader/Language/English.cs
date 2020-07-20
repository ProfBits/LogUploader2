using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogUploader.Languages
{
    public class English : BaseLanguage
    {
        public override CultureInfo Culture { get => new CultureInfo("en-us"); }

        public override string Succsess { get => "Success"; }
        public override string Fail { get => "Fail"; }

        public override string ColHeaderBoss { get => "Boss"; }
        public override string ColHeaderDate { get => "Date"; }
        public override string ColHeaderSize { get => "Size"; }
        public override string ColHeaderDataCorrected { get => "Data checked"; }
        public override string ColHeaderDuration { get => "Duration"; }
        public override string ColHeaderSuccess { get => "Kill"; }
        public override string ColHeaderHpLeft { get => "% left"; }
        public override string ColHeaderCM { get => "CM"; }
        public override string ColHeaderParsed { get => "Parsed"; }
        public override string ColHeaderUploaded { get => "Uploaded"; }
        public override string FilterHeader { get => "Filter"; }
        public override string FilterBoss { get => "Boss"; }
        public override string FilterHPLeft { get => "HP left"; }
        public override string FilterDuration { get => "Duration"; }
        public override string FilterDate { get => "Date"; }
        public override string FilterFrom { get => "from"; }
        public override string FilterTo { get => "to"; }
        public override string FilterSuccess { get => "Success"; }
        public override string FilterKill { get => "Kill"; }
        public override string FilterWipe { get => "Wipe"; }
        public override string FilterToday { get => "Today"; }
        public override string FilterReset { get => "Reset"; }
        public override string DetailsHeader { get => "Details"; }
        public override string DetailsBoss { get => "Boss:"; }
        public override string DetailsDate { get => "Date:"; }
        public override string DetailsSize { get => "Size:"; }
        public override string DetailsCorrected { get => "Corrected:"; }
        public override string DetailsDuration { get => "Duration:"; }
        public override string DetailsSuccess { get => "Success:"; }
        public override string DetailsHpLeft { get => "HP left:"; }
        public override string DetailsCM { get => "CM:"; }
        public override string DetailsParsed { get => "Parsed:"; }
        public override string DetailsUploaded { get => "Uploaded:"; }
        public override string DetailsOpenLocal { get => "open"; }
        public override string DetailsOpenRemote { get => "open"; }
        public override string DetailsAccName { get => "Account"; }
        public override string DetailsSubGroup { get => "SG"; }
        public override string DetailsDPS { get => "DPS"; }
        public override string ActionsHeader { get => "Actions"; }
        public override string ActionsParseLocal { get => "Parse local"; }
        public override string ActionsOpenLocal { get => "Open local"; }
        public override string ActionsUpload { get => "Upload"; }
        public override string ActionsOpenRemote { get => "dps.report"; }
        public override string ActionsParseAndUpload { get => "Parse local and upload"; }
        public override string ActionsCopyLinks { get => "Copy links"; }
        public override string ActionsCopied { get => "copied"; }
        public override string ActionsPostToDiscord { get => "Post to Discord"; }
        public override string ActionsViewInExplorer { get => "View in Explorer"; }
        public override string SettingsHeader { get => "Settings"; }
        public override string SettingsAutoParse { get => "Auto parse new logs"; }
        public override string SettingsAutoUpload { get => "Auto upload new logs"; }
        public override string SettingsAbout { get => "About"; }
        public override string SettingsSettings { get => "Settings ..."; }
        public override string FooterElements { get => "Elements"; }
        public override string FooterSelected { get => "Selected"; }
        public override string FooterShown { get => "Shown"; }
        public override string FooterProcessing { get => "Processing"; }
        public override string FooterParsing { get => "Parsing"; }
        public override string FooterUploading { get => "Uploading"; }

        public override string ProxySettings { get => "Proxy Settings"; }
        public override string ProxyUse { get => "Use Proxy"; }
        public override string ProxyHostename { get => "Hostname"; }
        public override string ProxyPort { get => "Port"; }
        public override string ProxyUser { get => "Username"; }
        public override string ProxyPassword { get => "Password"; }

        public override string AboutTitle { get => "About"; }
        public override string AboutProjectPage { get => "Projectpage github.com"; }
        public override string AboutViewLicense { get => "View License"; }
        public override string AboutSpecialThanks { get => "Special Thanks to:"; }
        public override string AboutBetaTesters { get => "Beta Testers:"; }
        public override string AboutCopyright { get => "Copyright:"; }
        public override string AboutView3rdParty { get => "View 3rd Party Libraries, Software"; }
        public override string AboutForSpellCheck { get => "for Spell Checking"; }
        public override string AboutVersion { get => "Version"; }
        public override string AboutViewPatchnotes { get => "View Patchnotes"; }

        public override string LicenseTitle { get => "License"; }
        public override string LicenseFor { get => "for"; }
        public override string LicenseBy { get => "by"; }

        public override string LicensesTitle { get => "3rd Party Software"; }
        public override string LicensesClose { get => "Close"; }

        public override string SoftwareItemProject { get => "Project"; }
        public override string SoftwareItemViewLicense { get => "view License"; }


        public override string InitTitle { get => "First Setup"; }
        public override string InitArcPaht { get => @"Arc Logs Path:"; }
        public override string InitBrowse { get => "Browse ..."; }
        public override string InitLanguage { get => "Language:"; }
        public override string InitCancle { get => "Cancel"; }
        public override string InitStart { get => "Start"; }
        public override string InitCancelSetupText { get => "Really abort the setup?"; }
        public override string InitCancelSetupTitel { get => "Confirm cancel"; }
        public override string InitInvalidPathTitel { get => "Invalid path"; }
        public override string InitInvalidPathText { get => @"...\arcdps.cbtlogs\ path is invalid!"; }


        public override string ConfigTitle { get => "Settings"; }
        public override string ConfigGeneralTitle { get => "General"; }
        public override string ConfigGeneralArcPaht { get => "Arc Log Folder Path"; }
        public override string ConfigGeneralBrowse { get => "Browse..."; }
        public override string ConfigGeneralLanguage { get => "Language"; }
        public override string ConfigDpsReportTitle { get => "dps.report"; }
        public override string ConfigDpsReportToken { get => "User Token:"; }
        public override string ConfigDpsReportGetToken { get => "Generate Token:"; }
        public override string ConfigDpsReportProxy { get => "Proxy Settings..."; }
        public override string ConfigCopyTitle { get => "Copy Links"; }
        public override string ConfigCopyBoss { get => "Include encounter"; }
        public override string ConfigCopySuccess { get => "Include success"; }
        public override string ConfigCopyInline { get => "Link in same line as encounter"; }
        public override string ConfigCopySpace { get => "Empty line between logs"; }
        public override string ConfigCopyEmotes { get => "Include RisingLight Discord Emotes"; }
        public override string ConfigDiscordTitle { get => "Discord Webhooks"; }
        public override string ConfigDiscordWebHookName { get => "Name:"; }
        public override string ConfigDiscordWebHookLink { get => "Link:"; }
        public override string ConfigDiscordWebHookFormat { get => "Format:"; }
        public override string ConfigDiscordWebHookAvatar { get => "Avatar URL:"; }
        public override string ConfigDiscordWebHookDelete { get => "Delete"; }
        public override string ConfigDiscordNoHooks { get => "No WebHooks configured\n\nAdd a new one!"; }
        public override string ConfigDiscordCount { get => "Count"; }
        public override string ConfigDiscordAdd { get => "Add"; }
        public override string ConfigDiscordOnlyUploaded { get => "Only post Uploaded"; }
        public override string ConfigDiscordNameAsUsername { get => "Name as Discord Username"; }
        public override string ConfigEiTitle { get => "EliteInsights"; }
        public override string ConfigEiCombatReplay { get => "Generate combat replay"; }
        public override string ConfigEiLightTheme { get => "Use light theme"; }
        public override string ConfigDefault { get => "Default"; }
        public override string ConfigCancel { get => "Cancel"; }
        public override string ConfigSave { get => "Save"; }
        public override string ConfigDefaultMsgTitel { get => "Reset Settings"; }
        public override string ConfigDefaultMsgText { get => "Restore the Default settings?\nThis will permanently delete your current settings!"; }
        public override string ConfigDiscardMsgTitel { get => "Discard Changes?"; }
        public override string ConfigDiscardMsgText { get => "Continue without saving?"; }

        public override string NewTitle { get => "What's new in "; }
        public override string NewHeading { get => "Patchnotes"; }
        public override string NewClose { get => "Close"; }

        public override string MiscGenericProcessing { get => "Processing"; }
        public override string MiscNoWebhookMsgTitel { get => "Setup Webhooks"; }
        public override string MiscNoWebhookMsgText { get => "You have currently no Webhooks set up.\nGo to Discord Webhooks in Settings and create a new one"; }
        public override string MiscDiscordPostGenDuration { get => "Duration"; }
        public override string MiscDiscordPostGenHpLeft { get => "HP left"; }
        public override string MiscDiscordPostGenGroupDPS { get => "Group DPS"; }
        public override string MiscDiscordPostGenTopDPS { get => "Top DPS"; }
        public override string MiscDiscordPostGenNoLink { get => "no Link"; }
        public override string MiscDiscordPostErrTitle { get => "Discord posting Error"; }
        public override string MiscDiscordPostErrMsg
        {
            get
            {
                return "Unable to Post to Webhook %s.\n" +
                    $"Make sure\n" +
                    $"- the address of the WebHook is correct\n" +
                    $"- if an avatar is specified check its link\n" +
                    $"- you are connected to the internet";
            }
        }
        public override string MiscDetailsMultibleBosses { get => "Multiple"; }
    }
}
