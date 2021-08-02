using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogUploader.Data.Repositories
{
    internal class AreasRepository : AreasProvider
    {
        internal RaidRepository RaidWingsData { get; } = new RaidRepository();
        internal StrikeRepository StrikesData { get; } = new StrikeRepository();
        internal FractalRepository FractalsData { get; } = new FractalRepository();
        internal DragonResponseMissionRepository DragonResponseMissionsData { get; } = new DragonResponseMissionRepository();
        internal TrainingAreaRepository TrainingData { get; } = new TrainingAreaRepository();
        internal WvWAreaRepository WvWData { get; } = new WvWAreaRepository();
        internal UnkowenAreaRepository UnknowenData { get; } = new UnkowenAreaRepository();

        public MultiAreaProvider<Raid> RaidWings { get => RaidWingsData; }
        public MultiAreaProvider<Strike> Strikes { get => StrikesData; }
        public MultiAreaProvider<Fractal> Fractals { get => FractalsData; }
        public MultiAreaProvider<DragonResponseMission> DragonResponseMissions { get => DragonResponseMissionsData; }
        public AreaProvider<Training> Training { get => TrainingData; }
        public AreaProvider<WvW> WvW { get => WvWData; }
        public AreaProvider<UnkowenGameArea> Unknowen { get => UnknowenData; }
    }
}
