﻿using System;
using System.Collections.Generic;
using LogUploader.Data.RaidOrgaPlus;

namespace LogUploader.Data.RaidOrgaPlus
{
    /// <summary>
    /// Represents a Player form RaidOrga
    /// </summary>
    public class RoPlusPlayer : IEquatable<RoPlusPlayer>
    {
        public string AccountName { get; set; }
        public string RaidOrgaName { get; set; }
        public long RaidOrgaID { get; set; }
        public bool IsLFG { get => RaidOrgaID == 1; }
        public ISet<Role> Roles { get; } = new SortedSet<Role>(new DefaultRoleOrdering());
        public int PDPS { get; set; }
        public int CDPS { get; set; }
        public int DPS { get; set; }
        public int PDPS_Target { get; set; }
        public int CDPS_Target { get; set; }
        public int DPS_Target { get; set; }
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
            PDPS_Target = player.DpsTargetsPower;
            CDPS_Target = player.DpsTargetsCondi;
            DPS_Target = player.DpsTargets;
        }

        public void SetAccount(Account account) => SetAccount(account, Type);
        public void SetAccount(Account account, PlayerType type)
        {
            AccountName = account.AccountName;
            RaidOrgaName = account.Name;
            RaidOrgaID = account.ID;
            Type = type;
        }

        public void SetLFG()
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
                   AccountName.ToLowerInvariant() == other.AccountName.ToLowerInvariant();
        }

        public override int GetHashCode()
        {
            return -220601745 + EqualityComparer<string>.Default.GetHashCode(AccountName.ToLowerInvariant());
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
}

internal class DefaultRoleOrdering : IComparer<Role>
{
    public int Compare(Role x, Role y)
    {
        return GetWeight(x) - GetWeight(y);
    }

    protected virtual int GetWeight(Role r)
    {
        switch (r)
        {
            case Role.Power:
                return 11;
            case Role.Condi:
                return 12;
            case Role.Heal:
                return 13;
            case Role.Quickness:
                return 21;
            case Role.Alacrity:
                return 22;
            case Role.Utility:
                return 23;
            case Role.Tank:
                return 31;
            case Role.Special:
                return 32;
            case Role.Kiter:
                return 33;
            case Role.Banner:
            case Role.Empty:
                return 99;
            default:
                return 40;
        }
    }
}
