using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LogUploader.Data.GameAreas;

namespace LogUploader.Data
{
    internal class GameDataRegistrator : IGameDataRegistrator
    {
        public IAreaRegistrator SetAreas { get; }
        public IBosseRegistrator SetBosses { get; }
        public IAddEnemyRegistrator SetAddEnemies { get; }
        public IMiscDataRegistrator SetMiscData { get; }
        public IAreaRepository Areas { get => SetAreas; }
        public IBosseRepository Bosses { get => SetBosses; }
        public IAddEnemyRepository AddEnemies { get => SetAddEnemies; }
        public IMiscDataRepository MiscData { get => SetMiscData; }

        public GameDataRegistrator() : this(new AreaRegistrator(), new BosseRegistrator(), new AddEnemyRegistrator(), new MiscDataRegistrator())
        {
        }

        public GameDataRegistrator(IAreaRegistrator areas, IBosseRegistrator bosses, IAddEnemyRegistrator addEnemies, IMiscDataRegistrator miscData)
        {
            SetAreas = areas ?? throw new ArgumentNullException(nameof(areas));
            SetBosses = bosses ?? throw new ArgumentNullException(nameof(bosses));
            SetAddEnemies = addEnemies ?? throw new ArgumentNullException(nameof(addEnemies));
            SetMiscData = miscData ?? throw new ArgumentNullException(nameof(miscData));
        }

    }

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

    internal interface IBosseRegistrator : IBosseRepository
    {
        void Register(Enemy.BasicInfo info, string FolderNameEN, string FolderNameDE, string avatarURL, string discordEmote, string eIName, int raidOrgaPlusID);
        void Register();
    }

    internal class BosseRegistrator : IBosseRegistrator
    {
        public bool Exists(int id)
        {
            return Boss.ExistsID(id);
        }

        public AbstractBoss Get(int id)
        {
            return Boss.GetByID(id);
        }

        public void Register(Enemy.BasicInfo info, string FolderNameEN, string FolderNameDE, string avatarURL, string discordEmote, string eIName, int raidOrgaPlusID)
        {
            new Boss(info, FolderNameEN, FolderNameDE, avatarURL, discordEmote, eIName, raidOrgaPlusID);
        }

        public void Register()
        {
            new Boss();
        }
    }

    internal interface IAddEnemyRegistrator : IAddEnemyRepository
    {
        void Register(Enemy.BasicInfo info, bool intresting);
        void Register();
    }

    internal class AddEnemyRegistrator : IAddEnemyRegistrator
    {
        public bool Exists(int id)
        {
            return AddEnemy.ExistsID(id);
        }

        public AbstractAddEnemy Get(int id)
        {
            return AddEnemy.GetByID(id);
        }

        public void Register(Enemy.BasicInfo info, bool intresting)
        {
            new AddEnemy(info, intresting);
        }

        public void Register()
        {
            new AddEnemy();
        }
    }

    internal interface IMiscDataRegistrator : IMiscDataRepository
    {
        void RegisterKillEmote(string emote);
        void RegisterWipeEmote(string emote);
    }

    internal class MiscDataRegistrator : IMiscDataRegistrator
    {
        public string KillEmote { get => MiscData.EmoteRaidKill; }
        public string WipeEmote { get => MiscData.EmoteRaidWipe; }

        public void RegisterKillEmote(string emote)
        {
            MiscData.EmoteRaidKill = emote;
        }

        public void RegisterWipeEmote(string emote)
        {
            MiscData.EmoteRaidWipe = emote;
        }
    }

    public interface IGameDataRepository
    {
        IAreaRepository Areas { get; }
        IBosseRepository Bosses { get; }
        IAddEnemyRepository AddEnemies { get; }
        IMiscDataRepository MiscData { get; }
    }

    public interface IAreaRepository
    {
        AbstractRaidWing GetRaidWing(int id);
        AbstractStrike GetStrike(int id);
        AbstractFractal GetFractal(int id);
        AbstractWvW GetWvW();
        AbstractDragonResponseMission GetDragonResponseMission(int id);
        AbstractTraining GetTraining();
        AbstractUnknowen GetUnkowen();
    }

    public interface IBosseRepository
    {
        AbstractBoss Get(int id);
        bool Exists(int id);
    }

    public interface IAddEnemyRepository
    {
        AbstractAddEnemy Get(int id);
        bool Exists(int id);
    }

    public interface IMiscDataRepository
    {
        string KillEmote { get; }
        string WipeEmote { get; }
    }

}
