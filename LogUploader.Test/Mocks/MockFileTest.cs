using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NUnit.Framework;

namespace LogUploader.Test.Mocks
{
    public class MockFileTest
    {
        private const string TEST_BASE_FOLDER = @"C:\UnitTest\FileIO\";
        private MockFileSystem TestFileSystem;
        private Wrapper.IFileIO FileIO;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            TestFileSystem = new MockFileSystem();
            FileIO = new MockFileIO(TestFileSystem);
        }

        [SetUp]
        public void SetUp()
        {
            TestFileSystem.Reset();
        }

        [Test]
        public void Create()
        {
            const string fileName = TEST_BASE_FOLDER + @"Create.txt";
            Assert.DoesNotThrow(() => FileIO.Create(fileName));
            Assert.IsTrue(FileIO.Exists(fileName));
        }

        [Test]
        public void Reset()
        {
            const string fileName = TEST_BASE_FOLDER + @"Reset.txt";

            FileIO.Create(fileName);
            TestFileSystem.Reset();
            Assert.IsFalse(FileIO.Exists(fileName));

            FileIO.Create(fileName);
            (FileIO as MockFileIO).Reset();
            Assert.IsFalse(FileIO.Exists(fileName));
        }

        [Test]
        public void CreateAndRead()
        {
            const string fileName = TEST_BASE_FOLDER + @"CreateAndRead.txt";
            const string content = @"Hello World";
            FileIO.WriteAllText(fileName, content, Encoding.UTF8);
            Assert.IsTrue(FileIO.Exists(fileName));
            Assert.AreEqual(content, FileIO.ReadAllText(fileName, Encoding.UTF8));
        }

        [Test]
        public void CreateAndReadBinary()
        {
            const string fileName = TEST_BASE_FOLDER + @"CreateAndReadBinary.bin";
            byte[] content = new byte[] { 42, 0, 3, 8 };
            FileIO.WriteAllBytes(fileName, content);
            Assert.IsTrue(FileIO.Exists(fileName));
            Assert.IsTrue(content.SequenceEqual(FileIO.ReadAllBytes(fileName)));
        }

        [Test]
        public void CreateAppendAndRead()
        {
            const string fileName = TEST_BASE_FOLDER + @"CreateAppendAndRead.txt";
            const string content = @"Hello World";
            FileIO.Create(fileName);
            FileIO.AppendAllText(fileName, content, Encoding.UTF8);
            Assert.IsTrue(FileIO.Exists(fileName));
            Assert.AreEqual(content, FileIO.ReadAllText(fileName, Encoding.UTF8));
        }

        [Test]
        public void AppendAndRead()
        {
            const string fileName = TEST_BASE_FOLDER + @"AppendAndRead.txt";
            const string content = @"Hello World";
            FileIO.AppendAllText(fileName, content, Encoding.UTF8);
            Assert.IsTrue(FileIO.Exists(fileName));
            Assert.AreEqual(content, FileIO.ReadAllText(fileName, Encoding.UTF8));
        }

        [Test]
        public void CreateAppendAndRead2()
        {
            const string fileName = TEST_BASE_FOLDER + @"CreateAppendAndRead2.txt";
            const string content = @"Hello World";
            FileIO.WriteAllText(fileName, content, Encoding.UTF8);
            FileIO.AppendAllText(fileName, content, Encoding.UTF8);
            Assert.IsTrue(FileIO.Exists(fileName));
            Assert.AreEqual(content + content, FileIO.ReadAllText(fileName, Encoding.UTF8));
        }

        [Test]
        public void CreateLinesAndRead()
        {
            const string fileName = TEST_BASE_FOLDER + @"CreateLinesAndRead\secret.txt";
            const string content = @"Hello World";
            FileIO.WriteAllLines(fileName, new string[] { content, content, content }, Encoding.UTF8);
            Assert.IsTrue(FileIO.Exists(fileName));
            Assert.IsTrue((new string[] { content, content, content }).SequenceEqual(FileIO.ReadAllLines(fileName, Encoding.UTF8)));
        }

        [Test]
        public void Move()
        {
            const string fileNameA = TEST_BASE_FOLDER + @"Move\from\a.txt";
            const string fileNameB = TEST_BASE_FOLDER + @"Move\to\b.txt";
            const string content = @"Hello World";
            FileIO.WriteAllText(fileNameA, content, Encoding.UTF8);
            Assert.IsTrue(FileIO.Exists(fileNameA));
            Assert.IsFalse(FileIO.Exists(fileNameB));
            FileIO.Move(fileNameA, fileNameB);
            Assert.IsFalse(FileIO.Exists(fileNameA));
            Assert.IsTrue(FileIO.Exists(fileNameB));
            Assert.AreEqual(content, FileIO.ReadAllText(fileNameB, Encoding.UTF8));
        }

        [Test]
        public void MoveError()
        {
            const string fileNameA = TEST_BASE_FOLDER + @"MoveError\from\a.txt";
            const string fileNameB = TEST_BASE_FOLDER + @"MoveError\to\b.txt";
            const string contentA = @"Hello World A";
            const string contentB = @"Hello World B";
            FileIO.WriteAllText(fileNameA, contentA, Encoding.UTF8);
            FileIO.WriteAllText(fileNameB, contentB, Encoding.UTF8);
            Assert.Catch<System.IO.IOException>(() => FileIO.Move(fileNameA, fileNameB));
            Assert.IsTrue(FileIO.Exists(fileNameA));
            Assert.IsTrue(FileIO.Exists(fileNameB));
            Assert.AreEqual(contentA, FileIO.ReadAllText(fileNameA, Encoding.UTF8));
            Assert.AreEqual(contentB, FileIO.ReadAllText(fileNameB, Encoding.UTF8));
        }

        [Test]
        public void Copy()
        {
            const string fileNameA = TEST_BASE_FOLDER + @"Copy\from\a.txt";
            const string fileNameB = TEST_BASE_FOLDER + @"Copy\to\b.txt";
            const string content = @"Hello World";
            FileIO.WriteAllText(fileNameA, content, Encoding.UTF8);
            Assert.IsTrue(FileIO.Exists(fileNameA));
            Assert.IsFalse(FileIO.Exists(fileNameB));
            FileIO.Copy(fileNameA, fileNameB, true);
            Assert.IsTrue(FileIO.Exists(fileNameA));
            Assert.IsTrue(FileIO.Exists(fileNameB));
            Assert.AreEqual(content, FileIO.ReadAllText(fileNameA, Encoding.UTF8));
            Assert.AreEqual(content, FileIO.ReadAllText(fileNameB, Encoding.UTF8));
        }

        [Test]
        public void CopyNoOverrideError()
        {
            const string fileNameA = TEST_BASE_FOLDER + @"CopyNoOverrideError\from\a.txt";
            const string fileNameB = TEST_BASE_FOLDER + @"CopyNoOverrideError\to\b.txt";
            const string content = @"Hello World";
            FileIO.WriteAllText(fileNameA, content, Encoding.UTF8);
            FileIO.Create(fileNameB);
            Assert.Catch<System.IO.IOException>(() => FileIO.Copy(fileNameA, fileNameB, false));
            Assert.IsTrue(FileIO.Exists(fileNameA));
            Assert.IsTrue(FileIO.Exists(fileNameB));
            Assert.AreEqual(content, FileIO.ReadAllText(fileNameA, Encoding.UTF8));
            Assert.AreEqual("", FileIO.ReadAllText(fileNameB, Encoding.UTF8));
        }

        [Test]
        public void CopyNoOverrideNoError()
        {
            const string fileNameA = TEST_BASE_FOLDER + @"CopyNoOverrideNoError\from\a.txt";
            const string fileNameB = TEST_BASE_FOLDER + @"CopyNoOverrideNoError\to\b.txt";
            const string content = @"Hello World";
            FileIO.WriteAllText(fileNameA, content, Encoding.UTF8);
            Assert.DoesNotThrow(() => FileIO.Copy(fileNameA, fileNameB, false));
            Assert.IsTrue(FileIO.Exists(fileNameA));
            Assert.IsTrue(FileIO.Exists(fileNameB));
            Assert.AreEqual(content, FileIO.ReadAllText(fileNameA, Encoding.UTF8));
            Assert.AreEqual(content, FileIO.ReadAllText(fileNameA, Encoding.UTF8));
        }
        [Test]
        public void CopyOverride()
        {
            const string fileNameA = TEST_BASE_FOLDER + @"CopyOverride\from\a.txt";
            const string fileNameB = TEST_BASE_FOLDER + @"CopyOverride\to\b.txt";
            const string content = @"Hello World";
            FileIO.WriteAllText(fileNameA, content, Encoding.UTF8);
            FileIO.WriteAllText(fileNameB, "old", Encoding.UTF8);
            Assert.DoesNotThrow(() => FileIO.Copy(fileNameA, fileNameB, true));
            Assert.IsTrue(FileIO.Exists(fileNameA));
            Assert.IsTrue(FileIO.Exists(fileNameB));
            Assert.AreEqual(content, FileIO.ReadAllText(fileNameA, Encoding.UTF8));
            Assert.AreEqual(content, FileIO.ReadAllText(fileNameA, Encoding.UTF8));
        }

        [Test]
        public void WriteOverrides()
        {
            const string fileNameA = TEST_BASE_FOLDER + @"write\WriteOverrides.txt";
            const string content = @"Hello World";
            FileIO.WriteAllText(fileNameA, "old", Encoding.UTF8);
            FileIO.WriteAllText(fileNameA, content, Encoding.UTF8);
            Assert.IsTrue(FileIO.Exists(fileNameA));
            Assert.AreEqual(content, FileIO.ReadAllText(fileNameA, Encoding.UTF8));
        }

        [Test]
        public void WriteLinesOverrides()
        {
            const string fileNameA = TEST_BASE_FOLDER + @"write\WriteLinesOverrides.txt";
            const string content = @"Hello World";
            FileIO.WriteAllLines(fileNameA, new string[] { "old", "", "bla" }, Encoding.UTF8);
            FileIO.WriteAllLines(fileNameA, new string[] { content, content, content }, Encoding.UTF8);
            Assert.IsTrue(FileIO.Exists(fileNameA));
            Assert.IsTrue((new string[] { content, content, content }).SequenceEqual(FileIO.ReadAllLines(fileNameA, Encoding.UTF8)));
        }

        [Test]
        public void WriteBinaryOverrides()
        {
            const string fileName = TEST_BASE_FOLDER + @"WriteBinaryOverrides\data2.bin";
            byte[] content = new byte[] { 42, 0, 3, 8 };
            byte[] contentNew = new byte[] { 0, 2, 3, 69, 54 };
            FileIO.WriteAllBytes(fileName, content);
            FileIO.WriteAllBytes(fileName, contentNew);
            Assert.IsTrue(FileIO.Exists(fileName));
            Assert.IsTrue(contentNew.SequenceEqual(FileIO.ReadAllBytes(fileName)));
        }

        [Test]
        public void WriteLinesSupportsEmptyLines()
        {
            const string fileNameA = TEST_BASE_FOLDER + @"write\WriteLinesSupportsEmptyLines.txt";
            string[] content = new string[] { "", "old", "", "old", "", "", "", "bla", "" };
            FileIO.WriteAllLines(fileNameA, content, Encoding.UTF8);
            Assert.IsTrue(FileIO.Exists(fileNameA));
            Assert.IsTrue(content.SequenceEqual(FileIO.ReadAllLines(fileNameA, Encoding.UTF8)));
        }

        [Test]
        public void ReadFileNotFound()
        {
            const string fileName = TEST_BASE_FOLDER + @"nonExitent\ReadFileNotFound\virtual.dat";
            Assert.Catch<System.IO.FileNotFoundException>(() => FileIO.ReadAllText(fileName));
            Assert.Catch<System.IO.FileNotFoundException>(() => FileIO.ReadAllText(fileName, Encoding.UTF8));
        }

        [Test]
        public void ReadLinesFileNotFound()
        {
            const string fileName = TEST_BASE_FOLDER + @"nonExitent\ReadLinesFileNotFound\virtual.dat";
            Assert.Catch<System.IO.FileNotFoundException>(() => FileIO.ReadAllLines(fileName, Encoding.UTF8));
        }

        [Test]
        public void ReadBytesFileNotFound()
        {
            const string fileName = TEST_BASE_FOLDER + @"nonExitent\ReadBytesFileNotFound\virtual.dat";
            Assert.Catch<System.IO.FileNotFoundException>(() => FileIO.ReadAllBytes(fileName));
        }

        [Test]
        public void CopyFileNotFoud()
        {
            const string fileNameA = TEST_BASE_FOLDER + @"nonExitent\CopyFileNotFoud\virtual.dat";
            const string fileNameB = TEST_BASE_FOLDER + @"nonExitent\CopyFileNotFoud\virtual2.dat";
            Assert.Catch<System.IO.FileNotFoundException>(() => FileIO.Copy(fileNameA, fileNameB, true));
            Assert.Catch<System.IO.FileNotFoundException>(() => FileIO.Copy(fileNameA, fileNameB, false));
        }

        [Test]
        public void MoveFileNotFoud()
        {
            const string fileNameA = TEST_BASE_FOLDER + @"nonExitent\MoveFileNotFoud\virtual.dat";
            const string fileNameB = TEST_BASE_FOLDER + @"nonExitent\MoveFileNotFoud\virtual2.dat";
            Assert.Catch<System.IO.FileNotFoundException>(() => FileIO.Move(fileNameA, fileNameB));
        }

        [Test]
        public void DeleteFile()
        {
            const string fileName = TEST_BASE_FOLDER + @"DeleteFile.txt";
            FileIO.Create(fileName);
            Assert.DoesNotThrow(() => FileIO.Delete(fileName));
        }

        [Test]
        public void DeleteNonExistentFile()
        {
            const string fileName = TEST_BASE_FOLDER + @"DeleteNonExistentFile.txt";
            Assert.Throws<System.IO.FileNotFoundException>(() => FileIO.Delete(fileName));
        }

        [Test]
        public void GetCreationTime()
        {
            const string fileName = TEST_BASE_FOLDER + @"GetCreationTime.txt";
            FileIO.Create(fileName);
            var now = DateTime.Now;
            var delta = FileIO.GetCreationTime(fileName).Subtract(now);
            Assert.LessOrEqual(Math.Abs(delta.Seconds), 3); //TODO Mock time provider, make tests repoduzeable
        }

        [Test]
        public void GetCreationTimeNonExistentFile()
        {
            const string fileName = TEST_BASE_FOLDER + @"GetCreationTimeNonExistentFile.txt";
            Assert.Throws<System.IO.FileNotFoundException>(() => FileIO.GetCreationTime(fileName));
        }


        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            TestFileSystem.Reset();
        }

    }
}
