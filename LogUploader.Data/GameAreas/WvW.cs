using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogUploader.Data.GameAreas
{
    public class AbstractWvW : GameArea
    {
        protected const string DEFAULT_NAME_EN = "World versus World";
        protected const string DEFAULT_NAME_DE = "Welt gegen Welt";
        protected const string DEFUALT_SHORT_NAME = "WvW";
        protected const string DEFAULT_AVATAR_URL = @"https://wiki.guildwars2.com/images/d/d2/Guild_emblem_004.png";

        protected AbstractWvW(IExtendedInfo info) : base(info)
        {
        }

        public override bool Equals(object obj)
        {
            if (obj is WvW)
                return base.Equals(obj);
            return false;
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return base.ToString();
        }
        public static bool operator ==(AbstractWvW a, AbstractWvW b)
        {
            return (NamedObject)a == (NamedObject)b;
        }
        public static bool operator !=(AbstractWvW a, AbstractWvW b)
        {
            return (NamedObject)a != (NamedObject)b;
        }
    }

    public class WvW : AbstractWvW
    {
        private static WvW wvw = null;

        private WvW(): this(DEFAULT_NAME_EN, DEFAULT_NAME_DE, DEFUALT_SHORT_NAME, DEFAULT_AVATAR_URL)
        {
            
        }

        private WvW(string name, string shortName, string avatarURL) : this(name, name, shortName, shortName, avatarURL)
        { }

        private WvW(string nameEN, string nameDE, string shortName, string avatarURL) : this(nameEN, nameDE, shortName, shortName, avatarURL)
        { }

        private WvW(string nameEN, string nameDE, string shortNameEN, string shortNameDE, string avatarURL) : this(new ExtendedInfo(nameEN, nameDE, shortNameEN, shortNameDE, avatarURL))
        { }

        private WvW(IExtendedInfo info) : base(info)
        {
            wvw = this;
        }


        public static WvW Create()
        {
            if (wvw != null)
                return wvw;
            return new WvW();
        }

        public static WvW Create(string name, string shortName, string avatarURL)
        {
            if (wvw != null)
                return wvw;
            return new WvW(name, shortName, avatarURL);
        }

        public static WvW Create(string nameEN, string nameDE, string shortName, string avatarURL)
        {
            if (wvw != null)
                return wvw;
            return new WvW(nameEN, nameDE, shortName, avatarURL);
        }

        public static WvW Create(string nameEN, string nameDE, string shortNameEN, string shortNameDE, string avatarURL)
        {
            if (wvw != null)
                return wvw;
            return new WvW(nameEN, nameDE, shortNameEN, shortNameDE, avatarURL);
        }

        public static WvW Create(IExtendedInfo info)
        {
            if (wvw != null)
                return wvw;
            return new WvW(info);
        }

        public static WvW Get() => wvw;

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public static bool operator ==(WvW a, WvW b)
        {
            return (AbstractWvW)a == (AbstractWvW)b;
        }
        public static bool operator !=(WvW a, WvW b)
        {
            return (AbstractWvW)a != (AbstractWvW)b;
        }
    }
}
