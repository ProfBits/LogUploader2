using LogUploader.Data.GameAreas;

namespace LogUploader.Data.Repositories
{
    public interface AreasProvider
    {
        MultiAreaProvider<DragonResponseMission> DragonResponseMissions { get; }
        MultiAreaProvider<Fractal> Fractals { get; }
        MultiAreaProvider<RaidWing> RaidWings { get; }
        MultiAreaProvider<Strike> Strikes { get; }
        AreaProvider<Training> Training { get; }
        AreaProvider<Unknowen> Unknowen { get; }
        AreaProvider<WvW> WvW { get; }
    }
}