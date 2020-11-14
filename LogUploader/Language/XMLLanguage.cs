using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace LogUploader.Languages
{
    public class XMLLanguage : BaseLanguage
    {
        [XmlIgnore]
        public CultureInfo culture;
        [XmlElement]
        public string succsess;
        [XmlElement]
        public string fail;

        [XmlElement]
        public string proxySettings;
        [XmlElement]
        public string proxyUse;
        [XmlElement]
        public string proxyHostename;
        [XmlElement]
        public string proxyPort;
        [XmlElement]
        public string proxyUser;
        [XmlElement]
        public string proxyPassword;

        [XmlElement]
        public string colHeaderBoss;
        [XmlElement]
        public string colHeaderDate;
        [XmlElement]
        public string colHeaderSize;
        [XmlElement]
        public string colHeaderDataCorrected;
        [XmlElement]
        public string colHeaderDuration;
        [XmlElement]
        public string colHeaderSuccess;
        [XmlElement]
        public string colHeaderHpLeft;
        [XmlElement]
        public string colHeaderCM;
        [XmlElement]
        public string colHeaderParsed;
        [XmlElement]
        public string colHeaderUploaded;
        [XmlElement]
        public string filterHeader;
        [XmlElement]
        public string filterBoss;
        [XmlElement]
        public string filterHPLeft;
        [XmlElement]
        public string filterDuration;
        [XmlElement]
        public string filterDate;
        [XmlElement]
        public string filterFrom;
        [XmlElement]
        public string filterTo;
        [XmlElement]
        public string filterSuccess;
        [XmlElement]
        public string filterKill;
        [XmlElement]
        public string filterWipe;
        [XmlElement]
        public string filterToday;
        [XmlElement]
        public string filterReset;
        [XmlElement]
        public string detailsHeader;
        [XmlElement]
        public string detailsBoss;
        [XmlElement]
        public string detailsDate;
        [XmlElement]
        public string detailsSize;
        [XmlElement]
        public string detailsCorrected;
        [XmlElement]
        public string detailsDuration;
        [XmlElement]
        public string detailsSuccess;
        [XmlElement]
        public string detailsHpLeft;
        [XmlElement]
        public string detailsCM;
        [XmlElement]
        public string detailsParsed;
        [XmlElement]
        public string detailsUploaded;
        [XmlElement]
        public string detailsOpenLocal;
        [XmlElement]
        public string detailsOpenRemote;
        [XmlElement]
        public string detailsAccName;
        [XmlElement]
        public string detailsSubGroup;
        [XmlElement]
        public string detailsDPS;
        [XmlElement]
        public string actionsHeader;
        [XmlElement]
        public string actionsParseLocal;
        [XmlElement]
        public string actionsOpenLocal;
        [XmlElement]
        public string actionsUpload;
        [XmlElement]
        public string actionsOpenRemote;
        [XmlElement]
        public string actionsParseAndUpload;
        [XmlElement]
        public string actionsCopyLinks;
        [XmlElement]
        public string actionsCopied;
        [XmlElement]
        public string actionsPostToDiscord;
        [XmlElement]
        public string actionsViewInExplorer;
        [XmlElement]
        public string settingsHeader;
        [XmlElement]
        public string settingsAutoParse;
        [XmlElement]
        public string settingsAutoUpload;
        [XmlElement]
        public string settingsAbout;
        [XmlElement]
        public string settingsSettings;
        [XmlElement]
        public string footerElements;
        [XmlElement]
        public string footerSelected;
        [XmlElement]
        public string footerShown;
        [XmlElement]
        public string footerProcessing;
        [XmlElement]
        public string footerParsing;
        [XmlElement]
        public string footerUploading;

        [XmlElement]
        public string aboutTitle;
        [XmlElement]
        public string aboutProjectPage;
        [XmlElement]
        public string aboutViewLicense;
        [XmlElement]
        public string aboutSpecialThanks;
        [XmlElement]
        public string aboutBetaTesters;
        [XmlElement]
        public string aboutCopyright;
        [XmlElement]
        public string aboutView3rdParty;
        [XmlElement]
        public string aboutForSpellCheck;
        [XmlElement]
        public string aboutVersion;
        [XmlElement]
        private string aboutViewPatchnotes;

        [XmlElement]
        public string licenseTitle;
        [XmlElement]
        public string licenseFor;
        [XmlElement]
        public string licenseBy;

        [XmlElement]
        public string licensesTitle;
        [XmlElement]
        public string licensesClose;

        [XmlElement]
        public string softwareItemProject;
        [XmlElement]
        public string softwareItemViewLicense;

        [XmlElement]
        public string initTitle;
        [XmlElement]
        public string initArcPaht;
        [XmlElement]
        public string initBrowse;
        [XmlElement]
        public string initLanguage;
        [XmlElement]
        public string initCancle;
        [XmlElement]
        public string initStart;
        [XmlElement]
        public string initCancelSetupTitel;
        [XmlElement]
        public string initCancelSetupText;
        [XmlElement]
        public string initInvalidPathTitel;
        [XmlElement]
        public string initInvalidPathText;

        [XmlElement]
        public string configTitle;
        [XmlElement]
        public string configGeneralTitle;
        [XmlElement]
        public string configGeneralArcPaht;
        [XmlElement]
        public string configGeneralBrowse;
        [XmlElement]
        public string configGeneralLanguage;
        [XmlElement]
        public string configDpsReportTitle;
        [XmlElement]
        public string configDpsReportToken;
        [XmlElement]
        public string configDpsReportGetToken;
        [XmlElement]
        public string configDpsReportProxy;
        [XmlElement]
        public string configCopyTitle;
        [XmlElement]
        public string configCopyBoss;
        [XmlElement]
        public string configCopySuccess;
        [XmlElement]
        public string configCopyInline;
        [XmlElement]
        public string configCopySpace;
        [XmlElement]
        public string configCopyEmotes;
        [XmlElement]
        public string configDiscordTitle;
        [XmlElement]
        public string configDiscordWebHookName;
        [XmlElement]
        public string configDiscordWebHookLink;
        [XmlElement]
        public string configDiscordWebHookFormat;
        [XmlElement]
        public string configDiscordWebHookAvatar;
        [XmlElement]
        public string configDiscordWebHookDelete;
        [XmlElement]
        public string configDiscordCount;
        [XmlElement]
        public string configDiscordNoHooks;
        [XmlElement]
        public string configDiscordAdd;
        [XmlElement]
        public string configDiscordOnlyUploaded;
        [XmlElement]
        public string configDiscordNameAsUsername;
        [XmlElement]
        public string configEiTitle;
        [XmlElement]
        public string configEiCombatReplay;
        [XmlElement]
        public string configEiLightTheme;
        [XmlElement]
        public string configEiAutoUpdate;
        [XmlElement]
        public string configEiUpdate;
        [XmlElement]
        public string configDefault;
        [XmlElement]
        public string configCancel;
        [XmlElement]
        public string configSave;
        [XmlElement]
        public string configDefaultMsgTitel;
        [XmlElement]
        public string configDefaultMsgText;
        [XmlElement]
        public string configDiscardMsgTitel;
        [XmlElement]
        public string configDiscardMsgText;
        [XmlElement]
        public string configRoPlusTitle;
        [XmlElement]
        public string configRoPlusUser;
        [XmlElement]
        public string configRoPlusPwd;




        [XmlElement]
        public string newTitle;
        [XmlElement]
        public string newHeading;
        [XmlElement]
        public string newClose;


        [XmlElement]
        public string playerToCorrectPlayer;
        [XmlElement]
        public string playerToCorrectAccount;
        [XmlElement]
        public string playerToCorrectMember;
        [XmlElement]
        public string playerToCorrectHelper;
        [XmlElement]
        public string playerToCorrectLFG;

        [XmlElement]
        public string correctPlayer;
        [XmlElement]
        public string correctPlayerTitle;
        [XmlElement]
        public string correctPlayerDone;

        [XmlElement]
        public string miscGenericProcessing;
        [XmlElement]
        public string miscNoWebhookMsgTitel;
        [XmlElement]
        public string miscNoWebhookMsgText;
        [XmlElement]
        public string miscDiscordPostGenDuration;
        [XmlElement]
        public string miscDiscordPostGenHpLeft;
        [XmlElement]
        public string miscDiscordPostGenGroupDPS;
        [XmlElement]
        public string miscDiscordPostGenTopDPS;
        [XmlElement]
        public string miscDiscordPostGenNoLink;
        [XmlElement]
        public string miscDiscordPostErrTitle;
        [XmlElement]
        public string miscDiscordPostErrMsg;
        [XmlElement]
        public string miscDetailsMultibleBosses;
        [XmlElement]
        public string miscRaidOrgaPlusNoAccount;
        [XmlElement]
        public string miscRaidOrgaPlusLoginErr;
        [XmlElement]
        public string miscRaidOrgaPlusNoRaid;

        public XMLLanguage()
        {
        }

        public XMLLanguage(ILanguage lang)
        {
            succsess = lang.Succsess;
            fail = lang.Fail;

            proxySettings = lang.ProxySettings;
            proxyUse = lang.ProxyUse;
            proxyHostename = lang.ProxyHostename;
            proxyPort = lang.ProxyPort;
            proxyUser = lang.ProxyUser;
            proxyPassword = lang.ProxyPassword;

            colHeaderBoss = lang.ColHeaderBoss;
            colHeaderDate = lang.ColHeaderDate;
            colHeaderSize = lang.ColHeaderSize;
            colHeaderDataCorrected = lang.ColHeaderDataCorrected;
            colHeaderDuration = lang.ColHeaderDuration;
            colHeaderSuccess = lang.ColHeaderSuccess;
            colHeaderHpLeft = lang.ColHeaderHpLeft;
            colHeaderCM = lang.ColHeaderCM;
            colHeaderParsed = lang.ColHeaderParsed;
            colHeaderUploaded = lang.ColHeaderUploaded;
            filterHeader = lang.FilterHeader;
            filterBoss = lang.FilterBoss;
            filterHPLeft = lang.FilterHPLeft;
            filterDuration = lang.FilterDuration;
            filterDate = lang.FilterDate;
            filterFrom = lang.FilterFrom;
            filterTo = lang.FilterTo;
            filterSuccess = lang.FilterSuccess;
            filterKill = lang.FilterKill;
            filterWipe = lang.FilterWipe;
            filterToday = lang.FilterToday;
            filterReset = lang.FilterReset;
            detailsHeader = lang.DetailsHeader;
            detailsBoss = lang.DetailsBoss;
            detailsDate = lang.DetailsDate;
            detailsSize = lang.DetailsSize;
            detailsCorrected = lang.DetailsCorrected;
            detailsDuration = lang.DetailsDuration;
            detailsSuccess = lang.DetailsSuccess;
            detailsHpLeft = lang.DetailsHpLeft;
            detailsCM = lang.DetailsCM;
            detailsParsed = lang.DetailsParsed;
            detailsUploaded = lang.DetailsUploaded;
            detailsOpenLocal = lang.DetailsOpenLocal;
            detailsOpenRemote = lang.DetailsOpenRemote;
            detailsAccName = lang.DetailsAccName;
            detailsSubGroup = lang.DetailsSubGroup;
            detailsDPS = lang.DetailsDPS;
            actionsHeader = lang.ActionsHeader;
            actionsParseLocal = lang.ActionsParseLocal;
            actionsOpenLocal = lang.ActionsOpenLocal;
            actionsUpload = lang.ActionsUpload;
            actionsOpenRemote = lang.ActionsOpenRemote;
            actionsParseAndUpload = lang.ActionsParseAndUpload;
            actionsCopyLinks = lang.ActionsCopyLinks;
            actionsCopied = lang.ActionsCopied;
            actionsPostToDiscord = lang.ActionsPostToDiscord;
            actionsViewInExplorer = lang.ActionsViewInExplorer;
            settingsHeader = lang.SettingsHeader;
            settingsAutoParse = lang.SettingsAutoParse;
            settingsAutoUpload = lang.SettingsAutoUpload;
            settingsAbout = lang.SettingsAbout;
            settingsSettings = lang.SettingsSettings;
            footerElements = lang.FooterElements;
            footerSelected = lang.FooterSelected;
            footerShown = lang.FooterShown;
            footerProcessing = lang.FooterProcessing;
            footerParsing = lang.FooterParsing;
            footerUploading = lang.FooterUploading;


            aboutTitle = lang.AboutTitle;
            aboutProjectPage = lang.AboutProjectPage;
            aboutViewLicense = lang.AboutViewLicense;
            aboutSpecialThanks = lang.AboutSpecialThanks;
            aboutBetaTesters = lang.AboutBetaTesters;
            aboutCopyright = lang.AboutCopyright;
            aboutView3rdParty = lang.AboutView3rdParty;
            aboutForSpellCheck = lang.AboutForSpellCheck;
            aboutVersion = lang.AboutVersion;
            aboutViewPatchnotes = lang.AboutViewPatchnotes;

            licenseTitle = lang.LicenseTitle;
            licenseFor = lang.LicenseFor;
            licenseBy = lang.LicenseBy;

            licensesTitle = lang.LicensesTitle;
            licensesClose = lang.LicensesClose;

            softwareItemProject = lang.SoftwareItemProject;
            softwareItemViewLicense = lang.SoftwareItemViewLicense;

            initTitle = lang.InitTitle;
            initArcPaht = lang.InitArcPaht;
            initBrowse = lang.InitBrowse;
            initLanguage = lang.InitLanguage;
            initCancle = lang.InitCancle;
            initStart = lang.InitStart;
            initCancelSetupTitel = lang.InitCancelSetupTitel;
            initCancelSetupText = lang.InitCancelSetupText;
            initInvalidPathTitel = lang.InitInvalidPathTitel;
            initInvalidPathText = lang.InitInvalidPathText;

            configTitle = lang.ConfigTitle;
            configGeneralTitle = lang.ConfigGeneralTitle;
            configGeneralArcPaht = lang.ConfigGeneralArcPaht;
            configGeneralBrowse = lang.ConfigGeneralBrowse;
            configGeneralLanguage = lang.ConfigGeneralLanguage;
            configDpsReportTitle = lang.ConfigDpsReportTitle;
            configDpsReportToken = lang.ConfigDpsReportToken;
            configDpsReportGetToken = lang.ConfigDpsReportGetToken;
            configDpsReportProxy = lang.ConfigDpsReportProxy;
            configCopyTitle = lang.ConfigCopyTitle;
            configCopyBoss = lang.ConfigCopyBoss;
            configCopySuccess = lang.ConfigCopySuccess;
            configCopyInline = lang.ConfigCopyInline;
            configCopySpace = lang.ConfigCopySpace;
            configCopyEmotes = lang.ConfigCopyEmotes;
            configDiscordTitle = lang.ConfigDiscordTitle;
            configDiscordWebHookName = lang.ConfigDiscordWebHookName;
            configDiscordWebHookLink = lang.ConfigDiscordWebHookLink;
            configDiscordWebHookFormat = lang.ConfigDiscordWebHookFormat;
            configDiscordWebHookAvatar = lang.ConfigDiscordWebHookAvatar;
            configDiscordWebHookDelete = lang.ConfigDiscordWebHookDelete;
            configDiscordCount = lang.ConfigDiscordCount;
            configDiscordNoHooks = lang.ConfigDiscordNoHooks;
            configDiscordAdd = lang.ConfigDiscordAdd;
            configDiscordOnlyUploaded = lang.ConfigDiscordOnlyUploaded;
            configDiscordNameAsUsername = lang.ConfigDiscordNameAsUsername;
            configEiTitle = lang.ConfigEiTitle;
            configEiCombatReplay = lang.ConfigEiCombatReplay;
            configEiLightTheme = lang.ConfigEiLightTheme;
            configEiAutoUpdate = lang.ConfigEiAutoUpdate;
            configEiUpdate = lang.ConfigEiUpdate;
            configDefault = lang.ConfigDefault;
            configCancel = lang.ConfigCancel;
            configSave = lang.ConfigSave;
            configDefaultMsgTitel = lang.ConfigDefaultMsgTitel;
            configDefaultMsgText = lang.ConfigDefaultMsgText;
            configDiscardMsgTitel = lang.ConfigDiscardMsgTitel;
            configDiscardMsgText = lang.ConfigDiscardMsgText;
            configRoPlusTitle = lang.ConfigRoPlusTitle;
            configRoPlusUser = lang.ConfigRoPlusUser;
            configRoPlusPwd = lang.ConfigRoPlusPwd;

            newTitle = lang.NewTitle;
            newHeading = lang.NewHeading;
            newClose = lang.NewClose;

            playerToCorrectPlayer = lang.PlayerToCorrectPlayer;
            playerToCorrectAccount = lang.PlayerToCorrectAccount;
            playerToCorrectMember = lang.PlayerToCorrectMember;
            playerToCorrectHelper = lang.PlayerToCorrectHelper;
            playerToCorrectLFG = lang.PlayerToCorrectLFG;

            correctPlayer = lang.CorrectPlayer;
            correctPlayerTitle = lang.CorrectPlayerTitle;
            correctPlayerDone = lang.CorrectPlayerDone;

            miscGenericProcessing = lang.MiscGenericProcessing;
            miscNoWebhookMsgTitel = lang.MiscNoWebhookMsgTitel;
            miscNoWebhookMsgText = lang.MiscNoWebhookMsgText;
            miscDiscordPostGenDuration = lang.MiscDiscordPostGenDuration;
            miscDiscordPostGenHpLeft = lang.MiscDiscordPostGenHpLeft;
            miscDiscordPostGenGroupDPS = lang.MiscDiscordPostGenGroupDPS;
            miscDiscordPostGenTopDPS = lang.MiscDiscordPostGenTopDPS;
            miscDiscordPostGenNoLink = lang.MiscDiscordPostGenNoLink;
            miscDiscordPostErrTitle = lang.MiscDiscordPostErrTitle;
            miscDiscordPostErrMsg = lang.MiscDiscordPostErrMsg;
            miscDetailsMultibleBosses = lang.MiscDetailsMultibleBosses;
            miscRaidOrgaPlusNoAccount = lang.MiscRaidOrgaPlusNoAccount;
            miscRaidOrgaPlusLoginErr = lang.MiscRaidOrgaPlusLoginErr;
            miscRaidOrgaPlusNoRaid = lang.MiscRaidOrgaPlusNoRaid;

        }
        [XmlIgnore]
        public override CultureInfo Culture => culture;
        public override string Succsess => succsess;
        public override string Fail => fail;
        public override string ProxySettings => proxySettings;
        public override string ProxyUse => proxyUse;
        public override string ProxyHostename => proxyHostename;
        public override string ProxyPort => proxyPort;
        public override string ProxyUser => proxyUser;
        public override string ProxyPassword => proxyPassword;
        public override string ColHeaderBoss => colHeaderBoss;
        public override string ColHeaderDate => colHeaderDate;
        public override string ColHeaderSize => colHeaderSize;
        public override string ColHeaderDataCorrected => colHeaderDataCorrected;
        public override string ColHeaderDuration => colHeaderDuration;
        public override string ColHeaderSuccess => colHeaderSuccess;
        public override string ColHeaderHpLeft => colHeaderHpLeft;
        public override string ColHeaderCM => colHeaderCM;
        public override string ColHeaderParsed => colHeaderParsed;
        public override string ColHeaderUploaded => colHeaderUploaded;
        public override string FilterHeader => filterHeader;
        public override string FilterBoss => filterBoss;
        public override string FilterHPLeft => filterHPLeft;
        public override string FilterDuration => filterDuration;
        public override string FilterDate => filterDate;
        public override string FilterFrom => filterFrom;
        public override string FilterTo => filterTo;
        public override string FilterSuccess => filterSuccess;
        public override string FilterKill => filterKill;
        public override string FilterWipe => filterWipe;
        public override string FilterToday => filterToday;
        public override string FilterReset => filterReset;
        public override string DetailsHeader => detailsHeader;
        public override string DetailsBoss => detailsBoss;
        public override string DetailsDate => detailsDate;
        public override string DetailsSize => detailsSize;
        public override string DetailsCorrected => detailsCorrected;
        public override string DetailsDuration => detailsDuration;
        public override string DetailsSuccess => detailsSuccess;
        public override string DetailsHpLeft => detailsHpLeft;
        public override string DetailsCM => detailsCM;
        public override string DetailsParsed => detailsParsed;
        public override string DetailsUploaded => detailsUploaded;
        public override string DetailsOpenLocal => detailsOpenLocal;
        public override string DetailsOpenRemote => detailsOpenRemote;
        public override string DetailsAccName => detailsAccName;
        public override string DetailsSubGroup => detailsSubGroup;
        public override string DetailsDPS => detailsDPS;
        public override string ActionsHeader => actionsHeader;
        public override string ActionsParseLocal => actionsParseLocal;
        public override string ActionsOpenLocal => actionsOpenLocal;
        public override string ActionsUpload => actionsUpload;
        public override string ActionsOpenRemote => actionsOpenRemote;
        public override string ActionsParseAndUpload => actionsParseAndUpload;
        public override string ActionsCopyLinks => actionsCopyLinks;
        public override string ActionsCopied => actionsCopied;
        public override string ActionsPostToDiscord => actionsPostToDiscord;
        public override string ActionsViewInExplorer => actionsViewInExplorer;
        public override string SettingsHeader => settingsHeader;
        public override string SettingsAutoParse => settingsAutoParse;
        public override string SettingsAutoUpload => settingsAutoUpload;
        public override string SettingsAbout => settingsAbout;
        public override string SettingsSettings => settingsSettings;
        public override string FooterElements => footerElements;
        public override string FooterSelected => footerSelected;
        public override string FooterShown => footerShown;
        public override string FooterProcessing => footerProcessing;
        public override string FooterParsing => footerParsing;
        public override string FooterUploading => footerUploading;

        public override string AboutTitle => aboutTitle;
        public override string AboutProjectPage => aboutProjectPage;
        public override string AboutViewLicense => aboutViewLicense;
        public override string AboutSpecialThanks => aboutSpecialThanks;
        public override string AboutBetaTesters => aboutBetaTesters;
        public override string AboutCopyright => aboutCopyright;
        public override string AboutView3rdParty => aboutView3rdParty;
        public override string AboutForSpellCheck => aboutForSpellCheck;
        public override string AboutVersion => aboutVersion;
        public override string AboutViewPatchnotes => aboutViewPatchnotes;
        public override string LicenseTitle => licenseTitle;
        public override string LicenseFor => licenseFor;
        public override string LicenseBy => licenseBy;
        public override string LicensesTitle => licensesTitle;
        public override string LicensesClose => licensesClose;
        public override string SoftwareItemProject => softwareItemProject;
        public override string SoftwareItemViewLicense => softwareItemViewLicense;
        public override string InitTitle => initTitle;
        public override string InitArcPaht => initArcPaht;
        public override string InitBrowse => initBrowse;
        public override string InitLanguage => initLanguage;
        public override string InitCancle => initCancle;
        public override string InitStart => initStart;
        public override string InitCancelSetupTitel => initCancelSetupTitel;
        public override string InitCancelSetupText => initCancelSetupText;
        public override string InitInvalidPathTitel => initInvalidPathTitel;
        public override string InitInvalidPathText => initInvalidPathText;

        public override string ConfigTitle => configTitle;
        public override string ConfigGeneralTitle => configGeneralTitle;
        public override string ConfigGeneralArcPaht => configGeneralArcPaht;
        public override string ConfigGeneralBrowse => configGeneralBrowse;
        public override string ConfigGeneralLanguage => configGeneralLanguage;
        public override string ConfigDpsReportTitle => configDpsReportTitle;
        public override string ConfigDpsReportToken => configDpsReportToken;
        public override string ConfigDpsReportGetToken => configDpsReportGetToken;
        public override string ConfigDpsReportProxy => configDpsReportProxy;
        public override string ConfigCopyTitle => configCopyTitle;
        public override string ConfigCopyBoss => configCopyBoss;
        public override string ConfigCopySuccess => configCopySuccess;
        public override string ConfigCopyInline => configCopyInline;
        public override string ConfigCopySpace => configCopySpace;
        public override string ConfigCopyEmotes => configCopyEmotes;
        public override string ConfigDiscordTitle => configDiscordTitle;
        public override string ConfigDiscordWebHookName => configDiscordWebHookName;
        public override string ConfigDiscordWebHookLink => configDiscordWebHookLink;
        public override string ConfigDiscordWebHookFormat => configDiscordWebHookFormat;
        public override string ConfigDiscordWebHookAvatar => configDiscordWebHookAvatar;
        public override string ConfigDiscordWebHookDelete => configDiscordWebHookDelete;
        public override string ConfigDiscordCount => configDiscordCount;
        public override string ConfigDiscordNoHooks => configDiscordNoHooks;
        public override string ConfigDiscordAdd => configDiscordAdd;
        public override string ConfigDiscordOnlyUploaded => configDiscordOnlyUploaded;
        public override string ConfigDiscordNameAsUsername => configDiscordNameAsUsername;
        public override string ConfigEiTitle => configEiTitle;
        public override string ConfigEiCombatReplay => configEiCombatReplay;
        public override string ConfigEiLightTheme => configEiLightTheme;
        public override string ConfigEiAutoUpdate => configEiAutoUpdate;
        public override string ConfigEiUpdate => configEiUpdate;
        public override string ConfigDefault => configDefault;
        public override string ConfigCancel => configCancel;
        public override string ConfigSave => configSave;
        public override string ConfigDefaultMsgTitel => configDefaultMsgTitel;
        public override string ConfigDefaultMsgText => configDefaultMsgText;
        public override string ConfigDiscardMsgTitel => configDiscardMsgTitel;
        public override string ConfigDiscardMsgText => configDiscardMsgText;
        public override string ConfigRoPlusTitle => configRoPlusTitle;
        public override string ConfigRoPlusUser => configRoPlusUser;
        public override string ConfigRoPlusPwd => configRoPlusPwd;

        public override string NewTitle => newTitle;
        public override string NewHeading => newHeading;
        public override string NewClose => newClose;


        public override string PlayerToCorrectPlayer => playerToCorrectPlayer;
        public override string PlayerToCorrectAccount => playerToCorrectAccount;
        public override string PlayerToCorrectMember => playerToCorrectMember;
        public override string PlayerToCorrectHelper => playerToCorrectHelper;
        public override string PlayerToCorrectLFG => playerToCorrectLFG;

        public override string CorrectPlayer => correctPlayer;
        public override string CorrectPlayerTitle => correctPlayerTitle;
        public override string CorrectPlayerDone => correctPlayerDone;

        public override string MiscGenericProcessing => miscGenericProcessing;
        public override string MiscNoWebhookMsgTitel => miscNoWebhookMsgTitel;
        public override string MiscNoWebhookMsgText => miscNoWebhookMsgText;
        public override string MiscDiscordPostGenDuration => miscDiscordPostGenDuration;
        public override string MiscDiscordPostGenHpLeft => miscDiscordPostGenHpLeft;
        public override string MiscDiscordPostGenGroupDPS => miscDiscordPostGenGroupDPS;
        public override string MiscDiscordPostGenTopDPS => miscDiscordPostGenTopDPS;
        public override string MiscDiscordPostGenNoLink => miscDiscordPostGenNoLink;
        public override string MiscDiscordPostErrTitle => miscDiscordPostErrTitle;
        public override string MiscDiscordPostErrMsg => miscDiscordPostErrMsg;
        public override string MiscDetailsMultibleBosses => miscDetailsMultibleBosses;
        public override string MiscRaidOrgaPlusNoAccount => miscRaidOrgaPlusNoAccount;
        public override string MiscRaidOrgaPlusLoginErr  => miscRaidOrgaPlusLoginErr;
        public override string MiscRaidOrgaPlusNoRaid => miscRaidOrgaPlusNoRaid;
    }
}
