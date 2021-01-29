using Extensiones;

using LogUploader.Data;
using LogUploader.Helper;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace LogUploader.Tools.RaidOrgaPlus
{
    /// <summary>
    /// A general purpose helper for various tasks
    /// </summary>
    public static class RaidOrgaHelper
    {
        public static Data.RaidOrgaPlus.Role GetRoleById(int id)
        {
            if (Enum.IsDefined(typeof(Data.RaidOrgaPlus.Role), (byte)id))
                return (Data.RaidOrgaPlus.Role)(byte)(id);
            return Data.RaidOrgaPlus.Role.Empty;
        }

        public static Data.RaidOrgaPlus.Role GetRoleByAbbreviation(string roleAbbreviation)
        {
            foreach (var r in Enum.GetValues(typeof(Data.RaidOrgaPlus.Role)).Cast<Data.RaidOrgaPlus.Role>())
            {
                if (r.GetAttribute<StringValueAttribute>().Value == roleAbbreviation)
                    return r;
            }
            return Data.RaidOrgaPlus.Role.Empty;
        }
    }
}
