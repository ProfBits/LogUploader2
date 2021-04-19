using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogUploader.Test.Mocks
{
    class MockDirectoryIO : Wrapper.IDirectoryIO, IMock
    {
        public static MockDirectoryIO Instance { get; } = new MockDirectoryIO();

        private MockDirectoryIO() { }

        public DirectoryInfo CreateDirectory(string path)
        {
            MockFileSystem.Data.CreateFolder(path);
            //TODO may cause issues
            //NUnit.Framework.Warn.If(true, "CreateDirectory returns DirectoryInfo null");
#warning CreateDirectory returns DirectoryInfo null;
            return null;
        }

        public void Delete(string path, bool recursive)
        {
            MockFileSystem.Data.DeleteFolder(path, recursive);
        }

        public bool Exists(string path)
        {
            return MockFileSystem.Data.DirectoryExits(path);
        }

        public string[] GetFiles(string path) => MockFileSystem.Data.GetDirectoryContent(path, false);

        public string[] GetFiles(string path, string searchPattern) => GetFiles(path, searchPattern, SearchOption.TopDirectoryOnly);

        public string[] GetFiles(string path, string searchPattern, SearchOption searchOption)
        {
            var files = MockFileSystem.Data.GetDirectoryContent(path, searchOption == SearchOption.AllDirectories);
            if (searchPattern == "*.*")
                return files;
            if (searchPattern.StartsWith("*."))
                return files.Where(p => p.EndsWith(searchPattern.Substring(1))).ToArray();
            throw new NotImplementedException();
        }

        public void Move(string sourceDirName, string destDirName)
        {
            MockFileSystem.Data.MoveDirectory(sourceDirName, destDirName);
        }

        public void Reset()
        {
            MockFileSystem.Data.Reset();
        }
    }
}
