using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NUnit.Framework;

using Extensiones;

namespace LogUploader.Test.Interfaces
{
    public class ExtensionsTests
    {
        [Test]
        public void GetAttributeExtensionTest()
        {
            ExtensionAttriAttribute attri = TestEnum.ValA.GetAttribute<ExtensionAttriAttribute>();
            Assert.NotNull(attri);
            Assert.AreEqual("ValA", attri.Data);
        }

        [Test]
        public void GetAttributeExtensionFailTest()
        {
            ExtensionAttriAttribute attri = TestEnum.ValB.GetAttribute<ExtensionAttriAttribute>();
            Assert.IsNull(attri);
        }

        private class ExtensionAttriAttribute : Attribute
        {
            public ExtensionAttriAttribute(string data)
            {
                Data = data;
            }

            public string Data { get; }

        }

        private enum TestEnum
        {
            [ExtensionAttri("ValA")]
            ValA,
            ValB
        }

    }

    
}
