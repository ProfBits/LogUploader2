using System;
using System.IO;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace LogUploader.Wrapper
{
    internal interface IFileIO
    {
        bool Exists(string path);
        void AppendAllText(string path, string contents, Encoding encoding);
        void AppendAllText(string path, string contents);
        void Copy(string sourceFileName, string destFileName, bool overwrite);
        FileStream Create(string path);
        FileStream Open(string path, FileMode mode, FileAccess access, FileShare share);
        byte[] ReadAllBytes(string path);
        string[] ReadAllLines(string path, Encoding encoding);
        string ReadAllText(string path);
        string ReadAllText(string path, Encoding encoding);
        void WriteAllBytes(string path, byte[] bytes);
        void WriteAllLines(string path, string[] contents, Encoding encoding);
        void WriteAllText(string path, string contents, Encoding encoding);
        void Move(string sourceFileName, string destFileName);
    }
}
