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
        public MultiAreaProvider<New.Raid> RaidWings { get; }
        public MultiAreaProvider<New.Strike> Strikes { get; }
        public MultiAreaProvider<New.Fractal> Fractals { get; }
        public MultiAreaProvider<New.DragonResponseMission> DragonResponseMissions { get; }
        public AreaProvider<New.Training> Training { get; }
        public AreaProvider<New.WvW> WvW { get; }
        public AreaProvider<New.UnkowenGameArea> Unknowen { get; }
    }
}
