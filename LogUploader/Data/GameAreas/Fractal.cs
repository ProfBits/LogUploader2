using LogUploader.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogUploader.Data.GameAreas
{
    public class Fractal : GameArea
    {
        private const string DEFAULT_NAME_EN = "Unkown Fractal";
        private const string DEFAULT_NAME_DE = "Unbekanntes Fraktal";
        private const string DEFUALT_SHORT_NAME_EN = "Fractal ???";
        private const string DEFUALT_SHORT_NAME_DE = "Fraktal ???";
        private const string DEFAULT_AVATAR_URL = @"https://wiki.guildwars2.com/images/d/d2/Guild_emblem_004.png";

        private readonly int Number = -1;

        private Fractal() : base(DEFAULT_NAME_EN, DEFAULT_NAME_DE, DEFUALT_SHORT_NAME_EN, DEFUALT_SHORT_NAME_DE, DEFAULT_AVATAR_URL)
        {
            RegisterFractal(this);
        }

        internal Fractal(string name, int number, string avatarURL) : this(name, name, number, avatarURL)
        { }

        internal Fractal(string nameEN, string nameDE, int number, string avatarURL) : this(new BasicInfo(nameEN, nameDE, avatarURL), number)
        { }

        internal Fractal(IBasicInfo info, int number) : base(info, $"Fractal {number}", $"Fraktal {number}")
        {
            Number = number;
            RegisterFractal(this);
        }

        static Fractal()
        {
            new Fractal();
        }

        private static readonly Dictionary<int, Fractal> RegisteredFractals = new Dictionary<int, Fractal>();

        public Fractal this[int number]
        {
            get
            {
                if (RegisteredFractals.ContainsKey(number))
                    return RegisteredFractals[number];
                throw new ArgumentException($"Fractal {number} does not exist");
            }
        }

        public static Dictionary<int, Fractal> Fractals { get => RegisteredFractals; }

        private void RegisterFractal(Fractal fractal)
        {
            RegisteredFractals.Add(fractal.Number, fractal);
        }

        public override string ToString()
        {
            return base.ToString();
        }

        public override bool Equals(object obj)
        {
            if (obj is Fractal f)
                return Number == f.Number;
            return false;
        }

        public override int GetHashCode()
        {
            return (base.GetHashCode() + Number).GetHashCode();
        }

        public static bool operator ==(Fractal a, Fractal b)
        {
            return GP.Compare(a, b);
        }
        public static bool operator !=(Fractal a, Fractal b)
        {
            return !(a == b);
        }
    }
}
