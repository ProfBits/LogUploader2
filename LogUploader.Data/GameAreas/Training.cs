using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogUploader.Data.GameAreas
{
    public class Training : GameArea
    {
        private const string DEFAULT_NAME_EN = "Special Forces Trainings Area";
        private const string DEFAULT_NAME_DE = "Übungsgelände der Spezialkräfte";
        private const string DEFUALT_SHORT_NAME = "Golem";
        private const string DEFAULT_AVATAR_URL = @"https://wiki.guildwars2.com/images/d/d2/Guild_emblem_004.png";

        private static Training training = null;

        private Training() : this(DEFAULT_NAME_EN, DEFAULT_NAME_DE, DEFUALT_SHORT_NAME, DEFAULT_AVATAR_URL)
        {

        }

        private Training(string name, string shortName, string avatarURL) : this(name, name, shortName, shortName, avatarURL)
        {
        }

        private Training(string nameEN, string nameDE, string shortName, string avatarURL) : this(nameEN, nameDE, shortName, shortName, avatarURL)
        {
        }

        private Training(string nameEN, string nameDE, string shortNameEN, string shortNameDE, string avatarURL) : this(new ExtendedInfo(nameEN, nameDE, shortNameEN, shortNameDE, avatarURL))
        {
        }

        private Training(IExtendedInfo info) : base(info)
        {
            training = this;
        }



        public static Training Create()
        {
            if (training != null)
                return training;
            return new Training();
        }

        public static Training Create(string name, string shortName, string avatarURL)
        {
            if (training != null)
                return training;
            return new Training(name, shortName, avatarURL);
        }

        public static Training Create(string nameEN, string nameDE, string shortName, string avatarURL)
        {
            if (training != null)
                return training;
            return new Training(nameEN, nameDE, shortName, avatarURL);
        }

        public static Training Create(string nameEN, string nameDE, string shortNameEN, string shortNameDE, string avatarURL)
        {
            if (training != null)
                return training;
            return new Training(nameEN, nameDE, shortNameEN, shortNameDE, avatarURL);
        }

        public static Training Create(IExtendedInfo info)
        {
            if (training != null)
                return training;
            return new Training(info);
        }

        public static Training Get() => training;

        public override string ToString()
        {
            return base.ToString();
        }

        public override bool Equals(object obj)
        {
            if (obj is Training)
                return base.Equals(obj);
            return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public static bool operator ==(Training a, Training b)
        {
            return (NamedObject)a == (NamedObject)b;
        }
        public static bool operator !=(Training a, Training b)
        {
            return (NamedObject)a != (NamedObject)b;
        }
    }
}
