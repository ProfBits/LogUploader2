using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LogUploader.Data.GameAreas;
using LogUploader.Helper;
using LogUploader.Localisation;

using static LogUploader.Data.Enemy;

namespace LogUploader.Data
{
    public class AbstractAddEnemy : Enemy, IEquatable<AbstractAddEnemy>
    {
        public bool IsInteresting { get; }

        public AbstractAddEnemy() : this(0, "Unknown", "Unbekannt", Unknowen.Get(), true)
        { }

        public AbstractAddEnemy(int id, string name, GameArea area, bool isIntressting) : this(id, name, name, area, isIntressting)
        { }

        public AbstractAddEnemy(int id, string nameEN, string nameDE, GameArea area, bool isIntersting) : base(id, nameEN, nameDE, area)
        {
            IsInteresting = isIntersting;
        }

        public AbstractAddEnemy(BasicInfo info, bool isIntersting) : this(info.ID, info.NameEN, info.NameDE, info.GameArea, isIntersting)
        { }

        public override bool Equals(object obj)
        {
            return Equals(obj as AbstractAddEnemy);
        }

        public bool Equals(AbstractAddEnemy other)
        {
            return other != null &&
                   base.Equals(other);
        }

        public override int GetHashCode()
        {
            return 624022166 + base.GetHashCode();
        }

        public static bool operator ==(AbstractAddEnemy left, AbstractAddEnemy right)
        {
            return EqualityComparer<AbstractAddEnemy>.Default.Equals(left, right);
        }

        public static bool operator !=(AbstractAddEnemy left, AbstractAddEnemy right)
        {
            return !(left == right);
        }
    }

    public class AddEnemy : AbstractAddEnemy
    {
        [Obsolete("Replaced with instance version xxxTBRxxx")]
        private static readonly Dictionary<int, AddEnemy> allAdds = new Dictionary<int, AddEnemy>();

        public AddEnemy() : this(0, "Unknown", "Unbekannt", Unknowen.Get(), true)
        { }

        public AddEnemy(int id, string name, GameArea area, bool isIntressting) : this(id, name, name, area, isIntressting)
        { }

        public AddEnemy(int id, string nameEN, string nameDE, GameArea area, bool isIntersting) : base(id, nameEN, nameDE, area, isIntersting)
        {
            try
            {
                allAdds.Add(id, this);
            }
            catch (ArgumentException) { };
        }

        public AddEnemy(BasicInfo info, bool isIntersting) : this(info.ID, info.NameEN, info.NameDE, info.GameArea, isIntersting)
        { }

        [Obsolete("Replaced with instance version xxxTBRxxx")]
        public static AddEnemy GetByID(int id)
        {
            try
            {
                return allAdds[id];
            }
            catch (KeyNotFoundException)
            {
                return null;
            }
        }

        [Obsolete("Replaced with instance version xxxTBRxxx")]
        public static AddEnemy GetByName(string name)
        {
            return allAdds.FirstOrDefault((e) => e.Value.Name == name).Value;
        }

        [Obsolete("Replaced with instance version xxxTBRxxx")]
        public static AddEnemy GetByName(string name, eLanguage lang)
        {
            return allAdds.FirstOrDefault((e) => e.Value.GetName(lang) == name).Value;
        }

        [Obsolete("Replaced with instance version xxxTBRxxx")]
        public static List<AddEnemy> GetByArea(GameArea area)
        {
            return allAdds.Where(e => e.Value.Area == area).Select(e => e.Value).ToList();
        }

        [Obsolete("Replaced with instance version xxxTBRxxx")]
        public static List<AddEnemy> All { get => allAdds.Values.ToList(); }

        [Obsolete("Replaced with instance version xxxTBRxxx")]
        public static bool ExistsID(int id)
        {
            return allAdds.ContainsKey(id);
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public static bool operator ==(AddEnemy a, AddEnemy b)
        {
            if ((object)a == null)
                return (object)b == null;

            return a.Equals(b);
        }

        public static bool operator !=(AddEnemy a, AddEnemy b)
        {
            return !(a == b);
        }
    }
}
