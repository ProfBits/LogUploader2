﻿using LogUploader.Tools;

using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace LogUploader.Data
{
    public class Profession : NamedObject, IProfession
    {
        /// <summary>
        /// dict&lt;nameEN, Prfession&gt;
        /// </summary>
        private static IReadOnlyDictionary<eProfession, Profession> Professions = new Dictionary<eProfession, Profession>();

        private Profession() : base("") { }

        internal Profession(eProfession profession, string nameEN, string nameDE, string iconPath, string emote, int raidOrgaPlusID, string abbreviation) : base(nameEN, nameDE)
        {
            GP.ValidateStringOneWord(nameEN);
            GP.ValidateStringOneWord(nameDE);

            ProfessionEnum = profession;
            IconPath = iconPath;
            Icon = Image.FromFile(iconPath);
            Emote = GP.ValidateDiscordEmote(emote);
            RaidOrgaPlusID = raidOrgaPlusID;
            Abbreviation = GP.ValidateStringOneWord(abbreviation);
        }

        [Obsolete("Replaced with instance version xxxTBRxxx")]
        public static Profession Get(eProfession profession)
        {
            return Professions.Where(e => e.Key == profession).FirstOrDefault().Value ?? Professions[eProfession.Unknown];
        }

        [Obsolete("Replaced with instance version xxxTBRxxx")]
        public static Profession Get(string name)
        {
            return Professions.Select(p => p.Value).Where(p => p.NameEN == name || p.NameDE == name).FirstOrDefault() ?? Professions[eProfession.Unknown];
        }

        [Obsolete("Replaced with instance version xxxTBRxxx")]
        public static Profession Get(int roPlusID)
        {
            return Professions.Select(p => p.Value).Where(p => p.RaidOrgaPlusID == roPlusID).FirstOrDefault() ?? Professions[eProfession.Unknown];
        }

        [Obsolete("Replaced with instance version xxxTBRxxx")]
        public static Profession GetByAbbreviation(string abbreviation)
        {
            return Professions.Select(p => p.Value).Where(p => p.Abbreviation == abbreviation).FirstOrDefault() ?? Professions[eProfession.Unknown];
        }

        [Obsolete("Replaced with instance version xxxTBRxxx")]
        public static Profession Unknown { get => Get(eProfession.Unknown); }

        public eProfession ProfessionEnum { get; }
        public string IconPath { get; }
        public string Emote { get; }
        public Image Icon { get; internal set; }
        public int RaidOrgaPlusID { get; }
        public string Abbreviation { get; }

        public static void Init(string path, IProgress<double> progress = null)
        {
            progress?.Report(0);
            var strData = JsonHandling.ReadJsonFile(path);
            InitWithContent(strData, progress);
        }

        internal static void InitWithContent(string strData, IProgress<double> progress = null)
        {
            progress?.Report(0.25);
            var exePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var jsonData = Newtonsoft.Json.Linq.JObject.Parse(strData);
            progress?.Report(0.5);
            var profList = (Newtonsoft.Json.Linq.JArray)jsonData["Professions"];
            Professions = profList.Select(json =>
            {
                int id = (int)json["ID"];
                string nameEN = (string)json["NameEN"];
                string nameDE = (string)json["NameDE"];
                string iconPath = exePath + (string)json["IconPath"];
                string emote = (string)json["Emote"];
                int raidOrgaPlusID = (int)json["RaidOrgaPlusID"];
                string abbreviation = (string)json["RaidOrgaPlusAbbreviation"];

                return new Profession(GP.IntToEnum<eProfession>(id), nameEN, nameDE, iconPath, emote, raidOrgaPlusID, abbreviation);
            })
            .ToDictionary(prof => prof.ProfessionEnum);
            progress?.Report(1);
        }

        public override string ToString()
        {
            return Name;
        }

        public static bool operator ==(Profession a, Profession b)
        {
            return a?.ProfessionEnum == b?.ProfessionEnum;
        }
        public static bool operator !=(Profession a, Profession b)
        {
            return !(a == b);
        }
        public static bool operator ==(Profession a, eProfession b)
        {
            return a.ProfessionEnum == b;
        }
        public static bool operator !=(Profession a, eProfession b)
        {
            return !(a == b);
        }
        public static bool operator ==(eProfession a, Profession b)
        {
            return a == b.ProfessionEnum;
        }
        public static bool operator !=(eProfession a, Profession b)
        {
            return !(a == b);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (obj is null)
            {
                return false;
            }

            if (obj is Profession p)
            {
                return this == p;
            }
            if (obj is eProfession e)
            {
                return this == e;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return ProfessionEnum.GetHashCode();
        }

        public bool Equals(IProfession other)
        {
            return this == (Profession)other;
        }

        public bool Equals(eProfession other)
        {
            return this == other;
        }
    }
}
