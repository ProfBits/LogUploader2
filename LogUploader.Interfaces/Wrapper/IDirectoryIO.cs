using System.Collections.Generic;
using System.IO;

namespace LogUploader.Wrapper
{
    interface IDirectoryIO : ISpecialDirectories
    {
        //TODO return void? or my own Directroy Info
        DirectoryInfo CreateDirectory(string path);
        void Delete(string path, bool recursive);
        bool Exists(string path);
        string[] GetFiles(string path);
        string[] GetFiles(string path, string searchPattern);
        string[] GetFiles(string path, string searchPattern, SearchOption searchOption);
        void Move(string sourceDirName, string destDirName);

        //TODO implement some form of DirectoryInfo
        //should contain
        // - number of files
        // - list of dirInfos (filtered)
        // - list of fileInfo (filtered)
    }
}
