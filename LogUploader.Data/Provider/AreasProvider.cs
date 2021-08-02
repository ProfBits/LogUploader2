namespace LogUploader.Data.Repositories
{
    public interface AreasProvider
    {
        MultiAreaProvider<DragonResponseMission> DragonResponseMissions { get; }
        MultiAreaProvider<Fractal> Fractals { get; }
        MultiAreaProvider<Raid> RaidWings { get; }
        MultiAreaProvider<Strike> Strikes { get; }
        AreaProvider<Training> Training { get; }
        AreaProvider<UnkowenGameArea> Unknowen { get; }
        AreaProvider<WvW> WvW { get; }
    }                
}