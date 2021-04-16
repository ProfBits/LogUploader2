using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogUploader.Test.Mocks
{
    internal static class MockSetup
    {
        internal static void InjectMoks()
        {
            Wrapper.FileIO.Backend = MockFileIO.Instance;
            Wrapper.DirectoryIO.Backend = MockDirectroyIO.Instance;
        }
    }
}
