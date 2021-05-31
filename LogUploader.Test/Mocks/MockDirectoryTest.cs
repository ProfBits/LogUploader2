using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NUnit.Framework;

namespace LogUploader.Test.Mocks
{
    public class MockDirectoryTest
    {
        private MockFileSystem TestFileSystem;
        private Wrapper.IDirectoryIO DirectoryIO;
        private Wrapper.IFileIO FileIO;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            TestFileSystem = new MockFileSystem();
            DirectoryIO = new MockDirectoryIO(TestFileSystem);
            FileIO = new MockFileIO(TestFileSystem);
        }

        [SetUp]
        public void SetUp()
        {
            TestFileSystem.Reset();
        }

        [Test]
        public void CreateDirecotry()
        {
            const string path = @"C:\\UnitTest\";
            Assert.DoesNotThrow(() => DirectoryIO.CreateDirectory(path));
            Assert.IsTrue(DirectoryIO.Exists(path));
            const string path2 = @"C:\\UnitTest\sub\sub\suub\dir\";
            Assert.DoesNotThrow(() => DirectoryIO.CreateDirectory(path2));
            Assert.IsTrue(DirectoryIO.Exists(path2));
        }

        [Test]
        public void DirecotryExits()
        {
            const string path = @"C:\\UnitTest\DirecotryExits\";
            DirectoryIO.CreateDirectory(path);
            Assert.IsTrue(DirectoryIO.Exists(path));
        }

        [Test]
        public void DirecotryExitsNot()
        {
            const string path = @"C:\\UnitTest\DirecotryExits\";
            const string path2 = @"C:\\UnitTest\DirecotryExitsNot\";
            DirectoryIO.CreateDirectory(path);
            Assert.IsFalse(DirectoryIO.Exists(path2));
        }

        [Test]
        public void DirecotryExitsEmpty()
        {
            const string path = @"C:\\UnitTest\DirecotryExitsEmpty\";
            Assert.IsFalse(DirectoryIO.Exists(path));
        }

        [Test]
        public void DirecotryOnPathAreCreated()
        {
            const string root = @"C:\\";
            const string f1 = root + @"UnitTest\";
            const string f2 = f1 + @"DirecotryExitsEmpty\";
            const string f3 = f2 + @"more path\";
            DirectoryIO.CreateDirectory(f3);
            Assert.IsTrue(DirectoryIO.Exists(f3));
            Assert.IsTrue(DirectoryIO.Exists(f2));
            Assert.IsTrue(DirectoryIO.Exists(f1));
        }

        [Test]
        public void Reset()
        {
            const string path = @"C:\\UnitTest\Reset\";
            DirectoryIO.CreateDirectory(path);
            Assert.IsTrue(DirectoryIO.Exists(path));
            TestFileSystem.Reset();
            Assert.IsFalse(DirectoryIO.Exists(path));
        }

        [Test]
        public void CreateDuplicateDirecotry()
        {
            const string path = @"C:\\UnitTest\CreateDuplicateDirecotry\";
            DirectoryIO.CreateDirectory(path);
            Assert.IsTrue(DirectoryIO.Exists(path));
            DirectoryIO.CreateDirectory(path);
            Assert.IsTrue(DirectoryIO.Exists(path));
        }

        [Test]
        public void DeleteDirecotry()
        {
            const string path = @"C:\\UnitTest\";
            const string folder = path + @"DeleteDirecotry\";
            DirectoryIO.CreateDirectory(folder);
            Assert.IsTrue(DirectoryIO.Exists(folder));
            DirectoryIO.Delete(folder, false);
            Assert.IsFalse(DirectoryIO.Exists(folder));
            Assert.IsTrue(DirectoryIO.Exists(path));
        }

        [Test]
        public void NotDeleteDirecotryWithContent()
        {
            const string path = @"C:\\UnitTest\NotDeleteDirecotryWithContent\";
            const string file = path + @"HelloWorld.txt";
            DirectoryIO.CreateDirectory(path);
            FileIO.Create(file);
            Assert.Catch<IOException>(() => DirectoryIO.Delete(path, false));
            Assert.IsTrue(DirectoryIO.Exists(path));
            Assert.IsTrue(FileIO.Exists(file));
        }

        [Test]
        public void NotDeleteDirecotryWithSubFolder()
        {
            const string path = @"C:\\UnitTest\NotDeleteDirecotryWithSubFolder\";
            const string dir = path + @"HelloWorld\";
            DirectoryIO.CreateDirectory(path);
            DirectoryIO.CreateDirectory(dir);
            Assert.Catch<IOException>(() => DirectoryIO.Delete(path, false));
            Assert.IsTrue(DirectoryIO.Exists(path));
            Assert.IsTrue(DirectoryIO.Exists(dir));
        }

        [Test]
        public void DeleteDirecotryWithContent()
        {
            const string path = @"C:\\UnitTest\DeleteDirecotryWithContent\";
            const string file = path + @"HelloWorld.txt";
            DirectoryIO.CreateDirectory(path);
            FileIO.Create(file);
            Assert.DoesNotThrow(() => DirectoryIO.Delete(path, true));
            Assert.IsFalse(DirectoryIO.Exists(path));
            Assert.IsFalse(FileIO.Exists(file));
        }

        [Test]
        public void DeleteDirecotryWithSubFolder()
        {
            const string path = @"C:\\UnitTest\DeleteDirecotryWithSubFolder\";
            const string dir = path + @"HelloWorld\";
            DirectoryIO.CreateDirectory(path);
            DirectoryIO.CreateDirectory(dir);
            Assert.DoesNotThrow(() => DirectoryIO.Delete(path, true));
            Assert.IsFalse(DirectoryIO.Exists(path));
            Assert.IsFalse(DirectoryIO.Exists(dir));
        }

        [Test]
        public void DeleteNoneExistentDirecotry()
        {
            const string path = @"C:\\UnitTest\DeleteNoneExistentDirecotry\";
            Assert.Catch<DirectoryNotFoundException>(() => DirectoryIO.Delete(path, false));
            Assert.Catch<DirectoryNotFoundException>(() => DirectoryIO.Delete(path, true));
        }

        [Test]
        public void GetFilesOnlyFiles()
        {
            const string path = @"C:\\UnitTest\DeleteDirecotryWithContent\";
            const string fileA = path + @"HelloWorld.txt";
            const string fileB = path + @"secret.txt";
            DirectoryIO.CreateDirectory(path);
            FileIO.Create(fileA);
            FileIO.Create(fileB);
            var data = DirectoryIO.GetFiles(path);
            Assert.AreEqual(2, data.Length);
            Assert.Contains(fileA, data);
            Assert.Contains(fileB, data);
        }

        [Test]
        public void GetFilesOnlyEmptySubFolders()
        {
            const string path = @"C:\\UnitTest\DeleteDirecotryWithContent\";
            const string subFolder = path + @"sub\";
            DirectoryIO.CreateDirectory(subFolder);
            var data = DirectoryIO.GetFiles(path);
            Assert.AreEqual(0, data.Length);
        }

        [Test]
        public void GetFilesEmptySubFoldersAndFiles()
        {
            const string path = @"C:\\UnitTest\DeleteDirecotryWithContent\";
            const string subFolder = path + @"sub\";
            const string fileA = path + @"HelloWorld.txt";
            const string fileB = path + @"secret.dat";
            DirectoryIO.CreateDirectory(subFolder);
            FileIO.Create(fileA);
            FileIO.Create(fileB);
            var data = DirectoryIO.GetFiles(path);
            Assert.AreEqual(2, data.Length);
            Assert.Contains(fileA, data);
            Assert.Contains(fileB, data);
        }

        [Test]
        public void GetFilesSubFoldersAndFiles()
        {
            const string path = @"C:\\UnitTest\DeleteDirecotryWithContent\";
            const string subFolder = path + @"sub\";
            const string fileA = path + @"HelloWorld.txt";
            const string fileB = subFolder + @"secret.txt";
            DirectoryIO.CreateDirectory(subFolder);
            FileIO.Create(fileA);
            FileIO.Create(fileB);
            var data = DirectoryIO.GetFiles(path);
            Assert.AreEqual(1, data.Length);
            Assert.Contains(fileA, data);
        }

        [Test]
        public void GetFilesOnlySubFolders()
        {
            const string path = @"C:\\UnitTest\DeleteDirecotryWithContent\";
            const string subFolder = path + @"sub\";
            const string fileA = subFolder + @"HelloWorld.txt";
            const string fileB = subFolder + @"secret.txt";
            DirectoryIO.CreateDirectory(subFolder);
            FileIO.Create(fileA);
            FileIO.Create(fileB);
            var data = DirectoryIO.GetFiles(path);
            Assert.AreEqual(0, data.Length);
        }

        [Test]
        public void GetAllFilesWithPattern()
        {
            const string path = @"C:\\UnitTest\DeleteDirecotryWithContent\";
            const string subFolder = path + @"sub\";
            const string fileA = path + @"HelloWorld.txt";
            const string fileB = path + @"secret.dat";
            const string fileC = subFolder + @"HelloWorld.txt";
            const string fileD = subFolder + @"secret.dat";
            DirectoryIO.CreateDirectory(subFolder);
            FileIO.Create(fileA);
            FileIO.Create(fileB);
            FileIO.Create(fileC);
            FileIO.Create(fileD);
            var data = DirectoryIO.GetFiles(path, "*.*", SearchOption.AllDirectories);
            Assert.AreEqual(4, data.Length);
            Assert.Contains(fileA, data);
            Assert.Contains(fileB, data);
            Assert.Contains(fileC, data);
            Assert.Contains(fileD, data);
        }

        [Test]
        public void GetFilesDataTypePattern()
        {
            const string path = @"C:\\UnitTest\DeleteDirecotryWithContent\";
            const string subFolder = path + @"sub\";
            const string fileA = path + @"HelloWorld.txt";
            const string fileB = path + @"secret.dat";
            const string fileC = subFolder + @"HelloWorld.txt";
            const string fileD = subFolder + @"secret.dat";
            DirectoryIO.CreateDirectory(subFolder);
            FileIO.Create(fileA);
            FileIO.Create(fileB);
            FileIO.Create(fileC);
            FileIO.Create(fileD);
            var data = DirectoryIO.GetFiles(path, "*.txt", SearchOption.AllDirectories);
            Assert.AreEqual(2, data.Length);
            Assert.Contains(fileA, data);
            Assert.Contains(fileC, data);
        }

        [Test]
        public void MoveDir()
        {
            const string path = @"C:\\UnitTest\MoveDir\";
            const string from = path + @"moveFrom\";
            const string to = path + @"moveTo\";
            const string file0 = path + @"Basic.txt";
            const string fileA = from + @"HelloWorld.txt";
            const string fileB = from + @"secret.dat";
            const string newFileA = to + @"moveFrom\" + @"HelloWorld.txt";
            const string newFileB = to + @"moveFrom\" + @"secret.dat";
            DirectoryIO.CreateDirectory(from);
            DirectoryIO.CreateDirectory(to);
            FileIO.WriteAllText(file0, path, Encoding.UTF8);
            FileIO.WriteAllText(fileA, to, Encoding.UTF8);
            FileIO.Create(fileB);
            Assert.DoesNotThrow(() => DirectoryIO.Move(from, to));
            Assert.IsFalse(FileIO.Exists(fileA));
            Assert.IsFalse(FileIO.Exists(fileB));
            Assert.IsTrue(FileIO.Exists(file0));
            Assert.IsTrue(FileIO.Exists(newFileA));
            Assert.IsTrue(FileIO.Exists(newFileB));
            Assert.AreEqual(path, FileIO.ReadAllText(file0, Encoding.UTF8));
            Assert.AreEqual(to, FileIO.ReadAllText(newFileA, Encoding.UTF8));
        }

        [Test]
        public void MoveNoSource()
        {
            const string path = @"C:\\UnitTest\MoveDir\";
            const string from = path + @"moveFrom\";
            const string to = path + @"moveTo\";
            DirectoryIO.CreateDirectory(to);
            Assert.Throws<DirectoryNotFoundException>(() => DirectoryIO.Move(from, to));
        }

        [Test]
        public void MoveNoDest()
        {
            const string path = @"C:\\UnitTest\MoveDir\";
            const string from = path + @"moveFrom\";
            const string to = path + @"moveTo\";
            DirectoryIO.CreateDirectory(from);
            Assert.Throws<DirectoryNotFoundException>(() => DirectoryIO.Move(from, to));
        }

        [Test]
        public void MoveNoSourceAndDest()
        {
            const string path = @"C:\\UnitTest\MoveDir\";
            const string from = path + @"moveFrom\";
            const string to = path + @"moveTo\";
            DirectoryIO.CreateDirectory(path);
            Assert.Throws<DirectoryNotFoundException>(() => DirectoryIO.Move(from, to));
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            TestFileSystem.Reset();
        }

    }
}
