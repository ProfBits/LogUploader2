﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LogUploader.Data.GameAreas;
using LogUploader.Helper;
using LogUploader.Interfaces;
using LogUploader.Localisation;

namespace LogUploader.Data
{
    public class AbstractBoss : Enemy, IAvatar
    {
        protected readonly NamedObject m_FolderName;
        public string DiscordEmote { get; }
        public string AvatarURL { get; }

        public string FolderName { get => m_FolderName.Name; }

        public string EIName { get; }
        public int RaidOrgaPlusID { get; }

        public AbstractBoss() : this(0, "Unknwon", "Unbekannt", "UnknownFolder", "UnbekannterOrdner", Unknowen.Get(), @"https://www.publicdomainpictures.net/pictures/280000/velka/ghost-on-black-background.jpg", ":grey_question:", "Unknowen", -1) { }

        public AbstractBoss(int id, string name, string FolderName, GameArea area, string avatarURL, string discordEmote, string eIName, int raidOrgaPlusID) : this(id, name, name, FolderName, FolderName, area, avatarURL, discordEmote, eIName, raidOrgaPlusID)
        {

        }

        public AbstractBoss(int id, string nameEN, string nameDE, string FolderNameEN, string FolderNameDE, GameArea area, string avatarURL, string discordEmote, string eIName, int raidOrgaPlusID) : base(id, nameEN, nameDE, area)
        {
            DiscordEmote = discordEmote;
            AvatarURL = avatarURL;
            m_FolderName = new NamedObject(FolderNameEN, FolderNameDE);
            EIName = eIName;
            RaidOrgaPlusID = raidOrgaPlusID;
        }

        public AbstractBoss(BasicInfo info, string FolderNameEN, string FolderNameDE, string avatarURL, string discordEmote, string eIName, int raidOrgaPlusID) : this(info.ID, info.NameEN, info.NameDE, FolderNameEN, FolderNameDE, info.GameArea, avatarURL, discordEmote, eIName, raidOrgaPlusID)
        { }

        public AbstractBoss(BasicInfo info, string FolderName, string avatarURL, string discordEmote, string eIName, int raidOrgaPlusID) : this(info.ID, info.NameEN, info.NameDE, FolderName, FolderName, info.GameArea, avatarURL, discordEmote, eIName, raidOrgaPlusID)
        { }

        public override bool Equals(object obj)
        {
            if (obj is Boss b)
                return ID == b.ID;
            return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode() ^ ID.GetHashCode();
        }

        public override string ToString()
        {
            return base.ToString();
        }
        public static bool operator ==(AbstractBoss a, AbstractBoss b)
        {
            if ((object)a == null)
                return (object)b == null;

            return a.Equals(b);
        }

        public static bool operator !=(AbstractBoss a, AbstractBoss b)
        {
            return !(a == b);
        }
    }

    public class Boss : AbstractBoss
    {
        private static readonly Dictionary<int, Boss> allBosses = new Dictionary<int, Boss>();

        public Boss() : this(0, "Unknwon", "Unbekannt", "UnknownFolder", "UnbekannterOrdner", Unknowen.Get(), @"https://www.publicdomainpictures.net/pictures/280000/velka/ghost-on-black-background.jpg", ":grey_question:", "Unknowen", -1) { }

        public Boss(int id, string name, string FolderName, GameArea area, string avatarURL, string discordEmote, string eIName, int raidOrgaPlusID) : this(id, name, name, FolderName, FolderName, area, avatarURL, discordEmote, eIName, raidOrgaPlusID)
        {

        }

        public Boss(int id, string nameEN, string nameDE, string FolderNameEN, string FolderNameDE, GameArea area, string avatarURL, string discordEmote, string eIName, int raidOrgaPlusID) : base(id, nameEN, nameDE, FolderNameEN, FolderNameDE, area, avatarURL, discordEmote, eIName, raidOrgaPlusID)
        {
            allBosses.Add(id, this);
        }

        public Boss(BasicInfo info, string FolderNameEN, string FolderNameDE, string avatarURL, string discordEmote, string eIName, int raidOrgaPlusID) : this(info.ID, info.NameEN, info.NameDE, FolderNameEN, FolderNameDE, info.GameArea, avatarURL, discordEmote, eIName, raidOrgaPlusID)
        { }

        public Boss(BasicInfo info, string FolderName, string avatarURL, string discordEmote, string eIName, int raidOrgaPlusID) : this(info.ID, info.NameEN, info.NameDE, FolderName, FolderName, info.GameArea, avatarURL, discordEmote, eIName, raidOrgaPlusID)
        { }

        public static Boss Unknown { get => Get(eBosses.Unknown); }

        public static Boss Get(eBosses boss)
        {
            return GetByID((int)boss);
        }

        public static Boss GetByID(int id)
        {
            switch (id)
            {
                /*Fake Ai's*/
                case 23255:
                case 23256:
                    id = 23254;
                    break;
            }

            try
            {
                return allBosses[id];
            }
            catch (KeyNotFoundException)
            {
                return GetByID(0);
            }
        }
        public static Boss GetByRaidOragPlusID(int id)
        {
            var boss = allBosses.FirstOrDefault((e) => e.Value.RaidOrgaPlusID == id).Value;
            if (boss == null)
                return GetByID(0);
            return boss;
        }

        public static Boss GetByName(string name)
        {
            var boss = allBosses.FirstOrDefault((e) => e.Value.Name == name).Value;
            if (boss == null)
                return GetByID(0);
            return boss;
        }

        public static Boss GetByName(string name, eLanguage lang)
        {
            var boss = allBosses.FirstOrDefault((e) => e.Value.GetName(lang) == name).Value;
            if (boss == null)
                return GetByID(0);
            return boss;
        }

        public static Boss GetByFolderName(string name)
        {
            var boss = allBosses.FirstOrDefault((e) => e.Value.CheckFolderName(name)).Value;
            if (boss == null)
            {
                switch (name)
                {
                    case "Gebrochener König":
                    case "Bezwungener König":
                        boss = Get(eBosses.BrokenKing);
                        break;
                }
            }
            if (boss == null)
                return GetByID(0);
            return boss;
        }

        public static Boss GetByEIName(string name)
        {
            var boss = allBosses.FirstOrDefault((e) => e.Value.EIName == name).Value;
            if (boss == null)
                return GetByID(0);
            return boss;
        }

        public static List<Boss> GetByArea(GameArea area)
        {
            return allBosses.Where(e => e.Value.Area == area).Select(e => e.Value).ToList();
        }

        public static List<Boss> All { get => allBosses.Values.Where(boss => boss.ID != 23255 && boss.ID != 23256).ToList(); }

        private bool CheckFolderName(string folderName)
        {
            return m_FolderName.HasName(folderName);
        }

        public static bool ExistsID(int id)
        {
            return allBosses.ContainsKey(id);
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public static bool operator ==(Boss a, Boss b)
        {
            if ((object)a == null)
                return (object)b == null;

            return a.Equals(b);
        }

        public static bool operator !=(Boss a, Boss b)
        {
            return !(a == b);
        }
    }
}
