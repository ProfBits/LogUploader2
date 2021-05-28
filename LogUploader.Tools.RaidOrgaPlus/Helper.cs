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
using LogUploader.RaidOrgaPlus.Data;

namespace LogUploader.Tools.RaidOrgaPlus
{
    /// <summary>
    /// A general purpose helper for various tasks
    /// </summary>
    internal static class RaidOrgaHelper
    {
        public static Role GetRoleById(int id)
        {
            if (Enum.IsDefined(typeof(Role), (byte)id))
                return (Role)(byte)(id);
            return Role.Empty;
        }

        public static Role GetRoleByAbbreviation(string roleAbbreviation)
        {
            foreach (var r in Enum.GetValues(typeof(Role)).Cast<Role>())
            {
                if (r.GetAttribute<StringValueAttribute>().Value == roleAbbreviation)
                    return r;
            }
            return Role.Empty;
        }
    }
}
