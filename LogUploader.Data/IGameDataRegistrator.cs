namespace LogUploader.Data
{
    internal interface IGameDataRegistrator : IGameDataRepository
    {
        IAreaRegistrator SetAreas { get; }
        IBosseRegistrator SetBosses { get; }
        IAddEnemyRegistrator SetAddEnemies { get; }
        IMiscDataRegistrator SetMiscData { get; }
    }
}