using Extensiones;

using LogUploader.Data;
using LogUploader.Properties;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace LogUploader.Helper
{
    /// <summary>
    /// A general purpose helper for various tasks
    /// </summary>
    public static class GP
    {
        public static string GetName(int id)
        {
            return GetEnemyByID(id)?.Name;
        }

        public static bool IsInteresting(int id)
        {
            var enemy = GetEnemyByID(id);
            if (enemy is Boss)
                return true;
            if (enemy is AddEnemy add)
                return add.IsInteresting;
            return true;
        }

        public static Enemy GetEnemyByID(int id)
        {
            if (Boss.ExistsID(id))
                return Boss.GetByID(id);
            if (AddEnemy.ExistsID(id))
                return AddEnemy.GetByID(id);
            return null;
        }

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
