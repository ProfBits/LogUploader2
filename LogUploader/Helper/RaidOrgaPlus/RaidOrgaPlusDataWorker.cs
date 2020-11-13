using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogUploader.Data.RaidOrgaPlus;
using LogUploader.Data;
using Extensiones.Linq;

namespace LogUploader.Helper.RaidOrgaPlus
{
    public static class RaidOrgaPlusDataWorker
    {
        public const int MIN_DATA_VERSION = 2;

        public static Raid UpdateRaid(Raid raid, IEnumerable<CachedLog> logs, Action<Delegate> invoker, IProgress<ProgressMessage> progress = null)
        {
            progress?.Report(new ProgressMessage(0.01, "Remove outdated logs"));
            logs = logs.Where(l => l.DataVersion >= MIN_DATA_VERSION);
            progress?.Report(new ProgressMessage(0.02, "Remove duplicated bosses"));
            logs = OnlyGetOnePerBoss(logs);
            CondenseStatues(logs);

            progress?.Report(new ProgressMessage(0.03, "Remove unused bosses from RO+ data"));
            RemoveUnused(raid, logs);

            progress?.Report(new ProgressMessage(0.04, "Create missing bosses"));
            InsertLogs(raid, logs);

            progress?.Report(new ProgressMessage(0.05, "Build encounters"));
            var encounters = GetEncounters(raid, logs);

            progress?.Report(new ProgressMessage(0.08, "Gather players"));
            var players = GetAllPlayers(raid, encounters);

            progress?.Report(new ProgressMessage(0.09, "Loading cached players"));
            var playerCache = new RaidOrgaPlusCache();

            progress?.Report(new ProgressMessage(0.10, "Applying cached players"));
            ApplyCache(players, playerCache, raid);

            progress?.Report(new ProgressMessage(0.11, "Assigen players"));
            ShowCorrectPlayerUI(players, raid, invoker);

            progress?.Report(new ProgressMessage(0.15, "Update players and cache"));
            UpdateCache(players, playerCache, raid.RaidID);

            progress?.Report(new ProgressMessage(0.16, "Update players and cache"));
            CorrectPlayers(players, encounters);

            UpdateRaidOrgaPlusData(raid, encounters, new Progress<ProgressMessage>((p) => progress?.Report(new ProgressMessage((p.Percent * 0.81) + 0.17, "Update boss " + p.Message))));

            progress?.Report(new ProgressMessage(0.98, "Update players to invite"));
            UpdatePlayersToInvite(raid, encounters);
            progress?.Report(new ProgressMessage(1, "Done"));
            return raid;
        }

        private static void UpdateCache(List<CheckPlayer> players, RaidOrgaPlusCache playerCache, long raidID)
        {
            foreach (var p in players)
            {
                if (p.Player.Type != p.BecomesType)
                {
                    playerCache.Set(raidID, p.Player.AccountName, p.BecomesAccount?.ID ?? 1);
                }
            }
            playerCache.Save();
        }

        private static void ApplyCache(List<CheckPlayer> players, RaidOrgaPlusCache playerCache, Raid r)
        {
            foreach (var p in players)
            {
                if (p.Player.Type != PlayerType.MEMBER)
                {
                    var becomesID = playerCache.Get(r.RaidID, p.Player.AccountName);
                    switch (becomesID)
                    {
                        case -1: //Not in cache
                            break;
                        case 1: //LFG
                            p.BecomesAccount = null;
                            p.BecomesType = PlayerType.LFG;
                            break;
                        default: //Knowen Account
                            if (r.IsMember(becomesID))
                            {
                                p.BecomesAccount = r.GetMember(becomesID);
                                p.BecomesType = PlayerType.MEMBER;
                            }
                            else if (r.IsInviteable(becomesID))
                            {
                                p.BecomesAccount = r.GetInviteable(becomesID);
                                p.BecomesType = PlayerType.INVITEABLE;
                            }
                            else if (r.IsHelper(becomesID))
                            {
                                p.BecomesAccount = r.GetHelper(becomesID);
                                p.BecomesType = PlayerType.HELPER;
                            }
                            break;
                    }
                }
            }
        }

        private static void RemoveUnused(Raid raid, IEnumerable<CachedLog> logs)
        {
            raid.Bosses = raid.Bosses.Where(boss => 
            logs.Any(log => Boss.getByID(log.BossID).RaidOrgaPlusID == boss.BossID && log.IsCM == boss.IsCM)
            ).ToList();
        }

        private static IEnumerable<CachedLog> OnlyGetOnePerBoss(IEnumerable<CachedLog> logs)
        {
            return logs.Where(log => log.DataCorrected)
                .GroupBy(log => Boss.getByID(log.BossID).RaidOrgaPlusID).SelectMany(group =>
            {
                if (group.All(e => !e.IsCM) || group.All(e => e.IsCM))
                {
                    if (group.Any(e => e.Succsess))
                        return new List<CachedLog>() { group.Last(e => e.Succsess) };
                    return new List<CachedLog>() { group.Last() };
                }
                var subGroup = group.GroupBy(e => e.IsCM);
                return subGroup.Select(grp =>
                {
                    if (group.Any(e => e.Succsess))
                        return group.Last(e => e.Succsess);
                    return group.Last();
                });
            }).OrderBy(log => log.Date);
        }

        private static IEnumerable<CachedLog> CondenseStatues(IEnumerable<CachedLog> logs)
        {
            if (logs.Any(log => Boss.getByID(log.BossID).RaidOrgaPlusID == 18))
            {
                CachedLog keep;
                if (logs.Any(log => Boss.getByID(log.BossID).RaidOrgaPlusID == 18 && log.Succsess))
                    keep = logs.Last(log => Boss.getByID(log.BossID).RaidOrgaPlusID == 18 && log.Succsess);
                else
                    keep = logs.Last(log => Boss.getByID(log.BossID).RaidOrgaPlusID == 18);

                return logs.Where(log => Boss.getByID(log.BossID).RaidOrgaPlusID != 18 || log == keep);
            }
            return logs;
        }

        private static void ShowCorrectPlayerUI(List<CheckPlayer> players, Raid raid, Action<Delegate> invoker)
        {
            Action a = () => {
                var ui = new GUIs.CorrectPlayer.CorrectPlayerUI(raid, players);
                ui.ShowDialog();
            };
            if (invoker == null)
                a();
            else
            invoker(a);
        }

        private static void UpdateRaidOrgaPlusData(Raid raid, List<Encounter> encounters, IProgress<ProgressMessage> progress = null)
        {
            var count = encounters.Count();
            foreach ((int index, var encounter) in encounters.Enumerate())
            {
                progress?.Report(new ProgressMessage((double)(index) / (double)count, $"{(int)(count + 1)} of {count}"));
                encounter.FillTeamComp();
                encounter.GuessRoles();
                encounter.RemoveNotAttededPlayers();
                encounter.RemoveDuplicates();
                encounter.UpdateNamedPlayers();
                encounter.UpdateUnnamedPlayers();
                encounter.EnsureAllPlayers();
                encounter.SortIfNew();
            }
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

        private static void CorrectPlayers(List<CheckPlayer> players, List<Encounter> encounters)
        {
            var lookup = players.ToDictionary(p => p.Player.AccountName, p => p);
            foreach (var player in encounters.SelectMany(e => e.Players))
                lookup[player.AccountName].Correct(player);
        }

        private static List<CheckPlayer> GetAllPlayers(Raid raid, List<Encounter> encounters)
        {
           return encounters.SelectMany(e => e.Players).Distinct().Where(p => p.AccountName.Contains(".")).Select(p => {
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
            {
                if (Boss.getByID(log.BossID).RaidOrgaPlusID > 0)
                    InsertLog(raid, log);
            }
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
            var tc = new TeamComp(-1, boss, log.IsCM, players, log.Succsess);
            raid.Bosses.Add(tc);
        }

        public class CheckPlayer
        {
            public RoPlusPlayer Player { get; set; }
            public Account BecomesAccount { get; set; }
            public PlayerType BecomesType { get; set; }

            public CheckPlayer(RoPlusPlayer p, Account a)
            {
                Player = p;
                BecomesAccount = a;
                BecomesType = p.Type;
                if (a == null)
                    p.Type = PlayerType.LFG;
            }

            internal void Correct(RoPlusPlayer player)
            {
                if (BecomesType == PlayerType.LFG)
                    player.setLFG();
                else
                    player.SetAccount(BecomesAccount, BecomesType);
            }
        }

        private class Encounter
        {
            public List<RoPlusPlayer> Players { get; set; } = new List<RoPlusPlayer>();
            public Boss Boss { get; set; }
            public TeamComp TC { get; set; }

            public Encounter(TeamComp tc, CachedLog log, Raid r)
            {
                TC = tc;
                Boss = tc.Encounter;
                Players.AddRange(log.PlayersNew.Select(p => new RoPlusPlayer(p, r)));
                if (Boss.RaidOrgaPlusID == 20)
                {
                    var sword = Players.Where(p => !p.AccountName.Contains('.')).FirstOrDefault();
                    if (sword != null)
                        Players.Remove(sword);
                }
                tc.Success = log.Succsess;
            }

            internal void GuessRoles()
            {
                switch (Boss.ID)
                {
                    //Special Bosses
                    case 17154:
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
                        //TODO Qadim1
                        goto case 43974;
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
                var ThreashholdDMG = Players.Max(p => p.DPS)/2;
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
                            if (orderdPlayer.GroupQuickness >= 25 && orderdPlayer.Concentration > 0)
                                orderdPlayer.Role = Role.Utility;
                            break;
                        case eProfession.Renegade:
                            if (orderdPlayer.GroupAlacrity >= 5)
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
                    if (player.PDPS < player.CDPS)
                        player.Role = Role.Condi;
                    else
                        player.Role = Role.Power;
                }
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
                    if (!TC.Exists(player.AccountName) && TC.Exists(Role.Empty))
                        TC.Get(Role.Empty).Set(player);
                foreach (var player in relevantPlayers)
                    if (!TC.Exists(player.AccountName) && TC.Exists(Profession.Unknown))
                        TC.Get(Profession.Unknown).Set(player);
                foreach (var player in relevantPlayers)
                    if (!TC.Exists(player.AccountName) && TC.Exists())
                        TC.Get().Set(player);
            }

            //TODO Implement EnsureAllPlayers properly
            internal void EnsureAllPlayers()
            {
                var tcData = Players.Select(p => p.RaidOrgaID).Distinct().ToDictionary(e => e, e => Players.Where(p => p.RaidOrgaID == e).Count());
                foreach (var p in TC.Players)
                {
                    if (!tcData.ContainsKey(p.ID))
                        //Error player missing in tc
                        throw new NotImplementedException();
                    if (tcData[p.ID] != TC.Players.Where(p2 => p.ID == p2.ID).Count())
                        //Error player to often or to few
                        throw new NotImplementedException();
                }
            }

            internal void FillTeamComp()
            {
                for (int i = 1; i <= 10; i++)
                    if (!TC.Players.Any(p => p.Pos == i))
                        TC.Players.Add(new Position(i, 0, "", Role.Empty, Profession.Unknown));
            }

            internal void SortIfNew()
            {
                if (!TC.aufstellungsID.HasValue)
                    TC.OrderPlayers();
            }
        }

        public class RoPlusPlayer : IEquatable<RoPlusPlayer>
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

            public int Condition { get; } = 0;
            public int Concentration { get; } = 0;
            public int Healing { get; } = 0;
            public int Toughness { get; } = 0;
            public float GroupQuickness { get; } = 0;
            public float GroupAlacrity { get; } = 0;

            public RoPlusPlayer(SimplePlayer player, Raid r)
            {
                AccountName = player.Account;
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
                PDPS = player.DpsAllPower;
                CDPS = player.DpsAllCondi;
                DPS = player.DpsAll;
                Class = player.Profession;
                Condition = player.Condition;
                Concentration = player.Concentration;
                Healing = player.Healing;
                Toughness = player.Toughness;
                GroupQuickness = player.GroupQuickness;
                GroupAlacrity = player.GroupAlacrity;
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
                return Equals(obj as RoPlusPlayer);
            }

            public bool Equals(RoPlusPlayer other)
            {
                return other != null &&
                       AccountName == other.AccountName;
            }

            public override int GetHashCode()
            {
                return -220601745 + EqualityComparer<string>.Default.GetHashCode(AccountName);
            }

            public static bool operator ==(RoPlusPlayer left, RoPlusPlayer right)
            {
                return EqualityComparer<RoPlusPlayer>.Default.Equals(left, right);
            }

            public static bool operator !=(RoPlusPlayer left, RoPlusPlayer right)
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
