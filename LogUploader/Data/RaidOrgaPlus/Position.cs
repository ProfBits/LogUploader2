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
            if (ID != 0)
                AccName = accName;
            else
                AccName = "";
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

        internal void UpdateProffessionRole(Profession @class, Role role, bool overrideTank = false)
        {
            Profession = @class;
            if (Role == Role.Empty)
                Role = role;
            //Override if RO+ and my guess just differ by power and condi role
            else if ((Role == Role.Power || Role == Role.Condi) && (role == Role.Power || role == Role.Condi))
                Role = role;
            //Override toughness tanks
            else if (overrideTank && (Role == Role.Tank || role == Role.Tank))
                Role = role;
        }

        internal void Set(RoPlusPlayer player)
        {
            Profession = player.Class;
            Role = player.Role;
            AccName = player.AccountName;
            ID = player.RaidOrgaID;
        }
    }
}
