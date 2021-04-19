using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using NUnit.Framework;

using LogUploader.Tools.Settings;

namespace LogUploader.Test.Mocks
{
    internal class MockWebIOFactory : Wrapper.IWebIOFactory
    {

        public static MockWebIOFactory Instance = new MockWebIOFactory();

        public MockWebIOFactory() { }

        public Wrapper.IWebIO Create(IProxySettings settings)
        {
            MockWebIO.Instance.ProxySettings = settings;
            return MockWebIO.Instance;
        }
    }

    internal class MockWebIO : Wrapper.IWebIO, IMock
    {
        public static MockWebIO Instance = new MockWebIO();

        public TimeSpan Timeout { get; set; }
        public IProxySettings ProxySettings { get; set; }

        public MockWebIO() { }

        public static readonly Dictionary<string, IMockResponseGenerator> Data = new Dictionary<string, IMockResponseGenerator>();

        public void Dispose() { }

        public async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            Uri requUri = request.RequestUri;
            string url = RemoveQuery(requUri);
            if (Data.ContainsKey(url))
                return await Data[url].GetResponse(request);
            throw new ArgumentException("Enpoint not registered for test! " + RemoveQuery(requUri));
        }

        private static string RemoveQuery(Uri requUri)
        {
            return requUri.AbsoluteUri.Substring(0, requUri.AbsoluteUri.Length - requUri.Query.Length);
        }

        public async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, HttpCompletionOption completionOption, CancellationToken cancellationToken)
        {
            Uri requUri = request.RequestUri;
            string url = RemoveQuery(requUri);
            if (Data.ContainsKey(url))
                return await Data[url].GetResponse(request);
            throw new ArgumentException("Enpoint not registered for test! " + requUri.AbsolutePath);
        }

        public void Reset()
        {
            Data.Clear();
        }
    }

    internal interface IMockResponseGenerator
    {
        Task<HttpResponseMessage> GetResponse(HttpRequestMessage request);
    }

    internal class StaticWebResponseGenerator : IMockResponseGenerator
    {
        public string GetResponseMsg { get; set; } = null;
        public string PostResponseMsg { get; set; } = null;
        public string PutResponseMsg { get; set; } = null;
        public string DeleteResponseMsg { get; set; } = null;
        public string OptionsResponseMsg { get; set; } = null;
        public string TraceResponseMsg { get; set; } = null;

        public Action<HttpRequestMessage> RequestValidation { private get; set; } = null;

        public StaticWebResponseGenerator()
        {
        }

        public StaticWebResponseGenerator(Action<HttpRequestMessage> requestValidation) : this()
        {
            RequestValidation = requestValidation;
        }

        public async Task<HttpResponseMessage> GetResponse(HttpRequestMessage request)
        {
            if (RequestValidation != null) RequestValidation(request);
            HttpResponseMessage response = new HttpResponseMessage(System.Net.HttpStatusCode.OK);
            response.RequestMessage = request;
            if (request.Method == HttpMethod.Get && GetResponseMsg != null)
            {
                response.Content = new StringContent(GetResponseMsg);
            }
            else if (request.Method == HttpMethod.Post && PostResponseMsg != null)
            {
                response.Content = new StringContent(PostResponseMsg);
            }
            else if (request.Method == HttpMethod.Put && PutResponseMsg != null)
            {
                response.Content = new StringContent(PutResponseMsg);
            }
            else if (request.Method == HttpMethod.Delete && DeleteResponseMsg != null)
            {
                response.Content = new StringContent(DeleteResponseMsg);
            }
            else if (request.Method == HttpMethod.Options && OptionsResponseMsg != null)
            {
                response.Content = new StringContent(OptionsResponseMsg);
            }
            else if (request.Method == HttpMethod.Trace && TraceResponseMsg != null)
            {
                response.Content = new StringContent(TraceResponseMsg);
            }
            else if (request.Method == HttpMethod.Head)
            {
                Assert.Fail("Http HEAD is not supported currently in mocks");
                return null;
            }
            else
            {
                Assert.Fail("The enpoint was not registered for Http methode " + request.Method);
                return null;
            }
            return await Task.Run(() => response); 
        }
    }
}
