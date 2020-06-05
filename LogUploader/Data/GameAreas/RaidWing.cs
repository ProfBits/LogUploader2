using LogUploader.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogUploader.Data.GameAreas
{
    public class RaidWing : GameArea
    {
        private const string DEFAULT_NAME_EN = "Unkown Raid Wing";
        private const string DEFAULT_NAME_DE = "Unbekannter Schlachtzug";
        private const string DEFUALT_SHORT_NAME_EN = "Wing ???";
        private const string DEFUALT_SHORT_NAME_DE = "Flügel ???";
        private const string DEFAULT_AVATAR_URL = @"https://wiki.guildwars2.com/images/d/d2/Guild_emblem_004.png";

        private readonly int Number = -1;

        private RaidWing() : base(DEFAULT_NAME_EN, DEFAULT_NAME_DE, DEFUALT_SHORT_NAME_EN, DEFUALT_SHORT_NAME_DE, DEFAULT_AVATAR_URL)
        {
            RegisterWing(this);
        }

        internal RaidWing(string name, int number, string avatarURL) : this(name, name, number, avatarURL)
        { }

        internal RaidWing(string nameEN, string nameDE, int number, string avatarURL) : this(new BasicInfo(nameEN, nameDE, avatarURL), number)
        { }

        internal RaidWing(IBasicInfo info, int number) : base(info, $"Wing {number}", $"Flügel {number}")
        {
            Number = number;
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

        public override string ToString()
        {
            return base.ToString();
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

        public static bool operator ==(RaidWing a, RaidWing b)
        {
            return GP.Compare(a, b);
        }
        public static bool operator !=(RaidWing a, RaidWing b)
        {
            return !(a == b);
        }
    }
}
