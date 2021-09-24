using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogUploader
{
    public enum eLanguage
    {
        [CombBoxView("English")]
        EN,
        [CombBoxView("German")]
        DE
    }

    public enum eDiscordPostFormat
    {
        [ObjectName("per Boss", "pro Boss")]
        PerBoss,
        [ObjectName("per Area", "pro Gebiet")]
        PerArea,
        [ObjectName("per Try Detaild", "pro Versuch ausführlich")]
        PerTryDetaild,
        [ObjectName("per Area with emotes", "pro Gebiet mit Emotes")]
        PerAreaEmotes,
        [ObjectName("per Area with classes", "pro Gebiet mit Klassen")]
        PerAreaClasses,
        [ObjectName("compact with emotes", "kompakt mit Emotes")]
        CompactWithEmotes,
        [ObjectName("compact with classes", "kompakt mit Klassen")]
        CompactWithClasses
    }

    public enum eProfession
    {
        Unknown = 0,

        Warrior = 1,
        Guardian = 2,
        Revenant = 3,
        Engineer = 4,
        Ranger = 5,
        Thief = 6,
        Elementalist = 7,
        Mesmer = 8,
        Necromancer = 9,

        Berserker = 10,
        Dragonhunter = 11,
        Herald = 12,
        Scrapper = 13,
        Druid = 14,
        Daredevil = 15,
        Tempest = 16,
        Chronomancer = 17,
        Reaper = 18,

        Spellbreaker = 19,
        Firebrand = 20,
        Renegade = 21,
        Holosmith = 22,
        Soulbeast = 23,
        Deadeye = 24,
        Weaver = 25,
        Mirage = 26,
        Scourge = 27,

        Bladesworn = 28,
        Willbender = 29,
        Vindicator = 30,
        Catalyst = 34,
        Virtuoso = 35,
        Harbinger = 36
    }
    public enum eBosses : int
    {
        Unknown = 0,
        WorldVersusWorld = 1,
        // Raid
        ValeGuardian = 15438,
        Gorseval = 15429,
        Sabetha = 15375,
        Slothasor = 16123,
        Berg = 16088,
        Zane = 16137,
        Narella = 16125,
        Matthias = 16115,
        Escort = 16253, // McLeod the Silent
        KeepConstruct = 16235,
        TwistedCastle = 16247,
        Xera = 16246,
        Cairn = 17194,
        MursaatOverseer = 17172,
        Samarog = 17188,
        Deimos = 17154,
        SoullessHorror = 19767,
        Desmina = 19828,
        BrokenKing = 19691,
        SoulEater = 19536,
        EyeOfJudgement = 19651,
        EyeOfFate = 19844,
        Dhuum = 19450,
        ConjuredAmalgamate = 43974, // Gadget
        Nikare = 21105,
        Kenut = 21089,
        Qadim = 20934,
        Adina = 22006,
        Sabir = 21964,
        PeerlessQadim = 22000,
        Freezie = 21333,
        // Strike Missions
        IcebroodConstruct = 22154,
        VoiceOfTheFallen = 22343,
        ClawOfTheFallen = 22481,
        VoiceAndClaw = 22315,
        FraenirOfJormag = 22492,
        IcebroodConstructFraenir = 22436,
        Boneskinner = 22521,
        WhisperOfJormag = 22711,
        VariniaStormsounder = 22836,
        // Fract
        MAMA = 17021,
        Siax = 17028,
        Ensolyss = 16948,
        Skorvald = 17632,
        Artsariiv = 17949,
        Arkk = 17759,
        // Golems
        MassiveGolem = 16202,
        AvgGolem = 16177,
        LGolem = 19676,
        MedGolem = 19645,
        StdGolem = 16199
    };

    internal enum eLogLevel : int
    {
        [Obsolete]
        SILETN,
        ERROR,
        WARN,
        MINIMAL,
        NORMAL,
        VERBOSE,
        DEBUG,
    }

    internal enum ExitCode : int
    {
        OK = 0,

        #region Errors (1-39)
        WIN32_EXCPTION = 1,
        CLR_EXCPTION = 2,
        #endregion

        #region Development/Test (40-49)
        LANG_XML_CREATION_BUILD = 42,
        DEV_TEST = 41,
        #endregion

        #region Expected Exits (100-199)
        UPDATING = 100,
        #endregion

        #region Startup Errors (200-299)
        STARTUP_FAILED = 200,
        ALREADY_RUNNING = 201,
        INIT_SETUP_FAILED = 202,
        EI_UPDATE_FATAL_ERROR = 203,
        LOAD_SETTINGS_ERROR = 204,
        #endregion
    }
}
