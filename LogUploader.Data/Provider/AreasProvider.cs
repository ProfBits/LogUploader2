using LogUploader.Data.GameAreas;

namespace LogUploader.Data.Repositories
{
    public interface AreasProvider
    {
        MultiAreaProvider<New.DragonResponseMission> DragonResponseMissions { get; }
        MultiAreaProvider<New.Fractal> Fractals { get; }
        MultiAreaProvider<New.Raid> RaidWings { get; }
        MultiAreaProvider<New.Strike> Strikes { get; }
        AreaProvider<New.Training> Training { get; }
        AreaProvider<New.UnkowenGameArea> Unknowen { get; }
        AreaProvider<New.WvW> WvW { get; }
    }                
}