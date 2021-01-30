using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogUploader.Data.GameAreas
{
    public class Unknowen : GameArea
    {

        private const string DEFAULT_NAME_EN = "Unknowen";
        private const string DEFAULT_NAME_DE = "Unbekannt";
        private const string DEFUALT_SHORT_NAME = "???";
        private const string DEFAULT_AVATAR_URL = @"https://wiki.guildwars2.com/images/d/d2/Guild_emblem_004.png";

        private static Unknowen unknowen = null;

        private Unknowen() : this(DEFAULT_NAME_EN, DEFAULT_NAME_DE, DEFUALT_SHORT_NAME, DEFAULT_AVATAR_URL)
        {
        }

        private Unknowen(string name, string shortName, string avatarURL) : this(name, name, shortName, shortName, avatarURL)
        {
        }

        private Unknowen(string nameEN, string nameDE, string shortName, string avatarURL) : this(nameEN, nameDE, shortName, shortName, avatarURL)
        {
        }

        private Unknowen(string nameEN, string nameDE, string shortNameEN, string shortNameDE, string avatarURL) : this(new ExtendedInfo(nameEN, nameDE, shortNameEN, shortNameDE, avatarURL))
        { }

        private Unknowen(IExtendedInfo info) : base(info)
        {
            unknowen = this;
        }

        public static Unknowen Create()
        {
            if (unknowen != null)
                return unknowen;
            return new Unknowen();
        }

        public static Unknowen Create(string name, string shortName, string avatarURL)
        {
            if (unknowen != null)
                return unknowen;
            return new Unknowen(name, shortName, avatarURL);
        }

        public static Unknowen Create(string nameEN, string nameDE, string shortName, string avatarURL)
        {
            if (unknowen != null)
                return unknowen;
            return new Unknowen(nameEN, nameDE, shortName, avatarURL);
        }

        public static Unknowen Create(string nameEN, string nameDE, string shortNameEN, string shortNameDE, string avatarURL)
        {
            if (unknowen != null)
                return unknowen;
            return new Unknowen(nameEN, nameDE, shortNameEN, shortNameDE, avatarURL);
        }

        public static Unknowen Create(IExtendedInfo info)
        {
            if (unknowen != null)
                return unknowen;
            return new Unknowen(info);
        }

        public static Unknowen Get() => unknowen;

        public override string ToString()
        {
            return base.ToString();
        }

        public override bool Equals(object obj)
        {
            if (obj is Unknowen)
                return base.Equals(obj);
            return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public static bool operator ==(Unknowen a, Unknowen b)
        {
            return (NamedObject)a == (NamedObject)b;
        }
        public static bool operator !=(Unknowen a, Unknowen b)
        {
            return (NamedObject)a != (NamedObject)b;
        }

    }
}
