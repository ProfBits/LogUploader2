using System;
using System.Collections.Generic;
using System.Linq;

namespace LogUploader.Data.Repositories
{
    internal class AddEnemyRepository : EnemyRepository<AddEnemy>, AddEnemyProvider
    {
        private readonly Dictionary<int, AddEnemy> Data;
        internal override IDictionary<int, AddEnemy> BaseData { get => Data; }

        public AddEnemyRepository()
        {
            Data = new Dictionary<int, AddEnemy>();
        }

        private int KeyMapper(AddEnemy addEnemy) => addEnemy.ID;

        internal override void Add(AddEnemy enemy)
        {
            if (enemy is null) throw new ArgumentNullException(nameof(enemy), "Cannot add a null addEnemy to the repository");
            Data.Add(KeyMapper(enemy), enemy);
        }

        public override IEnumerator<AddEnemy> GetEnumerator()
        {
            return Data.Select(b => b.Value).GetEnumerator();
        }
    }
}
