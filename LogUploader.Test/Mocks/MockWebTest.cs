using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
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
            var req = new HttpRequestMessage(HttpMethod.Get, reqUri);

            var answer = WebIO.SendAsync(req, default).Result;

            Assert.AreEqual(resGen.GetResponseMsg, answer.Content.ReadAsStringAsync().Result);
        }

        [Test]
        public void RequestWithPort()
        {
            const string answer = "Hello World from Port";
            const string answerNoPort = "Hello World without Port";
            const string reqUri = "https://test.com:80/unitTest";
            const string reqUriNoPort = "https://test.com/unitTest";
            var resGen = new StaticWebResponseGenerator();
            var resGenNoPort = new StaticWebResponseGenerator();
            resGen.GetResponseMsg = answer;
            resGenNoPort.GetResponseMsg = answerNoPort;
            MockWebIO.Data.Add(reqUri, resGen);
            MockWebIO.Data.Add(reqUriNoPort, resGenNoPort);
            var req = new HttpRequestMessage(HttpMethod.Get, reqUri);
            var reqNoPort = new HttpRequestMessage(HttpMethod.Get, reqUriNoPort);

            var resp = WebIO.SendAsync(req, default).Result;
            var respNoPort = WebIO.SendAsync(reqNoPort, default).Result;

            Assert.AreEqual(answer, resp.Content.ReadAsStringAsync().Result);
            Assert.AreEqual(answerNoPort, respNoPort.Content.ReadAsStringAsync().Result);
        }

        [Test]
        public void RequestWithQuery()
        {
            const string query = "?filter=result";
            const string answer = "Hello World without Query";
            const string answerQuery = "Hello World with Query";
            const string reqUri = "https://test.com/unitTest";
            const string reqUriQuery = reqUri + query;
            var resGen = new StaticWebResponseGenerator();
            var resGenQuery = new StaticWebResponseGenerator();
            resGen.GetResponseMsg = answer;
            resGenQuery.GetResponseMsg = answerQuery;
            MockWebIO.Data.Add(reqUri, resGen);
            MockWebIO.Data.Add(reqUriQuery, resGenQuery);
            var req = new HttpRequestMessage(HttpMethod.Get, reqUri);
            var reqQuery = new HttpRequestMessage(HttpMethod.Get, reqUriQuery);

            var resp = WebIO.SendAsync(req, default).Result;
            var respQuery = WebIO.SendAsync(reqQuery, default).Result;

            Assert.AreEqual(answer, resp.Content.ReadAsStringAsync().Result);
            Assert.AreEqual(answer, respQuery.Content.ReadAsStringAsync().Result);
            Assert.AreEqual("", resp.RequestMessage.RequestUri.Query);
            Assert.AreEqual(query, respQuery.RequestMessage.RequestUri.Query);
        }

        [Test]
        public void RequestWithPortAnQuery()
        {
            const string answerNoPortNoQuery = "answerNoPortNoQuery";
            const string answerPortNoQuery = "answerPortNoQuery";
            const string answerNoPortQuery = "answerNoPortQuery";
            const string answerPortQuery = "answerPortQuery";
            const string host = "https://test.com";
            const string port = ":1234";
            const string endpoint = "/unitTest";
            const string query = "?filter=query";
            const string UriNoPortNoQuery = host + endpoint;
            const string UriPortNoQuery = host + port + endpoint;
            const string UriNoPortQuery = host + endpoint + query;
            const string UriPortQuery = host + port + endpoint + query;
            var resGenNoPortNoQuery = new StaticWebResponseGenerator();
            var resGenPortNoQuery = new StaticWebResponseGenerator();
            var resGenNoPortQuery = new StaticWebResponseGenerator();
            var resGenPortQuery = new StaticWebResponseGenerator();
            resGenNoPortNoQuery.GetResponseMsg = answerNoPortNoQuery;
            resGenPortNoQuery.GetResponseMsg = answerPortNoQuery;
            resGenNoPortQuery.GetResponseMsg = answerNoPortQuery;
            resGenPortQuery.GetResponseMsg = answerPortQuery;
            MockWebIO.Data.Add(UriNoPortNoQuery, resGenNoPortNoQuery);
            MockWebIO.Data.Add(UriPortNoQuery, resGenPortNoQuery);
            MockWebIO.Data.Add(UriNoPortQuery, resGenNoPortQuery);
            MockWebIO.Data.Add(UriPortQuery, resGenPortQuery);
            var reqNoPortNoQuery = new HttpRequestMessage(HttpMethod.Get, UriNoPortNoQuery);
            var reqPortNoQuery = new HttpRequestMessage(HttpMethod.Get, UriPortNoQuery);
            var reqNoPortQuery = new HttpRequestMessage(HttpMethod.Get, UriNoPortQuery);
            var reqPortQuery = new HttpRequestMessage(HttpMethod.Get, UriPortQuery);

            var respNoPortNoQuery = WebIO.SendAsync(reqNoPortNoQuery, default).Result;
            var respPortNoQuery = WebIO.SendAsync(reqPortNoQuery, default).Result;
            var respNoPortQuery = WebIO.SendAsync(reqNoPortQuery, default).Result;
            var respPortQuery = WebIO.SendAsync(reqPortQuery, default).Result;

            Assert.AreEqual(answerNoPortNoQuery, respNoPortNoQuery.Content.ReadAsStringAsync().Result);
            Assert.AreEqual(answerPortNoQuery, respPortNoQuery.Content.ReadAsStringAsync().Result);
            Assert.AreEqual(answerNoPortNoQuery, respNoPortQuery.Content.ReadAsStringAsync().Result);
            Assert.AreEqual(answerPortNoQuery, respPortQuery.Content.ReadAsStringAsync().Result);
        }

        [Test]
        public void RequestDifferentUris()
        {
            const string answer = "Hello World from a.com";
            const string answer2 = "Hello World from b.com";
            const string reqUri = "https://a.com/unitTest";
            const string reqUri2 = "https://b.com/unitTest";
            var resGen = new StaticWebResponseGenerator();
            var resGen2 = new StaticWebResponseGenerator();
            resGen.GetResponseMsg = answer;
            resGen2.GetResponseMsg = answer2;
            MockWebIO.Data.Add(reqUri, resGen);
            MockWebIO.Data.Add(reqUri2, resGen2);
            var req = new HttpRequestMessage(HttpMethod.Get, reqUri);
            var req2 = new HttpRequestMessage(HttpMethod.Get, reqUri2);

            var resp = WebIO.SendAsync(req, default).Result;
            var resp2 = WebIO.SendAsync(req2, default).Result;

            Assert.AreEqual(answer, resp.Content.ReadAsStringAsync().Result);
            Assert.AreEqual(answer2, resp2.Content.ReadAsStringAsync().Result);
        }

        [Test]
        public void RequestDifferentEndpoints()
        {
            const string answer = "Hello World from user";
            const string answer2 = "Hello World from products";
            const string reqUri = "https://testcorp.com/user";
            const string reqUri2 = "https://testcorp.com/products";
            var resGen = new StaticWebResponseGenerator();
            var resGen2 = new StaticWebResponseGenerator();
            resGen.GetResponseMsg = answer;
            resGen2.GetResponseMsg = answer2;
            MockWebIO.Data.Add(reqUri, resGen);
            MockWebIO.Data.Add(reqUri2, resGen2);
            var req = new HttpRequestMessage(HttpMethod.Get, reqUri);
            var req2 = new HttpRequestMessage(HttpMethod.Get, reqUri2);

            var resp = WebIO.SendAsync(req, default).Result;
            var resp2 = WebIO.SendAsync(req2, default).Result;

            Assert.AreEqual(answer, resp.Content.ReadAsStringAsync().Result);
            Assert.AreEqual(answer2, resp2.Content.ReadAsStringAsync().Result);
        }

        [Test]
        public void RequestMessagePassThrough()
        {
            const string answer = "Hello World from user";
            const string reqUri = "https://unittest.com/endpoint";
            var resGen = new StaticWebResponseGenerator();
            resGen.GetResponseMsg = answer;
            MockWebIO.Data.Add(reqUri, resGen);
            var req = new HttpRequestMessage(HttpMethod.Get, reqUri);

            var resp = WebIO.SendAsync(req, default).Result;

            Assert.AreEqual(req, resp.RequestMessage);
        }

        [Test]
        public async Task ReqeustDownloadWithProgress()
        {
            const string reqUri = "https://test.com/unitTest/ReqeustDownloadWithProgress.txt";
            var resGen = new StaticWebResponseGenerator();
            resGen.GetResponseMsg = "Hello World, this is a lot of garbage data that will only be copied in memeory," +
                "yay i only try make this even longer and i am running out of ideas how to do that, is this already" +
                "enough? maybe a bit more or evem more questions to my self --- This is not funny!";
            MockWebIO.Data.Add(reqUri, resGen);
            using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
            using (System.IO.StreamReader sr = new System.IO.StreamReader(ms, Encoding.UTF8))
            {
                await WebIO.DownloadAsync(reqUri, ms, new TestProgress<double>((p) => {
                    Assert.LessOrEqual(p, 1);
                    Assert.GreaterOrEqual(p, 0);
                }), default);
                ms.Seek(0, System.IO.SeekOrigin.Begin);
                Assert.AreEqual(resGen.GetResponseMsg, sr.ReadToEnd());
            }
        }

        private class TestProgress<T> : IProgress<T>
        {
            private readonly Action<T> handler;

            public TestProgress(Action<T> handler)
            {
                this.handler = handler ?? throw new ArgumentNullException("Handler cannot be null");
            }

            public void Report(T value)
            {
                handler(value);
            }
        }


        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            MockWebIO.Instance.Reset();
        }
    }
}
