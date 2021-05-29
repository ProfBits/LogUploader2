using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using LogUploader.Tools;

using Newtonsoft.Json.Linq;

using NUnit.Framework;

namespace LogUploader.Test.Data
{
    public class DataConfigStructureTest
    {
        private JObject DataConfigContent = null;
        private const string ROOT_TAG = "LogUploaderData";

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            var assamblyPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            var relPath = "Data" + System.IO.Path.DirectorySeparatorChar + "DataConfig.json";

            Assert.DoesNotThrow(() =>
            {
                var jsonStr = JsonHandling.ReadJsonFile(assamblyPath + System.IO.Path.DirectorySeparatorChar + relPath);

                DataConfigContent = JObject.Parse(jsonStr);
            }, "Could not parse DataConfig.json");
        }

        [Test]
        public void DataConfigGeneralCompletnessTest()
        {
            Assert.AreEqual(1, DataConfigContent.Count, "Only the LogUploaderData token is expeted at the root level, no more");
            Assert.NotNull(DataConfigContent["LogUploaderData"], "A LogUploaderData token is expeted at the root level");
            Assert.AreEqual(JTokenType.Object, DataConfigContent["LogUploaderData"].Type, "A LogUploaderData object is expeted at the root level");
            var root = (JObject)DataConfigContent["LogUploaderData"];
            Assert.AreEqual(4, root.Count, "4 tokens below LogUploaderData are expected");
            Assert.NotNull(root["GameAreas"], "A GameAreas token is expeted");
            Assert.AreEqual(JTokenType.Object, root["GameAreas"].Type, "A GameAreas object is expeted");
            Assert.NotNull(root["Bosses"], "A Bosses token is expeted");
            Assert.AreEqual(JTokenType.Array, root["Bosses"].Type, "A Bosses arry is expeted");
            Assert.NotNull(root["AddEnemys"], "A AddEnemys token is expeted");
            Assert.AreEqual(JTokenType.Array, root["AddEnemys"].Type, "A AddEnemys array is expeted");
            Assert.NotNull(root["Misc"], "A Misc token is expeted");
            Assert.AreEqual(JTokenType.Object, root["Misc"].Type, "A Misc object is expeted");
        }

        [Test]
        public void DataConfigGameAreasCompletnessTest()
        {
            var gameAreas = DataConfigContent[ROOT_TAG]?["GameAreas"];
            Assert.NotNull(gameAreas, "GameAreas token missing");

            Assert.AreEqual(7, gameAreas.Children().Count(), "7 tokens below GameAreas are expected");

            Assert.NotNull(gameAreas["RaidWings"], "A RaidWings token is expeted");
            Assert.AreEqual(JTokenType.Array, gameAreas["RaidWings"].Type, "A RaidWings array is expeted");

            Assert.NotNull(gameAreas["StrikeMissions"], "A StrikeMissions token is expeted");
            Assert.AreEqual(JTokenType.Array, gameAreas["StrikeMissions"].Type, "A StrikeMissions array is expeted");

            Assert.NotNull(gameAreas["Fractals"], "A Fractals token is expeted");
            Assert.AreEqual(JTokenType.Array, gameAreas["Fractals"].Type, "A Fractals array is expeted");

            Assert.NotNull(gameAreas["WvW"], "A WvW token is expeted");
            Assert.AreEqual(JTokenType.Array, gameAreas["WvW"].Type, "A WvW array is expeted");

            Assert.NotNull(gameAreas["DragonResponseMissions"], "A DragonResponseMissions token is expeted");
            Assert.AreEqual(JTokenType.Array, gameAreas["DragonResponseMissions"].Type, "A DragonResponseMissions array is expeted");
            
            Assert.NotNull(gameAreas["Training"], "A Training token is expeted");
            Assert.AreEqual(JTokenType.Array, gameAreas["Training"].Type, "A Training array is expeted");

            Assert.NotNull(gameAreas["Unknown"], "A Unknown token is expeted");
            Assert.AreEqual(JTokenType.Array, gameAreas["Unknown"].Type, "A Unknown array is expeted");
        }

        [Test]
        public void DataConfigRaidCompletnessTest()
        {
            var raidWings = DataConfigContent[ROOT_TAG]?["GameAreas"]?["RaidWings"] as JArray;
            Assert.NotNull(raidWings, "RaidWings array missing");
            foreach (JObject element in raidWings)
            {
                Assert.AreEqual(4, element.Children().Count(), "4 tokens below each raidwing are expected");

                Assert.NotNull(element["ID"], "A ID token is ID");
                Assert.AreEqual(JTokenType.Integer, element["ID"].Type, "A ID Integer is expeted");

                Assert.NotNull(element["NameEN"], "A NameEN token is expeted");
                Assert.AreEqual(JTokenType.String, element["NameEN"].Type, "A NameEN String is expeted");

                Assert.NotNull(element["NameDE"], "A NameDE token is expeted");
                Assert.AreEqual(JTokenType.String, element["NameDE"].Type, "A NameDE String is expeted");

                Assert.NotNull(element["AvatarURL"], "A AvatarURL token is expeted");
                Assert.AreEqual(JTokenType.String, element["AvatarURL"].Type, "A AvatarURL String is expeted");
            }
        }

        [Test]
        public void DataConfigStrikeCompletnessTest()
        {
            var strikeMissions = DataConfigContent[ROOT_TAG]?["GameAreas"]?["StrikeMissions"] as JArray;
            Assert.NotNull(strikeMissions, "StrikeMissions array missing");
            foreach (JObject element in strikeMissions)
            {
                Assert.AreEqual(4, element.Children().Count(), "4 tokens below each StrikeMission are expected");

                Assert.NotNull(element["ID"], "A ID token is expeted");
                Assert.AreEqual(JTokenType.Integer, element["ID"].Type, "A ID Integer is expeted");

                Assert.NotNull(element["NameEN"], "A NameEN token is expeted");
                Assert.AreEqual(JTokenType.String, element["NameEN"].Type, "A NameEN String is expeted");

                Assert.NotNull(element["NameDE"], "A NameDE token is expeted");
                Assert.AreEqual(JTokenType.String, element["NameDE"].Type, "A NameDE String is expeted");

                Assert.NotNull(element["AvatarURL"], "A AvatarURL token is expeted");
                Assert.AreEqual(JTokenType.String, element["AvatarURL"].Type, "A AvatarURL String is expeted");
            }
        }

        [Test]
        public void DataConfigFractalCompletnessTest()
        {
            var fractals = DataConfigContent[ROOT_TAG]?["GameAreas"]?["Fractals"] as JArray;
            Assert.NotNull(fractals, "Fractals array missing");
            foreach (JObject element in fractals)
            {
                Assert.AreEqual(4, element.Children().Count(), "4 tokens below each Fractal are expected");

                Assert.NotNull(element["Level"], "A Level token is expeted");
                Assert.AreEqual(JTokenType.Integer, element["Level"].Type, "A Level Integer is expeted");

                Assert.NotNull(element["NameEN"], "A NameEN token is expeted");
                Assert.AreEqual(JTokenType.String, element["NameEN"].Type, "A NameEN String is expeted");

                Assert.NotNull(element["NameDE"], "A NameDE token is expeted");
                Assert.AreEqual(JTokenType.String, element["NameDE"].Type, "A NameDE String is expeted");

                Assert.NotNull(element["AvatarURL"], "A AvatarURL token is expeted");
                Assert.AreEqual(JTokenType.String, element["AvatarURL"].Type, "A AvatarURL String is expeted");
            }
        }

        [Test]
        public void DataConfigDRMCompletnessTest()
        {
            var dragonResponseMissions = DataConfigContent[ROOT_TAG]?["GameAreas"]?["DragonResponseMissions"] as JArray;
            Assert.NotNull(dragonResponseMissions, "DragonResponseMissions array missing");
            foreach (JObject element in dragonResponseMissions)
            {
                Assert.AreEqual(4, element.Children().Count(), "4 tokens below each dragonResponseMission are expected");

                Assert.NotNull(element["ID"], "A ID token is expeted");
                Assert.AreEqual(JTokenType.Integer, element["ID"].Type, "A ID Integer is expeted");

                Assert.NotNull(element["NameEN"], "A NameEN token is expeted");
                Assert.AreEqual(JTokenType.String, element["NameEN"].Type, "A NameEN String is expeted");

                Assert.NotNull(element["NameDE"], "A NameDE token is expeted");
                Assert.AreEqual(JTokenType.String, element["NameDE"].Type, "A NameDE String is expeted");

                Assert.NotNull(element["AvatarURL"], "A AvatarURL token is expeted");
                Assert.AreEqual(JTokenType.String, element["AvatarURL"].Type, "A AvatarURL String is expeted");
            }
        }

        [Test]
        public void DataConfigRemainingAreaCompletnessTest()
        {
            foreach (var area in new string[] { "WvW", "Training", "Unknown" })
            {
                var areaData = DataConfigContent[ROOT_TAG]?["GameAreas"]?[area] as JArray;
                Assert.NotNull(areaData, area + " array missing");
                
                foreach (JObject element in areaData)
                {
                    Assert.AreEqual(5, element.Children().Count(), "5 tokens below each area are expected");

                    Assert.NotNull(element["NameEN"], "A NameEN token is expeted");
                    Assert.AreEqual(JTokenType.String, element["NameEN"].Type, "A NameEN String is expeted");

                    Assert.NotNull(element["NameDE"], "A NameDE token is expeted");
                    Assert.AreEqual(JTokenType.String, element["NameDE"].Type, "A NameDE String is expeted");

                    Assert.NotNull(element["ShortNameEN"], "A ShortNameEN token is expeted");
                    Assert.AreEqual(JTokenType.String, element["ShortNameEN"].Type, "A ShortNameEN String is expeted");

                    Assert.NotNull(element["ShortNameDE"], "A ShortNameDE token is expeted");
                    Assert.AreEqual(JTokenType.String, element["ShortNameDE"].Type, "A ShortNameDE String is expeted");

                    Assert.NotNull(element["AvatarURL"], "A AvatarURL token is expeted");
                    Assert.AreEqual(JTokenType.String, element["AvatarURL"].Type, "A AvatarURL String is expeted");
                }
            }
        }

        [Test]
        public void DataConfigBossCompletnessTest()
        {
            var bosses = DataConfigContent[ROOT_TAG]?["Bosses"] as JArray;
            Assert.NotNull(bosses, "Bosses array missing");
            foreach (JToken boss in bosses)
            {
                Assert.AreEqual(11, boss.Children().Count(), "11 tokens below each boss are expected");
                Assert.AreEqual(JTokenType.Object, boss.Type, "A boss should be a object");

                Assert.NotNull(boss["ID"], "A ID token is expeted");
                Assert.AreEqual(JTokenType.Integer, boss["ID"].Type, "A ID Integer is expeted");

                Assert.NotNull(boss["NameEN"], "A NameEN token is expeted");
                Assert.AreEqual(JTokenType.String, boss["NameEN"].Type, "A NameEN String is expeted");

                Assert.NotNull(boss["NameDE"], "A NameDE token is expeted");
                Assert.AreEqual(JTokenType.String, boss["NameDE"].Type, "A NameDE String is expeted");

                Assert.NotNull(boss["FolderEN"], "A FolderEN token is expeted");
                Assert.AreEqual(JTokenType.String, boss["FolderEN"].Type, "A FolderEN String is expeted");

                Assert.NotNull(boss["FolderDE"], "A FolderDE token is expeted");
                Assert.AreEqual(JTokenType.String, boss["FolderDE"].Type, "A FolderDE String is expeted");

                Assert.NotNull(boss["EiName"], "A EiName token is expeted");
                Assert.AreEqual(JTokenType.String, boss["EiName"].Type, "A EiName String is expeted");

                Assert.NotNull(boss["GameAreaName"], "A GameAreaName token is expeted");
                Assert.AreEqual(JTokenType.String, boss["GameAreaName"].Type, "A GameAreaName String is expeted");

                Assert.NotNull(boss["GameAreaID"], "A GameAreaID token is expeted");
                Assert.AreEqual(JTokenType.Integer, boss["GameAreaID"].Type, "A GameAreaID Integer is expeted");

                Assert.NotNull(boss["DiscordEmote"], "A DiscordEmote token is expeted");
                Assert.AreEqual(JTokenType.String, boss["DiscordEmote"].Type, "A DiscordEmote String is expeted");

                Assert.NotNull(boss["AvatarURL"], "A AvatarURL token is expeted");
                Assert.AreEqual(JTokenType.String, boss["AvatarURL"].Type, "A AvatarURL String is expeted");

                Assert.NotNull(boss["RaidOrgaPlusID"], "A RaidOrgaPlusID token is expeted");
                Assert.AreEqual(JTokenType.Integer, boss["RaidOrgaPlusID"].Type, "A RaidOrgaPlusID Integer is expeted");
            }
        }

        [Test]
        public void DataConfigAddEnemysCompletnessTest()
        {
            var adds = DataConfigContent[ROOT_TAG]?["AddEnemys"] as JArray;
            Assert.NotNull(adds, "AddEnemys array missing");
            foreach (JToken add in adds)
            {
                Assert.AreEqual(6, add.Children().Count(), "11 tokens below each boss are expected");
                Assert.AreEqual(JTokenType.Object, add.Type, "A boss should be a object");

                Assert.NotNull(add["ID"], "A ID token is expeted");
                Assert.AreEqual(JTokenType.Integer, add["ID"].Type, "A ID Integer is expeted");

                Assert.NotNull(add["NameEN"], "A NameEN token is expeted");
                Assert.AreEqual(JTokenType.String, add["NameEN"].Type, "A NameEN String is expeted");

                Assert.NotNull(add["NameDE"], "A NameDE token is expeted");
                Assert.AreEqual(JTokenType.String, add["NameDE"].Type, "A NameDE String is expeted");

                Assert.NotNull(add["GameAreaName"], "A GameAreaName token is expeted");
                Assert.AreEqual(JTokenType.String, add["GameAreaName"].Type, "A GameAreaName String is expeted");

                Assert.NotNull(add["GameAreaID"], "A GameAreaID token is expeted");
                Assert.AreEqual(JTokenType.Integer, add["GameAreaID"].Type, "A GameAreaID Integer is expeted");

                Assert.NotNull(add["Intresting"], "A Intresting token is expeted");
                Assert.AreEqual(JTokenType.Boolean, add["Intresting"].Type, "A Intresting Boolean is expeted");
            }
        }

        [Test]
        public void DataConfigMiscCompletnessTest()
        {
            var misc = DataConfigContent[ROOT_TAG]?["Misc"] as JObject;
            Assert.NotNull(misc, "Misc section missing");
            Assert.AreEqual(1, misc.Count, "1 token below Misc is expected");

            Assert.NotNull(misc["Emotes"], "A Emotes token is expected");
            Assert.AreEqual(JTokenType.Object, misc["Emotes"].Type, "A Emotes object is expected");
            Assert.AreEqual(2, misc["Emotes"].Children().Count(), "2 tokens below Emotes are expected");

            Assert.NotNull(misc["Emotes"]["Kill"], "A Kill token is expected");
            Assert.AreEqual(JTokenType.String, misc["Emotes"]["Kill"].Type, "A Kill String is expected");
            Assert.NotNull(misc["Emotes"]["Wipe"], "A Wipe token is expected");
            Assert.AreEqual(JTokenType.String, misc["Emotes"]["Wipe"].Type, "A Wipe String is expected");
        }

        [Test]
        public void DataConfigInternalReferenceFromBossTest()
        {
            Dictionary<string, List<int>> areaMap = CreateAreaMap();
            List<Tuple<int, string, string, int>> actorMap = CreateActorMap();

            foreach (var actor in actorMap)
            {
                Assert.IsTrue(areaMap[actor.Item3].Contains(actor.Item4),
                    $"Area config of Boss \"{actor.Item2}\" Id: {actor.Item1} is invalid, area \"{actor.Item3} {actor.Item4}\" does not exist");
            }
        }

        [Test]
        public void DataConfigInternalReferenceFromAreaTest()
        {
            Dictionary<string, List<int>> areaMap = CreateAreaMap();
            List<Tuple<int, string, string, int>> actorMap = CreateActorMap();

            foreach (var area in areaMap.SelectMany(pair => pair.Value.Select(e => new Tuple<string, int>(pair.Key, e))))
            {
                Assert.IsTrue(actorMap.Any(actor => actor.Item3 == area.Item1 && actor.Item4 == area.Item2),
                    $"Area \"{area.Item1} {area.Item2}\" does not contain any actor");
            }
        }

        private List<Tuple<int, string, string, int>> CreateActorMap()
        {
            List<Tuple<int, string, string, int>> actorMap = new List<Tuple<int, string, string, int>>();

            var adds = DataConfigContent[ROOT_TAG]?["AddEnemys"] as JArray;
            Assert.NotNull(adds, "AddEnemys array missing");
            foreach (JToken add in adds)
            {
                actorMap.Add(new Tuple<int, string, string, int>(
                    (int)add["ID"],
                    (string)add["NameEN"],
                    (string)add["GameAreaName"],
                    (int)add["GameAreaID"]));
            }
            var bosses = DataConfigContent[ROOT_TAG]?["Bosses"] as JArray;
            Assert.NotNull(bosses, "Bosses array missing");
            foreach (JToken boss in bosses)
            {
                actorMap.Add(new Tuple<int, string, string, int>(
                    (int)boss["ID"],
                    (string)boss["NameEN"],
                    (string)boss["GameAreaName"],
                    (int)boss["GameAreaID"]));
            }

            return actorMap;
        }

        private Dictionary<string, List<int>> CreateAreaMap()
        {
            Dictionary<string, List<int>> areaMap = new Dictionary<string, List<int>>();
            foreach (var area in ValidGameAreas)
            {
                if ("WvW Training Unknown".Contains(area))
                {
                    areaMap.Add(area, new List<int>());
                    areaMap[area].Add(area == "Unknown" ? 0 : 1);
                }
                else
                {
                    areaMap.Add(area, new List<int>());
                    var areas = DataConfigContent[ROOT_TAG]?["GameAreas"]?[area] as JArray;
                    Assert.NotNull(areas, area + " array missing");
                    foreach (JObject item in areas)
                    {
                        areaMap[area].Add((int)item[area == "Fractals" ? "Level" : "ID"]);
                    }
                }
            }
            return areaMap;
        }

        private void ValidateString(string s)
        {
            Assert.NotNull(s);
            Assert.IsFalse(string.IsNullOrWhiteSpace(s), $"Invalid String \"{s}\"");
            Assert.AreEqual(s, s.Trim(), $"NotWhite space at front or end. Error: \"{s}\"");
        }

        private void ValidateUrl(string s)
        {
            ValidateString(s);
            Assert.DoesNotThrow(() => _ = new Uri(s), $"Invalid Uri \"{s}\"");
        }

        private readonly Regex DiscordEmoteRegEx = new Regex("^((:\\w+:)|(<:\\w+:\\d{18}>))$", RegexOptions.Compiled);

        private void ValidateDiscordEmote(string s)
        {
            ValidateString(s);
            Assert.IsTrue(DiscordEmoteRegEx.IsMatch(s), $"Discord Emote \"{s}\" has invlid format");
        }

        [Test]
        public void DataConfigRaidPlausibilityTest()
        {
            var raidWings = DataConfigContent[ROOT_TAG]?["GameAreas"]?["RaidWings"] as JArray;
            Assert.NotNull(raidWings, "RaidWings array missing");
            foreach (JObject element in raidWings)
            {
                Assert.GreaterOrEqual((int)element["ID"], 1);

                ValidateString((string)element["NameEN"]);

                ValidateString((string)element["NameDE"]);

                ValidateUrl((string)element["AvatarURL"]);
            }
        }

        [Test]
        public void DataConfigStrikePlausibilityTest()
        {
            var strikeMissions = DataConfigContent[ROOT_TAG]?["GameAreas"]?["StrikeMissions"] as JArray;
            Assert.NotNull(strikeMissions, "StrikeMissions array missing");
            foreach (JObject element in strikeMissions)
            {
                Assert.GreaterOrEqual((int)element["ID"], 1);

                ValidateString((string)element["NameEN"]);

                ValidateString((string)element["NameDE"]);

                ValidateUrl((string)element["AvatarURL"]);
            }
        }

        [Test]
        public void DataConfigFractalPlausibilityTest()
        {
            var fractals = DataConfigContent[ROOT_TAG]?["GameAreas"]?["Fractals"] as JArray;
            Assert.NotNull(fractals, "Fractals array missing");
            foreach (JObject element in fractals)
            {
                Assert.GreaterOrEqual((int)element["Level"], 1);

                ValidateString((string)element["NameEN"]);

                ValidateString((string)element["NameDE"]);

                ValidateUrl((string)element["AvatarURL"]);
            }
        }

        [Test]
        public void DataConfigDRMPlausibilityTest()
        {
            var dragonResponseMissions = DataConfigContent[ROOT_TAG]?["GameAreas"]?["DragonResponseMissions"] as JArray;
            Assert.NotNull(dragonResponseMissions, "StrikeMissions array missing");
            foreach (JObject element in dragonResponseMissions)
            {
                Assert.GreaterOrEqual((int)element["ID"], 1);

                ValidateString((string)element["NameEN"]);

                ValidateString((string)element["NameDE"]);

                ValidateUrl((string)element["AvatarURL"]);
            }
        }

        [Test]
        public void DataConfigRemainingAreaPlausibilityTest()
        {
            foreach (var area in new string[] { "WvW", "Training", "Unknown" })
            {
                var areaData = DataConfigContent[ROOT_TAG]?["GameAreas"]?[area] as JArray;
                Assert.NotNull(areaData, area + " array missing");

                foreach (JObject element in areaData)
                {
                    ValidateString((string)element["NameEN"]);

                    ValidateString((string)element["NameDE"]);

                    ValidateString((string)element["ShortNameEN"]);

                    ValidateString((string)element["ShortNameDE"]);

                    ValidateUrl((string)element["AvatarURL"]);
                }
            }
        }

        private readonly List<string> ValidGameAreas = new List<string> { "RaidWings", "StrikeMissions", "Fractals", "WvW", "DragonResponseMissions", "Training", "Unknown" };
        
        [Test]
        public void DataConfigBossPlausibilityTest()
        {
            var bosses = DataConfigContent[ROOT_TAG]?["Bosses"] as JArray;
            Assert.NotNull(bosses, "Boss array missing");
            List<int> ids = new List<int>();
            foreach (JToken boss in bosses)
            {
                Assert.GreaterOrEqual((int)boss["ID"], 0, "Id wrong by " + (string)boss["NameEN"]);
                ids.Add((int)boss["ID"]);

                ValidateString((string)boss["NameEN"]);
                ValidateString((string)boss["NameDE"]);
                ValidateString((string)boss["FolderEN"]);
                ValidateString((string)boss["FolderDE"]);
                ValidateString((string)boss["EiName"]);

                Assert.True(ValidGameAreas.Contains((string)boss["GameAreaName"]));

                Assert.GreaterOrEqual((int)boss["GameAreaID"], 0);

                ValidateDiscordEmote((string)boss["DiscordEmote"]);

                ValidateUrl((string)boss["AvatarURL"]);

                var RaidOrgaID = (int)boss["RaidOrgaPlusID"];
                Assert.IsTrue(RaidOrgaID > 0 || RaidOrgaID == -1, "RaidOrgaIO should be > 0 or == -1");
            }

            var bossCount = bosses.Count;
            var distinctIdCount = ids.Distinct().Count();

            if (bossCount != distinctIdCount)
            {
                var errMsg = GetDuplicateErrorMessage(ids);
                Assert.Fail(bossCount - distinctIdCount + " duplicated ID(s) Present: " + errMsg);
            }
        }

        private string GetDuplicateErrorMessage(IEnumerable<int> ids)
        {
            var duplicates = ids
                .Distinct()
                .AsParallel()
                .Where(num => ids.Count(e => e == num) > 1);
            return string.Join(", ", duplicates);
        }

        [Test]
        public void DataConfigAddEnemysPlausibilityTest()
        {
            var adds = DataConfigContent[ROOT_TAG]?["AddEnemys"] as JArray;
            Assert.NotNull(adds, "AddEnemys array missing");
            List<int> ids = new List<int>();
            foreach (JToken add in adds)
            {
                Assert.GreaterOrEqual((int)add["ID"], 0, "Invalid ID at addEnemy " + (string)add["NameEN"]);
                ids.Add((int)add["ID"]);

                ValidateString((string)add["NameEN"]);
                ValidateString((string)add["NameDE"]);

                Assert.True(ValidGameAreas.Contains((string)add["GameAreaName"]));

                Assert.GreaterOrEqual((int)add["GameAreaID"], 0);
            }

            var bossCount = adds.Count;
            var distinctIdCount = ids.Distinct().Count();

            if (bossCount != distinctIdCount)
            {
                var errMsg = GetDuplicateErrorMessage(ids);
                Assert.Fail(bossCount - distinctIdCount + " duplicated ID(s) Present: " + errMsg);
            }
        }

        [Test]
        public void DataConfigMiscPlausibilityTest()
        {
            var misc = DataConfigContent[ROOT_TAG]?["Misc"] as JObject;
            Assert.NotNull(misc, "Misc section missing");

            ValidateDiscordEmote((string)misc["Emotes"]["Kill"]);
            ValidateDiscordEmote((string)misc["Emotes"]["Wipe"]);
        }
    }
}
