using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using NUnit.Framework;

using Extensiones;
using Extensiones.Stream;
using System.IO;

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

        [Test]
        public async Task AsyncStreamCopyTest()
        {
            string str = "This string shall be copied.";
            Stream source = new MemoryStream(Encoding.ASCII.GetBytes(str));
            byte[] destBuff = new byte[str.Length];
            Stream dest = new MemoryStream(destBuff, true);
            await source.CopyToAsync(dest, 8, null, default);
            Assert.AreEqual(str, Encoding.ASCII.GetString(destBuff).Trim());
        }

        [Test]
        public async Task AsyncStreamCopyWithProgressTest()
        {
            string str = "This string shall be copied. and is very long -> aaabbbccceeedddfff1110002223334445566677788999";
            Stream source = new MemoryStream(Encoding.ASCII.GetBytes(str));
            byte[] destBuff = new byte[str.Length];
            Stream dest = new MemoryStream(destBuff, true);
            List<long> reports = new List<long>();
            await source.CopyToAsync(dest, 8, new SynchronProgress<long>(p => reports.Add(p)), default);
            Assert.AreEqual(str, Encoding.ASCII.GetString(destBuff).Trim());

            Assert.That(reports, Has.Count.GreaterThanOrEqualTo(3));
            CollectionAssert.IsOrdered(reports);
            Assert.AreEqual(reports.Min(), reports[0]);
        }

        [Test]
        public void AsyncStreamCopyInvalidArgumentsTest()
        {
            Stream source = new MemoryStream(Encoding.ASCII.GetBytes("Some string"));
            byte[] destBuff = new byte[11];
            Stream dest = new MemoryStream(destBuff, true);
            Stream readOnlyDest = new MemoryStream(destBuff, false);
            Assert.ThrowsAsync<ArgumentNullException>(() => ((Stream)null).CopyToAsync(dest, 8, null, default));
            Assert.ThrowsAsync<ArgumentNullException>(() => source.CopyToAsync(null, 8, null, default));
            Assert.ThrowsAsync<ArgumentException>(() => source.CopyToAsync(readOnlyDest, 8, null, default));
            Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => source.CopyToAsync(dest, 0, null, default));
            Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => source.CopyToAsync(dest, -1, null, default));
        }
    }

    
}
