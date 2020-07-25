using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogUploader.Data.RaidOrgaPlus;
using LogUploader.Data;

namespace LogUploader.Helper.RaidOrgaPlus
{
    public static class RaidOrgaPlusDataWorker
    {
        public static Raid UpdateRaid(Raid raid, IEnumerable<CachedLog> logs)
        {
            InsertLogs(raid, logs);
            var encounters = GetEncounters(raid, logs);
            var players = GetAllPlayers(raid, encounters);
            ShowCorrectPlayerUI(players, raid);
            CorrectPlayers(players);
            UpdateRaidOrgaPlusData(raid, encounters);

            return raid;
        }

        private static void ShowCorrectPlayerUI(List<CheckPlayer> players, Raid raid)
        {
            var ui = new GUIs.CorrectPlayer.CorrectPlayerUI(raid, players);
            ui.ShowDialog();
        }

        private static void UpdateRaidOrgaPlusData(Raid raid, List<Encounter> encounters)
        {
            foreach (var encounter in encounters)
            {
                encounter.GuessRoles();
                encounter.RemoveNotAttededPlayers();
                encounter.RemoveDuplicates();
                encounter.UpdateNamedPlayers();
                encounter.UpdateUnnamedPlayers();
                encounter.EnsureAllPlayers();
            }
            UpdatePlayersToInvite(raid, encounters);
        }

        private static void UpdatePlayersToInvite(Raid raid, List<Encounter> encounters)
        {
            raid.ToInvite = encounters
                .SelectMany(e => e.Players)
                .Distinct()
                .Where(p => p.Type == PlayerType.INVITEABLE)
                .Select(p => raid.GetInviteable(p.AccountName))
                .ToList();
        }

        private static void CorrectPlayers(List<CheckPlayer> players)
        {
            players.ForEach(p => p.Correct());
        }

        private static List<CheckPlayer> GetAllPlayers(Raid raid, List<Encounter> encounters)
        {
           return encounters.SelectMany(e => e.Players).Distinct().Select(p => {
                if (p.Type != PlayerType.LFG)
                    return new CheckPlayer(p, raid.GetAccount(p.AccountName, p.Type));
                return new CheckPlayer(p, null);
                }
            ).ToList();
        }

        private static List<Encounter> GetEncounters(Raid raid, IEnumerable<CachedLog> logs)
        {
            var encounters = new List<Encounter>();
            foreach (var log in logs)
            {
                encounters.Add(new Encounter(raid.GetTeamComp(Boss.getByID(log.BossID), log.IsCM), log, raid));
            }
            return encounters;
        }

        private static void InsertLogs(Raid raid, IEnumerable<CachedLog> logs)
        {
            foreach (var log in logs)
                InsertLog(raid, log);
            //TODO Duplicated bosses
        }

        private static void InsertLog(Raid raid, CachedLog log)
        {
            var boss = Boss.getByID(log.BossID);
            if (boss.RaidOrgaPlusID == -1 || raid.ExistsBoss(boss, log.IsCM))
                return;
            AddBoss(raid, boss, log);
        }

        private static void AddBoss(Raid raid, Boss boss, CachedLog log)
        {
            var players = new List<Position>();
            var tc = new TeamComp(-1, boss, log.IsCM, players);
            raid.Bosses.Add(tc);
        }

        public class CheckPlayer
        {
            public CheckedPlayer Player { get; set; }
            public Account BecomesAccount { get; set; }
            public PlayerType BecomesType { get; set; }

            public CheckPlayer(CheckedPlayer p, Account a)
            {
                Player = p;
                BecomesAccount = a;
                BecomesType = p.Type;
                if (a == null)
                    p.Type = PlayerType.LFG;

            }

            public void Correct()
            {
                if (BecomesType == PlayerType.LFG)
                    Player.setLFG();
                else
                    Player.SetAccount(BecomesAccount, BecomesType);
            }
        }

        private class Encounter
        {
            public List<CheckedPlayer> Players { get; set; } = new List<CheckedPlayer>();
            public Boss Boss { get; set; }
            public TeamComp TC { get; set; }
            public int CDPS { get; private set; }

            public Encounter(TeamComp tc, CachedLog log, Raid r)
            {
                TC = tc;
                Boss = tc.Encounter;
                Players.AddRange(log.Players.Select(p => new CheckedPlayer(p, r)));
            }

            internal void GuessRoles()
            {
                switch (Boss.ID)
                {
                    //Special Bosses
                    case 19651: //Eyes
                    case 19844: //Eyes
                        goto default;
                    case 19828: //River
                        GuessRiver();
                        break;
                    case 20934: //Qadim1
                        //TODO
                        goto default;
                    case 22000: //Qadim2
                        if (GuessDoubleChrono()) ;
                        else if (GuessFireBrigadeChrono()) ;
                        else if (GuessFireBrigade()) ;
                        else GenericGuesses();
                        SetQadim2Pylons();
                        break;

                    //Boonthive bosses
                    case 16115: //Mathias
                        if (GuessBoonThief(false))
                            return;
                        goto default;
                    case 17172: //Mo
                    case 19691: //Broken king
                    case 19536: //Soul Eater
                    case 22006: //Adina
                        if (GuessBoonThief())
                            return;
                        goto default;

                    //No Tank
                    case 15375: //Sabeta
                    case 16123: //Sloth
                    case 16088: //Trio
                    case 16137: //Trio
                    case 16125: //Trio
                    case 16247: //TC
                    case 17194: //Carin
                    case 17188: //Samarog
                    case 43974: //CA
                        if (GuessDoubleChrono(false))
                            return;
                        else if (GuessFireBrigadeChrono(false))
                            return;
                        else if (GuessFireBrigade(false))
                            return;
                        GenericGuesses(false);
                        return;

                    default:
                        if (GuessDoubleChrono())
                            return;
                        else if (GuessFireBrigadeChrono())
                            return;
                        else if (GuessFireBrigade())
                            return;
                        GenericGuesses();
                        return;
                }

            }

            private void GuessRiver()
            {
                var ThreashholdDMG = Players.Max(p => p.DPS)/2;
                foreach (var player in Players)
                {
                    if (player.DPS <= ThreashholdDMG)
                        player.Role = Role.Heal;
                    if (player.Class.RaidOrgaPlusID == 11)
                        player.Role = Role.Utility;
                }
                SetBS();
                FillUpDps();
            }

            private void SetQadim2Pylons()
            {
                var kiters = Players.OrderBy(p => p.DPS).Where(p => p.Class.RaidOrgaPlusID == 21 || p.Class.RaidOrgaPlusID == 24);
                kiters = kiters.Take(Math.Min(3, kiters.Count()));
                foreach (var kiter in kiters)
                    kiter.Role = Role.Kiter;
            }

            private void GenericGuesses(bool hasTank = true)
            {
                var orderdPlayers = Players.OrderBy(p => p.DPS).Take(5);
                if (hasTank && orderdPlayers.Take(3).Any(p => p.Class.RaidOrgaPlusID == 11))
                    orderdPlayers.First(p => p.Class.RaidOrgaPlusID == 11).Role = Role.Tank;
                
                foreach (var orderdPlayer in orderdPlayers)
                {
                    switch (orderdPlayer.Class.RaidOrgaPlusID)
                    {
                        //Chrono
                        case 11:
                        //Firebrand
                        case 26:
                        //Renegade
                        case 27:
                            orderdPlayer.Role = Role.Utility;
                            break;
                        //Durid
                        case 13:
                        //Tempest
                        case 10:
                        //Scourge
                        case 21:
                        default:
                            orderdPlayer.Role = Role.Heal;
                            break;
                    }
                }
                SetBS();
                FillUpDps();
            }

            private bool GuessFireBrigadeChrono(bool hasTank = true)
            {
                if (!FireBrigadeChronoRequMet())
                    return false;
                var orderdPlayers = Players.OrderBy(p => p.DPS);
                if (hasTank)
                    orderdPlayers.First(p => p.Class.RaidOrgaPlusID == 11).Role = Role.Tank;
                else
                    orderdPlayers.First(p => p.Class.RaidOrgaPlusID == 11).Role = Role.Utility;
                orderdPlayers.First(p => p.Class.RaidOrgaPlusID == 26 && p.Role == Role.Empty).Role = Role.Utility;
                orderdPlayers.First(p => p.Class.RaidOrgaPlusID == 27 && p.Role == Role.Empty).Role = Role.Heal;
                orderdPlayers.First(p => p.Role == Role.Empty).Role = Role.Heal;
                SetBS();
                FillUpDps();
                return true;
            }

            private bool GuessFireBrigade(bool hasTank = true)
            {
                if (!FireBrigadeRequMet())
                    return false;
                var orderdPlayers = Players.OrderBy(p => p.DPS);
                if (hasTank)
                    orderdPlayers.First(p => p.Class.RaidOrgaPlusID == 26).Role = Role.Tank;
                else
                    orderdPlayers.First(p => p.Class.RaidOrgaPlusID == 26).Role = Role.Utility;
                orderdPlayers.First(p => p.Class.RaidOrgaPlusID == 26 && p.Role == Role.Empty).Role = Role.Utility;
                orderdPlayers.First(p => p.Class.RaidOrgaPlusID == 27 && p.Role == Role.Empty).Role = Role.Heal;
                // TODO Better second heal detection if dps woud support it or not
                orderdPlayers.First(p => p.Role == Role.Empty).Role = Role.Heal;
                SetBS();
                FillUpDps();
                return true;
            }

            private bool GuessDoubleChrono(bool hasTank = true)
            {
                if (!DoubleChronoRequMet())
                    return false;
                var orderdPlayers = Players.OrderBy(p => p.DPS);
                if (hasTank)
                    orderdPlayers.First(p => p.Class.RaidOrgaPlusID == 11).Role = Role.Tank;
                else
                    orderdPlayers.First(p => p.Class.RaidOrgaPlusID == 11).Role = Role.Utility;
                orderdPlayers.First(p => p.Class.RaidOrgaPlusID == 11 && p.Role == Role.Empty).Role = Role.Utility;
                orderdPlayers.First(p => p.Role == Role.Empty).Role = Role.Heal;
                orderdPlayers.First(p => p.Role == Role.Empty).Role = Role.Heal;
                SetBS();
                FillUpDps();
                return true;
            }

            private bool GuessBoonThief(bool hasTank = true)
            {
                if (!(BoonThiveRequMet() && !(DoubleChronoRequMet() || FireBrigadeChronoRequMet())))
                    return false;
                var orderdPlayers = Players.OrderBy(p => p.DPS);
                orderdPlayers.First(p => p.Class.RaidOrgaPlusID == 6 || p.Class.RaidOrgaPlusID == 15).Role = Role.Utility;
                if (orderdPlayers.Take(3).Any(p => p.Class.RaidOrgaPlusID == 27))
                    orderdPlayers.First(p => p.Class.RaidOrgaPlusID == 27).Role = Role.Heal;
                else
                {
                    orderdPlayers.First(p => p.Class.RaidOrgaPlusID == 27).Role = Role.Utility;
                    orderdPlayers.First(p => p.Role == Role.Empty).Role = Role.Heal;
                }
                orderdPlayers.First(p => p.Role == Role.Empty).Role = Role.Heal;
                SetBS();
                FillUpDps();
                return true;
            }

            private void SetBS()
            {
                if (Players.Any(p => (p.Class.RaidOrgaPlusID == 7 || p.Class.RaidOrgaPlusID == 16) && p.Role == Role.Empty))
                    Players.OrderBy(p => p.DPS).First(p => (p.Class.RaidOrgaPlusID == 7 || p.Class.RaidOrgaPlusID == 16) && p.Role == Role.Empty).Role = Role.Banner;
            }

            private void FillUpDps()
            {
                foreach (var player in Players)
                {
                    if (player.Role == Role.Empty)
                    {
                        if (player.PDPS > CDPS)
                            player.Role = Role.Power;
                        player.Role = Role.Condi;
                    }
                }
            }

            private bool BoonThiveRequMet()
            {
                return Players.Any(p => p.Class.RaidOrgaPlusID == 6 || p.Class.RaidOrgaPlusID == 15) && Players.Any(p => p.Class.RaidOrgaPlusID == 27);
            }
            private bool DoubleChronoRequMet()
            {
                return Players.OrderBy(p => p.DPS).Take(Math.Min(6, Players.Count)).Count(p => p.Class.RaidOrgaPlusID == 11) >= 2;
            }
            private bool FireBrigadeChronoRequMet()
            {
                return Players.Any(p => p.Class.RaidOrgaPlusID == 11) && Players.Any(p => p.Class.RaidOrgaPlusID == 27) && Players.Any(p => p.Class.RaidOrgaPlusID == 26) && Players.OrderBy(p => p.DPS).Take(Math.Min(4, Players.Count)).Any(p => p.Class.RaidOrgaPlusID == 11);
            }
            private bool FireBrigadeRequMet()
            {
                return Players.Any(p => p.Class.RaidOrgaPlusID == 27) && Players.Count(p => p.Class.RaidOrgaPlusID == 26) >= 2;
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
                    TC.Players.Where(p => p.IsLFG()).Take(ROplusLFGCount - TargetLFGCount).Select(p => p.RemoveName());
            }

            internal void RemoveDuplicates()
            {
                var dupes = TC.Players.GroupBy(p => p.AccName).Where(g => g.Count() > 1);
                foreach (var dupe in dupes)
                {
                    var player = Players.Find(p => p.AccountName == dupe.Key);

                    if (dupe.Any(pos => pos.Profession == player.Class && pos.Role == player.Role))
                    {
                        var hit = dupe.First(pos => pos.Profession == player.Class && pos.Role == player.Role);
                        dupe.Where(p => p != hit).Select(pos => pos.RemoveName());

                    }
                    else if (dupe.Any(pos => pos.Role == player.Role))
                    {
                        var hit = dupe.First(pos => pos.Role == player.Role);
                        dupe.Where(p => p != hit).Select(pos => pos.RemoveName());
                        hit.Set(player);
                    }
                    else if (dupe.Any(pos => pos.Profession == player.Class))
                    {
                        var hit = dupe.First(pos => pos.Profession == player.Class);
                        dupe.Where(p => p != hit).Select(pos => pos.RemoveName());
                        hit.Set(player);
                    }
                    else
                    {
                        var hit = dupe.First();
                        dupe.Where(p => p != hit).Select(pos => pos.RemoveName());
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
                        pos.UpdateProffessionRole(player.Class, player.Role);
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
                    if (!TC.Exists(player.AccountName) && TC.Exists(Profession.Unknown))
                        TC.Get(Profession.Unknown).Set(player);
                foreach (var player in relevantPlayers)
                    if (!TC.Exists(player.AccountName) && TC.Exists(Role.Empty))
                        TC.Get(Role.Empty).Set(player);
                foreach (var player in relevantPlayers)
                    if (!TC.Exists(player.AccountName) && TC.Exists())
                        TC.Get().Set(player);
            }

            internal void EnsureAllPlayers()
            {
                var errorPlayers = Players.Where(player => TC.Players.Where(p => p.AccName == player.AccountName).Count() != Players.Where(p => p.AccountName == player.AccountName).Count());
                if (errorPlayers.Count() == 0)
                    return;

                //TODO Proper logging
                System.Windows.Forms.MessageBox.Show($"Not all players present ({errorPlayers.Count()} errors)");

                //TODO implement proper handling
                //var errors = errorPlayers.Select(error => new { player = error, actual = TC.Players.Where(p => p.AccName == error.AccountName).Count(), expected = Players.Where(p => p.AccountName == error.AccountName).Count() }); 
                //foreach (var error in errors)
                //{
                //    if (error.actual > error.expected)
                //        continue;
                //}
            }
        }

        public class CheckedPlayer : IEquatable<CheckedPlayer>
        {
            public string AccountName { get; set; }
            public string RaidOrgaName { get; set; }
            public long RaidOrgaID { get; set; }
            public bool IsLFG { get => RaidOrgaID == 1; }
            public Role Role { get; set; } = Role.Empty;
            public int PDPS { get; set; }
            public int CDPS { get; set; }
            public int DPS { get; set; }
            public Profession Class { get; set; }
            public PlayerType Type { get; set; } = PlayerType.LFG;

            public CheckedPlayer(CachedPlayer player, Raid r)
            {
                AccountName = player.AccountName;
                if (r.IsMember(AccountName))
                {
                    Type = PlayerType.MEMBER;
                    SetAccount(r.GetMember(AccountName));
                }
                else if (r.IsHelper(AccountName))
                {
                    Type = PlayerType.HELPER;
                    SetAccount(r.GetHelper(AccountName));
                }
                else if (r.IsInviteable(AccountName))
                {
                    Type = PlayerType.INVITEABLE;
                    SetAccount(r.GetInviteable(AccountName));
                }
                else
                {
                    RaidOrgaName = "";
                    RaidOrgaID = 1;
                }
                PDPS = player.PDPS;
                CDPS = player.CDPS;
                DPS = player.DPS;
                Class = player.Class;
            }

            public void SetAccount(Account account) => SetAccount(account, Type);
            public void SetAccount(Account account, PlayerType type)
            {
                AccountName = account.AccountName;
                RaidOrgaName = account.Name;
                RaidOrgaID = account.ID;
                Type = type;
            }

            public void setLFG()
            {
                RaidOrgaName = "";
                RaidOrgaID = 1;
                Type = PlayerType.LFG;
            }

            public override bool Equals(object obj)
            {
                return Equals(obj as CheckedPlayer);
            }

            public bool Equals(CheckedPlayer other)
            {
                return other != null &&
                       AccountName == other.AccountName;
            }

            public override int GetHashCode()
            {
                return -220601745 + EqualityComparer<string>.Default.GetHashCode(AccountName);
            }

            public string DisplayName { get
                {
                    if (IsLFG)
                        return $"LFG {AccountName}";
                    else
                        return $"{RaidOrgaName} ({AccountName})";

                } }

            public static bool operator ==(CheckedPlayer left, CheckedPlayer right)
            {
                return EqualityComparer<CheckedPlayer>.Default.Equals(left, right);
            }

            public static bool operator !=(CheckedPlayer left, CheckedPlayer right)
            {
                return !(left == right);
            }
        }


        public enum PlayerType
        {
            MEMBER,
            HELPER,
            INVITEABLE,
            LFG
        }
    }
}
