using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogUploader.IO
{
    internal class FileIOFactory
    {
        public static IFileIO? Instance;

        public static IFileIO Create()
        {
            var roots = new Dictionary<RootFolder, string>
            {
                { RootFolder.AppDataLocal, Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "LogUploader") },
                { RootFolder.AppDataRoaming, Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "LogUploader") },
                { RootFolder.EliteInsightsOutput, Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "LogUploader", "EiLogs") },
                { RootFolder.EliteInsightsOutput, Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "LogUploader", "EliteInsights") },
                { RootFolder.Logs, Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "LogUploader", "Logs") },
                { RootFolder.StaticData, Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location ?? throw new NotImplementedException()), "data") },
                { RootFolder.ProgrammFiles, Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location ?? throw new NotImplementedException())) },
                { RootFolder.Custom, "" },
            };

            Instance = new FileIO(roots);
            return Instance;
        }

        internal static IFileIO GetInstance()
        {
            if (Instance is null)
            {
                throw new NotImplementedException();
            }
            return Instance;
        }
    }
}
