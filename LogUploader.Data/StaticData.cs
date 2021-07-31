using LogUploader.Data.Repositories;

namespace LogUploader.Data
{
    public static class StaticData
    {
        public static AreasProvider Areas { get => AreaRepository; }
        internal static AreasRepository AreaRepository { get; } = new AreasRepository();
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
