using LogUploader.Helper;
using LogUploader.Tools.Logging;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogUploader.Tools
{
    public static class ExecuteHelper
    {
        private static Func<string, string, System.Windows.Forms.Form> getExceptionUI = (discardA, discarB) => null;

        public static Func<string, string, System.Windows.Forms.Form> GetExceptionUI
        {
            private get => getExceptionUI;
            set => getExceptionUI = value ?? throw new ArgumentNullException("Callback for Error UI creation cannot be null");
        }

        public static bool ExecuteSecure(Action a, bool CrashProgramm = false)
        {
            bool res = false;
            res = ExecuteSecure(() => { a(); return true; }, CrashProgramm);
            return res;
        }

        public static T ExecuteSecure<T>(Func<T> f, bool CrashProgramm = false)
        {
            try
            {
                return f();
            }
            catch (Win32Exception e)
            {
                Logger.Error("Win32 Error Code: " + e.NativeErrorCode + " (native) " + e.ErrorCode + " (managed)");
                Logger.LogException(e);
                if (CrashProgramm)
                {
                    var errorUI = GetExceptionUI("Win32 error", GetWin23ExeptionMessage(e));
                    errorUI?.ShowDialog();
                    GP.Exit(ExitCode.WIN32_EXCPTION);
                }
            }
            catch (Exception e)
            {
                Logger.Error("Normal ERROR");
                Logger.LogException(e);
                if (CrashProgramm)
                {
                    var errorUI = GetExceptionUI("Normal Exception", GetExceptionMessage(e));
                    errorUI?.ShowDialog();
                    GP.Exit(ExitCode.CLR_EXCPTION);
                }
            }
            return default;
        }

        private static string GetExceptionMessage(Exception e)
        {
            return $"Exception: {e.GetType()}\n" +
                                $"Message: {e.Message}\n" +
                                "StacTrace:\n" +
                                $"{e.StackTrace}";
        }

        private static string GetWin23ExeptionMessage(Win32Exception e)
        {
            var res = $"Exception: {e.GetType()}\n" +
                                    $"Message: {e.Message}\n" +
                                    $"NativeErrorCode: {e.NativeErrorCode}\n" +
                                    $"ErrorCode: {e.ErrorCode}\n";
            try
            {
                res += $"Data: {string.Join("\n", e?.Data?.Keys.Cast<object>().Select(k => k.ToString() + " : " + e.Data[k].ToString()))}\n";
            }
            catch (Exception)
            { }
            res += $"\nStackTrace: " +
                $"{e.StackTrace}";


            return res;
        }
    }
}
