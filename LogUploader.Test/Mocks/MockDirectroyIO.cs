using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogUploader.Test.Mocks
{
    class MockDirectroyIO : Wrapper.IDirectoryIO, IMock
    {
        public static MockDirectroyIO Instance { get; } = new MockDirectroyIO();

        private MockDirectroyIO() { }

        public DirectoryInfo CreateDirectory(string path)
        {
            MockFileSystem.Data.CreateFolder(path);
            //TODO may cause issues
            NUnit.Framework.Warn.If(true, "CreateDirectory returns DirectoryInfo null");
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

        public string[] GetFiles(string path)
        {
            return MockFileSystem.Data.GetDirectoryContent(path);
        }

        public string[] GetFiles(string path, string searchPattern)
        {
            if (searchPattern == "*.*")
                return GetFiles(path);
            if (searchPattern.StartsWith("*."))
                return GetFiles(path).Where(p => p.EndsWith(searchPattern.Substring(1))).ToArray();
            throw new NotImplementedException();
        }

        public void Move(string sourceDirName, string destDirName)
        {
            MockFileSystem.Data.MoveDirectory(sourceDirName, destDirName);
        }

        public void Reset()
        {
        }
    }
}
