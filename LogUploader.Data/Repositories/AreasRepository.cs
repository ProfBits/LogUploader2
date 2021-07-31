using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LogUploader.Data.GameAreas;

namespace LogUploader.Data.Repositories
{
    internal class AreasRepository : AreasProvider
    {
        public MultiAreaProvider<RaidWing> RaidWings { get; }
        public MultiAreaProvider<Strike> Strikes { get; }
        public MultiAreaProvider<Fractal> Fractals { get; }
        public MultiAreaProvider<DragonResponseMission> DragonResponseMissions { get; }
        public AreaProvider<Training> Training { get; }
        public AreaProvider<WvW> WvW { get; }
        public AreaProvider<Unknowen> Unknowen { get; }
    }
}
