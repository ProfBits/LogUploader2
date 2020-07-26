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

        private IEnumerable<Position> UnnamedPlayers { get => Players.Where(pos => string.IsNullOrEmpty(pos.AccName)); }

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
    }
}
