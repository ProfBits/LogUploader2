using LogUploader.Data.Repositories;

namespace LogUploader.Data
{
    public static class StaticData
    {
        public static AreaProvider Areas { get => AreaRepository; }
        internal static AreaRepository AreaRepository { get; } = new AreaRepository();
        public static BossProvider Bosses { get => BossRepository; }
        internal static BossRepository BossRepository { get; } = new BossRepository();
        public static AddEnemyProvider AddEnemies { get => AddEnemyRepository; }
        internal static AddEnemyRepository AddEnemyRepository { get; } = new AddEnemyRepository();
        public static ProfessionProvider Professions { get => ProfessionRepository; }
        internal static ProfessionRepository ProfessionRepository { get; } = new ProfessionRepository();
        public static MiscProvider Misc { get => MiscRepository; }
        internal static MiscRepository MiscRepository { get; } = new MiscRepository();
    }
}
