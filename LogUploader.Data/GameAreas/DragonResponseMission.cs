using LogUploader.Helper;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogUploader.Data.GameAreas
{
    class DragonResponseMission : GameArea
    {
        private const string DEFAULT_NAME_EN = "Unkown Dragon Response Mission";
        private const string DEFAULT_NAME_DE = "Unbekannte Drachenhilfe-Mission";
        private const string DEFUALT_SHORT_NAME_EN = "DRM ???";
        private const string DEFUALT_SHORT_NAME_DE = "DRM ???";
        private const string DEFAULT_AVATAR_URL = @"https://wiki.guildwars2.com/images/f/fe/Destroyer_Crab.jpg";

        private readonly int Number = -1;

        private DragonResponseMission() : base(DEFAULT_NAME_EN, DEFAULT_NAME_DE, DEFUALT_SHORT_NAME_EN, DEFUALT_SHORT_NAME_DE, DEFAULT_AVATAR_URL)
        {
            RegisterDragonResponseMission(this);
        }

        internal DragonResponseMission(string name, int number, string avatarURL) : this(name, name, number, avatarURL)
        {
        }

        internal DragonResponseMission(string nameEN, string nameDE, int number, string avatarURL) : this(new BasicInfo(nameEN, nameDE, avatarURL), number)
        { }

        internal DragonResponseMission(IBasicInfo info, int number) : base(info, $"DRM {number}", $"DRM {number}")
        {
            Number = number;
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

        public override string ToString()
        {
            return base.ToString();
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

        public static bool operator ==(DragonResponseMission a, DragonResponseMission b)
        {
            return GP.Compare(a, b);
        }
        public static bool operator !=(DragonResponseMission a, DragonResponseMission b)
        {
            return !(a == b);
        }
    }
}
