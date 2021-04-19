using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NUnit.Framework;

namespace LogUploader.Test.Mocks
{
    public class MockWebTest
    {

        private Wrapper.IWebIO WebIO;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            WebIO = MockWebIO.Instance;
        }

        [SetUp]
        public void SetUp()
        {
            MockWebIO.Instance.Reset();
        }

        [Test]
        public void Reset()
        {
            const string reqUri = "https://test.com/unitTest";
            var resGen = new StaticWebResponseGenerator();
            resGen.GetResponseMsg = "Hello World";
            MockWebIO.Data.Add(reqUri, resGen);
            MockWebIO.Instance.Reset();
            var aggregat = Assert.Catch<AggregateException>(() =>
            {
                var task = WebIO.SendAsync(new System.Net.Http.HttpRequestMessage(System.Net.Http.HttpMethod.Get, reqUri), default);
                task.Wait();
            });
            Assert.IsTrue(aggregat.InnerExceptions.Any(e => e is ArgumentException));
        }

        [Test]
        public void Request()
        {
            const string reqUri = "https://test.com/unitTest";
            var resGen = new StaticWebResponseGenerator();
            resGen.GetResponseMsg = "Hello World";
            MockWebIO.Data.Add(reqUri, resGen);
            var req = new System.Net.Http.HttpRequestMessage(System.Net.Http.HttpMethod.Get, reqUri);

            var answer = WebIO.SendAsync(req, default).Result;

            Assert.AreEqual(resGen.GetResponseMsg, answer.Content.ReadAsStringAsync().Result);
        }

        [Test]
        public void RequestWithPort()
        {
            Assert.Warn("Not implemented");
        }

        [Test]
        public void RequestWithQuery()
        {
            Assert.Warn("Not implemented");
        }

        [Test]
        public void RequestWithPortAnQuery()
        {
            Assert.Warn("Not implemented");
        }

        [Test]
        public void RequestDifferentUris()
        {
            Assert.Warn("Not implemented");
        }

        [Test]
        public void RequestDifferentEndpoints()
        {
            Assert.Warn("Not implemented");
        }

        [Test]
        public void RequestMessagePassThrough()
        {
            Assert.Warn("Not implemented");
        }

    }
}
