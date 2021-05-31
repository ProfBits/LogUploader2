using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LogUploader.Test.Mocks;

using NUnit.Framework;

[SetUpFixture]
public class TestSetup
{
    internal static MockFileSystem FileSystem { get; private set; }

    [OneTimeSetUp]
    public void GlobalSetup()
    {
        FileSystem = new MockFileSystem();
        var fileIO = new MockFileIO(FileSystem);
        MockFileIO.Instance = fileIO;
        LogUploader.Wrapper.FileIO.Backend = fileIO;
        var directoryIO = new MockDirectoryIO(FileSystem);
        MockDirectoryIO.Instance = directoryIO;
        LogUploader.Wrapper.DirectoryIO.Backend = directoryIO;

        LogUploader.Wrapper.WebIOFactory.Factory = MockWebIOFactory.Instance;

        CreateBasicFileSystemFake();
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
