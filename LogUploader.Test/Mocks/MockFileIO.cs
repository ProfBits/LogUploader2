using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NUnit.Framework;

namespace LogUploader.Test.Mocks
{
    internal class MockFileIO : Wrapper.IFileIO, IMock
    {
        internal static MockFileIO Instance = new MockFileIO();

        private MockFileIO() { }

        internal static Encoding DefaultEncoding { get; set; } = Encoding.ASCII;

        public void AppendAllText(string path, string contents, Encoding encoding)
        {
            MockFileSystem.Data.AppendFile(path, encoding.GetBytes(contents));
        }

        public void AppendAllText(string path, string contents) => AppendAllText(path, contents, DefaultEncoding);

        public void Copy(string sourceFileName, string destFileName, bool overwrite)
        {
            if (!overwrite && Exists(destFileName)) throw new IOException("File already exists " + destFileName);
            MockFileSystem.Data.WriteFile(destFileName, MockFileSystem.Data.ReadFile(sourceFileName));
        }

        public FileStream Create(string path)
        {
            WriteAllBytes(path, new byte[] { });

#warning CreateFile returns FileStream null;
            return null;
        }

        public bool Exists(string path)
        {
            return MockFileSystem.Data.FileExits(path);
        }

        public void Move(string sourceFileName, string destFileName)
        {
            if (Exists(destFileName)) throw new IOException("File already exists " + destFileName);
            MockFileSystem.Data.WriteFile(destFileName, MockFileSystem.Data.ReadFile(sourceFileName));
            MockFileSystem.Data.DeleteFile(sourceFileName);
        }

        public FileStream Open(string path, FileMode mode, FileAccess access, FileShare share)
        {
            //TODO NotImplementedException FileStream Open(string path, FileMode mode, FileAccess access, FileShare share)
            throw new NotImplementedException("Not sure how to implement");
        }

        public byte[] ReadAllBytes(string path)
        {
            return MockFileSystem.Data.ReadFile(path);
        }

        public string[] ReadAllLines(string path, Encoding encoding)
        {
            return encoding.GetString(MockFileSystem.Data.ReadFile(path)).Split('\n').Select(p => p.Trim('\r')).ToArray();
        }

        public string ReadAllText(string path)
        {
            Warn.If(true, "Should not be colled, specifiy Encoding");
            return ReadAllText(path, DefaultEncoding);
        }

        public string ReadAllText(string path, Encoding encoding)
        {
            return encoding.GetString(MockFileSystem.Data.ReadFile(path));
        }

        public void WriteAllBytes(string path, byte[] bytes)
        {
            MockFileSystem.Data.WriteFile(path, bytes);
        }

        public void WriteAllLines(string path, string[] contents, Encoding encoding)
        {
            WriteAllText(path, string.Join(Environment.NewLine, contents), encoding);
        }

        public void WriteAllText(string path, string contents, Encoding encoding)
        {
            WriteAllBytes(path, encoding.GetBytes(contents));
        }

        public void Reset()
        {
            MockFileSystem.Data.Reset();
        }

        public void Delete(string path)
        {
            MockFileSystem.Data.DeleteFile(path);
        }

        public DateTime GetCreationTime(string path)
        {
            try
            {
                return MockFileSystem.Data.GetCreationTime(path);
            }
            catch (IOException)
            {
                throw new FileNotFoundException(path);
            }
        }
    }
}
