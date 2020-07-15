using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LogUploader.Data.GameAreas;
using LogUploader.Helpers;
using LogUploader.Interfaces;

namespace LogUploader.Data
{
    public class Boss : Enemy, IAvatar
    {
        public string DiscordEmote { get; }
        public string AvatarURL { get; }

        private readonly NamedObject m_FolderName;

        public string FolderName { get => m_FolderName.Name; }

        public string EIName { get; }

        private static Dictionary<int, Boss> allBosses = new Dictionary<int, Boss>();

        internal Boss() : this(0, "Unknwon", "Unbekannt", "UnknownFolder", "UnbekannterOrdner", Unknowen.get(), "localhost", ":grey_question:", "Unknowen") { }

        internal Boss(int id, string name, string FolderName, GameArea area, string avatarURL, string discordEmote, string eIName) : this(id, name, name, FolderName, FolderName, area, avatarURL, discordEmote, eIName)
        {

        }

        internal Boss(int id, string nameEN, string nameDE, string FolderNameEN, string FolderNameDE, GameArea area, string avatarURL, string discordEmote, string eIName) : base(id, nameEN, nameDE, area)
        {
            DiscordEmote = discordEmote;
            AvatarURL = avatarURL;
            m_FolderName = new NamedObject(FolderNameEN, FolderNameDE);
            EIName = eIName;
            allBosses.Add(id, this);
        }

        internal Boss(BasicInfo info, string FolderNameEN, string FolderNameDE, string avatarURL, string discordEmote, string eIName) : this(info.ID, info.NameEN, info.NameDE, FolderNameEN, FolderNameDE, info.GameArea, avatarURL, discordEmote, eIName)
        { }

        internal Boss(BasicInfo info, string FolderName, string avatarURL, string discordEmote, string eIName) : this(info.ID, info.NameEN, info.NameDE, FolderName, FolderName, info.GameArea, avatarURL, discordEmote, eIName)
        { }

        public static Boss getByID(int id)
        {
            try
            {
                return allBosses[id];
            }
            catch (KeyNotFoundException)
            {
                return getByID(0);
            }
        }

        public static Boss getByName(string name)
        {
            var boss = allBosses.FirstOrDefault((e) => e.Value.Name == name).Value;
            if (boss == null)
                return getByID(0);
            return boss;
        }

        public static Boss getByName(string name, eLanguage lang)
        {
            var boss = allBosses.FirstOrDefault((e) => e.Value.getName(lang) == name).Value;
            if (boss == null)
                return getByID(0);
            return boss;
        }

        public static Boss getByFolderName(string name)
        {
            var boss = allBosses.FirstOrDefault((e) => e.Value.checkFolderName(name)).Value;
            if (boss == null)
                return getByID(0);
            return boss;
        }

        public static Boss getByEIName(string name)
        {
            var boss = allBosses.FirstOrDefault((e) => e.Value.EIName == name).Value;
            if (boss == null)
                return getByID(0);
            return boss;
        }

        public static List<Boss> getByArea(GameArea area)
        {
            return allBosses.Where(e => e.Value.Area == area).Select(e => e.Value).ToList();
        }

        public static List<Boss> All { get => allBosses.Values.ToList(); }

        private bool checkFolderName(string folderName)
        {
            return m_FolderName.hasName(folderName);
        }

        public override bool Equals(object obj)
        {
            if (obj is Boss b)
                return ID == b.ID;
            return false;
        }

        public override string ToString()
        {
            return base.ToString();
        }

        public override int GetHashCode()
        {
            return base.GetHashCode() ^ ID.GetHashCode();
        }

        public static bool ExistsID(int id)
        {
            return allBosses.ContainsKey(id);
        }

        public static bool operator ==(Boss a, Boss b)
        {
            return GP.Compare(a, b);
        }

        public static bool operator !=(Boss a, Boss b)
        {
            return !(a == b);
        }
    }
}
