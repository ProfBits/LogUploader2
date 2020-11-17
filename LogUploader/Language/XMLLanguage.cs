using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace LogUploader.Languages
{
    public class XMLLanguage : ILanguage
    {
        public XMLLanguage()
        {
        }

        public XMLLanguage(ILanguage lang)
        {
            Succsess = lang.Succsess;
            Fail = lang.Fail;

            ProxySettings = lang.ProxySettings;
            ProxyUse = lang.ProxyUse;
            ProxyHostename = lang.ProxyHostename;
            ProxyPort = lang.ProxyPort;
            ProxyUser = lang.ProxyUser;
            ProxyPassword = lang.ProxyPassword;

            ColHeaderBoss = lang.ColHeaderBoss;
            ColHeaderDate = lang.ColHeaderDate;
            ColHeaderSize = lang.ColHeaderSize;
            ColHeaderDataCorrected = lang.ColHeaderDataCorrected;
            ColHeaderDuration = lang.ColHeaderDuration;
            ColHeaderSuccess = lang.ColHeaderSuccess;
            ColHeaderHpLeft = lang.ColHeaderHpLeft;
            ColHeaderCM = lang.ColHeaderCM;
            ColHeaderParsed = lang.ColHeaderParsed;
            ColHeaderUploaded = lang.ColHeaderUploaded;
            FilterHeader = lang.FilterHeader;
            FilterBoss = lang.FilterBoss;
            FilterHPLeft = lang.FilterHPLeft;
            FilterDuration = lang.FilterDuration;
            FilterDate = lang.FilterDate;
            FilterFrom = lang.FilterFrom;
            FilterTo = lang.FilterTo;
            FilterSuccess = lang.FilterSuccess;
            FilterKill = lang.FilterKill;
            FilterWipe = lang.FilterWipe;
            FilterToday = lang.FilterToday;
            FilterReset = lang.FilterReset;
            DetailsHeader = lang.DetailsHeader;
            DetailsBoss = lang.DetailsBoss;
            DetailsDate = lang.DetailsDate;
            DetailsSize = lang.DetailsSize;
            DetailsCorrected = lang.DetailsCorrected;
            DetailsDuration = lang.DetailsDuration;
            DetailsSuccess = lang.DetailsSuccess;
            DetailsHpLeft = lang.DetailsHpLeft;
            DetailsCM = lang.DetailsCM;
            DetailsParsed = lang.DetailsParsed;
            DetailsUploaded = lang.DetailsUploaded;
            DetailsOpenLocal = lang.DetailsOpenLocal;
            DetailsOpenRemote = lang.DetailsOpenRemote;
            DetailsAccName = lang.DetailsAccName;
            DetailsSubGroup = lang.DetailsSubGroup;
            DetailsDPS = lang.DetailsDPS;
            ActionsHeader = lang.ActionsHeader;
            ActionsParseLocal = lang.ActionsParseLocal;
            ActionsOpenLocal = lang.ActionsOpenLocal;
            ActionsUpload = lang.ActionsUpload;
            ActionsOpenRemote = lang.ActionsOpenRemote;
            ActionsParseAndUpload = lang.ActionsParseAndUpload;
            ActionsCopyLinks = lang.ActionsCopyLinks;
            ActionsCopied = lang.ActionsCopied;
            ActionsPostToDiscord = lang.ActionsPostToDiscord;
            ActionsViewInExplorer = lang.ActionsViewInExplorer;
            SettingsHeader = lang.SettingsHeader;
            SettingsAutoParse = lang.SettingsAutoParse;
            SettingsAutoUpload = lang.SettingsAutoUpload;
            SettingsAbout = lang.SettingsAbout;
            SettingsSettings = lang.SettingsSettings;
            FooterElements = lang.FooterElements;
            FooterSelected = lang.FooterSelected;
            FooterShown = lang.FooterShown;
            FooterProcessing = lang.FooterProcessing;
            FooterParsing = lang.FooterParsing;
            FooterUploading = lang.FooterUploading;


            AboutTitle = lang.AboutTitle;
            AboutProjectPage = lang.AboutProjectPage;
            AboutViewLicense = lang.AboutViewLicense;
            AboutSpecialThanks = lang.AboutSpecialThanks;
            AboutBetaTesters = lang.AboutBetaTesters;
            AboutCopyright = lang.AboutCopyright;
            AboutView3rdParty = lang.AboutView3rdParty;
            AboutForSpellCheck = lang.AboutForSpellCheck;
            AboutVersion = lang.AboutVersion;
            AboutViewPatchnotes = lang.AboutViewPatchnotes;

            LicenseTitle = lang.LicenseTitle;
            LicenseFor = lang.LicenseFor;
            LicenseBy = lang.LicenseBy;

            LicensesTitle = lang.LicensesTitle;
            LicensesClose = lang.LicensesClose;

            SoftwareItemProject = lang.SoftwareItemProject;
            SoftwareItemViewLicense = lang.SoftwareItemViewLicense;

            InitTitle = lang.InitTitle;
            InitArcPaht = lang.InitArcPaht;
            InitBrowse = lang.InitBrowse;
            InitLanguage = lang.InitLanguage;
            InitCancle = lang.InitCancle;
            InitStart = lang.InitStart;
            InitCancelSetupTitel = lang.InitCancelSetupTitel;
            InitCancelSetupText = lang.InitCancelSetupText;
            InitInvalidPathTitel = lang.InitInvalidPathTitel;
            InitInvalidPathText = lang.InitInvalidPathText;

            ConfigTitle = lang.ConfigTitle;
            ConfigGeneralTitle = lang.ConfigGeneralTitle;
            ConfigGeneralArcPaht = lang.ConfigGeneralArcPaht;
            ConfigGeneralBrowse = lang.ConfigGeneralBrowse;
            ConfigGeneralLanguage = lang.ConfigGeneralLanguage;
            ConfigDpsReportTitle = lang.ConfigDpsReportTitle;
            ConfigDpsReportToken = lang.ConfigDpsReportToken;
            ConfigDpsReportGetToken = lang.ConfigDpsReportGetToken;
            ConfigDpsReportProxy = lang.ConfigDpsReportProxy;
            ConfigCopyTitle = lang.ConfigCopyTitle;
            ConfigCopyBoss = lang.ConfigCopyBoss;
            ConfigCopySuccess = lang.ConfigCopySuccess;
            ConfigCopyInline = lang.ConfigCopyInline;
            ConfigCopySpace = lang.ConfigCopySpace;
            ConfigCopyEmotes = lang.ConfigCopyEmotes;
            ConfigDiscordTitle = lang.ConfigDiscordTitle;
            ConfigDiscordWebHookName = lang.ConfigDiscordWebHookName;
            ConfigDiscordWebHookLink = lang.ConfigDiscordWebHookLink;
            ConfigDiscordWebHookFormat = lang.ConfigDiscordWebHookFormat;
            ConfigDiscordWebHookAvatar = lang.ConfigDiscordWebHookAvatar;
            ConfigDiscordWebHookDelete = lang.ConfigDiscordWebHookDelete;
            ConfigDiscordCount = lang.ConfigDiscordCount;
            ConfigDiscordNoHooks = lang.ConfigDiscordNoHooks;
            ConfigDiscordAdd = lang.ConfigDiscordAdd;
            ConfigDiscordOnlyUploaded = lang.ConfigDiscordOnlyUploaded;
            ConfigDiscordNameAsUsername = lang.ConfigDiscordNameAsUsername;
            ConfigEiTitle = lang.ConfigEiTitle;
            ConfigEiCombatReplay = lang.ConfigEiCombatReplay;
            ConfigEiLightTheme = lang.ConfigEiLightTheme;
            ConfigEiAutoUpdate = lang.ConfigEiAutoUpdate;
            ConfigEiUpdate = lang.ConfigEiUpdate;
            ConfigDefault = lang.ConfigDefault;
            ConfigCancel = lang.ConfigCancel;
            ConfigSave = lang.ConfigSave;
            ConfigDefaultMsgTitel = lang.ConfigDefaultMsgTitel;
            ConfigDefaultMsgText = lang.ConfigDefaultMsgText;
            ConfigDiscardMsgTitel = lang.ConfigDiscardMsgTitel;
            ConfigDiscardMsgText = lang.ConfigDiscardMsgText;
            ConfigRoPlusTitle = lang.ConfigRoPlusTitle;
            ConfigRoPlusUser = lang.ConfigRoPlusUser;
            ConfigRoPlusPwd = lang.ConfigRoPlusPwd;

            NewTitle = lang.NewTitle;
            NewHeading = lang.NewHeading;
            NewClose = lang.NewClose;

            PlayerToCorrectPlayer = lang.PlayerToCorrectPlayer;
            PlayerToCorrectAccount = lang.PlayerToCorrectAccount;
            PlayerToCorrectMember = lang.PlayerToCorrectMember;
            PlayerToCorrectHelper = lang.PlayerToCorrectHelper;
            PlayerToCorrectLFG = lang.PlayerToCorrectLFG;

            CorrectPlayer = lang.CorrectPlayer;
            CorrectPlayerTitle = lang.CorrectPlayerTitle;
            CorrectPlayerDone = lang.CorrectPlayerDone;

            MiscGenericProcessing = lang.MiscGenericProcessing;
            MiscNoWebhookMsgTitel = lang.MiscNoWebhookMsgTitel;
            MiscNoWebhookMsgText = lang.MiscNoWebhookMsgText;
            MiscDiscordPostGenDuration = lang.MiscDiscordPostGenDuration;
            MiscDiscordPostGenHpLeft = lang.MiscDiscordPostGenHpLeft;
            MiscDiscordPostGenGroupDPS = lang.MiscDiscordPostGenGroupDPS;
            MiscDiscordPostGenTopDPS = lang.MiscDiscordPostGenTopDPS;
            MiscDiscordPostGenNoLink = lang.MiscDiscordPostGenNoLink;
            MiscDiscordPostErrTitle = lang.MiscDiscordPostErrTitle;
            MiscDiscordPostErrMsg = lang.MiscDiscordPostErrMsg;
            MiscDetailsMultibleBosses = lang.MiscDetailsMultibleBosses;
            MiscRaidOrgaPlusNoAccount = lang.MiscRaidOrgaPlusNoAccount;
            MiscRaidOrgaPlusLoginErr = lang.MiscRaidOrgaPlusLoginErr;
            MiscRaidOrgaPlusNoRaid = lang.MiscRaidOrgaPlusNoRaid;

        }
        [XmlIgnore]
        public CultureInfo Culture { get; set; }
        public string Succsess { get; set; }
        public string Fail { get; set; }
        public string ProxySettings { get; set; }
        public string ProxyUse { get; set; }
        public string ProxyHostename { get; set; }
        public string ProxyPort { get; set; }
        public string ProxyUser { get; set; }
        public string ProxyPassword { get; set; }
        public string ColHeaderBoss { get; set; }
        public string ColHeaderDate { get; set; }
        public string ColHeaderSize { get; set; }
        public string ColHeaderDataCorrected { get; set; }
        public string ColHeaderDuration { get; set; }
        public string ColHeaderSuccess { get; set; }
        public string ColHeaderHpLeft { get; set; }
        public string ColHeaderCM { get; set; }
        public string ColHeaderParsed { get; set; }
        public string ColHeaderUploaded { get; set; }
        public string FilterHeader { get; set; }
        public string FilterBoss { get; set; }
        public string FilterHPLeft { get; set; }
        public string FilterDuration { get; set; }
        public string FilterDate { get; set; }
        public string FilterFrom { get; set; }
        public string FilterTo { get; set; }
        public string FilterSuccess { get; set; }
        public string FilterKill { get; set; }
        public string FilterWipe { get; set; }
        public string FilterToday { get; set; }
        public string FilterReset { get; set; }
        public string DetailsHeader { get; set; }
        public string DetailsBoss { get; set; }
        public string DetailsDate { get; set; }
        public string DetailsSize { get; set; }
        public string DetailsCorrected { get; set; }
        public string DetailsDuration { get; set; }
        public string DetailsSuccess { get; set; }
        public string DetailsHpLeft { get; set; }
        public string DetailsCM { get; set; }
        public string DetailsParsed { get; set; }
        public string DetailsUploaded { get; set; }
        public string DetailsOpenLocal { get; set; }
        public string DetailsOpenRemote { get; set; }
        public string DetailsAccName { get; set; }
        public string DetailsSubGroup { get; set; }
        public string DetailsDPS { get; set; }
        public string ActionsHeader { get; set; }
        public string ActionsParseLocal { get; set; }
        public string ActionsOpenLocal { get; set; }
        public string ActionsUpload { get; set; }
        public string ActionsOpenRemote { get; set; }
        public string ActionsParseAndUpload { get; set; }
        public string ActionsCopyLinks { get; set; }
        public string ActionsCopied { get; set; }
        public string ActionsPostToDiscord { get; set; }
        public string ActionsViewInExplorer { get; set; }
        public string SettingsHeader { get; set; }
        public string SettingsAutoParse { get; set; }
        public string SettingsAutoUpload { get; set; }
        public string SettingsAbout { get; set; }
        public string SettingsSettings { get; set; }
        public string FooterElements { get; set; }
        public string FooterSelected { get; set; }
        public string FooterShown { get; set; }
        public string FooterProcessing { get; set; }
        public string FooterParsing { get; set; }
        public string FooterUploading { get; set; }

        public string AboutTitle { get; set; }
        public string AboutProjectPage { get; set; }
        public string AboutViewLicense { get; set; }
        public string AboutSpecialThanks { get; set; }
        public string AboutBetaTesters { get; set; }
        public string AboutCopyright { get; set; }
        public string AboutView3rdParty { get; set; }
        public string AboutForSpellCheck { get; set; }
        public string AboutVersion { get; set; }
        public string AboutViewPatchnotes { get; set; }
        public string LicenseTitle { get; set; }
        public string LicenseFor { get; set; }
        public string LicenseBy { get; set; }
        public string LicensesTitle { get; set; }
        public string LicensesClose { get; set; }
        public string SoftwareItemProject { get; set; }
        public string SoftwareItemViewLicense { get; set; }
        public string InitTitle { get; set; }
        public string InitArcPaht { get; set; }
        public string InitBrowse { get; set; }
        public string InitLanguage { get; set; }
        public string InitCancle { get; set; }
        public string InitStart { get; set; }
        public string InitCancelSetupTitel { get; set; }
        public string InitCancelSetupText { get; set; }
        public string InitInvalidPathTitel { get; set; }
        public string InitInvalidPathText { get; set; }

        public string ConfigTitle { get; set; }
        public string ConfigGeneralTitle { get; set; }
        public string ConfigGeneralArcPaht { get; set; }
        public string ConfigGeneralBrowse { get; set; }
        public string ConfigGeneralLanguage { get; set; }
        public string ConfigDpsReportTitle { get; set; }
        public string ConfigDpsReportToken { get; set; }
        public string ConfigDpsReportGetToken { get; set; }
        public string ConfigDpsReportProxy { get; set; }
        public string ConfigCopyTitle { get; set; }
        public string ConfigCopyBoss { get; set; }
        public string ConfigCopySuccess { get; set; }
        public string ConfigCopyInline { get; set; }
        public string ConfigCopySpace { get; set; }
        public string ConfigCopyEmotes { get; set; }
        public string ConfigDiscordTitle { get; set; }
        public string ConfigDiscordWebHookName { get; set; }
        public string ConfigDiscordWebHookLink { get; set; }
        public string ConfigDiscordWebHookFormat { get; set; }
        public string ConfigDiscordWebHookAvatar { get; set; }
        public string ConfigDiscordWebHookDelete { get; set; }
        public string ConfigDiscordCount { get; set; }
        public string ConfigDiscordNoHooks { get; set; }
        public string ConfigDiscordAdd { get; set; }
        public string ConfigDiscordOnlyUploaded { get; set; }
        public string ConfigDiscordNameAsUsername { get; set; }
        public string ConfigEiTitle { get; set; }
        public string ConfigEiCombatReplay { get; set; }
        public string ConfigEiLightTheme { get; set; }
        public string ConfigEiAutoUpdate { get; set; }
        public string ConfigEiUpdate { get; set; }
        public string ConfigDefault { get; set; }
        public string ConfigCancel { get; set; }
        public string ConfigSave { get; set; }
        public string ConfigDefaultMsgTitel { get; set; }
        public string ConfigDefaultMsgText { get; set; }
        public string ConfigDiscardMsgTitel { get; set; }
        public string ConfigDiscardMsgText { get; set; }
        public string ConfigRoPlusTitle { get; set; }
        public string ConfigRoPlusUser { get; set; }
        public string ConfigRoPlusPwd { get; set; }

        public string NewTitle { get; set; }
        public string NewHeading { get; set; }
        public string NewClose { get; set; }


        public string PlayerToCorrectPlayer { get; set; }
        public string PlayerToCorrectAccount { get; set; }
        public string PlayerToCorrectMember { get; set; }
        public string PlayerToCorrectHelper { get; set; }
        public string PlayerToCorrectLFG { get; set; }

        public string CorrectPlayer { get; set; }
        public string CorrectPlayerTitle { get; set; }
        public string CorrectPlayerDone { get; set; }

        public string MiscGenericProcessing { get; set; }
        public string MiscNoWebhookMsgTitel { get; set; }
        public string MiscNoWebhookMsgText { get; set; }
        public string MiscDiscordPostGenDuration { get; set; }
        public string MiscDiscordPostGenHpLeft { get; set; }
        public string MiscDiscordPostGenGroupDPS { get; set; }
        public string MiscDiscordPostGenTopDPS { get; set; }
        public string MiscDiscordPostGenNoLink { get; set; }
        public string MiscDiscordPostErrTitle { get; set; }
        public string MiscDiscordPostErrMsg { get; set; }
        public string MiscDetailsMultibleBosses { get; set; }
        public string MiscRaidOrgaPlusNoAccount { get; set; }
        public string MiscRaidOrgaPlusLoginErr  { get; set; }
        public string MiscRaidOrgaPlusNoRaid { get; set; }
    }
}
