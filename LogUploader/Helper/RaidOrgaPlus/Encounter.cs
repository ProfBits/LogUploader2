using System;
using System.Collections.Generic;
using System.Linq;
using LogUploader.Data.RaidOrgaPlus;
using LogUploader.Data;

namespace LogUploader.Helper.RaidOrgaPlus
{
    /// <summary>
    /// Connects Log with TeamComp and merges them
    /// </summary>
    public class Encounter
    {
        public List<RoPlusPlayer> Players { get; set; }
        public Boss Boss { get; set; }
        public TeamComp TC { get; set; }

        private static readonly Boss[] BossRequiresExclude = new Boss[] { 
            Boss.Get(eBosses.Deimos), // Frindly NPC concept - Desmina
            Boss.Get(eBosses.Desmina), // Frindly NCP concept - Saul
            Boss.Get(eBosses.ConjuredAmalgamate) // Sword fake Player
        };

        public Encounter(TeamComp tc, CachedLog log, Raid r)
        {
            TC = tc;
            Boss = tc.Encounter;
            Players = GetPlayersFromLog(log.PlayersNew, r, tc.Encounter);
            tc.Success = log.Succsess;
        }

        private static List<RoPlusPlayer> GetPlayersFromLog(IEnumerable<SimplePlayer> newPlayer, Raid r, Boss boss)
        {
            var players = new List<RoPlusPlayer>();
            players.AddRange(newPlayer.Select(p => new RoPlusPlayer(p, r)));
            if (BossRequiresExclude.Contains(boss))
            {
                RemoveFakePlayers(players);
            }
            return players;
        }

        private static void RemoveFakePlayers(List<RoPlusPlayer> players)
        {
            players.RemoveAll(p => !p.AccountName.Contains('.') || p == null);
        }

        internal void GuessRoles()
        {
            RolePedictor.PredictRoles((eBosses)Boss.ID, Players);
        }

        internal void RefineRoles()
        {
        }

        internal void RemoveNotAttededPlayers()
        {
            foreach (var position in TC.Players)
            {
                if (position.IsLFG())
                {
                    position.Free();
                } 
                else if (!Players.Any(p => p.RaidOrgaID == position.ID))
                {
                    position.Free();
                }
                else if (position.IsFree())
                {
                    continue;
                }
            }
        }

        internal void MergePlayers()
        {
            MergeNamedPlayers();
            MergeUnnamedPlayers();
            OverrideRemaining();
            MergeDuplicatedPlayers();
        }

        private void MergeNamedPlayers()
        {
            foreach (var player in Players.GroupBy(p => p.AccountName).Where(g => TC.Exists(g.Key)))
            {
                MergePlayer(TC.GetByName(player.First().AccountName), player.First());
            }
        }

        private void MergeUnnamedPlayers()
        {
            foreach (var player in Players.Where(p => !TC.Exists(p.AccountName)))
            {
                if (TC.Exists(player.Class, player.Roles))
                    MergePlayer(TC.Get(player.Class, player.Roles), player);
                if (TC.Exists(player.Class))
                    MergePlayer(TC.Get(player.Class), player);
                if (TC.Exists(player.Roles))
                    MergePlayer(TC.Get(player.Roles), player);
            }
        }

        private static void MergePlayer(Position position, RoPlusPlayer player)
        {
            if (position == null)
            {
                return;
            }

            position.Set(player);
        }

        private void OverrideRemaining()
        {
            foreach (var player in Players.Where(p => !TC.Exists(p.AccountName)))
            {
                MergePlayer(TC.Get(), player);
            }
        }

        private void MergeDuplicatedPlayers()
        {
            foreach (var player in Players.GroupBy(p => p.AccountName).Where(g => TC.Exists(g.Key)).Where(g => g.Count() > 1))
            {
                var dupes = player.Skip(1).ToList();
                MergeDuplicatedPlayer(dupes);
            }
        }

        private void MergeDuplicatedPlayer(List<RoPlusPlayer> dupes)
        {
            for (int i = 0; i < dupes.Count(); i++)
            {
                var dupe = dupes[0];

                var pos = TC.Players.Where(p => p.AccName == dupe.AccountName).Skip(i + 1).FirstOrDefault();
                if (pos is null)
                {
                    pos = GetBackpuPosForDupe(dupe);
                }

                MergePlayer(pos, dupe);
            }
        }

        private Position GetBackpuPosForDupe(RoPlusPlayer dupe)
        {
            if (TC.Exists(dupe.Class, dupe.Roles))
            {
                return TC.Get(dupe.Class, dupe.Roles);
            }
            if (TC.Exists(dupe.Class))
            {
                return TC.Get(dupe.Class, dupe.Roles);
            }
            if (TC.Exists(dupe.Roles))
            {
                return TC.Get(dupe.Class, dupe.Roles);
            }
            return TC.Get();
        }

        internal void EnsureAllPlayers()
        {
            var playersInLog = Players.Count;
            var playersInTC = TC.Players.Count(pos => pos.IsPlayer() || pos.IsLFG());

            if (playersInLog != playersInTC)
            {
                Logger.Warn("Failed to merge log and teamcomp. Number of players differ.");
                Logger.Debug("Actual players:\n" + string.Join("\n", Players.Select(p => GetPlayerString(p))));
                Logger.Debug("TC elments:\n" + string.Join("\n", TC.Players.Select(p => GetElementString(p))));
            }
        }

        private string GetPlayerString(RoPlusPlayer player)
        {
            var res = "";
            res += $"{player.AccountName}:{player.RaidOrgaID} {player.Class} Roles: ";
            res += string.Join(", ", player.Roles.Select(r => r.ToString()));
            return res;
        }

        private string GetElementString(Position pos)
        {
            var res = "";
            res += $"{pos.AccName}:{pos.ID} {pos.Profession} Roles: ";
            res += string.Join(", ", pos.Roles.Select(r => r.ToString()));
            return res;
        }

        internal void FillTeamComp()
        {
            for (int i = 1; i <= 10; i++)
                if (!TC.Players.Any(p => p.Pos == i))
                    TC.Players.Add(new Position(i, 0, "", Role.Empty, Profession.Unknown));
        }

        internal void SortIfNew()
        {
            if (!TC.AufstellungsID.HasValue)
                TC.OrderPlayers(Boss);
        }

        private class RolePedictor
        {
            public static void PredictRoles(eBosses encounter, IEnumerable<RoPlusPlayer> players)
            {
                var orderedPlayers = OrderPlayersByDps(players);
                foreach (var e in orderedPlayers)
                {
                    GetPlayerPredictor(e.player.Class.ProfessionEnum)(e.player, e.relativePos);
                }
                GetEncounterCorrector(encounter)(orderedPlayers);
            }

            private static IEnumerable<(RoPlusPlayer player, int relativePos)> OrderPlayersByDps(IEnumerable<RoPlusPlayer> players)
            {
                var orderedPLayers = players.OrderBy(p => p.DPS).ToList();
                for (int i = 0; i < orderedPLayers.Count; i++)
                {
                    yield return (orderedPLayers[i], i);
                }
            }

            private static Action<RoPlusPlayer, int> GetPlayerPredictor(eProfession professionEnum)
            {
                switch (professionEnum)
                {
                    case eProfession.Harbinger:
                    case eProfession.Untamed:
                    case eProfession.Deadeye:
                        return CombinedPredictor(
                            SimpleQuicknessPredictor,
                            SimpleDpsClassPredictor
                            );
                    case eProfession.Mirage:
                    case eProfession.Bladesworn:
                        return CombinedPredictor(
                            SimpleAlacPredictor,
                            SimpleDpsClassPredictor
                            );
                    case eProfession.Catalyst:
                    case eProfession.Berserker:
                    case eProfession.Herald:
                    case eProfession.Scrapper:
                    case eProfession.Firebrand:
                        return CombinedPredictor(
                            SimpleQuicknessPredictor,
                            SimpleHealOrDpsPredictor
                            );
                    case eProfession.Druid:
                    case eProfession.Tempest:
                    case eProfession.Renegade:
                    case eProfession.Mechanist:
                    case eProfession.Scourge:
                    case eProfession.Specter:
                    case eProfession.Willbender:
                        return CombinedPredictor(
                            SimpleAlacPredictor,
                            SimpleHealOrDpsPredictor
                            );
                    case eProfession.Chronomancer:
                        return CombinedPredictor(
                            SimpleAlacPredictor,
                            SimpleQuicknessPredictor,
                            SimpleHealOrDpsPredictor
                            );
                    case eProfession.Thief:
                    case eProfession.Daredevil:
                        return BoonThiefPredictor;
                    case eProfession.Necromancer:
                    case eProfession.Reaper:
                    case eProfession.Mesmer:
                    case eProfession.Virtuoso:
                    case eProfession.Engineer:
                    case eProfession.Holosmith:
                    case eProfession.Ranger:
                    case eProfession.Soulbeast:
                    case eProfession.Revenant:
                    case eProfession.Guardian:
                    case eProfession.Dragonhunter:
                    case eProfession.Vindicator:
                    case eProfession.Elementalist:
                    case eProfession.Weaver:
                    case eProfession.Warrior:
                    case eProfession.Spellbreaker:
                    case eProfession.Unknown:
                    default:
                        return SimpleDpsClassPredictor;
                }
            }

            private static Action<RoPlusPlayer, int> CombinedPredictor(params Action<RoPlusPlayer, int>[]  predictors)
            {
                return (RoPlusPlayer player, int pos) =>
                {
                    foreach (var predictor in predictors)
                    {
                        predictor(player, pos);
                    }
                };
            }

            private static void SimpleQuicknessPredictor(RoPlusPlayer player, int _)
            {
                if (player.GroupQuickness > 10)
                {
                    player.Roles.Add(Role.Quickness);
                }
            }

            private static void SimpleAlacPredictor(RoPlusPlayer player, int _)
            {
                if (player.GroupAlacrity > 10)
                {
                    player.Roles.Add(Role.Alacrity);
                }
            }

            private static void SimpleDpsClassPredictor(RoPlusPlayer player, int _)
            {
                if ((double)player.PDPS / player.DPS > 0.40)
                {
                    player.Roles.Add(Role.Power);
                }
                if ((double)player.CDPS / player.DPS > 0.40)
                {
                    player.Roles.Add(Role.Condi);
                }
            }

            private static void BoonThiefPredictor(RoPlusPlayer player, int pos)
            {
                if (player.GroupQuickness > 20)
                {
                    player.Roles.Add(Role.Utility);
                    return;
                }
                SimpleDpsClassPredictor(player, pos);
            }

            private static void SimpleHealOrDpsPredictor(RoPlusPlayer player, int pos)
            {
                if (player.Healing >= 5)
                {
                    player.Roles.Add(Role.Heal);
                }
                else
                {
                    SimpleDpsClassPredictor(player, pos);
                }
            }

            private static Action<IEnumerable<(RoPlusPlayer player, int pos)>> GetEncounterCorrector(eBosses encounter)
            {
                switch (encounter)
                {
                    case eBosses.Qadim:
                        return CombinedCorrector(
                            ThoughnessTankCorrector,
                            QadimCorrector,
                            RoleReduce
                            );
                    case eBosses.SoullessHorror:
                        return CombinedCorrector(
                            DoubleTankCorrector,
                            GolemPusherCorrector,
                            RoleReduce
                            );
                    case eBosses.PeerlessQadim:
                        return CombinedCorrector(
                            ThoughnessTankCorrector,
                            PeerlessQadimPylons,
                            RoleReduce
                            );
                    case eBosses.Deimos:
                        return CombinedCorrector(
                            ThoughnessTankCorrector,
                            DeimosHKCorrector,
                            RoleReduce
                            );
                    case eBosses.Nikare:
                    case eBosses.Kenut:
                        return CombinedCorrector(
                            DoubleTankCorrector,
                            RoleReduce
                            );
                    case eBosses.MassiveGolem:
                    case eBosses.AvgGolem:
                    case eBosses.LGolem:
                    case eBosses.MedGolem:
                    case eBosses.StdGolem:
                        return _ => { };
                    case eBosses.ValeGuardian:
                    case eBosses.Gorseval:
                    case eBosses.KeepConstruct:
                    case eBosses.Xera:
                    case eBosses.Desmina:
                    case eBosses.BrokenKing:
                    case eBosses.SoulEater:
                    case eBosses.Dhuum:
                    case eBosses.Adina:
                    case eBosses.Sabir:
                        return CombinedCorrector(
                            ThoughnessTankCorrector,
                            RoleReduce
                            );
                    default:
                        return CombinedCorrector(
                            RoleReduce
                            );

                }
            }

            private static Action<IEnumerable<(RoPlusPlayer player, int pos)>> CombinedCorrector(params Action<IEnumerable<(RoPlusPlayer player, int pos)>>[] correctors)
            {
                return players =>
                {
                    foreach (var corrector in correctors)
                    {
                        corrector(players);
                    }
                };
            }

            private static void ThoughnessTankCorrector(IEnumerable<(RoPlusPlayer player, int pos)> players)
            {
                var maxThougness = players.Max(p => p.player.Toughness);
                if (maxThougness == 0) return;
                players.OrderBy(p => p.pos).Where(p => p.player.Toughness == maxThougness).First().player.Roles.Add(Role.Tank);
            }

            private static void DoubleTankCorrector(IEnumerable<(RoPlusPlayer player, int pos)> players)
            {
                foreach (var tank in players.Select(p => p.player)
                    .Where(p => p.Toughness > 0)
                    .OrderByDescending(p => p.Toughness)
                    .ThenBy(p => p.DPS)
                    .Take(2))
                {
                    tank.Roles.Add(Role.Tank);
                }
                
            }

            private static void DeimosHKCorrector(IEnumerable<(RoPlusPlayer player, int pos)> players)
            {
                if (players.Where(p => !p.player.Roles.Contains(Role.Tank)).Any())
                {
                    var hk = players.Where(p => !p.player.Roles.Contains(Role.Tank))
                        .OrderBy(p => p.pos)
                        .First().player;
                    hk.Roles.Clear();
                    hk.Roles.Add(Role.Special);
                }
            }

            private static void PeerlessQadimPylons(IEnumerable<(RoPlusPlayer player, int pos)> players)
            {
                var kiters = players.OrderBy(p => p.pos)
                    .Where(p => !p.player.Roles.Contains(Role.Tank))
                    .Where(p => p.player.Class == eProfession.Deadeye 
                                                     || p.player.Class == eProfession.Scourge 
                                                     || p.player.Class == eProfession.Virtuoso);
                kiters = kiters.Take(Math.Min(3, kiters.Count()));
                foreach (var kiter in kiters)
                    kiter.player.Roles.Add(Role.Kiter);
            }

            private static void GolemPusherCorrector(IEnumerable<(RoPlusPlayer player, int pos)> players)
            {
                var kanidats = players.Where(p => p.player.Class == eProfession.Druid && p.player.Roles.Contains(Role.Heal)).ToList();
                if (kanidats.Count == 0)
                {
                    kanidats = players.Where(p => p.player.Roles.Contains(Role.Heal)).ToList();
                }
                if (kanidats.Count == 1)
                {
                    kanidats[0].player.Roles.Add(Role.Special);
                }
                else if (kanidats.Count > 1)
                {
                    kanidats.OrderBy(p => p.pos).First().player.Roles.Add(Role.Special);
                }
            }
            
            private static void QadimCorrector(IEnumerable<(RoPlusPlayer player, int pos)> players)
            {
                /*
                    case 20934: //Qadim1
            "ID": 21285,
            "NameEN": "Ancient Invoked Hydra",
            "ID": 21183,
            "NameEN": "Wyvern Patriarch",
            "ID": 21073,
            "NameEN": "Apocalypse Bringer",
            "ID": 20997,
            "NameEN": "Wyvern Matriarch",
                 */

                /* 1. Tank least damage chrono? toughness
                 * 2. Healer least damage healing power
                 * 3. kite deadeye or deardeaviel least overall dps
                 * ?. bs warrier least dps ??
                 * 4. lamp deadeye or deardeaviel or reaper or anyting? least overall dps
                 * 5. rest is dps
                 */

                if (!players.Any(p => p.player.Roles.Contains(Role.Tank)))
                {
                    //Tank without thoughness
                    var tank = players.Where(p => p.player.Class == eProfession.Chronomancer || p.player.Class == eProfession.Mechanist)
                        .OrderBy(p => p.pos);
                    if (tank.Any())
                    {
                        tank.First().player.Roles.Add(Role.Tank);
                    }
                }

                var kiters = players.Where(p => 
                           p.player.Class == eProfession.Deadeye
                        || p.player.Class == eProfession.Daredevil
                        || p.player.Class == eProfession.Mechanist)
                    .Where(p => !p.player.Roles.Contains(Role.Tank))
                    .OrderBy(p => p.pos);
                if (kiters.Any())
                {
                    kiters.First().player.Roles.Add(Role.Kiter);
                }


                var lamps = players.Where(p =>
                           p.player.Class == eProfession.Reaper
                        || p.player.Class == eProfession.Daredevil)
                    .Where(p => !p.player.Roles.Contains(Role.Tank))
                    .Where(p => !p.player.Roles.Contains(Role.Kiter))
                    .OrderBy(p => p.pos);
                if (lamps.Any())
                {
                    lamps.First().player.Roles.Add(Role.Special);
                }
            }

            private static void RoleReduce(IEnumerable<(RoPlusPlayer player, int pos)> players)
            {
                foreach (var player in players.Select(p => p.player))
                {
                    if (player.Roles.Contains(Role.Tank) || player.Roles.Contains(Role.Heal))
                    {
                        player.Roles.Remove(Role.Power);
                        player.Roles.Remove(Role.Condi);
                    }

                    if (player.Roles.Contains(Role.Utility) && (player.Roles.Contains(Role.Alacrity) || player.Roles.Contains(Role.Quickness)))
                    {
                        player.Roles.Remove(Role.Alacrity);
                        player.Roles.Remove(Role.Quickness);
                    }

                    if (player.Roles.Count > 4)
                    {
                        var mostRelevantRoles = player.Roles.OrderByDescending(r => GetRoleImportance(r)).Take(4);
                        player.Roles.IntersectWith(mostRelevantRoles);
                    }
                }
            }

            private static int GetRoleImportance(Role r)
            {
                switch (r)
                {
                    case Role.Empty:
                    case Role.Banner:
                    case Role.Utility:
                        return 0;
                    case Role.Power:
                    case Role.Condi:
                        return 1;
                    case Role.Quickness:
                    case Role.Alacrity:
                        return 3;
                    case Role.Heal:
                        return 4;
                    case Role.Special:
                    case Role.Kiter:
                        return 5;
                    case Role.Tank:
                        return 6;
                    default:
                        return 0;
                }
            }
        }
    }
}
