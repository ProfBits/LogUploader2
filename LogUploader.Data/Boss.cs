using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LogUploader.Helper;
using LogUploader.Interfaces;
using LogUploader.Localisation;

namespace LogUploader.Data
{
    public class Boss : Enemy, IAvatar, IEquatable<Boss>, IEquatable<eBosses>
    {
        protected readonly NamedObject m_FolderName;
        public string DiscordEmote { get; }
        public string AvatarURL { get; }

        public string FolderName { get => m_FolderName.Name; }

        public string EIName { get; }
        public int RaidOrgaPlusID { get; }

        public Boss() : this(0, "Unknwon", "Unbekannt", "UnknownFolder", "UnbekannterOrdner", new UnkowenGameArea(), @"https://www.publicdomainpictures.net/pictures/280000/velka/ghost-on-black-background.jpg", ":grey_question:", "Unknowen", -1) { }

        public Boss(int id, string name, string FolderName, GameArea area, string avatarURL, string discordEmote, string eIName, int raidOrgaPlusID) : this(id, name, name, FolderName, FolderName, area, avatarURL, discordEmote, eIName, raidOrgaPlusID)
        {

        }

        public Boss(int id, string nameEN, string nameDE, string FolderNameEN, string FolderNameDE, GameArea area, string avatarURL, string discordEmote, string eIName, int raidOrgaPlusID) : base(id, nameEN, nameDE, area)
        {
            DiscordEmote = Tools.GP.ValidateDiscordEmote(discordEmote);
            AvatarURL = Tools.GP.ValidateStringMultiWord(avatarURL);
            m_FolderName = new NamedObject(Tools.GP.ValidateStringMultiWord(FolderNameEN), Tools.GP.ValidateStringMultiWord(FolderNameDE));
            EIName = Tools.GP.ValidateStringMultiWord(eIName);
            RaidOrgaPlusID = raidOrgaPlusID;
        }

        public string FolderNameEN { get => m_FolderName.NameEN; }
        public string FolderNameDE { get => m_FolderName.NameDE; }

        private bool CheckFolderName(string folderName)
        {
            return m_FolderName.HasName(folderName);
        }
        public override bool Equals(object obj)
        {
            return Equals(obj as Boss);
        }

        public bool Equals(Boss other)
        {
            return other != null &&
                   base.Equals(other);
        }

        public override int GetHashCode()
        {
            return 624022166 + base.GetHashCode();
        }

        public bool Equals(eBosses other)
        {
            return ID == (int)other;
        }

        public static bool operator ==(Boss left, Boss right)
        {
            return EqualityComparer<Boss>.Default.Equals(left, right);
        }

        public static bool operator !=(Boss left, Boss right)
        {
            return !(left == right);
        }

        public static bool operator ==(Boss left, eBosses right)
        {
            return left?.Equals(right) ?? false;
        }

        public static bool operator !=(Boss left, eBosses right)
        {
            return !(left == right);
        }

        public static bool operator ==(eBosses left, Boss right)
        {
            return right?.Equals(left) ?? false;
        }

        public static bool operator !=(eBosses left, Boss right)
        {
            return !(left == right);
        }
    }
}
