using LogUploader.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogUploader.Data.GameAreas
{
    public class Strike : GameArea
    {
        private const string DEFAULT_NAME_EN = "Unkown Strike Mission";
        private const string DEFAULT_NAME_DE = "Unbekannte Angriffs Mission";
        private const string DEFUALT_SHORT_NAME_EN = "Strike ???";
        private const string DEFUALT_SHORT_NAME_DE = "Angriff ???";
        private const string DEFAULT_AVATAR_URL = @"https://wiki.guildwars2.com/images/d/d2/Guild_emblem_004.png";

        private readonly int Number = -1;

        public Strike() : base(DEFAULT_NAME_EN, DEFAULT_NAME_DE, DEFUALT_SHORT_NAME_EN, DEFUALT_SHORT_NAME_DE, DEFAULT_AVATAR_URL)
        {
            RegisterStrike(this);
        }

        public Strike(string name, int number, string avatarURL) : this(name, name, number, avatarURL)
        {
        }

        public Strike(string nameEN, string nameDE, int number, string avatarURL) : this(new BasicInfo(nameEN, nameDE, avatarURL), number)
        { }

        public Strike(IBasicInfo info, int number) : base(info, $"Strike {number}", $"Angriff {number}")
        {
            Number = number;
            RegisterStrike(this);
        }

        static Strike()
        {
            new Strike();
        }

        private static readonly Dictionary<int, Strike> RegisteredStrikes = new Dictionary<int, Strike>();

        public Strike this[int number]
        {
            get
            {
                if (RegisteredStrikes.ContainsKey(number))
                    return RegisteredStrikes[number];
                throw new ArgumentException($"Strike {number} does not exist");
            }
        }

        public static Dictionary<int, Strike> StrikeMissions { get => RegisteredStrikes; }

        private void RegisterStrike(Strike strike)
        {
            RegisteredStrikes.Add(strike.Number, strike);
        }

        public override string ToString()
        {
            return base.ToString();
        }

        public override bool Equals(object obj)
        {
            if (obj is Strike s)
                return Number == s.Number;
            return false;
        }

        public override int GetHashCode()
        {
            return (base.GetHashCode() + Number).GetHashCode();
        }

        public static bool operator ==(Strike a, Strike b)
        {
            return GP.Compare(a, b);
        }
        public static bool operator !=(Strike a, Strike b)
        {
            return !(a == b);
        }
    }
}
