using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogUploader.Data.StaticDataLoader
{
    internal class GameDataRegistrator : IGameDataRegistrator
    {
        public IAreaRegistrator SetAreas { get; }
        public IBosseRegistrator SetBosses { get; }
        public IAddEnemyRegistrator SetAddEnemies { get; }
        public IMiscDataRegistrator SetMiscData { get; }
        public IAreaRepository Areas { get => SetAreas; }
        public IBosseRepository Bosses { get => SetBosses; }
        public IAddEnemyRepository AddEnemies { get => SetAddEnemies; }
        public IMiscDataRepository MiscData { get => SetMiscData; }

        public GameDataRegistrator() : this(new AreaRegistrator(), new BosseRegistrator(), new AddEnemyRegistrator(), new MiscDataRegistrator())
        {
        }

        public GameDataRegistrator(IAreaRegistrator areas, IBosseRegistrator bosses, IAddEnemyRegistrator addEnemies, IMiscDataRegistrator miscData)
        {
            SetAreas = areas ?? throw new ArgumentNullException(nameof(areas));
            SetBosses = bosses ?? throw new ArgumentNullException(nameof(bosses));
            SetAddEnemies = addEnemies ?? throw new ArgumentNullException(nameof(addEnemies));
            SetMiscData = miscData ?? throw new ArgumentNullException(nameof(miscData));
        }

    }

}
