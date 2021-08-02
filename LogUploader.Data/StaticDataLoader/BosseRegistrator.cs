using System;

using LogUploader.Data.Repositories;


namespace LogUploader.Data.StaticDataLoader
{
    internal class BosseRegistrator : IBosseRegistrator
    {
        BossRepository Repo;

        public BosseRegistrator(BossRepository repo)
        {
            Repo = repo ?? throw new ArgumentNullException(nameof(repo));
        }

        public bool Exists(int id)
        {
            return Repo.Exists(id);
        }

        public Boss Get(int id)
        {
            return Repo.Get(id);
        }

        public void Register((int iD, string nameEN, string nameDE, GameArea gameArea) basicInfo, (string folderNameEN, string folderNameDE, string avatarURL, string discordEmote, string eIName, int raidOrgaPlusID) extendedInfo)
        {
            (int iD, string nameEN, string nameDE, GameArea gameArea) = basicInfo;
            (string folderNameEN, string folderNameDE, string avatarURL, string discordEmote, string eIName, int raidOrgaPlusID) = extendedInfo;
            Boss boss = new Boss(iD, nameEN, nameDE, folderNameEN, folderNameDE, gameArea, avatarURL, discordEmote, eIName, raidOrgaPlusID);
            Repo.Add(boss);
        }

        public void Register()
        {
            Boss boss = new Boss();
            Repo.Add(boss);
        }
    }

}
