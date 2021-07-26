using LogUploader.Helper;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static LogUploader.Data.GameAreas.GameArea;

namespace LogUploader.Data.GameAreas
{
    public class AbstractDragonResponseMission : GameArea
    {
        protected const string DEFAULT_NAME_EN = "Unkown Dragon Response Mission";
        protected const string DEFAULT_NAME_DE = "Unbekannte Drachenhilfe-Mission";
        protected const string DEFUALT_SHORT_NAME_EN = "DRM ???";
        protected const string DEFUALT_SHORT_NAME_DE = "DRM ???";
        protected const string DEFAULT_AVATAR_URL = @"https://wiki.guildwars2.com/images/f/fe/Destroyer_Crab.jpg";
        
        protected readonly int Number = -1;

        protected AbstractDragonResponseMission() : base(DEFAULT_NAME_EN, DEFAULT_NAME_DE, DEFUALT_SHORT_NAME_EN, DEFUALT_SHORT_NAME_DE, DEFAULT_AVATAR_URL)
        {
        }

        public AbstractDragonResponseMission(string name, int number, string avatarURL) : this(name, name, number, avatarURL)
        {
        }

        public AbstractDragonResponseMission(string nameEN, string nameDE, int number, string avatarURL) : this(new BasicInfo(nameEN, nameDE, avatarURL), number)
        { }

        public AbstractDragonResponseMission(IBasicInfo info, int number) : base(info, $"DRM {number}", $"DRM {number}")
        {
            Number = number;
        }

        public override bool Equals(object obj)
        {
            if (obj is DragonResponseMission s)
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

        public static bool operator ==(AbstractDragonResponseMission a, AbstractDragonResponseMission b)
        {
            if ((object)a == null)
                return (object)b == null;

            return a.Equals(b);
        }

        public static bool operator !=(AbstractDragonResponseMission a, AbstractDragonResponseMission b)
        {
            return !(a == b);
        }
    }

    public class DragonResponseMission : AbstractDragonResponseMission
    {
        protected DragonResponseMission() : base()
        {
            RegisterDragonResponseMission(this);
        }

        public DragonResponseMission(string name, int number, string avatarURL) : this(name, name, number, avatarURL)
        {
        }

        public DragonResponseMission(string nameEN, string nameDE, int number, string avatarURL) : this(new BasicInfo(nameEN, nameDE, avatarURL), number)
        { }

        public DragonResponseMission(IBasicInfo info, int number) : base(info, number)
        {
            RegisterDragonResponseMission(this);
        }

        static DragonResponseMission()
        {
            new DragonResponseMission();
        }

        private static readonly Dictionary<int, DragonResponseMission> RegisteredDragonResponseMissions = new Dictionary<int, DragonResponseMission>();

        public DragonResponseMission this[int number]
        {
            get
            {
                if (RegisteredDragonResponseMissions.ContainsKey(number))
                    return RegisteredDragonResponseMissions[number];
                throw new ArgumentException($"Dragon Response Mission {number} does not exist");
            }
        }

        public static Dictionary<int, DragonResponseMission> DragonResponseMissions { get => RegisteredDragonResponseMissions; }

        private void RegisterDragonResponseMission(DragonResponseMission DragonResponseMission)
        {
            RegisteredDragonResponseMissions.Add(DragonResponseMission.Number, DragonResponseMission);
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public static bool operator ==(DragonResponseMission a, DragonResponseMission b)
        {
            if ((object)a == null)
                return (object)b == null;

            return a.Equals(b);
        }
        public static bool operator !=(DragonResponseMission a, DragonResponseMission b)
        {
            return !(a == b);
        }
    }
}
