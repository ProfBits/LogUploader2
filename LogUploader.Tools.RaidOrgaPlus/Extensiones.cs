using LogUploader.Data.RaidOrgaPlus;

using System;
using System.Collections.Generic;
using System.Linq;

namespace LogUploader.Tools.RaidOrgaPlus
{
    public static class Extensiones
    {
        public static Role GetRoleByAbbreviation(this LogUploader.Data.RaidOrgaPlus.Role _, string roleAbbreviation)
        {
            return RaidOrgaHelper.GetRoleByAbbreviation(roleAbbreviation);
        }
    }
}
