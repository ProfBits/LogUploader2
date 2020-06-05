using LogUploader.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogUploader.Data.GameAreas
{
    public abstract class GameArea : NamedObject, IAvatar
    {
        public GameArea(string name, string shortName, string avatarURL) : this(name, name, shortName, shortName, avatarURL)
        { }

        public GameArea(string nameEN, string nameDE, string shortName, string avatarURL) : this(nameEN, nameDE, shortName, shortName, avatarURL)
        { }

        public GameArea(string nameEN, string nameDE, string shortNameEN, string shortNameDE, string avatarURL) : this(new ExtendedInfo(nameEN, nameDE, shortNameEN, shortNameDE, avatarURL))
        { }

        public GameArea(IBasicInfo info, string shortNameEN, string shortNameDE) : this(new ExtendedInfo(info, shortNameEN, shortNameDE))
        { }

        public GameArea(IBasicInfo info, string shortName) : this(new ExtendedInfo(info, shortName, shortName))
        { }

        public GameArea(IExtendedInfo info) : base(info.NameEN, info.NameDE)
        {
            AvatarURL = info.AvatarURL;

            m_ShortName = new NamedObject(info.ShortNameEN, info.ShortNameDE);
        }

        public string AvatarURL { get; }

        private readonly NamedObject m_ShortName;

        public string ShortName { get => m_ShortName.Name; }
        public string getShortName(eLanguage language) => m_ShortName.getName(language);

        public override string ToString()
        {
            return base.ToString();
        }

        public override int GetHashCode()
        {
            return $"{NameEN}{NameDE}{AvatarURL}".GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (base.Equals(obj) && obj is GameArea other)
            {
                return AvatarURL == other.AvatarURL && m_ShortName == other.m_ShortName;
            }
            return false;
        }

        public interface IBasicInfo
        {
            string NameEN { get; }
            string NameDE { get; }
            string AvatarURL { get; }
        }

        public interface IExtendedInfo : IBasicInfo
        {
            string ShortNameEN { get; }
            string ShortNameDE { get; }
        }

        public struct BasicInfo : IBasicInfo
        {
            public string NameEN { get; }
            public string NameDE { get; }
            public string AvatarURL { get; }

            public BasicInfo(string nameEN, string nameDE, string avatarURL)
            {
                NameEN = nameEN;
                NameDE = nameDE;
                AvatarURL = avatarURL;
            }
        }

        public struct ExtendedInfo : IExtendedInfo
        {
            public string NameEN { get; }
            public string NameDE { get; }
            public string ShortNameEN { get; }
            public string ShortNameDE { get; }
            public string AvatarURL { get; }

            public ExtendedInfo(string nameEN, string nameDE, string shortNameEN, string shortNameDE, string avatarURL)
            {
                NameEN = nameEN;
                NameDE = nameDE;
                ShortNameEN = shortNameEN;
                ShortNameDE = shortNameDE;
                AvatarURL = avatarURL;
            }

            public ExtendedInfo(IBasicInfo basic, string shortNameEN, string shortNameDE) : this(basic.NameEN, basic.NameDE, shortNameEN, shortNameDE, basic.AvatarURL)
            { }
        }
    }
}
