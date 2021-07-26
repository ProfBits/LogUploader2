namespace LogUploader.Data.StaticDataLoader
{
    internal class BosseRegistrator : IBosseRegistrator
    {
        public bool Exists(int id)
        {
            return Boss.ExistsID(id);
        }

        public AbstractBoss Get(int id)
        {
            return Boss.GetByID(id);
        }

        public void Register(Enemy.BasicInfo info, string FolderNameEN, string FolderNameDE, string avatarURL, string discordEmote, string eIName, int raidOrgaPlusID)
        {
            new Boss(info, FolderNameEN, FolderNameDE, avatarURL, discordEmote, eIName, raidOrgaPlusID);
        }

        public void Register()
        {
            new Boss();
        }
    }

}
