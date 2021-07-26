using LogUploader.Data.GameAreas;

namespace LogUploader.Data.StaticDataLoader
{
    internal interface IGameDataRegistrator : IGameDataRepository
    {
        IAreaRegistrator SetAreas { get; }
        IBosseRegistrator SetBosses { get; }
        IAddEnemyRegistrator SetAddEnemies { get; }
        IMiscDataRegistrator SetMiscData { get; }
    }


    internal interface IAreaRegistrator : IAreaRepository
    {
        void RegisterRaidWing(GameArea.BasicInfo basicInfo, int id);
        void RegisterStrike(GameArea.BasicInfo basicInfo, int id);
        void RegisterFractal(GameArea.BasicInfo basicInfo, int id);
        void RegisterWvW(GameArea.ExtendedInfo basicInfo);
        void RegisterDragonResponseMission(GameArea.BasicInfo basicInfo, int id);
        void RegisterTraining(GameArea.ExtendedInfo basicInfo);
        void RegisterUnkowen(GameArea.ExtendedInfo basicInfo);
    }

    internal interface IBosseRegistrator : IBosseRepository
    {
        void Register(Enemy.BasicInfo info, string FolderNameEN, string FolderNameDE, string avatarURL, string discordEmote, string eIName, int raidOrgaPlusID);
        void Register();
    }

    internal interface IAddEnemyRegistrator : IAddEnemyRepository
    {
        void Register(Enemy.BasicInfo info, bool intresting);
        void Register();
    }
    internal interface IMiscDataRegistrator : IMiscDataRepository
    {
        void RegisterKillEmote(string emote);
        void RegisterWipeEmote(string emote);
    }

}