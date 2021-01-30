﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LogUploader.Data.GameAreas;
using LogUploader.Helper;
using LogUploader.Localisation;

namespace LogUploader.Data
{
    public class AddEnemy : Enemy
    {
        public bool IsInteresting { get; }

        private static readonly Dictionary<int, AddEnemy> allAdds = new Dictionary<int, AddEnemy>();

        public AddEnemy() : this(0, "Unknown", "Unbekannt", Unknowen.Get(), true)
        { }

        public AddEnemy(int id, string name, GameArea area, bool isIntressting) : this(id, name, name, area, isIntressting)
        { }

        public AddEnemy(int id, string nameEN, string nameDE, GameArea area, bool isIntersting) : base(id, nameEN, nameDE, area)
        {
            IsInteresting = isIntersting;
            allAdds.Add(id, this);
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

        public override bool Equals(object obj)
        {
            if (obj is AddEnemy e)
                return ID == e.ID;
            return false;
        }

        public override string ToString()
        {
            return base.ToString();
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public static bool ExistsID(int id)
        {
            return allAdds.ContainsKey(id);
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
