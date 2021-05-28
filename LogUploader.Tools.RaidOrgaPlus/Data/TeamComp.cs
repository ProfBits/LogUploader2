using LogUploader.Data;

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogUploader.RaidOrgaPlus.Data
{
    internal class TeamComp
    {
        [JsonIgnore]
        public long ID { get; set; }

        [JsonProperty("aufstellungId", NullValueHandling = NullValueHandling.Ignore)]
        public long? AufstellungsID
        {
            get
            {
                if (ID >= 0)
                    return ID;
                else
                    return null;
            }
        }

        [JsonIgnore]
        public Boss Encounter { get; set; }

        [JsonProperty("bossId")]
        public long BossID { get => Encounter.RaidOrgaPlusID; }

        [JsonProperty("isCM")]
        public bool IsCM { get; set; }

        [JsonProperty("positionen")]
        public List<Position> Players { get; set; }

        [JsonProperty("success")]
        public bool Success { get; set; } = false;

        public TeamComp(long iD, Boss encounter, bool isCM, List<Position> players, bool success = false)
        {
            ID = iD;
            Encounter = encounter;
            IsCM = isCM;
            Players = players;
            Success = success;
        }

        private IEnumerable<Position> UnnamedPlayers { get => Players.Where(pos => string.IsNullOrEmpty(pos.AccName) || pos.AccName == "???"); }

        public bool Exists(string accountName)
        {
            return Players.Any(p => p.AccName == accountName);
        }
        public bool Exists(IProfession @class, Role role)
        {
            return UnnamedPlayers.Any(p => p.Profession.Equals(@class) && p.Role == role);
        }

        public bool Exists(IProfession @class)
        {
            return UnnamedPlayers.Any(p => p.Profession.Equals(@class));
        }

        public bool Exists(Role role)
        {
            return UnnamedPlayers.Any(p => p.Role == role);
        }

        public bool Exists()
        {
            return UnnamedPlayers.Any();
        }

        public Position GetByName(string accountName)
        {
            return Players.First(p => p.AccName == accountName);
        }

        public Position Get(IProfession @class, Role role)
        {
            return UnnamedPlayers.First(p => p.Profession.Equals(@class) && p.Role == role);
        }

        public Position Get(IProfession @class)
        {
            return UnnamedPlayers.First(p => p.Profession.Equals(@class));
        }

        public Position Get(Role role)
        {
            return UnnamedPlayers.First(p => p.Role == role);
        }

        public Position Get()
        {
            return UnnamedPlayers.First();
        }

        public void OrderPlayers(Boss b = null)
        {
            IComparer<Role> comparator;

            switch (b.ID)
            {
                case 17154:
                    comparator = new DeimosRoleComparator();
                    break;
                case 16253:
                    comparator = new EscortRoleComparator();
                    break;
                default:
                    comparator = new RoleComparator();
                    break;
            }

            Players = Players.OrderBy(p => p.Role, comparator).ThenBy(p => p.ClassID).ThenBy(p => p.AccName).Select((p, i) =>
            {
                p.Pos = i + 1;
                return p;
            }).ToList();
        }

        private class RoleComparator : IComparer<Role>
        {
            public int Compare(Role x, Role y)
            {
                var x1 = GetRoleNumber(x);
                var y1 = GetRoleNumber(y);
                if (x1 > y1)
                    return 1;
                if (x1 < y1)
                    return -1;
                return 0;
            }

            protected virtual int GetRoleNumber(Role r)
            {
                switch (r)
                {
                    case Role.Tank:
                        return 0;
                    case Role.Utility:
                        return 10;
                    case Role.Heal:
                        return 20;
                    case Role.Banner:
                        return 30;
                    case Role.Special:
                        return 40;
                    case Role.Kiter:
                        return 50;
                    case Role.Power:
                        return 60;
                    case Role.Condi:
                        return 70;
                    case Role.Empty:
                        return 80;
                    default:
                        return 100;
                }
            }
        }

        private class EscortRoleComparator : RoleComparator
        {
            protected override int GetRoleNumber(Role r)
            {

                switch (r)
                {
                    case Role.Special:
                        return 0;
                    case Role.Utility:
                        return 10;
                    case Role.Heal:
                        return 20;
                    case Role.Banner:
                        return 40;
                    case Role.Kiter:
                        return 50;
                    case Role.Power:
                        return 60;
                    case Role.Condi:
                        return 70;
                    case Role.Tank:
                        return 75;
                    case Role.Empty:
                        return 80;
                    default:
                        return 100;
                }
            }
        }

        private class DeimosRoleComparator : RoleComparator
        {
            protected override int GetRoleNumber(Role r)
            {
                if (r == Role.Special)
                    return 110;
                else
                    return base.GetRoleNumber(r);
            }
        }
    }
}
