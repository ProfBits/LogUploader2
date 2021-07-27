
using System;
using System.Collections.Generic;
using System.Linq;

using LogUploader.Tools.RaidOrgaPlus.Data;

namespace LogUploader.Tools.RaidOrgaPlus
{
    internal static class Extensiones
    {
        public static Role GetRoleByAbbreviation(this Role _, string roleAbbreviation)
        {
            return RaidOrgaHelper.GetRoleByAbbreviation(roleAbbreviation);
        }
    }
}
