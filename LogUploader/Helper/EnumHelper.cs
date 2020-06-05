using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogUploader.Helpers
{
    public static class EnumHelper
    {
        public static IEnumerable<T> GetValues<T>() where T : struct
        {
            //var values = Enum.GetValues(typeof(T));
            //foreach (var element in values)
            //    yield return (T) element;
            return Enum.GetValues(typeof(T)).Cast<T>();
        }
    }
}
