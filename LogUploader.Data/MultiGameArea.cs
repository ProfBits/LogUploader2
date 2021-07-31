using System;
using System.Collections.Generic;

namespace LogUploader.Data.New
{
    public abstract class MultiGameArea : GameArea, IEquatable<MultiGameArea>
    {
        public int ID { get; }

        internal MultiGameArea(int id, string nameEN, string nameDE, string shortNameEN, string shortNameDE, string avatarURL) :
            base (nameEN, nameDE, shortNameEN, shortNameDE, avatarURL)
        {
            ID = id;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as MultiGameArea);
        }

        public bool Equals(MultiGameArea other)
        {
            return other != null &&
                   base.Equals(other) &&
                   ID == other.ID;
        }

        public override int GetHashCode()
        {
            int hashCode = 2082127350;
            hashCode = hashCode * -1521134295 + base.GetHashCode();
            hashCode = hashCode * -1521134295 + ID.GetHashCode();
            return hashCode;
        }

        public static bool operator ==(MultiGameArea left, MultiGameArea right)
        {
            return EqualityComparer<MultiGameArea>.Default.Equals(left, right);
        }

        public static bool operator !=(MultiGameArea left, MultiGameArea right)
        {
            return !(left == right);
        }
    }

    public class Raid : MultiGameArea, IEquatable<Raid>
    {
        internal const int DEFAULT_ID = 0;
        internal const string DEFAULT_NAME_EN = "Unkown Raid Wing";
        internal const string DEFAULT_NAME_DE = "Unbekannter Schlachtzug";
        internal const string DEFUALT_SHORT_NAME_EN = "Wing ???";
        internal const string DEFUALT_SHORT_NAME_DE = "Flügel ???";
        internal const string DEFAULT_AVATAR_URL = @"https://wiki.guildwars2.com/images/d/d2/Guild_emblem_004.png";

        internal Raid() :
            this(DEFAULT_ID, DEFAULT_NAME_EN, DEFAULT_NAME_DE, DEFUALT_SHORT_NAME_EN, DEFUALT_SHORT_NAME_DE, DEFAULT_AVATAR_URL)
        { }

        public Raid(int id, string nameEN, string nameDE, string shortNameEN, string shortNameDE, string avatarURL) :
            base(id, nameEN, nameDE, shortNameEN, shortNameDE, avatarURL)
        {
        }

        public Raid(int id, string nameEN, string nameDE, string avatarURL) :
            base(id, nameEN, nameDE, $"Wing {id}", $"Flügel {id}", avatarURL)
        {
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Raid);
        }

        public bool Equals(Raid other)
        {
            return other != null &&
                   ID == other.ID;
        }

        public override int GetHashCode()
        {
            int hashCode = 2082127350;
            hashCode = hashCode * -1521134295 + GetType().GetHashCode();
            hashCode = hashCode * -1521134295 + ID.GetHashCode();
            return hashCode;
        }

        public static bool operator ==(Raid left, Raid right)
        {
            return EqualityComparer<Raid>.Default.Equals(left, right);
        }

        public static bool operator !=(Raid left, Raid right)
        {
            return !(left == right);
        }
    }

    public class Strike : MultiGameArea, IEquatable<Strike>
    {
        internal const int DEFAULT_ID = 0;
        internal const string DEFAULT_NAME_EN = "Unkown Strike Mission";
        internal const string DEFAULT_NAME_DE = "Unbekannte Angriffs Mission";
        internal const string DEFUALT_SHORT_NAME_EN = "Strike ???";
        internal const string DEFUALT_SHORT_NAME_DE = "Angriff ???";
        internal const string DEFAULT_AVATAR_URL = @"https://wiki.guildwars2.com/images/d/d2/Guild_emblem_004.png";

        internal Strike() :
            this(DEFAULT_ID, DEFAULT_NAME_EN, DEFAULT_NAME_DE, DEFUALT_SHORT_NAME_EN, DEFUALT_SHORT_NAME_DE, DEFAULT_AVATAR_URL)
        { }

        public Strike(int id, string nameEN, string nameDE, string shortNameEN, string shortNameDE, string avatarURL) :
            base(id, nameEN, nameDE, shortNameEN, shortNameDE, avatarURL)
        {
        }

        public Strike(int id, string nameEN, string nameDE, string avatarURL) :
            base(id, nameEN, nameDE, $"Strike {id}", $"Angriff {id}", avatarURL)
        {
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Strike);
        }

        public bool Equals(Strike other)
        {
            return other != null &&
                   ID == other.ID;
        }

        public override int GetHashCode()
        {
            int hashCode = 2082127350;
            hashCode = hashCode * -1521134295 + GetType().GetHashCode();
            hashCode = hashCode * -1521134295 + ID.GetHashCode();
            return hashCode;
        }

        public static bool operator ==(Strike left, Strike right)
        {
            return EqualityComparer<Strike>.Default.Equals(left, right);
        }

        public static bool operator !=(Strike left, Strike right)
        {
            return !(left == right);
        }
    }

    public class Fractal : MultiGameArea, IEquatable<Fractal>
    {
        internal const int DEFAULT_ID = 0;
        internal const string DEFAULT_NAME_EN = "Unkown Fractal";
        internal const string DEFAULT_NAME_DE = "Unbekanntes Fraktal";
        internal const string DEFUALT_SHORT_NAME_EN = "Fractal ???";
        internal const string DEFUALT_SHORT_NAME_DE = "Fraktal ???";
        internal const string DEFAULT_AVATAR_URL = @"https://wiki.guildwars2.com/images/d/d2/Guild_emblem_004.png";

        internal Fractal() :
            this(DEFAULT_ID, DEFAULT_NAME_EN, DEFAULT_NAME_DE, DEFUALT_SHORT_NAME_EN, DEFUALT_SHORT_NAME_DE, DEFAULT_AVATAR_URL)
        { }

        public Fractal(int id, string nameEN, string nameDE, string shortNameEN, string shortNameDE, string avatarURL) :
            base(id, nameEN, nameDE, shortNameEN, shortNameDE, avatarURL)
        {
        }

        public Fractal(int id, string nameEN, string nameDE, string avatarURL) :
            base(id, nameEN, nameDE, $"Fractal {id}", $"Fraktal {id}", avatarURL)
        {
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Fractal);
        }

        public bool Equals(Fractal other)
        {
            return other != null &&
                   ID == other.ID;
        }

        public override int GetHashCode()
        {
            int hashCode = 2082127350;
            hashCode = hashCode * -1521134295 + GetType().GetHashCode();
            hashCode = hashCode * -1521134295 + ID.GetHashCode();
            return hashCode;
        }

        public static bool operator ==(Fractal left, Fractal right)
        {
            return EqualityComparer<Fractal>.Default.Equals(left, right);
        }

        public static bool operator !=(Fractal left, Fractal right)
        {
            return !(left == right);
        }
    }

    public class DragonResponseMission : MultiGameArea, IEquatable<DragonResponseMission>
    {
        internal const int DEFAULT_ID = 0;
        internal const string DEFAULT_NAME_EN = "Unkown Dragon Response Mission";
        internal const string DEFAULT_NAME_DE = "Unbekannte Drachenhilfe-Mission";
        internal const string DEFUALT_SHORT_NAME_EN = "DRM ???";
        internal const string DEFUALT_SHORT_NAME_DE = "DHM ???";
        internal const string DEFAULT_AVATAR_URL = @"https://wiki.guildwars2.com/images/f/fe/Destroyer_Crab.jpg";

        internal DragonResponseMission() :
            this(DEFAULT_ID, DEFAULT_NAME_EN, DEFAULT_NAME_DE, DEFUALT_SHORT_NAME_EN, DEFUALT_SHORT_NAME_DE, DEFAULT_AVATAR_URL)
        { }

        public DragonResponseMission(int id, string nameEN, string nameDE, string shortNameEN, string shortNameDE, string avatarURL) :
            base(id, nameEN, nameDE, shortNameEN, shortNameDE, avatarURL)
        {
        }

        public DragonResponseMission(int id, string nameEN, string nameDE, string avatarURL) :
            base(id, nameEN, nameDE, $"DRM {id}", $"DHM {id}", avatarURL)
        {
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as DragonResponseMission);
        }

        public bool Equals(DragonResponseMission other)
        {
            return other != null &&
                   ID == other.ID;
        }

        public override int GetHashCode()
        {
            int hashCode = 2082127350;
            hashCode = hashCode * -1521134295 + GetType().GetHashCode();
            hashCode = hashCode * -1521134295 + ID.GetHashCode();
            return hashCode;
        }

        public static bool operator ==(DragonResponseMission left, DragonResponseMission right)
        {
            return EqualityComparer<DragonResponseMission>.Default.Equals(left, right);
        }

        public static bool operator !=(DragonResponseMission left, DragonResponseMission right)
        {
            return !(left == right);
        }
    }
}
