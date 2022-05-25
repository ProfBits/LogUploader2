using Newtonsoft.Json;

using System.Linq;

namespace LogUploader.Data.RaidOrgaPlus
{
    public class Position
    {
        public Position(int pos, long iD, string accName, Role role, Profession profession)
            : this(pos, iD, accName, new Role[] { role }, profession)
        {

        }

        public Position(int pos, long iD, string accName, Role[] role, Profession profession)
        {
            Pos = pos;
            ID = iD;
            if (ID != 0)
                AccName = accName;
            else
                AccName = "";
            if (role != null && role.Length > 0)
            {
                Roles = role;
            }
            else
            {
                Roles = new Role[] { Role.Empty };
            }
            Profession = profession;
        }

        [JsonProperty("position")]
        public int Pos { get; set; }

        [JsonProperty("spielerId")]
        public long ID { get; set; }

        [JsonIgnore]
        public string AccName { get; set; }

        [JsonIgnore]
        public Role[] Roles { get; set; }

        [JsonProperty("roleId")]
        public string RoleID { get => string.Join(", ", Roles.Select(r => (byte)r)); }

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

        internal void Set(RoPlusPlayer player)
        {
            Profession = player.Class;
            Roles = player.Roles.ToArray();
            AccName = player.AccountName;
            ID = player.RaidOrgaID;
        }

        internal void Free()
        {
            ID = 0;
            AccName = "";
        }

        internal bool IsFree()
        {
            return ID == 0;
        }

        internal bool IsPlayer()
        {
            return ID > 1;
        }
    }
}
