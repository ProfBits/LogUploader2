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

        private bool overrideTank = false;

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
            var nonePlayers = players.Where(p => !p.AccountName.Contains('.'));
            foreach (var notPlayer in nonePlayers)
            {
                if (notPlayer != null)
                    players.Remove(notPlayer);
            }
        }

        internal void GuessRoles()
        {
            switch (Boss.ID)
            {
                //Special Bosses
                case 17154: //Deimos
                    SetTank();
                    SetDeimosHK();
                    AssigenSupporter();
                    SetBS();
                    FillUpDps();
                    break;
                case 19651: //Eyes
                case 19844: //Eyes
                    AssigenSupporter();
                    SetBS();
                    FillUpDps();
                    break;
                case 19828: //River
                    GuessRiver();
                    break;
                case 20934: //Qadim1
                    GuessQadim1();
                    break;
                case 22000: //Qadim2
                    SetTank();
                    SetQadim2Pylons();
                    AssigenSupporter();
                    SetBS();
                    FillUpDps();
                    break;
                case 19767: //SH
                    SetTank();
                    goto default;
                case 21105: //Largos
                case 21089:
                    SetLargosTank();
                    goto default;
                case 16253: //Escort
                    SetTower();
                    goto case 43974;

                //No Tank
                case 15375: //Sabeta
                case 16123: //Sloth
                case 16088: //Trio
                case 16137: //Trio
                case 16125: //Trio
                case 16115: //Mathias
                case 16247: //TC
                case 17194: //Carin
                case 17172: //Mo
                case 17188: //Samarog
                case 43974: //CA
                    AssigenSupporter();
                    SetBS();
                    FillUpDps();
                    return;

                default:
                    SetTank();
                    AssigenSupporter();
                    SetBS();
                    FillUpDps();
                    return;
            }

        }

        private void SetTower()
        {
            var validProfessions = new eProfession[] { eProfession.Chronomancer, eProfession.Mirage, eProfession.Mesmer };
            var orderdPlayers = Players
                .Where(p => p.Role == Role.Empty && p.Toughness > 0 && p.DPS_Target < 500)
                .Where(p => validProfessions.Contains(p.Class.ProfessionEnum))
                .OrderByDescending(p => p.Toughness)
                .ThenBy(p => p.DPS);
                
            if (orderdPlayers.Count() > 0)
            {
                foreach (var p in orderdPlayers.Take(Math.Max(orderdPlayers.Count(), 2)))
                    p.Role = Role.Special;
            }
        }

        private void GuessQadim1()
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
             * 5. res is dps
             */

            SetTank();
            if (!Players.Any(p => p.Role == Role.Tank))
            {
                AssigenSupporter();

                var tank = Players.Where(p => p.Role == Role.Empty || p.Role == Role.Utility && p.Class == eProfession.Chronomancer)
                    .OrderBy(p => p.Class == eProfession.Chronomancer ? 0 : 1)
                    .ThenBy(p => p.DPS)
                    .FirstOrDefault();
                if (tank != null)
                    tank.Role = Role.Tank;
            }
            else
            {
                AssigenSupporter();
            }
            var kiter = Players.Where(p => p.Role == Role.Empty && (p.Class == eProfession.Deadeye || p.Class == eProfession.Daredevil))
                .OrderBy(p => p.DPS)
                .FirstOrDefault();
            if (kiter != null)
                kiter.Role = Role.Kiter;
            SetBS();
            var lamp = Players.Where(p => p.Role == Role.Empty)
                .OrderBy(p => (p.Class == eProfession.Deadeye || p.Class == eProfession.Daredevil || p.Class == eProfession.Reaper) ? 0 : 1)
                    .ThenBy(p => p.DPS)
                    .FirstOrDefault();
            if (lamp != null)
                lamp.Role = Role.Special;
            FillUpDps();

        }

        private void SetLargosTank()
        {
            if (Players.Where(p => p.Toughness >= 5).Count() >= 2)
                SetTank();
        }

        private void SetDeimosHK()
        {
            var hk = Players.Where(p => p.Role == Role.Empty).OrderBy(p => p.DPS).FirstOrDefault();
            if (hk != null)
                hk.Role = Role.Special;
        }

        private void GuessRiver()
        {
            var ThreashholdDMG = Players.Max(p => p.DPS) / 2;
            foreach (var player in Players)
            {
                if (player.DPS <= ThreashholdDMG && player.Healing > 1)
                    player.Role = Role.Heal;
                if (player.Class == eProfession.Chronomancer)
                    player.Role = Role.Utility;
                if (player.Class == eProfession.Scrapper && player.DPS < 500)
                    player.Role = Role.Special;
            }
            SetBS();
            FillUpDps();
        }

        internal void RefineRoles()
        {
            if (TC.Players.Any(p => p.Role == Role.Banner))
            {
                var tcBanners = TC.Players.Where(p => p.Role == Role.Banner && (p.Profession == eProfession.Warrior || p.Profession == eProfession.Berserker || p.Profession == eProfession.Spellbreaker)).Select(p => p.AccName);
                var actualWarriers = Players.Where(p => p.Class == eProfession.Warrior || p.Class == eProfession.Berserker || p.Class == eProfession.Spellbreaker);
                if (actualWarriers.Any(p => tcBanners.Contains(p.AccountName)))
                {
                    foreach (var actualWarrier in actualWarriers)
                    {
                        if (tcBanners.Contains(actualWarrier.AccountName))
                            actualWarrier.Role = Role.Banner;
                        else
                            SetDps(actualWarrier);
                    }
                }
            }
        }

        private void SetQadim2Pylons()
        {
            var kiters = Players.OrderBy(p => p.DPS).Where(p => p.Class == eProfession.Deadeye || p.Class == eProfession.Scourge);
            kiters = kiters.Take(Math.Min(3, kiters.Count()));
            foreach (var kiter in kiters)
                kiter.Role = Role.Kiter;
        }

        private void AssigenSupporter()
        {
            var orderdPlayers = Players.Where(p => p.Role == Role.Empty).OrderBy(p => p.DPS);
            foreach (var orderdPlayer in orderdPlayers)
            {
                switch (orderdPlayer.Class.ProfessionEnum)
                {
                    case eProfession.Firebrand:
                        if (orderdPlayer.GroupQuickness >= 10)
                            if (orderdPlayer.Healing > 2)
                                orderdPlayer.Role = Role.Heal;
                            else
                                orderdPlayer.Role = Role.Utility;
                        break;
                    case eProfession.Chronomancer:
                    case eProfession.Thief:
                    case eProfession.Daredevil:
                        if (orderdPlayer.GroupQuickness >= 20 && orderdPlayer.Concentration > 0)
                            orderdPlayer.Role = Role.Utility;
                        break;
                    case eProfession.Herald:
                        if (orderdPlayer.Healing < 4 && orderdPlayer.Concentration > 1)
                            orderdPlayer.Role = Role.Utility;
                        break;
                    case eProfession.Renegade:
                        if (orderdPlayer.GroupAlacrity >= 10)
                            if (orderdPlayer.Healing > 1)
                                orderdPlayer.Role = Role.Heal;
                            else if (orderdPlayer.Concentration > 0)
                                orderdPlayer.Role = Role.Utility;
                        break;
                    case eProfession.Druid:
                    case eProfession.Tempest:
                    case eProfession.Scourge:
                    //Maybe special healer dectection
                    default:
                        if (orderdPlayer.Healing > 2)
                            orderdPlayer.Role = Role.Heal;
                        break;
                }
            }
        }

        private void SetTank()
        {
            var orderdPlayers = Players.Where(p => p.Role == Role.Empty).OrderBy(p => p.DPS);
            if (orderdPlayers.Count() <= 0) return;
            var maxThougness = orderdPlayers.Max(p => p.Toughness);
            if (maxThougness == 0) return;
            orderdPlayers.Where(p => p.Toughness == maxThougness).First().Role = Role.Tank;
            overrideTank = true;
        }


        private void SetBS()
        {
            if (Players.Any(p => p.Role == Role.Banner))
                return;
            if (Players.Any(p => (p.Class == eProfession.Warrior || p.Class == eProfession.Berserker) && p.Role == Role.Empty))
                Players.OrderBy(p => p.DPS).First(p => (p.Class == eProfession.Warrior || p.Class == eProfession.Berserker) && p.Role == Role.Empty).Role = Role.Banner;
        }

        private void FillUpDps()
        {
            foreach (var player in Players.Where(p => p.Role == Role.Empty))
            {
                SetDps(player);
            }
        }

        private static void SetDps(RoPlusPlayer player)
        {
            if (player.PDPS < player.CDPS)
                player.Role = Role.Condi;
            else
                player.Role = Role.Power;
        }

        internal void RemoveNotAttededPlayers()
        {
            foreach (var player in TC.Players)
            {
                if (player.ID >= 0 && !Players.Any(p => p.RaidOrgaID == player.ID))
                    player.RemoveName();
            }

            var ROplusLFGCount = TC.Players.Where(p => p.IsLFG()).Count();
            var TargetLFGCount = Players.Where(p => p.IsLFG).Count();

            if (ROplusLFGCount > TargetLFGCount)
                foreach (var p in TC.Players.Where(p => p.IsLFG()).Take(ROplusLFGCount - TargetLFGCount))
                    p.RemoveName();
        }

        //TODO has to improve
        internal void RemoveDuplicates()
        {
            var dupes = TC.Players.Where(p => !(p.IsLFG() || string.IsNullOrEmpty(p.AccName))).GroupBy(p => p.AccName).Where(g => g.Count() > 1);
            foreach (var dupe in dupes)
            {
                var player = Players.Find(p => p.AccountName == dupe.Key);

                if (dupe.Any(pos => pos.Profession == player.Class && pos.Role == player.Role))
                {
                    var hit = dupe.First(pos => pos.Profession == player.Class && pos.Role == player.Role);
                    foreach (var pos in dupe.Where(p => p != hit))
                        pos.RemoveName();
                }
                else if (dupe.Any(pos => pos.Role == player.Role))
                {
                    var hit = dupe.First(pos => pos.Role == player.Role);
                    foreach (var pos in dupe.Where(p => p != hit))
                        pos.RemoveName();
                    hit.Set(player);
                }
                else if (dupe.Any(pos => pos.Profession == player.Class))
                {
                    var hit = dupe.First(pos => pos.Profession == player.Class);
                    foreach (var pos in dupe.Where(p => p != hit))
                        pos.RemoveName();
                    hit.Set(player);
                }
                else
                {
                    var hit = dupe.First();
                    foreach (var pos in dupe.Where(p => p != hit))
                        pos.RemoveName();
                    hit.Set(player);
                }
            }
        }

        internal void UpdateNamedPlayers()
        {
            foreach (var player in Players)
            {
                if (!player.IsLFG && TC.Exists(player.AccountName))
                {
                    Position pos = TC.GetByName(player.AccountName);
                    pos.UpdateProffessionRole(player.Class, player.Role, overrideTank);
                }
            }
        }

        internal void UpdateUnnamedPlayers()
        {
            var relevantPlayers = Players.Where(p => !TC.Exists(p.AccountName));

            foreach (var player in relevantPlayers)
                if (!TC.Exists(player.AccountName) && TC.Exists(player.Class, player.Role))
                    TC.Get(player.Class, player.Role).Set(player);
            foreach (var player in relevantPlayers)
                if (!TC.Exists(player.AccountName) && TC.Exists(Profession.Unknown, player.Role))
                    TC.Get(Profession.Unknown, player.Role).Set(player);
            foreach (var player in relevantPlayers)
                if (!TC.Exists(player.AccountName) && TC.Exists(player.Class, Role.Empty))
                    TC.Get(player.Class, Role.Empty).Set(player);
            foreach (var player in relevantPlayers)
                if (!TC.Exists(player.AccountName) && TC.Exists(Profession.Unknown, Role.Empty))
                    TC.Get(Profession.Unknown, Role.Empty).Set(player);

            relevantPlayers = Players.Where(p => !TC.Exists(p.AccountName));
            foreach (var player in relevantPlayers)
                if (!TC.Exists(player.AccountName) && TC.Exists(player.Role))
                    TC.Get(player.Role).Set(player);
            foreach (var player in relevantPlayers)
                if (!TC.Exists(player.AccountName) && TC.Exists(player.Class))
                    TC.Get(player.Class).Set(player);

            relevantPlayers = Players.Where(p => !TC.Exists(p.AccountName));
            foreach (var player in relevantPlayers)
                if (!TC.Exists(player.AccountName) && TC.Exists(Role.Empty))
                    TC.Get(Role.Empty).Set(player);
            foreach (var player in relevantPlayers)
                if (!TC.Exists(player.AccountName) && TC.Exists(Profession.Unknown))
                    TC.Get(Profession.Unknown).Set(player);
            foreach (var player in relevantPlayers)
                if (!TC.Exists(player.AccountName) && TC.Exists())
                    TC.Get().Set(player);
        }

        internal void InsertDuplicatesPlayers()
        {
            var relevantPlayers = Players
                .Where(p => Players.Where(p2 => p.RaidOrgaID == p2.RaidOrgaID).Count() > TC.Players.Where(p2 => p2.ID == p.RaidOrgaID).Count())
                .Where(p => !TC.Players.Exists(p2 => p.AccountName == p2.AccName && p.Class.RaidOrgaPlusID == p2.ClassID))
                .Distinct();

            foreach (var player in relevantPlayers)
                if (TC.Exists(player.Class, player.Role))
                    TC.Get(player.Class, player.Role).Set(player);
            foreach (var player in relevantPlayers)
                if (TC.Exists(Profession.Unknown, player.Role))
                    TC.Get(Profession.Unknown, player.Role).Set(player);
            foreach (var player in relevantPlayers)
                if (TC.Exists(player.Class, Role.Empty))
                    TC.Get(player.Class, Role.Empty).Set(player);
            foreach (var player in relevantPlayers)
                if (TC.Exists(Profession.Unknown, Role.Empty))
                    TC.Get(Profession.Unknown, Role.Empty).Set(player);

            relevantPlayers = Players
                .Where(p => Players.Where(p2 => p.RaidOrgaID == p2.RaidOrgaID).Count() > TC.Players.Where(p2 => p2.ID == p.RaidOrgaID).Count())
                .Where(p => !TC.Players.Exists(p2 => p.AccountName == p2.AccName && p.Class.RaidOrgaPlusID == p2.ClassID))
                .Distinct();
            foreach (var player in relevantPlayers)
                if (TC.Exists(player.Role))
                    TC.Get(player.Role).Set(player);
            foreach (var player in relevantPlayers)
                if (TC.Exists(player.Class))
                    TC.Get(player.Class).Set(player);

            relevantPlayers = Players
                .Where(p => Players.Where(p2 => p.RaidOrgaID == p2.RaidOrgaID).Count() > TC.Players.Where(p2 => p2.ID == p.RaidOrgaID).Count())
                .Where(p => !TC.Players.Exists(p2 => p.AccountName == p2.AccName && p.Class.RaidOrgaPlusID == p2.ClassID))
                .Distinct();
            foreach (var player in relevantPlayers)
                if (TC.Exists(Role.Empty))
                    TC.Get(Role.Empty).Set(player);
            foreach (var player in relevantPlayers)
                if (TC.Exists(Profession.Unknown))
                    TC.Get(Profession.Unknown).Set(player);
            foreach (var player in relevantPlayers)
                if (TC.Exists())
                    TC.Get().Set(player);
        }

        internal bool EnsureAllPlayers()
        {
            var LogData = Players.Select(p => p.RaidOrgaID).Distinct().ToDictionary(e => e, e => Players.Where(p => p.RaidOrgaID == e).Count());
            var RopData = TC.Players.Select(p => p.ID).Distinct().ToDictionary(e => e, e => TC.Players.Where(p => p.ID == e).Count());
            if (!LogData.All(l => RopData[l.Key] == l.Value))
            {
                string err = "Players in TeamComp do not match up";
                Logger.Error(err);
                Logger.Debug("LogData");
                foreach (var p in LogData) Logger.Error($"{{{p.Key}; {p.Value}}}");
                Logger.Debug("TcData");
                foreach (var p in RopData) Logger.Error($"{{{p.Key}; {p.Value}}}");

                return false;
            }
            return true;
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
    }
}
