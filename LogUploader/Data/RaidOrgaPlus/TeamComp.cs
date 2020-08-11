using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogUploader.Data.RaidOrgaPlus
{
    public class TeamComp
    {
        [JsonIgnore]
        public long ID { get; set; }

        [JsonProperty("aufstellungId")]
        public long? aufstellungsID
        {
            get
            {
                if (ID >= 0)
                    return ID;
                else
                    return null;
            }
        }
#if ROPLUSHUMAN
        [JsonProperty("Boss")]
        public string BossName { get => Encounter.Name; }
#endif

        [JsonIgnore]
        public Boss Encounter { get; set; }

        [JsonProperty("bossId")]
        public long BossID { get => Encounter.RaidOrgaPlusID; }

        [JsonProperty("isCM")]
        public bool IsCM { get; set; }

        [JsonProperty("positionen")]
        public List<Position> Players { get; set; }

        public TeamComp(long iD, Boss encounter, bool isCM, List<Position> players)
        {
            ID = iD;
            Encounter = encounter;
            IsCM = isCM;
            Players = players;
        }

        private IEnumerable<Position> UnnamedPlayers { get => Players.Where(pos => string.IsNullOrEmpty(pos.AccName) || pos.AccName == "???"); }

        internal bool Exists(string accountName)
        {
            return Players.Any(p => p.AccName == accountName);
        }
        internal bool Exists(Profession @class, Role role)
        {
            return UnnamedPlayers.Any(p => p.Profession.Equals(@class) && p.Role == role);
        }

        internal bool Exists(Profession @class)
        {
            return UnnamedPlayers.Any(p => p.Profession.Equals(@class));
        }

        internal bool Exists(Role role)
        {
            return UnnamedPlayers.Any(p => p.Role == role);
        }

        internal bool Exists()
        {
            return UnnamedPlayers.Any();
        }

        internal Position GetByName(string accountName)
        {
            return Players.First(p => p.AccName == accountName);
        }

        internal Position Get(Profession @class, Role role)
        {
            return UnnamedPlayers.First(p => p.Profession.Equals(@class) && p.Role == role);
        }

        internal Position Get(Profession @class)
        {
            return UnnamedPlayers.First(p => p.Profession.Equals(@class));
        }

        internal Position Get(Role role)
        {
            return UnnamedPlayers.First(p => p.Role == role);
        }

        internal Position Get()
        {
            return UnnamedPlayers.First();
        }

        internal void OrderPlayers()
        {
            Players = Players.OrderBy(p => p.Role, new RoleComparator()).Select((p, i) =>
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

            private int GetRoleNumber(Role r)
            {
                switch (r)
                {
                    case Role.Tank:
                        return 0;
                    case Role.Utility:
                        return 10;
                    case Role.Heal:
                        return 20;
                    case Role.Special:
                        return 30;
                    case Role.Banner:
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
    }
}
