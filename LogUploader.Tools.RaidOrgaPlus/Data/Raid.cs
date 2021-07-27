using LogUploader.Data;

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogUploader.Tools.RaidOrgaPlus.Data
{
    /*
     *  {
     *      session: <gültige Session-ID aus Login mit Raidleiter-Rechten für entsprechenden Raid>
     *      auth: <gültige Session-ID aus Login mit Raidleiter-Rechten für entsprechenden Raid>
     *      body: {
     *          terminId: 123,
     *          aufstellungen: [
     *              {
     *                  aufstellungId?: 1234,
     *                  bossId?: 12,
     *                  isCM?: true / false,
     *                  success?: true / false,
     *                  positionen: [
     *                      {
     *                          position: 1,
     *                          spielerId: 12,
     *                          classId: 20,
     *                          roleId: 5
     *                      }
     *                  ]
     *              }
     *          ]}
     *  }
     * 
     */
    public class Raid
    {
        [JsonProperty("terminId")]
        public long TerminID { get; set; }

        [JsonIgnore()]
        public long RaidID { get; set; }

        [JsonIgnore()]
        internal List<Account> Players;

        [JsonIgnore()]
        internal List<Account> Helper;

        [JsonIgnore()]
        internal List<Account> Inviteable;

        [JsonProperty("aufstellungen")]
        internal List<TeamComp> Bosses;

        [JsonIgnore()]
        internal List<string> Unknown;

        [JsonIgnore()]
        internal List<Account> ToInvite;

        internal Raid(long terminID, long raidID, List<Account> players, List<Account> helper, List<Account> inviteable, List<TeamComp> bosses)
        {
            TerminID = terminID;
            RaidID = raidID;
            Players = players;
            Helper = helper;
            Inviteable = inviteable;
            Bosses = bosses;
        }

        internal bool IsMember(string accountName)
        {
            return Players.Any(p => p.AccountName == accountName);
        }
        internal bool IsMember(long id)
        {
            return Players.Any(p => p.ID == id);
        }

        internal Account GetMember(string accountName)
        {
            return Players.First(p => p.AccountName == accountName);
        }
        internal Account GetMember(long id)
        {
            return Players.First(p => p.ID == id);
        }

        internal bool IsHelper(string accountName)
        {
            return Helper.Any(p => p.AccountName == accountName);
        }
        internal bool IsHelper(long id)
        {
            return Helper.Any(p => p.ID == id);
        }

        internal Account GetHelper(string accountName)
        {
            return Helper.First(p => p.AccountName == accountName);
        }
        internal Account GetHelper(long id)
        {
            return Helper.First(p => p.ID == id);
        }

        internal bool IsInviteable(string accountName)
        {
            return Inviteable.Any(p => p.AccountName == accountName);
        }
        internal bool IsInviteable(long id)
        {
            return Inviteable.Any(p => p.ID == id);
        }

        internal Account GetInviteable(string accountName)
        {
            return Inviteable.First(p => p.AccountName == accountName);
        }
        internal Account GetInviteable(long id)
        {
            return Inviteable.First(p => p.ID == id);
        }

        internal bool ExistsBoss(Boss boss, bool isCM)
        {
            return Bosses.Any(b => b.Encounter.RaidOrgaPlusID == boss.RaidOrgaPlusID && b.IsCM == isCM);
        }

        internal TeamComp GetTeamComp(Boss boss, bool isCM)
        {
            return Bosses.First(b => b.Encounter.RaidOrgaPlusID == boss.RaidOrgaPlusID && b.IsCM == isCM);
        }

        internal Account GetAccount(string accountName, PlayerType type)
        {
            switch (type)
            {
                case PlayerType.MEMBER:
                    return GetMember(accountName);
                case PlayerType.HELPER:
                    return GetHelper(accountName);
                case PlayerType.INVITEABLE:
                    return GetInviteable(accountName);
                case PlayerType.LFG:
                default:
                    return GetLFGPlayer();
            }
        }

        internal Account GetLFGPlayer()
        {
            return new Account(1, "", "LFG");
        }

        internal string GetPostJson()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
