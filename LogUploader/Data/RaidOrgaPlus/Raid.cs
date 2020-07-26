using LogUploader.Helper.RaidOrgaPlus;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogUploader.Data.RaidOrgaPlus
{
    public class Raid
    {
        [JsonProperty("terminId")]
        public long TerminID { get; set; }

        [JsonIgnore()]
        public long RaidID { get; set; }

        [JsonIgnore()]
        public List<Account> Players;

        [JsonIgnore()]
        public List<Account> Helper;

        [JsonIgnore()]
        public List<Account> Inviteable;

        [JsonProperty("aufstellungen")]
        public List<TeamComp> Bosses;

        [JsonIgnore()]
        public List<string> Unknown;

        [JsonIgnore()]
        public List<Account> ToInvite;

        public Raid(long terminID, long raidID, List<Account> players, List<Account> helper, List<Account> inviteable, List<TeamComp> bosses)
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

        internal Account GetMember(string accountName)
        {
            return Players.First(p => p.AccountName == accountName);
        }

        internal bool IsHelper(string accountName)
        {
            return Helper.Any(p => p.AccountName == accountName);
        }

        internal Account GetHelper(string accountName)
        {
            return Helper.First(p => p.AccountName == accountName);
        }

        internal bool IsInviteable(string accountName)
        {
            return Inviteable.Any(p => p.AccountName == accountName);
        }

        internal Account GetInviteable(string accountName)
        {
            return Inviteable.First(p => p.AccountName == accountName);
        }

        internal bool ExistsBoss(Boss boss, bool isCM)
        {
            return Bosses.Any(b => b.Encounter.RaidOrgaPlusID == boss.RaidOrgaPlusID && b.IsCM == isCM);
        }

        internal TeamComp GetTeamComp(Boss boss, bool isCM)
        {
            return Bosses.First(b => b.Encounter.RaidOrgaPlusID == boss.RaidOrgaPlusID && b.IsCM == isCM);
        }

        internal Account GetAccount(string accountName, RaidOrgaPlusDataWorker.PlayerType type)
        {
            switch (type)
            {
                case RaidOrgaPlusDataWorker.PlayerType.MEMBER:
                    return GetMember(accountName);
                case RaidOrgaPlusDataWorker.PlayerType.HELPER:
                    return GetHelper(accountName);
                case RaidOrgaPlusDataWorker.PlayerType.INVITEABLE:
                    return GetInviteable(accountName);
                case RaidOrgaPlusDataWorker.PlayerType.LFG:
                    return GetLFGPlayer();
                default:
                    //TODO error handling
                    throw new NotImplementedException();
            }
        }

        private Account GetLFGPlayer()
        {
            return new Account(1, "", "LFG");
        }

        public string getPostJson()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
