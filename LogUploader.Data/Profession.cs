using LogUploader.Tools;

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

        public eProfession ProfessionEnum { get; }
        public string IconPath { get; }
        public string Emote { get; }
        public Image Icon { get; internal set; }
        public int RaidOrgaPlusID { get; }
        public string Abbreviation { get; }

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
