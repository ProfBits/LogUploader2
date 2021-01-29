using Extensiones;

using LogUploader.Data;

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

        public static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dateTime = dateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dateTime;
        }

        public static string GetLocalOffset()
        {
            var offest = Math.Round(TimeZone.CurrentTimeZone.GetUtcOffset(DateTime.Now.ToUniversalTime()).TotalHours, 1);
            var Zone = TimeZone.CurrentTimeZone.StandardName;
            return Zone + " " + offest;
        }

        public static string GetDateTimeString(DateTime dateTime)
        {
            string date = $"{dateTime.Year.ToString().PadLeft(4, '0')}-{dateTime.Month.ToString().PadLeft(2, '0')}-{dateTime.Day.ToString().PadLeft(2, '0')}";
            string time = $"{dateTime.Hour.ToString().PadLeft(2, '0')}:{dateTime.Minute.ToString().PadLeft(2, '0')}:{dateTime.Second.ToString().PadLeft(2, '0')}";
            return $"{date} {time}";
        }

        [Obsolete]
        public static bool Compare<T>(T obj1, T obj2)
        {
            if ((object)obj1 == null)
                return (object)obj2 == null;

            return obj1.Equals(obj2);
        }

        public static string ReadJsonFile(string path)
        {
            return File.ReadAllText(path, Encoding.GetEncoding("iso-8859-1"));
        }
        public static void WriteJsonFile(string path, string text)
        {
            File.WriteAllText(path, text, Encoding.GetEncoding("iso-8859-1"));
        }

        public static Version GetVersion()
        {
            var fi = FileVersionInfo.GetVersionInfo(System.Reflection.Assembly.GetEntryAssembly().Location);
            return new Version(fi.ProductMajorPart, fi.ProductMinorPart, fi.ProductBuildPart, fi.ProductPrivatePart);
        }

        public static T IntToEnum<T>(int id) where T : struct
        {
            T e = (T)(object)id;
            if (Enum.GetNames(typeof(T)).Contains(Enum.GetName(typeof(T), e)))
                return e;
            return (T)(object)0;
        }
    }
}
