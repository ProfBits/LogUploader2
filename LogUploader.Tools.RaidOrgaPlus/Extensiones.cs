
using LogUploader.RaidOrgaPlus.Data;

using System;
using System.Collections.Generic;
using System.Linq;

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
