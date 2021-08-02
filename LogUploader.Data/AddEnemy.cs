using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LogUploader.Helper;
using LogUploader.Localisation;

using static LogUploader.Data.Enemy;

namespace LogUploader.Data
{
    public class AddEnemy : Enemy, IEquatable<AddEnemy>
    {
        public bool IsInteresting { get; }

        public AddEnemy() : this(0, "Unknown", "Unbekannt", new UnkowenGameArea(), true)
        { }

        public AddEnemy(int id, string name, GameArea area, bool isIntressting) : this(id, name, name, area, isIntressting)
        { }

        public AddEnemy(int id, string nameEN, string nameDE, GameArea area, bool isIntersting) : base(id, nameEN, nameDE, area)
        {
            IsInteresting = isIntersting;
        }

        public AddEnemy(BasicInfo info, bool isIntersting) : this(info.ID, info.NameEN, info.NameDE, info.GameArea, isIntersting)
        { }

        public override bool Equals(object obj)
        {
            return Equals(obj as AddEnemy);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public bool Equals(AddEnemy other)
        {
            return other != null &&
                   base.Equals(other);
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
