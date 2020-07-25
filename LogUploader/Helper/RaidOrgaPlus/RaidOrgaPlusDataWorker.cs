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
            ShowCorrectPlayerUI(players, encounters);
            CorrectPlayers(players);
            UpdateRaidOrgaPlusData(raid, encounters);

            return raid;
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

        private class CheckPlayer
        {
            public Player Player { get; set; }
            public Account BecomesAccount { get; set; }
            public PlayerType BecomesType { get; set; }

            public CheckPlayer(Player p, Account a)
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
            public List<Player> Players { get; set; } = new List<Player>();
            public Boss Boss { get; set; }
            public TeamComp TC { get; set; }

            public Encounter(TeamComp tc, CachedLog log, Raid r)
            {
                TC = tc;
                Boss = tc.Encounter;
                Players.AddRange(log.Players.Select(p => new Player(p, r)));
            }

            internal void GuessRoles()
            {
                throw new NotImplementedException();
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
                throw new NotImplementedException();
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
                //TODO maybe remove lfg players form this

                foreach (var player in relevantPlayers)
                    if (!TC.Exists(player.AccountName) && TC.Exists(player.Class, player.Role))
                        TC.Get(player.Class, player.Role).Set(player);
                foreach (var player in relevantPlayers)
                    if (!TC.Exists(player.AccountName) && TC.Exists(Profession.Unkowen, player.Role))
                        TC.Get(Profession.Unkowen, player.Role).Set(player);
                foreach (var player in relevantPlayers)
                    if (!TC.Exists(player.AccountName) && TC.Exists(player.Class, Role.Empty))
                        TC.Get(player.Class, Role.Empty).Set(player);
                foreach (var player in relevantPlayers)
                    if (!TC.Exists(player.AccountName) && TC.Exists(Profession.Unkowen, Role.Empty))
                        TC.Get(Profession.Unkowen, Role.Empty).Set(player);

                relevantPlayers = Players.Where(p => !TC.Exists(p.AccountName));
                foreach (var player in relevantPlayers)
                    if (!TC.Exists(player.AccountName) && TC.Exists(player.Role))
                        TC.Get(player.Role).Set(player);
                foreach (var player in relevantPlayers)
                    if (!TC.Exists(player.AccountName) && TC.Exists(player.Class))
                        TC.Get(player.Class).Set(player);

                relevantPlayers = Players.Where(p => !TC.Exists(p.AccountName));
                foreach (var player in relevantPlayers)
                    if (!TC.Exists(player.AccountName) && TC.Exists(Profession.Unkowen))
                        TC.Get(Profession.Unkowen).Set(player);
                foreach (var player in relevantPlayers)
                    if (!TC.Exists(player.AccountName) && TC.Exists(Role.Empty))
                        TC.Get(Role.Empty).Set(player);
                foreach (var player in relevantPlayers)
                    if (!TC.Exists(player.AccountName) && TC.Exists())
                        TC.Get().Set(player);
            }

            internal void EnsureAllPlayers()
            {
                throw new NotImplementedException();
            }
        }

        public class Player : IEquatable<Player>
        {
            public string AccountName { get; set; }
            public string RaidOrgaName { get; set; }
            public long RaidOrgaID { get; set; }
            public bool IsLFG { get => RaidOrgaID < 0; }
            public Role Role { get; set; } = Role.Empty;
            public int PDPS { get; set; }
            public int CDPS { get; set; }
            public int DPS { get; set; }
            public Profession Class { get; set; }
            public PlayerType Type { get; set; } = PlayerType.LFG;

            public Player(CachedPlayer player, Raid r)
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
                    RaidOrgaID = -1;
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
                RaidOrgaID = -1;
                Type = PlayerType.LFG;
            }

            public override bool Equals(object obj)
            {
                return Equals(obj as Player);
            }

            public bool Equals(Player other)
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

            public static bool operator ==(Player left, Player right)
            {
                return EqualityComparer<Player>.Default.Equals(left, right);
            }

            public static bool operator !=(Player left, Player right)
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
