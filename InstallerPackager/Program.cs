using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace InstallerPackager
{
    class Program
    {
        private const string INSTALL_FOLDER_NAME = "LogUploaderInstallerTEMP";

        static void Main()
        {
            var installerFolder = INSTALL_FOLDER_NAME;
            installerFolder = PrepeareInstallFolder(installerFolder);
            if (installerFolder == null) return;
            RunInstaller(installerFolder);
        }

        private static void RunInstaller(string installerFolder)
        {
            try
            {
                HideFolder(installerFolder);
                mRecreateAllExecutableResources(installerFolder);
                File.Move(installerFolder + Path.PathSeparator + "InstallerPackager.installer.LogUploaderSetup.msi", installerFolder + Path.PathSeparator + "LogUploaderSetup.msi");
                ExecuteAndWaitForInstaller(installerFolder);
            }
            finally
            {
                DeletFolderAndContent(installerFolder);
            }
        }

        private static string PrepeareInstallFolder(string installerFolder)
        {
            try
            {
                UpdateWorkDir();
                installerFolder = CreateTempFolder(installerFolder);
            }
            catch (Exception)
            {
                DeletFolderAndContent(installerFolder);
                return null;
            }

            return installerFolder;
        }

        private static void HideFolder(string installerFolder)
        {
            new DirectoryInfo(installerFolder).Attributes |= FileAttributes.Hidden;
        }

        private static void DeletFolderAndContent(string installerFolder)
        {
            if (Directory.Exists(installerFolder))
                Directory.Delete(installerFolder, true);
        }

        private static void UpdateWorkDir()
        {
            Assembly currentAssembly = Assembly.GetExecutingAssembly();
            var workDir = Path.GetDirectoryName(currentAssembly.Location);
            Environment.CurrentDirectory = workDir;
        }

        private static void ExecuteAndWaitForInstaller(string installerFolder)
        {
            using (var p = new Process())
            {
                p.StartInfo = new ProcessStartInfo(installerFolder + Path.PathSeparator + "InstallerPackager.installer.setup.exe");
                p.Start();
                p.WaitForExit();
            }
        }

        private static string CreateTempFolder(string installerFolder)
        {
            installerFolder = FindAvailabeFolderName(installerFolder);
            Directory.CreateDirectory(installerFolder);
            return installerFolder;
        }

        private static string FindAvailabeFolderName(string installerFolder)
        {
            int i = 0;
            while (Directory.Exists(installerFolder + i))
                i++;

            return installerFolder + i;
        }

        /*
         * Thanks to https://web.archive.org/web/20140223075628/http://www.cs.nyu.edu/~vs667/articles/embed_executable_tutorial
         * Refactored to improve readeability
         */
        //======================================================
        //Recreate all executable resources
        //======================================================
        private static void mRecreateAllExecutableResources(string folderName)
        {
            // Get Current Assembly refrence
            Assembly currentAssembly = Assembly.GetExecutingAssembly();
            // Get all imbedded resources
            string[] arrResources = currentAssembly.GetManifestResourceNames();

            foreach (string resourceName in arrResources)
            {
                if (resourceName.EndsWith(".exe") || resourceName.EndsWith(".msi"))
                    SaveResourceToDisk(resourceName, currentAssembly, folderName);

            }
        }

        private static void SaveResourceToDisk(string resourceName, Assembly currentAssembly, string folderName)
        {
            //Name of the file saved on disk
            string saveAsName = folderName + Path.PathSeparator + resourceName;
            using (FileStream streamToOutputFile = CreateFile(saveAsName))
            using (Stream streamToResourceFile = currentAssembly.GetManifestResourceStream(resourceName))
            {
                CopyToFileStream(streamToOutputFile, streamToResourceFile);

                streamToOutputFile.Close();
                streamToResourceFile.Close();
            }
        }

        private static FileStream CreateFile(string saveAsName, bool overrideExisting = false)
        {
            FileInfo fileInfoOutputFile = new FileInfo(saveAsName);

            if (overrideExisting && fileInfoOutputFile.Exists)
                fileInfoOutputFile.Delete();

            FileStream streamToOutputFile = fileInfoOutputFile.OpenWrite();
            return streamToOutputFile;
        }

        private static void CopyToFileStream(FileStream streamToOutputFile, Stream streamToResourceFile)
        {
            const int size = 4096;
            byte[] bytes = new byte[4096];
            int numBytes;
            while ((numBytes = streamToResourceFile.Read(bytes, 0, size)) > 0)
            {
                streamToOutputFile.Write(bytes, 0, numBytes);
            }
        }
    }
}
