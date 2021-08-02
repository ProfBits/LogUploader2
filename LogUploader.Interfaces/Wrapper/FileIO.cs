using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace LogUploader.Wrapper
{
    public static class FileIO
    {
        private static IFileIO backend = new SystemFileIO();
        private static string absolutPathToMainFolder;

        internal static IFileIO Backend { private get => backend; set => backend = value ?? throw new ArgumentException("FileIO backend cannot be null!"); }

        public static void AppendAllText(string path, string contents, Encoding encoding) => Backend.AppendAllText(path, contents, encoding);

        public static void AppendAllText(string path, string contents) => AppendAllText(path, contents);

        public static void Copy(string sourceFileName, string destFileName, bool overwrite) => Backend.Copy(sourceFileName, destFileName, overwrite);

        public static FileStream Create(string path) => Backend.Create(path);

        public static void Delete(string path) => Backend.Delete(path);

        public static bool Exists(string path) => Backend.Exists(path);

        public static void Move(string sourceFileName, string destFileName) => Backend.Move(sourceFileName, destFileName);

        public static FileStream Open(string path, FileMode mode, FileAccess access, FileShare share) => Backend.Open(path, mode, access, share);

        public static byte[] ReadAllBytes(string path) => Backend.ReadAllBytes(path);

        public static string[] ReadAllLines(string path, Encoding encoding) => Backend.ReadAllLines(path, encoding);

        public static string ReadAllText(string path) => Backend.ReadAllText(path);

        public static string ReadAllText(string path, Encoding encoding) => Backend.ReadAllText(path, encoding);

        public static void WriteAllBytes(string path, byte[] bytes) => Backend.WriteAllBytes(path, bytes);

        public static void WriteAllLines(string path, string[] contents, Encoding encoding) => Backend.WriteAllLines(path, contents, encoding);

        public static void WriteAllText(string path, string contents, Encoding encoding) => Backend.WriteAllText(path, contents, encoding);

        public static DateTime GetCreationTime(string path) => Backend.GetCreationTime(path);

        public static string AbsolutPathToMainFolder
        {
            get
            {
                if (absolutPathToMainFolder is null)
                {
                    string absolutMainExePath = Assembly.GetAssembly(typeof(FileIO)).Location;
                    string absolutMainDir = Path.GetDirectoryName(absolutMainExePath);
                    absolutPathToMainFolder = absolutMainDir + Path.DirectorySeparatorChar;
                }
                return absolutPathToMainFolder;
            }
            private set => absolutPathToMainFolder = value;
        }

    }

    internal sealed class SystemFileIO : IFileIO
    {
        public void AppendAllText(string path, string contents, Encoding encoding) => File.AppendAllText(path, contents, encoding);

        public void AppendAllText(string path, string contents) => AppendAllText(path, contents);

        public void Copy(string sourceFileName, string destFileName, bool overwrite) => File.Copy(sourceFileName, destFileName, overwrite);

        public FileStream Create(string path) => File.Create(path);

        public void Delete(string path) => File.Delete(path);

        public bool Exists(string path) => File.Exists(path);

        public void Move(string sourceFileName, string destFileName) => File.Move(sourceFileName, destFileName);

        public FileStream Open(string path, FileMode mode, FileAccess access, FileShare share) => File.Open(path, mode, access, share);

        public byte[] ReadAllBytes(string path) => File.ReadAllBytes(path);

        public string[] ReadAllLines(string path, Encoding encoding) => File.ReadAllLines(path, encoding);

        public string ReadAllText(string path) => File.ReadAllText(path);

        public string ReadAllText(string path, Encoding encoding) => File.ReadAllText(path, encoding);

        public void WriteAllBytes(string path, byte[] bytes) => File.WriteAllBytes(path, bytes);

        public void WriteAllLines(string path, string[] contents, Encoding encoding) => File.WriteAllLines(path, contents, encoding);

        public void WriteAllText(string path, string contents, Encoding encoding) => File.WriteAllText(path, contents, encoding);
        public DateTime GetCreationTime(string path) => File.GetCreationTime(path);
    }

}
