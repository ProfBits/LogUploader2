using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogUploader.Languages
{
    public class German : ILanguage
    {
        public CultureInfo Culture { get => new CultureInfo("de-de"); }

        public string Succsess { get => "Erfolg"; }
        public string Fail { get => "Fehlschlag"; }

        public string ColHeaderBoss { get => "Boss"; }
        public string ColHeaderDate { get => "Datum"; }
        public string ColHeaderSize { get => "Gröse"; }
        public string ColHeaderDataCorrected { get => "Überprüft"; }
        public string ColHeaderDuration { get => "Dauer"; }
        public string ColHeaderSuccess { get => "Erfolg"; }
        public string ColHeaderHpLeft { get => "% übrig"; }
        public string ColHeaderCM { get => "CM"; }
        public string ColHeaderParsed { get => "Parsed"; }
        public string ColHeaderUploaded { get => "Uploaded"; }
        public string FilterHeader { get => "Filter"; }
        public string FilterBoss { get => "Boss"; }
        public string FilterHPLeft { get => "HP übrig"; }
        public string FilterDuration { get => "Dauer"; }
        public string FilterDate { get => "Datum"; }
        public string FilterFrom { get => "von"; }
        public string FilterTo { get => "bis"; }
        public string FilterSuccess { get => "Erfolg"; }
        public string FilterKill { get => "Kill"; }
        public string FilterWipe { get => "Wipe"; }
        public string FilterToday { get => "Heute"; }
        public string FilterReset { get => "Reset"; }
        public string DetailsHeader { get => "Details"; }
        public string DetailsBoss { get => "Boss:"; }
        public string DetailsDate { get => "Datum:"; }
        public string DetailsSize { get => "Größe:"; }
        public string DetailsCorrected { get => "Überprüft:"; }
        public string DetailsDuration { get => "Dauer:"; }
        public string DetailsSuccess { get => "Erfolg:"; }
        public string DetailsHpLeft { get => "HP übrig:"; }
        public string DetailsCM { get => "CM:"; }
        public string DetailsParsed { get => "Parsed:"; }
        public string DetailsUploaded { get => "Hochgeladen:"; }
        public string DetailsOpenLocal { get => "öffnen"; }
        public string DetailsOpenRemote { get => "öffnen"; }
        public string DetailsAccName { get => "Account"; }
        public string DetailsSubGroup { get => "SG"; }
        public string DetailsDPS { get => "DPS"; }
        public string ActionsHeader { get => "Aktionen"; }
        public string ActionsParseLocal { get => "Parse lokal"; }
        public string ActionsOpenLocal { get => "Öffne lokal"; }
        public string ActionsUpload { get => "Upload"; }
        public string ActionsOpenRemote { get => "dps.report"; }
        public string ActionsParseAndUpload { get => "Parse lokal und upload"; }
        public string ActionsCopyLinks { get => "Links kopieren"; }
        public string ActionsCopied { get => "kopiert"; }
        public string ActionsPostToDiscord { get => "in Discord posten"; }
        public string ActionsViewInExplorer { get => "Zeige im Explorer"; }
        public string SettingsHeader { get => "Einstellungen"; }
        public string SettingsAutoParse { get => "Auto parse neue Logs"; }
        public string SettingsAutoUpload { get => "Auto upload neue Logs"; }
        public string SettingsAbout { get => "Über"; }
        public string SettingsSettings { get => "Einstellungen ..."; }
        public string FooterElements { get => "Elemente"; }
        public string FooterSelected { get => "Ausgewählt"; }
        public string FooterShown { get => "Angezeigt"; }
        public string FooterProcessing { get => "Verarbeite"; }
        public string FooterParsing { get => "Parse"; }
        public string FooterUploading { get => "Uploading"; }


        public string ProxySettings { get => "Proxy Einstellungen"; }
        public string ProxyUse { get => "Proxy nutzen"; }
        public string ProxyHostename { get => "Proxyserver Adresse"; }
        public string ProxyPort { get => "Port"; }
        public string ProxyUser { get => "Benutzername"; }
        public string ProxyPassword { get => "Passwort"; }


        public string AboutTitle { get => "Über"; }
        public string AboutProjectPage { get => "Projektseite github.com"; }
        public string AboutViewLicense { get => "Zeige Lizenz"; }
        public string AboutSpecialThanks { get => "Vielen Dank an:"; }
        public string AboutBetaTesters { get => "Beta Tester:"; }
        public string AboutCopyright { get => "Copyright:"; }
        public string AboutView3rdParty { get => "Zeige 3rd Party Bibliotheken, Software"; }
        public string AboutForSpellCheck { get => "für Spell Checking"; }
        public string AboutVersion { get => "Version"; }
        public string AboutViewPatchnotes { get => "Zeige Update-Notes"; }

        public string LicenseTitle { get => "Lizenz"; }
        public string LicenseFor { get => "für"; }
        public string LicenseBy { get => "von"; }

        public string LicensesTitle { get => "Software dritter"; }
        public string LicensesClose { get => "Schliessen"; }

        public string SoftwareItemProject { get => "Project"; }
        public string SoftwareItemViewLicense { get => "zeige Lizenz"; }

        public string InitTitle { get => "First Setup"; }
        public string InitArcPaht { get => @"Arc Logs Pfad:"; }
        public string InitBrowse { get => "Auswählen ..."; }
        public string InitLanguage { get => "Sprache:"; }
        public string InitCancle { get => "Abbrechen"; }
        public string InitStart { get => "Start"; }
        public string InitCancelSetupText { get => "Setup wirklich abbrechen?"; }
        public string InitCancelSetupTitel { get => "Abbruch bestätigen"; }
        public string InitInvalidPathTitel { get => "Ungültiger Pfad"; }
        public string InitInvalidPathText { get => @"Pfad ""...\arcdps.cbtlogs\"" is ungültig!"; }


        public string ConfigTitle { get => "Einstellungen"; }
        public string ConfigGeneralTitle { get => "Allgemein"; }
        public string ConfigGeneralArcPaht { get => "Arc Log Ordner Pfad"; }
        public string ConfigGeneralBrowse { get => "Auswählen"; }
        public string ConfigGeneralLanguage { get => "Sprache"; }
        public string ConfigGeneralAllowBeta { get => "Erlaube Beta Versionen"; }
        public string ConfigDpsReportTitle { get => "dps.report"; }
        public string ConfigDpsReportToken { get => "User Token:"; }
        public string ConfigDpsReportGetToken { get => "Token generieren:"; }
        public string ConfigDpsReportProxy { get => "Proxy Einstellungen..."; }
        public string ConfigCopyTitle { get => "Links kopieren"; }
        public string ConfigCopyBoss { get => "mit Bossname"; }
        public string ConfigCopySuccess { get => "mit Erfolg"; }
        public string ConfigCopyInline { get => "Link in gleiche Zeile wie Boss"; }
        public string ConfigCopySpace { get => "leere Zeile zwischen Logs"; }
        public string ConfigCopyEmotes { get => "mit RisingLight Discord Emotes"; }
        public string ConfigDiscordTitle { get => "Discord Webhooks"; }
        public string ConfigDiscordWebHookName { get => "Name:"; }
        public string ConfigDiscordWebHookLink { get => "Link:"; }
        public string ConfigDiscordWebHookFormat { get => "Format:"; }
        public string ConfigDiscordWebHookAvatar { get => "Avatar URL:"; }
        public string ConfigDiscordWebHookDelete { get => "Löschen"; }
        public string ConfigDiscordNoHooks { get => "Keine Webhooks vorhanden\n\nFüge einen neuen hinzu!"; }
        public string ConfigDiscordCount { get => "Anzahl"; }
        public string ConfigDiscordAdd { get => "Hinzufügen"; }
        public string ConfigDiscordOnlyUploaded { get => "Nur hochgeladene posten"; }
        public string ConfigDiscordNameAsUsername { get => "Name als Discord Username"; }
        public string ConfigEiTitle { get => "EliteInsights"; }
        public string ConfigEiCombatReplay { get => "Erzeuge Combat-Replay"; }
        public string ConfigEiLightTheme { get => "Nutze helles Thema"; }
        public string ConfigEiAutoUpdate { get => "Auto Update"; }
        public string ConfigEiUpdate { get => "Update / Reinstall"; }
        public string ConfigDefault { get => "Standard"; }
        public string ConfigCancel { get => "Abbrechen"; }
        public string ConfigSave { get => "Speichern"; }
        public string ConfigDefaultMsgTitel { get => "Einstellungen zurücksetzen"; }
        public string ConfigDefaultMsgText { get => "Standard Einstellungen wiederherstellen?\nDies wird deine aktuellen Einstellungen dauerhaft löschen!"; }
        public string ConfigDiscardMsgTitel { get => "Änderungen verwerfen?"; }
        public string ConfigDiscardMsgText { get => "Fortfahren ohne speichern?"; }
        public string ConfigRoPlusTitle { get => "RaidOrga+"; }
        public string ConfigRoPlusUser { get => "Benutzer"; }
        public string ConfigRoPlusPwd { get => "Passwort"; }
        public string ConfigRoPlusNote { get => "Hinweis:\nNur Termine von Raids bei denen du Leiter bist werden angezeigt."; }
        public string ConfigExport { get => "Export"; }
        public string ConfigImport { get => "Import"; }
        public string ConfigExportFileFilter { get => "LogUploaderSettings Datein|*.lus|Alle Datein|*.*"; }
        public string ConfigExportSaveTitle { get => "Datei zum exportieren wählen"; }
        public string ConfigImportOpenTitle { get => "Datei zum importieren wählen"; }
        public string ConfigExportPwdPromptTitle { get => "Passwort"; }
        public string ConfigExportPwdPromptText { get => "Passwort eingeben:"; }
        public string ConfigExportPwdPromptNewText { get => "Passwort eingeben (leer ist kein Passwort):"; }
        public string ConfigExportPwdFailTitle { get => "Passwort Fehler"; }
        public string ConfigExportPwdFailText { get => "Passwort Falsch"; }
        public string ConfigExportMessageTitle { get => "Export Status"; }
        public string ConfigExportMessageFail { get => "Export Fehlgeschagen"; }
        public string ConfigExportMessageSucc { get => "Export Abgeschlossen"; }
        public string ConfigImportMessageTitle { get => "Import Status"; }
        public string ConfigImportMessageFail { get => "Import Fehlgeschagen"; }
        public string ConfigImportMessageSucc { get => "Import Abgeschlossen"; }

        public string NewTitle { get => "Neuerungen in "; }
        public string NewHeading { get => "Update-Notes"; }
        public string NewClose { get => "Schliessen"; }


        public string PlayerToCorrectPlayer { get => "Spieler"; }
        public string PlayerToCorrectAccount { get => "Account"; }
        public string PlayerToCorrectMember { get => "Mitglied"; }
        public string PlayerToCorrectHelper { get => "Helfer"; }
        public string PlayerToCorrectLFG { get => "LFG"; }

        public string CorrectPlayer { get => "Spieler korregieren"; }
        public string CorrectPlayerTitle { get => "Spieler zuordnen"; }
        public string CorrectPlayerDone { get => "Ferig"; }


        public string MiscGenericProcessing { get => "Verarbeite"; }
        public string MiscNoWebhookMsgTitel { get => "Webhooks erstellen"; }
        public string MiscNoWebhookMsgText { get => "Du hast aktuell keine WebHooks eingestellt\nGehe zu Discord Webhooks in den Einstellungen und erstelle einen neuen."; }
        public string MiscDiscordPostGenDuration { get => "Dauer"; }
        public string MiscDiscordPostGenHpLeft { get => "HP übrig"; }
        public string MiscDiscordPostGenGroupDPS { get => "Gruppen DPS"; }
        public string MiscDiscordPostGenTopDPS { get => "Top DPS"; }
        public string MiscDiscordPostGenNoLink { get => "kein Link"; }
        public string MiscDiscordPostErrTitle { get => "Discord-Post Fehler"; }
        public string MiscDiscordPostErrMsg
        {
            get
            {
                return "Es war nicht möglich über den Webhook %s zu posten.\n" +
                    $"Stelle sicher, dass\n" +
                    $"- die Adresse des Webhooks korrekt ist\n" +
                    $"- der Link des genutzten Avatars korrekt ist (wenn du einen nutzt)\n" +
                    $"- du mit dem Internet verbunden bist";
            }
        }
        public string MiscDetailsMultibleBosses { get => "Verschieden"; }
        public string MiscRaidOrgaPlusNoAccount { get => "Kein konto eingestellt"; }
        public string MiscRaidOrgaPlusLoginErr { get => "Login fehlgeschlagen"; }
        public string MiscRaidOrgaPlusNoRaid { get => "Kein Termin mit Leiter rechten"; }
    }
}
