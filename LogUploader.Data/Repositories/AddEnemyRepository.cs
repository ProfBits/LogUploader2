using System;
using System.Collections.Generic;
using System.Linq;

namespace LogUploader.Data.Repositories
{
    internal class AddEnemyRepository : EnemyRepository<AddEnemy>, AddEnemyProvider
    {
        private readonly MultiKeyInValueDictionary<int, string, string, AddEnemy> Data;
        internal override IMultiKeyBaseDictionary<int, string, string, AddEnemy> BaseData { get => Data; }

        public AddEnemyRepository()
        {
            Data = new MultiKeyInValueDictionary<int, string, string, AddEnemy>(
                add => add.ID,
                add => add.NameEN,
                add => add.NameDE
                );
        }

        internal override void Add(AddEnemy enemy)
        {
            if (enemy is null) throw new ArgumentNullException(nameof(enemy), "Cannot add a null addEnemy to the repository");
            Data.Add(enemy);
        }

        public override IEnumerator<AddEnemy> GetEnumerator()
        {
            return Data.Select(b => b.Value).GetEnumerator();
        }
    }
}
