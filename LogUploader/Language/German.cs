using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogUploader.Languages
{
    public class German : BaseLanguage
    {
        public override CultureInfo Culture { get => new CultureInfo("de-de"); }

        public override string Succsess { get => "Erfolg"; }
        public override string Fail { get => "Fehlschlag"; }

        public override string ColHeaderBoss { get => "Boss"; }
        public override string ColHeaderDate { get => "Datum"; }
        public override string ColHeaderSize { get => "Gröse"; }
        public override string ColHeaderDataCorrected { get => "Überprüft"; }
        public override string ColHeaderDuration { get => "Dauer"; }
        public override string ColHeaderSuccess { get => "Erfolg"; }
        public override string ColHeaderHpLeft { get => "% übrig"; }
        public override string ColHeaderCM { get => "CM"; }
        public override string ColHeaderParsed { get => "Parsed"; }
        public override string ColHeaderUploaded { get => "Uploaded"; }
        public override string FilterHeader { get => "Filter"; }
        public override string FilterBoss { get => "Boss"; }
        public override string FilterHPLeft { get => "HP übrig"; }
        public override string FilterDuration { get => "Dauer"; }
        public override string FilterDate { get => "Datum"; }
        public override string FilterFrom { get => "von"; }
        public override string FilterTo { get => "bis"; }
        public override string FilterSuccess { get => "Erfolg"; }
        public override string FilterKill { get => "Kill"; }
        public override string FilterWipe { get => "Wipe"; }
        public override string FilterToday { get => "Heute"; }
        public override string FilterReset { get => "Reset"; }
        public override string DetailsHeader { get => "Details"; }
        public override string DetailsBoss { get => "Boss:"; }
        public override string DetailsDate { get => "Datum:"; }
        public override string DetailsSize { get => "Größe:"; }
        public override string DetailsCorrected { get => "Überprüft:"; }
        public override string DetailsDuration { get => "Dauer:"; }
        public override string DetailsSuccess { get => "Erfolg:"; }
        public override string DetailsHpLeft { get => "HP übrig:"; }
        public override string DetailsCM { get => "CM:"; }
        public override string DetailsParsed { get => "Parsed:"; }
        public override string DetailsUploaded { get => "Hochgeladen:"; }
        public override string DetailsOpenLocal { get => "öffnen"; }
        public override string DetailsOpenRemote { get => "öffnen"; }
        public override string DetailsAccName { get => "Account"; }
        public override string DetailsSubGroup { get => "SG"; }
        public override string DetailsDPS { get => "DPS"; }
        public override string ActionsHeader { get => "Aktionen"; }
        public override string ActionsParseLocal { get => "Parse lokal"; }
        public override string ActionsOpenLocal { get => "Öffne lokal"; }
        public override string ActionsUpload { get => "Upload"; }
        public override string ActionsOpenRemote { get => "dps.report"; }
        public override string ActionsParseAndUpload { get => "Parse lokal und upload"; }
        public override string ActionsCopyLinks { get => "Links kopieren"; }
        public override string ActionsCopied { get => "kopiert"; }
        public override string ActionsPostToDiscord { get => "in Discord posten"; }
        public override string ActionsViewInExplorer { get => "Zeige im Explorer"; }
        public override string SettingsHeader { get => "Einstellungen"; }
        public override string SettingsAutoParse { get => "Auto parse neue Logs"; }
        public override string SettingsAutoUpload { get => "Auto upload neue Logs"; }
        public override string SettingsAbout { get => "Über"; }
        public override string SettingsSettings { get => "Einstellungen ..."; }
        public override string FooterElements { get => "Elemente"; }
        public override string FooterSelected { get => "Ausgewählt"; }
        public override string FooterShown { get => "Angezeigt"; }
        public override string FooterProcessing { get => "Verarbeite"; }
        public override string FooterParsing { get => "Parse"; }
        public override string FooterUploading { get => "Uploading"; }


        public override string ProxySettings { get => "Proxy Einstellungen"; }
        public override string ProxyUse { get => "Proxy nutzen"; }
        public override string ProxyHostename { get => "Proxyserver Adresse"; }
        public override string ProxyPort { get => "Port"; }
        public override string ProxyUser { get => "Benutzername"; }
        public override string ProxyPassword { get => "Passwort"; }


        public override string AboutTitle { get => "Über"; }
        public override string AboutProjectPage { get => "Projektseite github.com"; }
        public override string AboutViewLicense { get => "Zeige Lizenz"; }
        public override string AboutSpecialThanks { get => "Vielen Dank an:"; }
        public override string AboutBetaTesters { get => "Beta Tester:"; }
        public override string AboutCopyright { get => "Copyright:"; }
        public override string AboutView3rdParty { get => "Zeige 3rd Party Bibliotheken, Software"; }
        public override string AboutForSpellCheck { get => "für Spell Checking"; }
        public override string AboutVersion { get => "Version"; }
        public override string AboutViewPatchnotes { get => "Zeige Update-Notes"; }

        public override string LicenseTitle { get => "Lizenz"; }
        public override string LicenseFor { get => "für"; }
        public override string LicenseBy { get => "von"; }

        public override string LicensesTitle { get => "Software dritter"; }
        public override string LicensesClose { get => "Schliessen"; }

        public override string SoftwareItemProject { get => "Project"; }
        public override string SoftwareItemViewLicense { get => "zeige Lizenz"; }

        public override string InitTitle { get => "First Setup"; }
        public override string InitArcPaht { get => @"Arc Logs Pfad:"; }
        public override string InitBrowse { get => "Auswählen ..."; }
        public override string InitLanguage { get => "Sprache:"; }
        public override string InitCancle { get => "Abbrechen"; }
        public override string InitStart { get => "Start"; }
        public override string InitCancelSetupText { get => "Setup wirklich abbrechen?"; }
        public override string InitCancelSetupTitel { get => "Abbruch bestätigen"; }
        public override string InitInvalidPathTitel { get => "Ungültiger Pfad"; }
        public override string InitInvalidPathText { get => @"Pfad ""...\arcdps.cbtlogs\"" is ungültig!"; }


        public override string ConfigTitle { get => "Einstellungen"; }
        public override string ConfigGeneralTitle { get => "Allgemein"; }
        public override string ConfigGeneralArcPaht { get => "Arc Log Ordner Pfad"; }
        public override string ConfigGeneralBrowse { get => "Auswählen"; }
        public override string ConfigGeneralLanguage { get => "Sprache"; }
        public override string ConfigDpsReportTitle { get => "dps.report"; }
        public override string ConfigDpsReportToken { get => "User Token:"; }
        public override string ConfigDpsReportGetToken { get => "Token generieren:"; }
        public override string ConfigDpsReportProxy { get => "Proxy Einstellungen..."; }
        public override string ConfigCopyTitle { get => "Links kopieren"; }
        public override string ConfigCopyBoss { get => "mit Bossname"; }
        public override string ConfigCopySuccess { get => "mit Erfolg"; }
        public override string ConfigCopyInline { get => "Link in gleiche Zeile wie Boss"; }
        public override string ConfigCopySpace { get => "leere Zeile zwischen Logs"; }
        public override string ConfigCopyEmotes { get => "mit RisingLight Discord Emotes"; }
        public override string ConfigDiscordTitle { get => "Discord Webhooks"; }
        public override string ConfigDiscordWebHookName { get => "Name:"; }
        public override string ConfigDiscordWebHookLink { get => "Link:"; }
        public override string ConfigDiscordWebHookFormat { get => "Format:"; }
        public override string ConfigDiscordWebHookAvatar { get => "Avatar URL:"; }
        public override string ConfigDiscordWebHookDelete { get => "Löschen"; }
        public override string ConfigDiscordNoHooks { get => "Keine Webhooks vorhanden\n\nFüge einen neuen hinzu!"; }
        public override string ConfigDiscordCount { get => "Anzahl"; }
        public override string ConfigDiscordAdd { get => "Hinzufügen"; }
        public override string ConfigDiscordOnlyUploaded { get => "Nur hochgeladene posten"; }
        public override string ConfigDiscordNameAsUsername { get => "Name als Discord Username"; }
        public override string ConfigEiTitle { get => "EliteInsights"; }
        public override string ConfigEiCombatReplay { get => "Erzeuge Combat-Replay"; }
        public override string ConfigEiLightTheme { get => "Nutze helles Thema"; }
        public override string ConfigDefault { get => "Standard"; }
        public override string ConfigCancel { get => "Abbrechen"; }
        public override string ConfigSave { get => "Speichern"; }
        public override string ConfigDefaultMsgTitel { get => "Einstellungen zurücksetzen"; }
        public override string ConfigDefaultMsgText { get => "Standard Einstellungen wiederherstellen?\nDies wird deine aktuellen Einstellungen dauerhaft löschen!"; }
        public override string ConfigDiscardMsgTitel { get => "Änderungen verwerfen?"; }
        public override string ConfigDiscardMsgText { get => "Fortfahren ohne speichern?"; }
        public override string ConfigRoPlusTitle { get => "RaidOrga+"; }
        public override string ConfigRoPlusUser { get => "Benutzer"; }
        public override string ConfigRoPlusPwd { get => "Passwort"; }

        public override string NewTitle { get => "Neuerungen in "; }
        public override string NewHeading { get => "Update-Notes"; }
        public override string NewClose { get => "Schliessen"; }

        public override string MiscGenericProcessing { get => "Verarbeite"; }
        public override string MiscNoWebhookMsgTitel { get => "Webhooks erstellen"; }
        public override string MiscNoWebhookMsgText { get => "Du hast aktuell keine WebHooks eingestellt\nGehe zu Discord Webhooks in den Einstellungen und erstelle einen neuen."; }
        public override string MiscDiscordPostGenDuration { get => "Dauer"; }
        public override string MiscDiscordPostGenHpLeft { get => "HP übrig"; }
        public override string MiscDiscordPostGenGroupDPS { get => "Gruppen DPS"; }
        public override string MiscDiscordPostGenTopDPS { get => "Top DPS"; }
        public override string MiscDiscordPostGenNoLink { get => "kein Link"; }
        public override string MiscDiscordPostErrTitle { get => "Discord-Post Fehler"; }
        public override string MiscDiscordPostErrMsg
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
        public override string MiscDetailsMultibleBosses { get => "Verschieden"; }
        public override string MiscRaidOrgaPlusNoAccount { get => "Kein konto eingestellt"; }
        public override string MiscRaidOrgaPlusLoginErr { get => "Login fehlgeschlagen"; }
        public override string MiscRaidOrgaPlusNoRaid { get => "Kein Termin mit Leiter rechten"; }
    }
}
