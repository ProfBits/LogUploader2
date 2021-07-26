using LogUploader.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static LogUploader.Data.GameAreas.GameArea;

namespace LogUploader.Data.GameAreas
{
    public class AbstractStrike : GameArea
    {
        protected const string DEFAULT_NAME_EN = "Unkown Strike Mission";
        protected const string DEFAULT_NAME_DE = "Unbekannte Angriffs Mission";
        protected const string DEFUALT_SHORT_NAME_EN = "Strike ???";
        protected const string DEFUALT_SHORT_NAME_DE = "Angriff ???";
        protected const string DEFAULT_AVATAR_URL = @"https://wiki.guildwars2.com/images/d/d2/Guild_emblem_004.png";

        protected readonly int Number = -1;

        protected AbstractStrike() : base(DEFAULT_NAME_EN, DEFAULT_NAME_DE, DEFUALT_SHORT_NAME_EN, DEFUALT_SHORT_NAME_DE, DEFAULT_AVATAR_URL)
        {
        }

        public AbstractStrike(string name, int number, string avatarURL) : this(name, name, number, avatarURL)
        {
        }

        public AbstractStrike(string nameEN, string nameDE, int number, string avatarURL) : this(new BasicInfo(nameEN, nameDE, avatarURL), number)
        { }

        public AbstractStrike(IBasicInfo info, int number) : base(info, $"Strike {number}", $"Angriff {number}")
        {
            Number = number;
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

        public override string ToString()
        {
            return base.ToString();
        }

        public static bool operator ==(AbstractStrike a, AbstractStrike b)
        {
            if ((object)a == null)
                return (object)b == null;

            return a.Equals(b);
        }
        public static bool operator !=(AbstractStrike a, AbstractStrike b)
        {
            return !(a == b);
        }
    }

    public class Strike : AbstractStrike
    {
        protected Strike() : base()
        {
            RegisterStrike(this);
        }

        public Strike(string name, int number, string avatarURL) : this(name, name, number, avatarURL)
        {
        }

        public Strike(string nameEN, string nameDE, int number, string avatarURL) : this(new BasicInfo(nameEN, nameDE, avatarURL), number)
        { }

        public Strike(IBasicInfo info, int number) : base(info, number)
        {
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

    }
}
