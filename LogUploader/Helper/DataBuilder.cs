using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LogUploader.Data.GameAreas;
using LogUploader.Data;
using LogUploader.JSONHelper;

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
            var data = ParseJson(json).GetTypedElement<JSONHelper.JSONObject>(TagRoot);
            var GameAreas = data.GetTypedElement<JSONHelper.JSONObject>(TagAreaRoot);
            CreateAreas(GameAreas);
            var Bosses = data.GetTypedList<JSONHelper.JSONObject>(TagBossRoot);
            var AddEnemys = data.GetTypedList<JSONHelper.JSONObject>(TagAddRoot);

            CreateBosses(Bosses);
            CreateAdds(AddEnemys);

            CreateMiscData(data.GetTypedElement<JSONHelper.JSONObject>(TagMiscRoot));
        }

        private static void CreateMiscData(JSONHelper.JSONObject json)
        {
            var emotes = json.GetTypedElement<JSONObject>(TagMiscEmoteRoot);
            MiscData.EmoteRaidKill = emotes.GetTypedElement<string>(TagKill);
            MiscData.EmoteRaidWipe = emotes.GetTypedElement<string>(TagWipe);
        }

        private static JSONHelper.JSONObject ParseJson(string json)
        {
            return new JSONHelper.JSONHelper().Desirealize(json);
        }

        private static async Task<JSONHelper.JSONObject> ParseJsonAsync(string json)
        {
            return await Task.Run(() => new JSONHelper.JSONHelper().Desirealize(json));
        }

        private static void CreateAreas(JSONHelper.JSONObject gameAreaData)
        {
            CreateRaidWings(gameAreaData.GetTypedList<JSONHelper.JSONObject>(TagRaidRoot));
            CreateStrikeMissions(gameAreaData.GetTypedList<JSONHelper.JSONObject>(TagStrikeRoot));
            CreateFractals(gameAreaData.GetTypedList<JSONHelper.JSONObject>(TagFractalRoot));
            CreateWvW(gameAreaData.GetTypedList<JSONHelper.JSONObject>(TagWvWRoot));
            CreateTraining(gameAreaData.GetTypedList<JSONHelper.JSONObject>(TagTrainingRoot));
            CreateUnknown(gameAreaData.GetTypedList<JSONHelper.JSONObject>(TagUnknownRoot));
        }

        private static void CreateRaidWings(List<JSONHelper.JSONObject> wingsData)
        {
            foreach (var raidWing in wingsData)
            {
                var ID = (int) raidWing.GetTypedElement<double>(TagID);
                var basicInfo = GetAreaBasicInfo(raidWing);

                new RaidWing(basicInfo, ID);
            }
        }

        private static void CreateStrikeMissions(List<JSONHelper.JSONObject> strikesData)
        {
            foreach (var strike in strikesData)
            {
                var ID = (int) strike.GetTypedElement<double>(TagID);
                var basicInfo = GetAreaBasicInfo(strike);

                new Strike(basicInfo, ID);
            }
        }

        private static void CreateFractals(List<JSONHelper.JSONObject> fractalsData)
        {
            foreach (var fractal in fractalsData)
            {
                var level = (int) fractal.GetTypedElement<double>(TagLevel);
                var basicInfo = GetAreaBasicInfo(fractal);

                new Fractal(basicInfo, level);
            }
        }

        private static void CreateWvW(List<JSONHelper.JSONObject> wvwData)
        {
            foreach (var wvw in wvwData)
            {
                var info = GetAreaExtendedInfo(wvw);
                WvW.Create(info);
            }
        }

        private static void CreateTraining(List<JSONHelper.JSONObject> trainingsData)
        {
            foreach (var training in trainingsData)
            {
                var info = GetAreaExtendedInfo(training);
                Training.Create(info);
            }
        }

        private static void CreateUnknown(List<JSONHelper.JSONObject> unknownData)
        {
            foreach (var unknown in unknownData)
            {
                var info = GetAreaExtendedInfo(unknown);
                Unknowen.Create(info);
            }
        }
        
        private static void CreateBosses(List<JSONHelper.JSONObject> bossData)
        {
            foreach (var boss in bossData)
            {
                CreateBoss(boss);
            }
            if (!Boss.ExistsID(0))
                new Boss();
        }

        private static async Task CreateBossesAsync(List<JSONHelper.JSONObject> bossData)
        {
            await Task.Run(() =>
            {
                CreateBosses(bossData);
            });
        }

        private static void CreateBoss(JSONHelper.JSONObject boss)
        {
            var basics = GetBasicEnemyInfo(boss);
            var folderEN = boss.GetTypedElement<string>(TagFolderEN);
            var folderDE = boss.GetTypedElement<string>(TagFolderDE);
            var EiName = boss.GetTypedElement<string>(TagEiName);
            var avatarURL = GetAvatarUrl(boss);
            var DiscordEmote = boss.GetTypedElement<string>(TagDiscordEmote);
            var raidOrgaPlusId = (int) boss.GetTypedElement<double>(TagRaidOrgaPlusID);


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

        private static void CreateAdds(List<JSONHelper.JSONObject> addData)
        {
            foreach (var add in addData)
            {
               CreateAdd(add);
            }
            if (!AddEnemy.ExistsID(0))
                new AddEnemy();
        }
        
        private static async Task CreateAddsAsync(List<JSONHelper.JSONObject> addData)
        {
            List<Task> addTaks = new List<Task>();
            foreach (var add in addData)
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
        
        private static void CreateAdd(JSONHelper.JSONObject add)
        {
            var basics = GetBasicEnemyInfo(add);
            var intresting = add.GetTypedElement<bool>(TagIntresting);
            new AddEnemy(basics, intresting);
        }

        private static Enemy.BasicInfo GetBasicEnemyInfo(JSONHelper.JSONObject enemy)
        {
            var ID = (int)enemy.GetTypedElement<double>(TagID);
            var name = GetNameInfo(enemy);
            var gameAreaName = enemy.GetTypedElement<string>(TagGameAreaName);
            var gameAreaID = (int)enemy.GetTypedElement<double>(TagGameAreaID);
            var gameArea = detimenGameArea(gameAreaName, gameAreaID);
            
            return new Enemy.BasicInfo(ID, name.EN, name.DE, gameArea);
        }

        private static GameArea.BasicInfo GetAreaBasicInfo(JSONHelper.JSONObject area)
        {
            var name = GetNameInfo(area);
            string avatarURL = GetAvatarUrl(area);

            return new GameArea.BasicInfo(name.EN, name.DE, avatarURL);
        }

        private static string GetAvatarUrl(JSONObject obj)
        {
            return obj.GetTypedElement<string>(TagAvatarURL);
        }

        private static GameArea.ExtendedInfo GetAreaExtendedInfo(JSONHelper.JSONObject area)
        {
            var shortNameEN = area.GetTypedElement<string>(TagShortNameEN);
            var shortNameDE = area.GetTypedElement<string>(TagShortNameDE);

            return new GameArea.ExtendedInfo(GetAreaBasicInfo(area), shortNameEN, shortNameDE);
        }

        private static NameInfo GetNameInfo(JSONHelper.JSONObject obj)
        {
            var nameEN = obj.GetTypedElement<string>(TagNameEN);
            var nameDE = obj.GetTypedElement<string>(TagNameDE);
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
