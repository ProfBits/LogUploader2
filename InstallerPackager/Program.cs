﻿using System;
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

        static void Main(string[] args)
        {
            var installerFolder = INSTALL_FOLDER_NAME;
            try
            {
                UpdateWorkDir();
                installerFolder = CreateTempFolder(installerFolder);
            }
            catch (Exception)
            {
                DeletFolderAndContent(installerFolder);
                return;
            }
            try
            {
                HideFolder(installerFolder);
                mRecreateAllExecutableResources(installerFolder);
                File.Move(installerFolder + '\\' + "InstallerPackager.installer.LogUploaderSetup.msi", installerFolder + '\\' + "LogUploaderSetup.msi");
                ExecuteAndWaitForInstaller(installerFolder);
            }
            finally
            {
                DeletFolderAndContent(installerFolder);
            }
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
                p.StartInfo = new ProcessStartInfo(installerFolder + '\\' + "InstallerPackager.installer.setup.exe");
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
                { //or other extension desired
                    //Name of the file saved on disk
                    string saveAsName = folderName + '\\' + resourceName;
                    FileInfo fileInfoOutputFile = new FileInfo(saveAsName);
                    //CHECK IF FILE EXISTS AND DO SOMETHING DEPENDING ON YOUR NEEDS
                    if (fileInfoOutputFile.Exists)
                    {
                        //overwrite if desired  (depending on your needs)
                        //fileInfoOutputFile.Delete();
                    }
                    //OPEN NEWLY CREATING FILE FOR WRITTING
                    FileStream streamToOutputFile = fileInfoOutputFile.OpenWrite();
                    //GET THE STREAM TO THE RESOURCES
                    Stream streamToResourceFile =
                                        currentAssembly.GetManifestResourceStream(resourceName);

                    //---------------------------------
                    //SAVE TO DISK OPERATION
                    //---------------------------------
                    const int size = 4096;
                    byte[] bytes = new byte[4096];
                    int numBytes;
                    while ((numBytes = streamToResourceFile.Read(bytes, 0, size)) > 0)
                    {
                        streamToOutputFile.Write(bytes, 0, numBytes);
                    }

                    streamToOutputFile.Close();
                    streamToResourceFile.Close();
                }//end_if

            }//end_foreach
        }//end_mRecreateAllExecutableResources
    }
}
