﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogUploader.Data;
using Extensiones.Linq;
using LogUploader.Tools.RaidOrgaPlus.GUI;
using LogUploader.Tools.RaidOrgaPlus.Data;

namespace LogUploader.Tools.RaidOrgaPlus
{
    public static partial class RaidOrgaPlusDataWorker
    {
        public const int MIN_DATA_VERSION = 2;

        public static Data.Raid UpdateRaid(Data.Raid raid, IEnumerable<ICachedLog> logs, Action<Delegate> invoker, IProgress<ProgressMessage> progress = null)
        {
            progress?.Report(new ProgressMessage(0.01, "Remove outdated logs"));
            logs = logs.Where(l => l.DataVersion >= MIN_DATA_VERSION)
                .Where(l => Boss.GetByID(l.BossID).RaidOrgaPlusID >= 1);
            progress?.Report(new ProgressMessage(0.02, "Remove duplicated bosses"));
            logs = OnlyGetOnePerBoss(logs);
            CondenseStatues(logs);

            MatchCMandNormal(raid, logs);
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

            progress?.Report(new ProgressMessage(0.11, "Assign players"));
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

        private static void ApplyCache(List<CheckPlayer> players, RaidOrgaPlusCache playerCache, Data.Raid r)
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

        private static void RemoveUnused(Data.Raid raid, IEnumerable<ICachedLog> logs)
        {
            raid.Bosses = raid.Bosses.Where(boss =>
            logs.Any(log => StaticData.Bosses.Get(log.BossID).RaidOrgaPlusID == boss.BossID && log.IsCM == boss.IsCM)
            ).ToList();

        }

        private static void MatchCMandNormal(Data.Raid raid, IEnumerable<ICachedLog> logs)
        {
            foreach (var boss in raid.Bosses.GroupBy(b => b.BossID))
            {
                var count = boss.Count();
                var anyNormal = boss.Any(b => !b.IsCM);
                var anyCM = boss.Any(b => b.IsCM);

                var raidBoss = logs.Where(b => StaticData.Bosses.Get(b.BossID).RaidOrgaPlusID == boss.Key);
                var raidAnyNormal = raidBoss.Any(b => !b.IsCM);
                var raidAnyCM = raidBoss.Any(b => b.IsCM);

                var succLogs = raidBoss.Where(b => b.Succsess);
                if (count == 1 && ((anyCM && !anyNormal) || (!anyCM && anyNormal)))
                {
                    //Only one CM or one normal in RO+
                    if (succLogs.Count() == 1)
                        boss.First().IsCM = succLogs.First().IsCM;
                    else if (!raidAnyNormal)
                        boss.First().IsCM = true;
                    else if (!raidAnyCM)
                        boss.First().IsCM = false;
                }
            }
        }

        private static IEnumerable<ICachedLog> OnlyGetOnePerBoss(IEnumerable<ICachedLog> logs)
        {
            return logs.Where(log => log.DataCorrected)
                .GroupBy(log => StaticData.Bosses.Get(log.BossID).RaidOrgaPlusID).SelectMany(group =>
            {
                if (group.All(e => !e.IsCM) || group.All(e => e.IsCM))
                {
                    if (group.Any(e => e.Succsess))
                        return new List<ICachedLog>() { group.Last(e => e.Succsess) };
                    return new List<ICachedLog>() { group.Last() };
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

        private static IEnumerable<ICachedLog> CondenseStatues(IEnumerable<ICachedLog> logs)
        {
            if (logs.Any(log => StaticData.Bosses.Get(log.BossID).RaidOrgaPlusID == 18))
            {
                ICachedLog keep;
                if (logs.Any(log => StaticData.Bosses.Get(log.BossID).RaidOrgaPlusID == 18 && log.Succsess))
                    keep = logs.Last(log => StaticData.Bosses.Get(log.BossID).RaidOrgaPlusID == 18 && log.Succsess);
                else
                    keep = logs.Last(log => StaticData.Bosses.Get(log.BossID).RaidOrgaPlusID == 18);

                return logs.Where(log => StaticData.Bosses.Get(log.BossID).RaidOrgaPlusID != 18 || log == keep);
            }
            return logs;
        }

        private static void ShowCorrectPlayerUI(List<CheckPlayer> players, Data.Raid raid, Action<Delegate> invoker)
        {
            Action a = () => {
                var ui = new CorrectPlayerUI(raid, players);
                ui.ShowDialog();
            };
            if (invoker == null)
                a();
            else
            invoker(a);
        }

        private static void UpdateRaidOrgaPlusData(Data.Raid raid, List<Encounter> encounters, IProgress<ProgressMessage> progress = null)
        {
            var count = encounters.Count();
            foreach ((int index, var encounter) in encounters.Enumerate())
            {
                progress?.Report(new ProgressMessage((double)(index) / (double)count, $"{(int)(count + 1)} of {count}"));
                encounter.FillTeamComp();
                encounter.GuessRoles();
                encounter.RefineRoles();
                encounter.RemoveNotAttededPlayers();
                encounter.RemoveDuplicates();
                encounter.UpdateNamedPlayers();
                encounter.UpdateUnnamedPlayers();
                for (int i = 0; i < 10 && !encounter.EnsureAllPlayers(); i++)
                    encounter.InsertDuplicatesPlayers();
                
                encounter.SortIfNew();
            }
        }

        private static void UpdatePlayersToInvite(Data.Raid raid, List<Encounter> encounters)
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

        private static List<CheckPlayer> GetAllPlayers(Data.Raid raid, List<Encounter> encounters)
        {
           return encounters.SelectMany(e => e.Players).Distinct().Where(p => p.AccountName.Contains(".")).Select(p => {
                if (p.Type != PlayerType.LFG)
                    return new CheckPlayer(p, raid.GetAccount(p.AccountName, p.Type));
                return new CheckPlayer(p, null);
                }
            ).ToList();
        }

        private static List<Encounter> GetEncounters(Data.Raid raid, IEnumerable<ICachedLog> logs)
        {
            var encounters = new List<Encounter>();
            foreach (var log in logs)
            {
                try
                {
                    var tc = raid.GetTeamComp(StaticData.Bosses.Get(log.BossID), log.IsCM);
                    encounters.Add(new Encounter(tc, log, raid));
                }
                catch (InvalidOperationException e)
                {
                    //Ignore, something went worng, can't do anything about it. Just log it with debug info.

                    Logger.Error($"Failed to get team comp for log with boss id {log.BossID} (\"{Boss.GetByID(log.BossID)}\") and isCM:{log.IsCM}");
                    Logger.Error($"Availabe teamcomps: {string.Join(", ", raid.Bosses.Select(b => $"{b.Encounter.NameEN} isCM:{b.IsCM}"))}");
                    Logger.Error($"Exception ");
                    Logger.LogException(e);
                }
                
            }
            return encounters;
        }

        private static void InsertLogs(Data.Raid raid, IEnumerable<ICachedLog> logs)
        {
            foreach (var log in logs)
            {
                if (StaticData.Bosses.Get(log.BossID).RaidOrgaPlusID > 0)
                    InsertLog(raid, log);
            }
            //TODO Duplicated bosses, 2 ore more times the same boss
        }

        private static void InsertLog(Data.Raid raid, ICachedLog log)
        {
            var boss = StaticData.Bosses.Get(log.BossID);
            if (boss.RaidOrgaPlusID == -1 || raid.ExistsBoss(boss, log.IsCM))
                return;
            AddBoss(raid, boss, log);
        }

        private static void AddBoss(Data.Raid raid, Boss boss, ICachedLog log)
        {
            var players = new List<Position>();
            var tc = new TeamComp(-1, boss, log.IsCM, players, log.Succsess);
            raid.Bosses.Add(tc);
        }
    }
}
