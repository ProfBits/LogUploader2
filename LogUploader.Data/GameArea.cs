using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LogUploader.Localisation;

namespace LogUploader.Data.New
{
    public abstract class GameArea : NamedObject, IAvatar, IEquatable<GameArea>
    {
        private readonly NamedObject shortName;

        internal GameArea(string nameEN, string nameDE, string shortNameEN, string shortNameDE, string avatarURL) :
            base(Tools.GP.ValidateStringMultiWord(nameEN), Tools.GP.ValidateStringMultiWord(nameDE))
        {
            shortName = new NamedObject(Tools.GP.ValidateStringMultiWord(shortNameEN), Tools.GP.ValidateStringMultiWord(shortNameDE));
            AvatarURL = Tools.GP.ValidateStringMultiWord(avatarURL);
        }

        public string ShortName { get; }
        public string ShortNameEN { get => shortName.NameEN; }
        public string ShortNameDE { get => shortName.NameDE; }
        public string AvatarURL { get; }

        public string GetShortName(eLanguage lang)
        {
            return shortName.GetName(lang);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as GameArea);
        }

        public bool Equals(GameArea other)
        {
            return other != null &&
                   base.Equals(other) &&
                   GetType().Equals(other.GetType()) &&
                   EqualityComparer<NamedObject>.Default.Equals(shortName, other.shortName) &&
                   AvatarURL == other.AvatarURL;
        }

        public override int GetHashCode()
        {
            int hashCode = -1988182647;
            hashCode = hashCode * -1521134295 + base.GetHashCode();
            hashCode = hashCode * -1521134295 + GetType().GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<NamedObject>.Default.GetHashCode(shortName);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(AvatarURL);
            return hashCode;
        }
        public static bool operator ==(GameArea left, GameArea right)
        {
            return EqualityComparer<GameArea>.Default.Equals(left, right);
        }

        public static bool operator !=(GameArea left, GameArea right)
        {
            return !(left == right);
        }
    }

    public class Training : GameArea, IEquatable<Training>
    {
        internal const string DEFAULT_NAME_EN = "Special Forces Trainings Area";
        internal const string DEFAULT_NAME_DE = "Übungsgelände der Spezialkräfte";
        internal const string DEFUALT_SHORT_NAME_EN = "Golem";
        internal const string DEFUALT_SHORT_NAME_DE = "Golem";
        internal const string DEFAULT_AVATAR_URL = @"https://wiki.guildwars2.com/images/d/d2/Guild_emblem_004.png";

        internal Training() :
            this(DEFAULT_NAME_EN, DEFAULT_NAME_DE, DEFUALT_SHORT_NAME_EN, DEFUALT_SHORT_NAME_DE, DEFAULT_AVATAR_URL)
        { }

        internal Training(string nameEN, string nameDE, string shortNameEN, string shortNameDE, string avatarURL) :
            base(nameEN, nameDE, shortNameEN, shortNameDE, avatarURL)
        {
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Training);
        }

        public bool Equals(Training other)
        {
            return other != null &&
                   base.Equals(other);
        }

        public override int GetHashCode()
        {
            return 624022166 + base.GetHashCode();
        }

        public static bool operator ==(Training left, Training right)
        {
            return EqualityComparer<Training>.Default.Equals(left, right);
        }

        public static bool operator !=(Training left, Training right)
        {
            return !(left == right);
        }
    }

    public class WvW : GameArea, IEquatable<WvW>
    {
        internal const string DEFAULT_NAME_EN = "World versus World";
        internal const string DEFAULT_NAME_DE = "Welt gegen Welt";
        internal const string DEFUALT_SHORT_NAME_EN = "WvW";
        internal const string DEFUALT_SHORT_NAME_DE = "WvW";
        internal const string DEFAULT_AVATAR_URL = @"https://wiki.guildwars2.com/images/d/d2/Guild_emblem_004.png";

        internal WvW() :
            this(DEFAULT_NAME_EN, DEFAULT_NAME_DE, DEFUALT_SHORT_NAME_EN, DEFUALT_SHORT_NAME_DE, DEFAULT_AVATAR_URL)
        { }

        public WvW(string nameEN, string nameDE, string shortNameEN, string shortNameDE, string avatarURL) : base(nameEN, nameDE, shortNameEN, shortNameDE, avatarURL)
        {
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as WvW);
        }

        public bool Equals(WvW other)
        {
            return other != null &&
                   base.Equals(other);
        }

        public override int GetHashCode()
        {
            return 624022166 + base.GetHashCode();
        }

        public static bool operator ==(WvW left, WvW right)
        {
            return EqualityComparer<WvW>.Default.Equals(left, right);
        }

        public static bool operator !=(WvW left, WvW right)
        {
            return !(left == right);
        }
    }

    public class UnkowenGameArea : GameArea, IEquatable<UnkowenGameArea>
    {
        protected const string DEFAULT_NAME_EN = "Unknowen";
        protected const string DEFAULT_NAME_DE = "Unbekannt";
        protected const string DEFUALT_SHORT_NAME_EN = "???";
        protected const string DEFUALT_SHORT_NAME_DE = "???";
        protected const string DEFAULT_AVATAR_URL = @"https://wiki.guildwars2.com/images/d/d2/Guild_emblem_004.png";

        internal UnkowenGameArea() :
            this(DEFAULT_NAME_EN, DEFAULT_NAME_DE, DEFUALT_SHORT_NAME_EN, DEFUALT_SHORT_NAME_DE, DEFAULT_AVATAR_URL)
        { }

        public UnkowenGameArea(string nameEN, string nameDE, string shortNameEN, string shortNameDE, string avatarURL) : base(nameEN, nameDE, shortNameEN, shortNameDE, avatarURL)
        {
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as UnkowenGameArea);
        }

        public bool Equals(UnkowenGameArea other)
        {
            return other != null &&
                   base.Equals(other);
        }

        public override int GetHashCode()
        {
            return 624022166 + base.GetHashCode();
        }

        public static bool operator ==(UnkowenGameArea left, UnkowenGameArea right)
        {
            return EqualityComparer<UnkowenGameArea>.Default.Equals(left, right);
        }

        public static bool operator !=(UnkowenGameArea left, UnkowenGameArea right)
        {
            return !(left == right);
        }
    }

}
