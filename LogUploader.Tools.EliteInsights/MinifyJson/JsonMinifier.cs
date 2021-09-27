using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

using Newtonsoft.Json;

namespace LogUploader.Tools.EliteInsights.MinifyJson
{
    public static class JsonMinifier
    {
        private readonly static string[] TO_EXCLUDE = new string[]
        {
            //root
            "personalBuffs",
            "damageModMap",
            "buffMap",
            "skillMap",
            "uploadLinks",
            "combatReplayMetaData",

            //player
            "targetDamage1S",
            "targetDamageDist",
            "damageModifiers",
            "damageModifiersTarget",
            "selfBuffs",
            "deathRecap",
            "targetPowerDamage1S",
            "targetConditionDamage1S",
            "targetBreakbarDamage1S",
            "targetDamageDist",
            "statsTargets",
            "offGroupBuffs",
            "buffUptimesActive",
            "selfBuffsActive",
            "groupBuffsActive",
            "offGroupBuffsActive",
            "squadBuffsActive",

            //target
            "avgConditions",
            "avgBoons",
            "buffs",
            "breakbarPercents",

            //taget and player
            "minions",
            "totalDamageDist",
            "totalDamageTaken",
            "rotation",
            "damage1S",
            "conditionsStates",
            "powerDamage1S",
            "conditionDamage1S",
            "breakbarDamage1S",
            "boonsStates",
            "activeCombatMinions",
            "healthPercents",
            "barrierPercents",
            "combatReplayData"
        };

        private static readonly Encoding ENDODING = Encoding.UTF8;

        public static void Minify(string[] files)
        {
            //var files = Directory.GetFiles("json");
            for (int i = 0; i < files.Length; i++)
            {
                var file = files[i];
                FileInfo f = new FileInfo(file);
                Console.WriteLine($"Processing {i + 1} of {files.Length} ({f.Name})");
                var fs = f.OpenRead();
                var res = ReduceJson(fs, TO_EXCLUDE);
                File.WriteAllText(f.DirectoryName + f.Name.Substring(0, f.Extension.Length) + ".small" + f.Extension, res, ENDODING);
                GC.Collect();
                Task.Delay(1000).Wait();
            }
        }

        internal static string ReduceJson(Stream stream, params string[] toExclude)
        {
            if (stream is null) throw new ArgumentNullException(nameof(stream));
            if (!stream.CanSeek) throw new ArgumentException("Stream needs to be seekeable", nameof(stream));

            List<(long beginn, long end)> ToSkip = new List<(long beginn, long end)>();

            HashSet<string> organizedeToExclude = new HashSet<string>(toExclude);
            string result;

            using (StreamReader sr = new StreamReader(stream, ENDODING))
            using (JsonTextReader jtr = new JsonTextReader(sr))
            {
                Console.WriteLine("\tParsing...");

                var data = JToken.ReadFrom(jtr);

                Console.WriteLine("\tReducing...");

                RemoveTokens(data, organizedeToExclude);

                Console.WriteLine("\tFinding players...");
                List<(string acc, string name, int num)> players = FindPlayers(data);

                Console.WriteLine("\tCreateing new json...");

                result = data.ToString(Formatting.None);

                Console.WriteLine("\tRenaming players...");
                result = RenamePlayers(result, players);
            }

            return result;
        }

        internal static string RenamePlayers(string result, List<(string acc, string name, int num)> players)
        {
            foreach ((string acc, string name, int num) in players)
            {
                result = result.Replace(acc, $"Account.{1000 + num}");
                result = result.Replace(name, $"Player {num}");
            }

            return result;
        }

        internal static List<(string acc, string name, int num)> FindPlayers(JToken data)
        {
            List<(string acc, string name, int num)> players = new List<(string acc, string name, int num)>();
            int num = 1;

            foreach (var player in data["players"])
            {
                string acc = (string)player["account"];
                string name = (string)player["name"];
                int i = num++;
                players.Add((acc, name, i));
            }

            return players;
        }

        internal static void RemoveTokens(JToken data, HashSet<string> toExclude)
        {
            if (data is null) throw new ArgumentNullException(nameof(data));
            if (toExclude is null) throw new ArgumentNullException(nameof(toExclude));

            var children = data.Children<JProperty>().ToList();
            foreach (var child in children)
            {
                if (toExclude.Contains(child.Name))
                    child.Remove();
            }

            foreach (var child in data.Children())
            {
                RemoveTokens(child, toExclude);
            }
        }
    }
}
