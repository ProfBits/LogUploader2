using LogUploader.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogUploader.Data.GameAreas
{
    public class AbstractFractal : GameArea
    {
        protected const string DEFAULT_NAME_EN = "Unkown Fractal";
        protected const string DEFAULT_NAME_DE = "Unbekanntes Fraktal";
        protected const string DEFUALT_SHORT_NAME_EN = "Fractal ???";
        protected const string DEFUALT_SHORT_NAME_DE = "Fraktal ???";
        protected const string DEFAULT_AVATAR_URL = @"https://wiki.guildwars2.com/images/d/d2/Guild_emblem_004.png";

        protected readonly int Number = -1;

        protected AbstractFractal() : base(DEFAULT_NAME_EN, DEFAULT_NAME_DE, DEFUALT_SHORT_NAME_EN, DEFUALT_SHORT_NAME_DE, DEFAULT_AVATAR_URL)
        {
        }

        public AbstractFractal(string name, int number, string avatarURL) : this(name, name, number, avatarURL)
        { }

        public AbstractFractal(string nameEN, string nameDE, int number, string avatarURL) : this(new BasicInfo(nameEN, nameDE, avatarURL), number)
        { }

        public AbstractFractal(IBasicInfo info, int number) : base(info, $"Fractal {number}", $"Fraktal {number}")
        {
            Number = number;
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

        public override string ToString()
        {
            return base.ToString();
        }
        public static bool operator ==(AbstractFractal a, AbstractFractal b)
        {
            if ((object)a == null)
                return (object)b == null;

            return a.Equals(b);
        }
        public static bool operator !=(AbstractFractal a, AbstractFractal b)
        {
            return !(a == b);
        }
    }

    public class Fractal : AbstractFractal
    {
        protected Fractal() : base()
        {
            RegisterFractal(this);
        }

        public Fractal(string name, int number, string avatarURL) : this(name, name, number, avatarURL)
        { }

        public Fractal(string nameEN, string nameDE, int number, string avatarURL) : this(new BasicInfo(nameEN, nameDE, avatarURL), number)
        { }

        public Fractal(IBasicInfo info, int number) : base(info, number)
        {
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

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public static bool operator ==(Fractal a, Fractal b)
        {
            if ((object)a == null)
                return (object)b == null;

            return a.Equals(b);
        }
        public static bool operator !=(Fractal a, Fractal b)
        {
            return !(a == b);
        }
    }
}
