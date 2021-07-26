using LogUploader.Data.GameAreas;

namespace LogUploader.Data.StaticDataLoader
{
    internal class AreaRegistrator : IAreaRegistrator
    {
        public AbstractDragonResponseMission GetDragonResponseMission(int id)
        {
            return DragonResponseMission.DragonResponseMissions[id];
        }

        public AbstractFractal GetFractal(int id)
        {
            return Fractal.Fractals[id];
        }

        public AbstractRaidWing GetRaidWing(int id)
        {
            return RaidWing.RaidWings[id];
        }

        public AbstractStrike GetStrike(int id)
        {
            return Strike.StrikeMissions[id];
        }

        public AbstractTraining GetTraining()
        {
            return Training.Get();
        }

        public AbstractUnknowen GetUnkowen()
        {
            return Unknowen.Get();
        }

        public AbstractWvW GetWvW()
        {
            return WvW.Get();
        }

        public void RegisterDragonResponseMission(GameArea.BasicInfo basicInfo, int id)
        {
            new DragonResponseMission(basicInfo, id);
        }

        public void RegisterFractal(GameArea.BasicInfo basicInfo, int id)
        {
            new Fractal(basicInfo, id);
        }

        public void RegisterRaidWing(GameArea.BasicInfo basicInfo, int id)
        {
            new RaidWing(basicInfo, id);
        }

        public void RegisterStrike(GameArea.BasicInfo basicInfo, int id)
        {
            new Strike(basicInfo, id);
        }

        public void RegisterTraining(GameArea.ExtendedInfo basicInfo)
        {
            Training.Create(basicInfo);
        }

        public void RegisterUnkowen(GameArea.ExtendedInfo basicInfo)
        {
            Unknowen.Create(basicInfo);
        }

        public void RegisterWvW(GameArea.ExtendedInfo basicInfo)
        {
            WvW.Create(basicInfo);
        }
    }

}
