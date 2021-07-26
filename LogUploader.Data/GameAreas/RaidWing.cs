using LogUploader.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogUploader.Data.GameAreas
{
    public abstract class AbstractRaidWing : GameArea
    {
        protected const string DEFAULT_NAME_EN = "Unkown Raid Wing";
        protected const string DEFAULT_NAME_DE = "Unbekannter Schlachtzug";
        protected const string DEFUALT_SHORT_NAME_EN = "Wing ???";
        protected const string DEFUALT_SHORT_NAME_DE = "Flügel ???";
        protected const string DEFAULT_AVATAR_URL = @"https://wiki.guildwars2.com/images/d/d2/Guild_emblem_004.png";
        
        protected readonly int Number = -1;

        protected AbstractRaidWing() : base(DEFAULT_NAME_EN, DEFAULT_NAME_DE, DEFUALT_SHORT_NAME_EN, DEFUALT_SHORT_NAME_DE, DEFAULT_AVATAR_URL)
        {
        }

        public AbstractRaidWing(string name, int number, string avatarURL) : this(name, name, number, avatarURL)
        { }

        public AbstractRaidWing(string nameEN, string nameDE, int number, string avatarURL) : this(new BasicInfo(nameEN, nameDE, avatarURL), number)
        { }

        public AbstractRaidWing(IBasicInfo info, int number) : base(info, $"Wing {number}", $"Flügel {number}")
        {
            Number = number;
        }

        public override bool Equals(object obj)
        {
            if (obj is RaidWing r)
                return Number == r.Number;
            return false;
        }

        public override int GetHashCode()
        {
            return (base.GetHashCode() + Number).GetHashCode();
        }

        public override string ToString()
        {
            return base.ToString();
        }

        public static bool operator ==(AbstractRaidWing a, AbstractRaidWing b)
        {
            if ((object)a == null)
                return (object)b == null;

            return a.Equals(b);
        }
        public static bool operator !=(AbstractRaidWing a, AbstractRaidWing b)
        {
            return !(a == b);
        }
    }

    public class RaidWing : AbstractRaidWing
    {
        protected RaidWing() : base()
        {
            RegisterWing(this);
        }

        public RaidWing(string name, int number, string avatarURL) : this(name, name, number, avatarURL)
        { }

        public RaidWing(string nameEN, string nameDE, int number, string avatarURL) : this(new BasicInfo(nameEN, nameDE, avatarURL), number)
        { }

        public RaidWing(IBasicInfo info, int number) : base(info, number)
        {
            RegisterWing(this);
        }

        static RaidWing()
        {
            new RaidWing();
        }

        private static readonly Dictionary<int, RaidWing> RegisteredWings = new Dictionary<int, RaidWing>();

        public RaidWing this[int number]
        {
            get
            {
                if (RegisteredWings.ContainsKey(number))
                    return RegisteredWings[number];
                throw new ArgumentException($"Wing {number} does not exist");
            }
        }

        public static Dictionary<int, RaidWing> RaidWings { get => RegisteredWings; }

        private void RegisterWing(RaidWing wing)
        {
            RegisteredWings.Add(wing.Number, wing);
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public static bool operator ==(RaidWing a, RaidWing b)
        {
            if ((object)a == null)
                return (object)b == null;

            return a.Equals(b);
        }
        public static bool operator !=(RaidWing a, RaidWing b)
        {
            return !(a == b);
        }
    }
}
