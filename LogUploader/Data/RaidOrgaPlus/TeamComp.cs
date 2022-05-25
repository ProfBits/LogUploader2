using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.Linq;

namespace LogUploader.Data.RaidOrgaPlus
{
    public class TeamComp
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

        internal bool Exists(string accountName)
        {
            return Players.Any(p => p.AccName == accountName);
        }
        internal bool Exists(Profession @class, ISet<Role> roles)
        {
            return UnnamedPlayers.Any(p => p.Profession.Equals(@class) && roles.SetEquals(p.Roles));
        }

        internal bool Exists(Profession @class)
        {
            return UnnamedPlayers.Any(p => p.Profession.Equals(@class));
        }

        internal bool Exists(ISet<Role> roles)
        {
            return UnnamedPlayers.Any(p => roles.SetEquals(p.Roles));
        }

        internal Position GetByName(string accountName)
        {
            return Players.First(p => p.AccName == accountName);
        }

        internal Position Get(Profession @class, ISet<Role> roles)
        {
            return UnnamedPlayers.First(p => p.Profession.Equals(@class) && roles.SetEquals(p.Roles));
        }

        internal Position Get(Profession @class)
        {
            return UnnamedPlayers.First(p => p.Profession.Equals(@class));
        }

        internal Position Get(ISet<Role> roles)
        {
            return UnnamedPlayers.First(p => roles.SetEquals(p.Roles));
        }

        internal Position Get()
        {
            if (UnnamedPlayers.Any())
            {
                return UnnamedPlayers.First();
            }
            else
            {
                var dupes = UnnamedPlayers.GroupBy(p => p.AccName).Where(g => g.Count() > 0).ToArray();
                if (dupes.Any())
                {
                    return dupes.First().First();
                }
                else
                {
                    return null;
                }
            }
        }

        internal void OrderPlayers(Boss b = null)
        {
            IComparer<IEnumerable<Role>> comparator;

            switch ((eBosses)(b?.ID ?? 0))
            {
                case eBosses.Deimos:
                    comparator = new RolesComparator(RolesComparator.Deimos);
                    break;
                case eBosses.Escort:
                    comparator = new RolesComparator(RolesComparator.Escort);
                    break;
                default:
                    comparator = new RolesComparator(RolesComparator.General);
                    break;
            }

            Players = Players.OrderBy(p => p.Roles, comparator).ThenBy(p => p.ClassID).ThenBy(p => p.AccName).Select((p, i) =>
            {
                p.Pos = i + 1;
                return p;
            }).ToList();
        }

        private class RolesComparator : IComparer<IEnumerable<Role>>
        {
            private readonly Func<Role, int> GetRoleWeight;

            public RolesComparator(Func<Role, int> getRoleWeight)
            {
                GetRoleWeight = getRoleWeight;
            }

            public int Compare(IEnumerable<Role> x, IEnumerable<Role> y)
            {
                var x1 = x.Select(GetRoleWeight).Min();
                var y1 = y.Select(GetRoleWeight).Min();
                if (x1 > y1)
                    return 1;
                if (x1 < y1)
                    return -1;
                return 0;
            }

            public static int General(Role r)
            {
                switch (r)
                {
                    case Role.Tank:
                        return 0;
                    case Role.Quickness:
                        return 5;
                    case Role.Heal:
                        return 10;
                    case Role.Alacrity:
                        return 15;
                    case Role.Utility:
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
            public static int Escort(Role r)
            {
                if (r == Role.Special)
                    return -10;
                else if (r == Role.Tank)
                    return 110;
                else
                    return General(r);
            }
            public static int Deimos(Role r)
            {
                if (r == Role.Special)
                    return 110;
                else
                    return General(r);
            }

        }
    }
}
