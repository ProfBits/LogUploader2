using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogUploader.Data.StaticDataLoader
{
    internal class GameDataRegistrator : IGameDataRegistrator
    {
        public IAreaRegistrator Areas { get; }
        public IBosseRegistrator Bosses { get; }
        public IAddEnemyRegistrator AddEnemies { get; }
        public IMiscDataRegistrator MiscData { get; }

        public GameDataRegistrator() :
            this(new AreaRegistrator(StaticData.AreaRepository), new BosseRegistrator(StaticData.BossRepository),
                new AddEnemyRegistrator(StaticData.AddEnemyRepository), new MiscDataRegistrator(StaticData.MiscRepository))
        {
        }

        public GameDataRegistrator(IAreaRegistrator areas, IBosseRegistrator bosses, IAddEnemyRegistrator addEnemies, IMiscDataRegistrator miscData)
        {
            Areas = areas ?? throw new ArgumentNullException(nameof(areas));
            Bosses = bosses ?? throw new ArgumentNullException(nameof(bosses));
            AddEnemies = addEnemies ?? throw new ArgumentNullException(nameof(addEnemies));
            MiscData = miscData ?? throw new ArgumentNullException(nameof(miscData));
        }

    }

}
