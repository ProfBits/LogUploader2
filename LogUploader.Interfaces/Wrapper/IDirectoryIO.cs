using System.Collections.Generic;
using System.IO;

namespace LogUploader.Wrapper
{
    interface IDirectoryIO
    {
        DirectoryInfo CreateDirectory(string path);
        void Delete(string path, bool recursive);
        bool Exists(string path);
        string[] GetFiles(string path);
        string[] GetFiles(string path, string searchPattern);
        string[] GetFiles(string path, string searchPattern, SearchOption searchOption);
        void Move(string sourceDirName, string destDirName);
    }
}
