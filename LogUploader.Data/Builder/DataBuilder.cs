using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LogUploader.Data;

using Newtonsoft.Json.Linq;
using System.Xml.Linq;

namespace LogUploader.Data.StaticDataLoader
{
    public class DataBuilder
    {
        private const string TagRoot = "LogUploaderData";
        private const string TagAreaRoot = "GameAreas";
        private const string TagBossRoot = "Bosses";
        private const string TagAddRoot = "AddEnemys";
        private const string TagMiscRoot = "Misc";

        private const string TagRaidRoot = "RaidWings";
        private const string TagStrikeRoot = "StrikeMissions";
        private const string TagDRMRoot = "DragonResponseMissions";
        private const string TagFractalRoot = "Fractals";
        private const string TagWvWRoot = "WvW";
        private const string TagTrainingRoot = "Training";
        private const string TagUnknownRoot = "Unknown";
        private const string TagMiscEmoteRoot = "Emotes";

        private const string TagID = "ID";

        private const string TagNameEN = "NameEN";
        private const string TagNameDE = "NameDE";
        private const string TagShortNameEN = "ShortNameEN";
        private const string TagShortNameDE = "ShortNameDE";
        private const string TagFolderEN = "FolderEN";
        private const string TagFolderDE = "FolderDE";
        private const string TagAvatarURL = "AvatarURL";
        private const string TagLevel = "Level";
        private const string TagEiName = "EiName";
        private const string TagGameAreaName = "GameAreaName";
        private const string TagGameAreaID = "GameAreaID";
        private const string TagDiscordEmote = "DiscordEmote";
        private const string TagRaidOrgaPlusID = "RaidOrgaPlusID";
        private const string TagIntresting = "Intresting";
        private const string TagWipe = "Wipe";
        private const string TagKill = "Kill";

        private IGameDataRegistrator Registrator { get; }

        internal DataBuilder(IGameDataRegistrator registrator)
        {
            Registrator = registrator;
        }

        public static void LoadDataJson(string json) => LoadDataJson(json, new GameDataRegistrator());

        internal static void LoadDataJson(string json, IGameDataRegistrator registrator)
        {
            new DataBuilder(registrator).LoadData(json);
        }

        private void LoadData(string json)
        {
            var data = (JObject)ParseJson(json)[TagRoot];
            var GameAreas = (JObject)data[TagAreaRoot];
            CreateAreas(GameAreas);
            var Bosses = (JArray)data[TagBossRoot];
            var AddEnemys = (JArray)data[TagAddRoot];

            CreateBosses(Bosses);
            CreateAdds(AddEnemys);

            CreateMiscData((JObject)data[TagMiscRoot]);
        }

        private void CreateMiscData(JObject json)
        {
            var emotes = (JObject)json[TagMiscEmoteRoot];
            Registrator.MiscData.RegisterKillEmote((string)emotes[TagKill]);
            Registrator.MiscData.RegisterWipeEmote((string)emotes[TagWipe]);
        }

        private JObject ParseJson(string json)
        {
            return JObject.Parse(json);
        }

        private async Task<JObject> ParseJsonAsync(string json)
        {
            return await Task.Run(() => JObject.Parse(json));
        }

        private void CreateAreas(JObject gameAreaData)
        {
            CreateRaidWings((JArray)gameAreaData[TagRaidRoot]);
            CreateStrikeMissions((JArray)gameAreaData[TagStrikeRoot]);
            CreateDrMissions((JArray)gameAreaData[TagDRMRoot]);
            CreateFractals((JArray)gameAreaData[TagFractalRoot]);
            CreateWvW((JArray)gameAreaData[TagWvWRoot]);
            CreateTraining((JArray)gameAreaData[TagTrainingRoot]);
            CreateUnknown((JArray)gameAreaData[TagUnknownRoot]);
        }

        private void CreateRaidWings(JArray wingsData)
        {
            foreach (JObject raidWing in wingsData)
            {
                var id = (int) raidWing[TagID];
                var basicInfo = GetAreaBasicInfo(raidWing);

                Registrator.Areas.RegisterRaidWing(id, basicInfo);
            }
        }

        private void CreateStrikeMissions(JArray strikesData)
        {
            foreach (JObject strike in strikesData)
            {
                var id = (int) strike[TagID];
                var basicInfo = GetAreaBasicInfo(strike);

                Registrator.Areas.RegisterStrike(id, basicInfo);
            }
        }

        private void CreateDrMissions(JArray drmData)
        {
            foreach (JObject drm in drmData)
            {
                var id = (int) drm[TagID];
                var basicInfo = GetAreaBasicInfo(drm);

                Registrator.Areas.RegisterDragonResponseMission(id, basicInfo);
            }
        }

        private void CreateFractals(JArray fractalsData)
        {
            foreach (JObject fractal in fractalsData)
            {
                var level = (int) fractal[TagLevel];
                var basicInfo = GetAreaBasicInfo(fractal);

                Registrator.Areas.RegisterFractal(level, basicInfo);
            }
        }

        private void CreateWvW(JArray wvwData)
        {
            foreach (JObject wvw in wvwData)
            {
                var basicInfo = GetAreaBasicInfo(wvw);
                var info = GetAreaExtendedInfo(wvw);
                Registrator.Areas.RegisterWvW(basicInfo, info);
            }
        }

        private void CreateTraining(JArray trainingsData)
        {
            foreach (JObject training in trainingsData)
            {
                var basicInfo = GetAreaBasicInfo(training);
                var info = GetAreaExtendedInfo(training);
                Registrator.Areas.RegisterTraining(basicInfo, info);
            }
        }

        private void CreateUnknown(JArray unknownData)
        {
            foreach (JObject unknown in unknownData)
            {
                var basicInfo = GetAreaBasicInfo(unknown);
                var info = GetAreaExtendedInfo(unknown);
                Registrator.Areas.RegisterUnkowen(basicInfo, info);
            }
        }
        
        private void CreateBosses(JArray bossData)
        {
            foreach (JObject boss in bossData)
            {
                CreateBoss(boss);
            }
            if (!Registrator.Bosses.Exists(0))
                Registrator.Bosses.Register();
        }

        private async Task CreateBossesAsync(JArray bossData)
        {
            await Task.Run(() =>
            {
                CreateBosses(bossData);
            });
        }

        private void CreateBoss(JObject boss)
        {
            var basics = GetBasicEnemyInfo(boss);
            var folderEN = (string)boss[TagFolderEN];
            var folderDE = (string)boss[TagFolderDE];
            var EiName = (string)boss[TagEiName];
            var avatarURL = GetAvatarUrl(boss);
            var DiscordEmote = (string)boss[TagDiscordEmote];
            var raidOrgaPlusId = (int)boss[TagRaidOrgaPlusID];

            Registrator.Bosses.Register(basics, (folderEN, folderDE, avatarURL, DiscordEmote, EiName, raidOrgaPlusId));
        }

        private GameArea DetimenGameArea(string gameAreaName, int gameAreaID)
        {
            switch (gameAreaName)
            {
                case TagRaidRoot:
                    return Registrator.Areas.GetRaidWing(gameAreaID);
                case TagStrikeRoot:
                    return Registrator.Areas.GetStrike(gameAreaID);
                case TagDRMRoot:
                    return Registrator.Areas.GetDragonResponseMission(gameAreaID);
                case TagFractalRoot:
                    return Registrator.Areas.GetFractal(gameAreaID);
                case TagWvWRoot:
                    return Registrator.Areas.GetWvW();
                case TagTrainingRoot:
                    return Registrator.Areas.GetTraining();
                case TagUnknownRoot:
                    return Registrator.Areas.GetUnkowen();
                default:
                    return null;
            }
        }

        private void CreateAdds(JArray addData)
        {
            foreach (JObject add in addData)
            {
               CreateAdd(add);
            }
            if (!Registrator.AddEnemies.Exists(0))
                Registrator.AddEnemies.Register();
        }
        
        private async Task CreateAddsAsync(JArray addData)
        {
            List<Task> addTaks = new List<Task>();
            foreach (JObject add in addData)
            {
                addTaks.Add(Task.Run(() => CreateAdd(add)));
            }
            foreach (var task in addTaks)
            {
                await task;
            }
            if (!Registrator.AddEnemies.Exists(0))
                Registrator.AddEnemies.Register();
        }
        
        private void CreateAdd(JObject add)
        {
            var basics = GetBasicEnemyInfo(add);
            var intresting = (bool)add[TagIntresting];
            Registrator.AddEnemies.Register(basics, intresting);
        }

        private (int iD, string nameEN, string nameDE, GameArea gameArea) GetBasicEnemyInfo(JObject enemy)
        {
            var ID = (int)enemy[TagID];
            (string nameEN, string nameDE) = GetNameInfo(enemy);
            var gameAreaName = (string)enemy[TagGameAreaName];
            var gameAreaID = (int)enemy[TagGameAreaID];
            var gameArea = DetimenGameArea(gameAreaName, gameAreaID);
            
            return (ID, nameEN, nameDE, gameArea);
        }

        private (string nameEN, string nameDE, string avatarURL) GetAreaBasicInfo(JObject area)
        {
            (string nameEN, string nameDE) = GetNameInfo(area);
            string avatarURL = GetAvatarUrl(area);

            return (nameEN, nameDE, avatarURL);
        }

        private string GetAvatarUrl(JObject obj)
        {
            return (string)obj[TagAvatarURL];
        }

        private (string shortNameEN, string shortNameDE) GetAreaExtendedInfo(JObject area)
        {
            var shortNameEN = (string)area[TagShortNameEN];
            var shortNameDE = (string)area[TagShortNameDE];

            return (shortNameEN, shortNameDE);
        }

        private (string nameEN, string nameDE) GetNameInfo(JObject obj)
        {
            var nameEN = (string)obj[TagNameEN];
            var nameDE = (string)obj[TagNameDE];
            return (nameEN, nameDE);
        }
    }
}
