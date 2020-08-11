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

#if ROPLUSHUMAN
        [JsonProperty("account")]
#else
        [JsonIgnore]
#endif
        public string AccName { get; set; }

#if ROPLUSHUMAN
        [JsonProperty("Role")]
        public string RoleName { get => Role.ToString(); }
#endif
        [JsonIgnore]
        public Role Role { get; set; }

        [JsonProperty("roleId")]
        public byte RoleID { get => (byte)Role; }

#if ROPLUSHUMAN
        [JsonProperty("Profession")]
        public string Profstest { get => Profession.Name; }
#endif
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
            if (Role == Role.Empty)
                Role = role;
            //Override if RO+ and my guess just differ by power and condi role
            else if ((Role == Role.Power || Role == Role.Condi) && (role == Role.Power || role == Role.Condi))
                Role = role;
        }

        internal void Set(RaidOrgaPlusDataWorker.RoPlusPlayer player)
        {
            Profession = player.Class;
            Role = player.Role;
            AccName = player.AccountName;
            ID = player.RaidOrgaID;
        }
    }
}
