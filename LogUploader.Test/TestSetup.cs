using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using LogUploader.Test.Mocks;

using NUnit.Framework;

[SetUpFixture]
public class TestSetup
{
    private const string TestAvatarUrl = "https://www.nonExistenDomain.fake/fake.png";

    internal static MockFileSystem FileSystem { get; private set; }

    [OneTimeSetUp]
    public void GlobalSetup()
    {
        var sw = new System.Diagnostics.Stopwatch();
        sw.Start();
        FileSystem = new MockFileSystem();
        var fileIO = new MockFileIO(FileSystem);
        MockFileIO.Instance = fileIO;
        LogUploader.Wrapper.FileIO.Backend = fileIO;
        var directoryIO = new MockDirectoryIO(FileSystem);
        MockDirectoryIO.Instance = directoryIO;
        LogUploader.Wrapper.DirectoryIO.Backend = directoryIO;

        LogUploader.Wrapper.WebIOFactory.Factory = MockWebIOFactory.Instance;

        CreateBasicFileSystemFake();

        InsertStaticData();
        sw.Stop();
        TestContext.WriteLine($"GlobalTestSetup done in {Math.Round(sw.Elapsed.TotalMilliseconds)} ms");
    }

    private void InsertStaticData()
    {
        string relPathBoss = GetPathToTestFiles("bossData_0621.json");
        string contentBoss = System.IO.File.ReadAllText(relPathBoss, Encoding.GetEncoding("iso-8859-1"));
        LogUploader.Data.StaticDataLoader.DataBuilder.LoadDataJson(contentBoss);

        string relPathProf = GetPathToTestFiles("profData_0721.json");
        string contentProf = System.IO.File.ReadAllText(relPathProf, Encoding.GetEncoding("iso-8859-1"));
        LogUploader.Data.Profession.InitWithContent(contentProf);
        
    }

    private static string GetPathToTestFiles(string fileName)
    {
        return GetPathToTestFiles("static", fileName);
    }

    public static string GetPathToTestFiles(string folder, string file)
    {
        var assamblyPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
        var relPath = "TestFiles" + System.IO.Path.DirectorySeparatorChar + folder + System.IO.Path.DirectorySeparatorChar + file;
        return assamblyPath + System.IO.Path.DirectorySeparatorChar + relPath;
    }

    private void CreateBasicFileSystemFake()
    {
        //TODO CreateBasicFileSystemFake()
        //throw new NotImplementedException();
    }

    [OneTimeTearDown]
    public void GlobalTeardown()
    {
        FileSystem.Reset();
    }
}
