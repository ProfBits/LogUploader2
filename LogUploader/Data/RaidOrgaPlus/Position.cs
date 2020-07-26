using LogUploader.Helper.RaidOrgaPlus;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogUploader.Data.RaidOrgaPlus
{
    public class Position
    {
        public Position(int pos, long iD, string accName, Role role, Profession profession)
        {
            Pos = pos;
            ID = iD;
            AccName = accName;
            Role = role;
            Profession = profession;
        }

        [JsonProperty("position")]
        public int Pos { get; set; }

        [JsonProperty("spielerId")]
        public long ID { get; set; }

        [JsonIgnore]
        public string AccName { get; set; }

        [JsonIgnore]
        public Role Role { get; set; }

        [JsonProperty("roleId")]
        public byte RoleID { get => (byte)Role; }

        [JsonIgnore]
        public Profession Profession { get; set; }

        [JsonProperty("classId")]
        public int ClassID { get => Profession.RaidOrgaPlusID; }

        internal bool IsLFG()
        {
            return ID == 1;
        }

        internal void RemoveName()
        {
            ID = 0;
            AccName = "";
        }

        internal void UpdateProffessionRole(Profession @class, Role role)
        {
            Profession = @class;
            if (Role != Role.Empty)
                Role = role;
        }

        internal void Set(RaidOrgaPlusDataWorker.CheckedPlayer player)
        {
            Profession = player.Class;
            Role = player.Role;
            AccName = player.AccountName;
            ID = player.RaidOrgaID;
        }
    }
}
