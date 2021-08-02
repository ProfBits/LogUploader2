using System;

using LogUploader.Data.Repositories;

namespace LogUploader.Data.StaticDataLoader
{
    internal class AreaRegistrator : IAreaRegistrator
    {
        AreasRepository Repo;
        TrainingAreaRepository TrainingRepository { get => Repo.TrainingData; }
        WvWAreaRepository WvWRepository { get => Repo.WvWData; }
        UnkowenAreaRepository UnkowenRepository { get => Repo.UnknowenData; }
        RaidRepository RaidRepository { get => Repo.RaidWingsData; }
        StrikeRepository StrikeRepository { get => Repo.StrikesData; }
        FractalRepository FractalRepository { get => Repo.FractalsData; }
        DragonResponseMissionRepository DragonResponseMissionRepository { get => Repo.DragonResponseMissionsData; }

        public AreaRegistrator(AreasRepository areasRepository)
        {
            Repo = areasRepository ?? throw new ArgumentNullException(nameof(areasRepository));
        }

        public bool ExitsDragonResponseMission(int id)
        {
            return DragonResponseMissionRepository.Exists(id);
        }

        public bool ExitsFractal(int id)
        {
            return FractalRepository.Exists(id);
        }

        public bool ExitsRaidWing(int id)
        {
            return RaidRepository.Exists(id);
        }

        public bool ExitsStrike(int id)
        {
            return StrikeRepository.Exists(id);
        }

        public bool ExitsTraining()
        {
            return true;
        }

        public bool ExitsUnkowen()
        {
            return true;
        }

        public bool ExitsWvW()
        {
            return true;
        }

        public DragonResponseMission GetDragonResponseMission(int id)
        {
            return DragonResponseMissionRepository.Get(id);
        }

        public Fractal GetFractal(int id)
        {
            return FractalRepository.Get(id);
        }

        public Raid GetRaidWing(int id)
        {
            return RaidRepository.Get(id);
        }

        public Strike GetStrike(int id)
        {
            return StrikeRepository.Get(id);
        }

        public Training GetTraining()
        {
            return TrainingRepository.Get();
        }

        public UnkowenGameArea GetUnkowen()
        {
            return UnkowenRepository.Get();
        }

        public WvW GetWvW()
        {
            return WvWRepository.Get();
        }

        public void RegisterDragonResponseMission(int id, (string nameEN, string nameDE, string avatarURL) basicData)
        {
            (string nameEN, string nameDE, string avatarURL) = basicData;
            DragonResponseMission drm = new DragonResponseMission(id, nameEN, nameDE, avatarURL);
            DragonResponseMissionRepository.Add(drm);
        }

        public void RegisterFractal(int id, (string nameEN, string nameDE, string avatarURL) basicData)
        {
            (string nameEN, string nameDE, string avatarURL) = basicData;
            Fractal fractal = new Fractal(id, nameEN, nameDE, avatarURL);
            FractalRepository.Add(fractal);
        }

        public void RegisterRaidWing(int id, (string nameEN, string nameDE, string avatarURL) basicData)
        {
            (string nameEN, string nameDE, string avatarURL) = basicData;
            Raid raid = new Raid(id, nameEN, nameDE, avatarURL);
            RaidRepository.Add(raid);
        }

        public void RegisterStrike(int id, (string nameEN, string nameDE, string avatarURL) basicData)
        {
            (string nameEN, string nameDE, string avatarURL) = basicData;
            Strike strike = new Strike(id, nameEN, nameDE, avatarURL);
            StrikeRepository.Add(strike);
        }

        public void RegisterTraining((string nameEN, string nameDE, string avatarURL) basicData, (string shortNameEN, string shortNameDE) extendedData)
        {
            (string nameEN, string nameDE, string avatarURL) = basicData;
            (string shortNameEN, string shortNameDE) = extendedData;
            Training training = new Training(nameEN, nameDE, shortNameEN, shortNameDE, avatarURL);
            TrainingRepository.SetArea(training);
        }

        public void RegisterUnkowen((string nameEN, string nameDE, string avatarURL) basicData, (string shortNameEN, string shortNameDE) extendedData)
        {
            (string nameEN, string nameDE, string avatarURL) = basicData;
            (string shortNameEN, string shortNameDE) = extendedData;
            UnkowenGameArea unkowen = new UnkowenGameArea(nameEN, nameDE, shortNameEN, shortNameDE, avatarURL);
            UnkowenRepository.SetArea(unkowen);
        }

        public void RegisterWvW((string nameEN, string nameDE, string avatarURL) basicData, (string shortNameEN, string shortNameDE) extendedData)
        {
            (string nameEN, string nameDE, string avatarURL) = basicData;
            (string shortNameEN, string shortNameDE) = extendedData;
            WvW wvw = new WvW(nameEN, nameDE, shortNameEN, shortNameDE, avatarURL);
            WvWRepository.SetArea(wvw);
        }
    }

}
