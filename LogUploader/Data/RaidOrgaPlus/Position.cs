using LogUploader.Helper.RaidOrgaPlus;
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

        public int Pos { get; set; }
        public long ID { get; set; }
        public string AccName { get; set; }
        public Role Role { get; set; }
        public Profession Profession { get; set; }

        internal bool IsLFG()
        {
            throw new NotImplementedException();
        }

        internal object RemoveName()
        {
            throw new NotImplementedException();
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
