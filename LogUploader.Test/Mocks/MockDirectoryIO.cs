using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace LogUploader.Test.Mocks
{
    class MockDirectoryIO : Wrapper.IDirectoryIO, IMock
    {
        private static MockDirectoryIO instance = null;
        public static MockDirectoryIO Instance
        {
            get => instance ?? throw new NullReferenceException("Global File IO not set yet");
            set => instance = instance ?? value ?? throw new ArgumentNullException("Global FileIO cannot be null");
        }
        private readonly MockFileSystem Data;
        public string AppDataLocal { get => Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\LogUploader\\"; }
        public string AppDataRoaming { get => Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\LogUploader\\"; }
        public string InstallFolder { get => Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + '\\'; }

        internal MockDirectoryIO(MockFileSystem fileSystem)
        {
            Data = fileSystem;
        }

        public DirectoryInfo CreateDirectory(string path)
        {
            Data.CreateFolder(path);
            //TODO may cause issues
            //NUnit.Framework.Warn.If(true, "CreateDirectory returns DirectoryInfo null");
#warning CreateDirectory returns DirectoryInfo null;
            return null;
        }

        public void Delete(string path, bool recursive)
        {
            Data.DeleteFolder(path, recursive);
        }

        public bool Exists(string path)
        {
            return Data.DirectoryExits(path);
        }

        public string[] GetFiles(string path) => Data.GetDirectoryContent(path, false);

        public string[] GetFiles(string path, string searchPattern) => GetFiles(path, searchPattern, SearchOption.TopDirectoryOnly);

        public string[] GetFiles(string path, string searchPattern, SearchOption searchOption)
        {
            var files = Data.GetDirectoryContent(path, searchOption == SearchOption.AllDirectories);
            if (searchPattern == "*.*")
                return files;
            if (searchPattern.StartsWith("*."))
                return files.Where(p => p.EndsWith(searchPattern.Substring(1))).ToArray();
            throw new NotImplementedException();
        }

        public void Move(string sourceDirName, string destDirName)
        {
            Data.MoveDirectory(sourceDirName, destDirName);
        }

        public void Reset()
        {
            Data.Reset();
        }
    }
}
