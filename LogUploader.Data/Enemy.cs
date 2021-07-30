using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LogUploader.Data.GameAreas;

namespace LogUploader.Data
{
    public abstract class Enemy : NamedObject, IEquatable<Enemy>
    {
        public int ID { get; }
        public GameAreas.GameArea Area { get; }

        internal Enemy(int id, string name, GameArea area) : this(id, name, name, area)
        {

        }

        internal Enemy(int id, string nameEN, string nameDE, GameArea area) : base(Tools.GP.ValidateStringMultiWord(nameEN), Tools.GP.ValidateStringMultiWord(nameDE))
        {
            if (id < 0) throw new ArgumentOutOfRangeException(nameof(id), id, "The id of an enemy cannot be less than 0.");
            ID = id;
            Area = area ?? throw new ArgumentNullException(nameof(area), "The area of an enemy cannot be null.");
        }

        internal Enemy(BasicInfo info) : this(info.ID, info.NameEN, info.NameDE, info.GameArea)
        { }

        public struct BasicInfo
        {
            public int ID { get; }
            public string NameEN { get; }
            public string NameDE { get; }
            public GameArea GameArea { get; }

            public BasicInfo(int iD, string nameEN, string nameDE, GameArea gameArea)
            {
                ID = iD;
                NameEN = nameEN;
                NameDE = nameDE;
                GameArea = gameArea;
            }
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Enemy);
        }

        public bool Equals(Enemy other)
        {
            return other != null &&
                   ID == other.ID;
        }

        public override int GetHashCode()
        {
            int hashCode = 2082127350;
            hashCode = hashCode * -1521134295 + ID.GetHashCode();
            return hashCode;
        }

        public static bool operator ==(Enemy left, Enemy right)
        {
            return EqualityComparer<Enemy>.Default.Equals(left, right);
        }

        public static bool operator !=(Enemy left, Enemy right)
        {
            return !(left == right);
        }
    }
}
