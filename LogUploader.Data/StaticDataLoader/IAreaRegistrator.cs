
using LogUploader.Data.GameAreas;

namespace LogUploader.Data.StaticDataLoader
{
    internal interface IAreaRegistrator : IAreaRepository
    {
        void RegisterRaidWing(GameArea.BasicInfo basicInfo, int id);
        void RegisterStrike(GameArea.BasicInfo basicInfo, int id);
        void RegisterFractal(GameArea.BasicInfo basicInfo, int id);
        void RegisterWvW(GameArea.ExtendedInfo basicInfo);
        void RegisterDragonResponseMission(GameArea.BasicInfo basicInfo, int id);
        void RegisterTraining(GameArea.ExtendedInfo basicInfo);
        void RegisterUnkowen(GameArea.ExtendedInfo basicInfo);
    }

}
