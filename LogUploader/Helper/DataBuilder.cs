using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LogUploader.Data.GameAreas;
using LogUploader.Data;

using Newtonsoft.Json.Linq;

namespace LogUploader.Helper
{
    public static class DataBuilder
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


        public static void LoadDataJson(string json)
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

        private static void CreateMiscData(JObject json)
        {
            var emotes = (JObject)json[TagMiscEmoteRoot];
            MiscData.EmoteRaidKill = (string)emotes[TagKill];
            MiscData.EmoteRaidWipe = (string)emotes[TagWipe];
        }

        private static JObject ParseJson(string json)
        {
            return JObject.Parse(json);
        }

        private static async Task<JObject> ParseJsonAsync(string json)
        {
            return await Task.Run(() => JObject.Parse(json));
        }

        private static void CreateAreas(JObject gameAreaData)
        {
            CreateRaidWings((JArray)gameAreaData[TagRaidRoot]);
            CreateStrikeMissions((JArray)gameAreaData[TagStrikeRoot]);
            CreateDrMissions((JArray)gameAreaData[TagDRMRoot]);
            CreateFractals((JArray)gameAreaData[TagFractalRoot]);
            CreateWvW((JArray)gameAreaData[TagWvWRoot]);
            CreateTraining((JArray)gameAreaData[TagTrainingRoot]);
            CreateUnknown((JArray)gameAreaData[TagUnknownRoot]);
        }

        private static void CreateRaidWings(JArray wingsData)
        {
            foreach (JObject raidWing in wingsData)
            {
                var ID = (int) raidWing[TagID];
                var basicInfo = GetAreaBasicInfo(raidWing);

                new RaidWing(basicInfo, ID);
            }
        }

        private static void CreateStrikeMissions(JArray strikesData)
        {
            foreach (JObject strike in strikesData)
            {
                var ID = (int) strike[TagID];
                var basicInfo = GetAreaBasicInfo(strike);

                new Strike(basicInfo, ID);
            }
        }

        private static void CreateDrMissions(JArray drmData)
        {
            foreach (JObject drm in drmData)
            {
                var ID = (int) drm[TagID];
                var basicInfo = GetAreaBasicInfo(drm);

                new DragonResponseMission(basicInfo, ID);
            }
        }

        private static void CreateFractals(JArray fractalsData)
        {
            foreach (JObject fractal in fractalsData)
            {
                var level = (int) fractal[TagLevel];
                var basicInfo = GetAreaBasicInfo(fractal);

                new Fractal(basicInfo, level);
            }
        }

        private static void CreateWvW(JArray wvwData)
        {
            foreach (JObject wvw in wvwData)
            {
                var info = GetAreaExtendedInfo(wvw);
                WvW.Create(info);
            }
        }

        private static void CreateTraining(JArray trainingsData)
        {
            foreach (JObject training in trainingsData)
            {
                var info = GetAreaExtendedInfo(training);
                Training.Create(info);
            }
        }

        private static void CreateUnknown(JArray unknownData)
        {
            foreach (JObject unknown in unknownData)
            {
                var info = GetAreaExtendedInfo(unknown);
                Unknowen.Create(info);
            }
        }
        
        private static void CreateBosses(JArray bossData)
        {
            foreach (JObject boss in bossData)
            {
                CreateBoss(boss);
            }
            if (!Boss.ExistsID(0))
                new Boss();
        }

        private static async Task CreateBossesAsync(JArray bossData)
        {
            await Task.Run(() =>
            {
                CreateBosses(bossData);
            });
        }

        private static void CreateBoss(JObject boss)
        {
            var basics = GetBasicEnemyInfo(boss);
            var folderEN = (string)boss[TagFolderEN];
            var folderDE = (string)boss[TagFolderDE];
            var EiName = (string)boss[TagEiName];
            var avatarURL = GetAvatarUrl(boss);
            var DiscordEmote = (string)boss[TagDiscordEmote];
            var raidOrgaPlusId = (int)boss[TagRaidOrgaPlusID];


            new Boss(basics, folderEN, folderDE, avatarURL, DiscordEmote, EiName, raidOrgaPlusId);
        }

        private static GameArea detimenGameArea(string gameAreaName, int gameAreaID)
        {
            switch (gameAreaName)
            {
                case TagRaidRoot:
                    return RaidWing.RaidWings[gameAreaID];
                case TagStrikeRoot:
                    return Strike.StrikeMissions[gameAreaID];
                case TagDRMRoot:
                    return DragonResponseMission.DragonResponseMissions[gameAreaID];
                case TagFractalRoot:
                    return Fractal.Fractals[gameAreaID];
                case TagWvWRoot:
                    return WvW.get();
                case TagTrainingRoot:
                    return Training.get();
                case TagUnknownRoot:
                    return Unknowen.get();
                default:
                    return null;
            }
        }

        private static void CreateAdds(JArray addData)
        {
            foreach (JObject add in addData)
            {
               CreateAdd(add);
            }
            if (!AddEnemy.ExistsID(0))
                new AddEnemy();
        }
        
        private static async Task CreateAddsAsync(JArray addData)
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
            if (!AddEnemy.ExistsID(0))
                new AddEnemy();
        }
        
        private static void CreateAdd(JObject add)
        {
            var basics = GetBasicEnemyInfo(add);
            var intresting = (bool)add[TagIntresting];
            new AddEnemy(basics, intresting);
        }

        private static Enemy.BasicInfo GetBasicEnemyInfo(JObject enemy)
        {
            var ID = (int)enemy[TagID];
            var name = GetNameInfo(enemy);
            var gameAreaName = (string)enemy[TagGameAreaName];
            var gameAreaID = (int)enemy[TagGameAreaID];
            var gameArea = detimenGameArea(gameAreaName, gameAreaID);
            
            return new Enemy.BasicInfo(ID, name.EN, name.DE, gameArea);
        }

        private static GameArea.BasicInfo GetAreaBasicInfo(JObject area)
        {
            var name = GetNameInfo(area);
            string avatarURL = GetAvatarUrl(area);

            return new GameArea.BasicInfo(name.EN, name.DE, avatarURL);
        }

        private static string GetAvatarUrl(JObject obj)
        {
            return (string)obj[TagAvatarURL];
        }

        private static GameArea.ExtendedInfo GetAreaExtendedInfo(JObject area)
        {
            var shortNameEN = (string)area[TagShortNameEN];
            var shortNameDE = (string)area[TagShortNameDE];

            return new GameArea.ExtendedInfo(GetAreaBasicInfo(area), shortNameEN, shortNameDE);
        }

        private static NameInfo GetNameInfo(JObject obj)
        {
            var nameEN = (string)obj[TagNameEN];
            var nameDE = (string)obj[TagNameDE];
            return new NameInfo(nameEN, nameDE);
        }

        private struct NameInfo
        {
            public string EN { get; }
            public string DE { get; }

            public NameInfo(string nameEN, string nameDE)
            {
                EN = nameEN;
                DE = nameDE;
            }
        }

    }
}
