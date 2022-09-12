using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogUploader.IO
{
    public class FileIOFactory
    {
        private static IFileIO? Instance;

        private static IFileIO Create()
        {
            var roots = new Dictionary<RootFolder, string>
            {
                { RootFolder.AppDataLocal, Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "LogUploader") },
                { RootFolder.AppDataRoaming, Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "LogUploader") },
                { RootFolder.EliteInsightsOutput, Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "LogUploader", "EiLogs") },
                { RootFolder.EliteInsights, Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "LogUploader", "EliteInsights") },
                { RootFolder.Logs, Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "LogUploader", "Logs") },
                //TODO
                //{ RootFolder.StaticData, Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location ?? throw new NotImplementedException()), "data") },
                //{ RootFolder.ProgrammFiles, Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location ?? "sdg")) },
                { RootFolder.Custom, "" },
            };

            Instance = new FileIO(roots);
            return Instance;
        }

        public static IFileIO GetInstance()
        {
            if (Instance is null)
            {
                Instance = Create();
            }
            return Instance;
        }
    }
}
