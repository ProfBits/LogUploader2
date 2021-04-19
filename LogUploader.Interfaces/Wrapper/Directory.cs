using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogUploader.Wrapper
{
    public static class DirectoryIO
    {
        internal static IDirectoryIO Backend { private get; set; } = new SystemDirectoryIO();

        public static DirectoryInfo CreateDirectory(string path) => Backend.CreateDirectory(path);
                
        public static void Delete(string path, bool recursive) => Backend.Delete(path, recursive);

        public static bool Exists(string path) => Backend.Exists(path);
                
        public static string[] GetFiles(string path) => Backend.GetFiles(path);

        public static string[] GetFiles(string path, string searchPattern) => Backend.GetFiles(path, searchPattern);

        public static string[] GetFiles(string path, string searchPattern, SearchOption searchOption) => Backend.GetFiles(path, searchPattern, searchOption);
                
        public static void Move(string sourceDirName, string destDirName) => Backend.Move(sourceDirName, destDirName);
    }

    internal class SystemDirectoryIO : IDirectoryIO
    {
        public DirectoryInfo CreateDirectory(string path) => Directory.CreateDirectory(path);

        public void Delete(string path, bool recursive) => Directory.Delete(path, recursive);

        public bool Exists(string path) => Directory.Exists(path);

        public string[] GetFiles(string path) => Directory.GetFiles(path);

        public string[] GetFiles(string path, string searchPattern) => Directory.GetFiles(path, searchPattern);

        public string[] GetFiles(string path, string searchPattern, SearchOption searchOption) => Directory.GetFiles(path, searchPattern, searchOption);

        public void Move(string sourceDirName, string destDirName) => Directory.Move(sourceDirName, destDirName);
    }
}
