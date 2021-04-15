using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogUploader.Tools
{
    public static class ListToDataTable
    {
        public static DataTable Convert<T>(IEnumerable<T> list)
        {
            var dt = new DataTable();
            return Fill(list, dt);
        }

        public static DataTable Fill<T>(IEnumerable<T> list, DataTable dt)
        {
            SetupDataTable<T>(dt);

            Dictionary<string, Func<object, object>> propReaders = GetGetValuePointersAndCreateColumne<T>(dt);

            //construct helper for inserting
            Action<T> InsertItem = CreateInsertingHelper<T>(dt, propReaders);

            //insert
            foreach (var item in list)
            {
                InsertItem(item);
            }

            //don e
            return dt;
        }

        private static void SetupDataTable<T>(DataTable dt)
        {
            dt.Clear();
            dt.Rows.Clear();
            dt.Columns.Clear();

            dt.TableName = nameof(T);
        }

        private static Action<T> CreateInsertingHelper<T>(DataTable dt, Dictionary<string, Func<object, object>> propReaders)
        {
            return (T item) =>
            {
                var r = dt.NewRow();
                foreach (var i in propReaders)
                {
                    r.SetField(i.Key, i.Value(item));
                }
                dt.Rows.Add(r);
            };
        }

        private static Dictionary<string, Func<object, object>> GetGetValuePointersAndCreateColumne<T>(DataTable dt)
        {
            var propReaders = new Dictionary<string, Func<object, object>>();

            //get all CanRead Properties and GetValue() pointers
            //and create table columns
            foreach (var prop in typeof(T).GetProperties())
            {
                if (prop.CanRead)
                {
                    propReaders.Add(prop.Name, prop.GetValue);
                    dt.Columns.Add(new DataColumn(prop.Name, prop.PropertyType));
                }
            }

            return propReaders;
        }
    }
}
