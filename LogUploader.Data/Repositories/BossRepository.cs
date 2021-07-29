using System;

namespace LogUploader.Data.Repositories
{
    internal class BossRepository : EnemyRepository<Boss>, BossProvider
    {
        private readonly MultiKeyInValueDictionary<int, string, string, string, string, string, Boss> Data;

        public BossRepository()
        {
            Data = new MultiKeyInValueDictionary<int, string, string, string, string, string, Boss>(
                boss => boss.ID,
                boss => boss.NameEN,
                boss => boss.NameDE,
                boss => boss.FolderNameEN,
                boss => boss.FolderNameDE,
                boss => boss.EIName
                );
        }

        internal override IMultiKeyBaseDictionary<int, string, string, Boss> BaseData { get => Data; }

        internal override void Add(Boss enemy)
        {
            if (enemy is null) throw new ArgumentNullException(nameof(enemy), "Cannot add a null boss to the repository");
            Data.Add(enemy);
        }
    }
}
