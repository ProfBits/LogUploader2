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
    public class AbstractAddEnemy : Enemy
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
            if (obj is AddEnemy e)
                return ID == e.ID;
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
        public static bool operator ==(AbstractAddEnemy a, AbstractAddEnemy b)
        {
            if ((object)a == null)
                return (object)b == null;

            return a.Equals(b);
        }

        public static bool operator !=(AbstractAddEnemy a, AbstractAddEnemy b)
        {
            return !(a == b);
        }
    }

    public class AddEnemy : AbstractAddEnemy
    {
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

        public static AddEnemy GetByName(string name)
        {
            return allAdds.FirstOrDefault((e) => e.Value.Name == name).Value;
        }

        public static AddEnemy GetByName(string name, eLanguage lang)
        {
            return allAdds.FirstOrDefault((e) => e.Value.GetName(lang) == name).Value;
        }

        public static List<AddEnemy> GetByArea(GameArea area)
        {
            return allAdds.Where(e => e.Value.Area == area).Select(e => e.Value).ToList();
        }

        public static List<AddEnemy> All { get => allAdds.Values.ToList(); }

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
