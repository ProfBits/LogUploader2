using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LogUploader.Data.GameAreas;

namespace LogUploader.Data
{
    public abstract class Enemy : NamedObject
    {
        public int ID { get; }
        public GameAreas.GameArea Area { get; }

        internal Enemy(int id, string name, GameArea area) : this(id, name, name, area)
        {

        }

        internal Enemy(int id, string nameEN, string nameDE, GameArea area) : base(nameEN, nameDE)
        {
            ID = id;
            Area = area;
        }

        internal Enemy(BasicInfo info) : this(info.ID, info.NameEN, info.NameDE, info.GameArea)
        { }

        public override bool Equals(object obj)
        {
            if (obj is Enemy e)
                return ID == e.ID;
            return false;
        }

        public override string ToString()
        {
            return base.ToString();
        }

        public override int GetHashCode()
        {
            return ID.GetHashCode();
        }

        public static bool operator ==(Enemy a, Enemy b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(Enemy a, Enemy b)
        {
            return !(a == b);
        }
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
    }
}
