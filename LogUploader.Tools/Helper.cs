﻿using Extensiones;

using LogUploader.Data;
using LogUploader.Tools.Logging;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace LogUploader.Tools
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

        public static void Exit(ExitCode exitCode)
        {
            Logger.Error($"Programm Exit Reason: {(int)exitCode} {exitCode}");
            Environment.Exit((int)exitCode);
        }
    }
}
