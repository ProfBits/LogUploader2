namespace LogUploader.Data.StaticDataLoader
{
    internal interface IGameDataRegistrator
    {
        IAreaRegistrator Areas { get; }
        IBosseRegistrator Bosses { get; }
        IAddEnemyRegistrator AddEnemies { get; }
        IMiscDataRegistrator MiscData { get; }
    }


    internal interface IAreaRegistrator
    {
        void RegisterRaidWing(int id, (string nameEN, string nameDE, string avatarURL) basicData);
        void RegisterStrike(int id, (string nameEN, string nameDE, string avatarURL) basicData);
        void RegisterFractal(int id, (string nameEN, string nameDE, string avatarURL) basicData);
        void RegisterWvW((string nameEN, string nameDE, string avatarURL) basicData, (string shortNameEN, string shortNameDE) extendedData);
        void RegisterDragonResponseMission(int id, (string nameEN, string nameDE, string avatarURL) basicData);
        void RegisterTraining((string nameEN, string nameDE, string avatarURL) basicData, (string shortNameEN, string shortNameDE) extendedData);
        void RegisterUnkowen((string nameEN, string nameDE, string avatarURL) basicData, (string shortNameEN, string shortNameDE) extendedData);

        bool ExitsRaidWing(int id);
        bool ExitsStrike(int id);
        bool ExitsFractal(int id);
        bool ExitsWvW();
        bool ExitsDragonResponseMission(int id);
        bool ExitsTraining();
        bool ExitsUnkowen();

        Raid GetRaidWing(int id);
        Strike GetStrike(int id);
        Fractal GetFractal(int id);
        WvW GetWvW();
        DragonResponseMission GetDragonResponseMission(int id);
        Training GetTraining();
        UnkowenGameArea GetUnkowen();
    }

    internal interface IBosseRegistrator
    {
        void Register((int iD, string nameEN, string nameDE, GameArea gameArea) basicInfo,
            (string folderNameEN, string folderNameDE, string avatarURL, string discordEmote, string eIName, int raidOrgaPlusID) extendedInfo);
        void Register();
        bool Exists(int id);
        Boss Get(int id);
    }

    internal interface IAddEnemyRegistrator
    {
        void Register((int iD, string nameEN, string nameDE, GameArea gameArea) basicInfo, bool intresting);
        void Register();
        bool Exists(int id);
        AddEnemy Get(int id);
    }
    internal interface IMiscDataRegistrator
    {
        void RegisterKillEmote(string emote);
        void RegisterWipeEmote(string emote);
    }

}