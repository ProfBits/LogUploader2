using LogUploader.Data.GameAreas;

namespace LogUploader.Data.StaticDataLoader
{
    public interface IGameDataRepository
    {
        IAreaRepository Areas { get; }
        IBosseRepository Bosses { get; }
        IAddEnemyRepository AddEnemies { get; }
        IMiscDataRepository MiscData { get; }
    }


    public interface IAreaRepository
    {
        AbstractRaidWing GetRaidWing(int id);
        AbstractStrike GetStrike(int id);
        AbstractFractal GetFractal(int id);
        AbstractWvW GetWvW();
        AbstractDragonResponseMission GetDragonResponseMission(int id);
        AbstractTraining GetTraining();
        AbstractUnknowen GetUnkowen();
    }

    public interface IBosseRepository
    {
        AbstractBoss Get(int id);
        bool Exists(int id);
    }

    public interface IAddEnemyRepository
    {
        AbstractAddEnemy Get(int id);
        bool Exists(int id);
    }

    public interface IMiscDataRepository
    {
        string KillEmote { get; }
        string WipeEmote { get; }
    }

}
