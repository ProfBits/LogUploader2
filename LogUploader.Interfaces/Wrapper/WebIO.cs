using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using LogUploader.Tools.Settings;

namespace LogUploader.Wrapper
{
    public static class WebIOFactory
    {
        //TODO singelton HttpsClient

        internal static IWebIOFactory Factory { private get; set; } = new HttpClientWebIOFactory();

        public static IWebIO Create(IProxySettings settings) => Factory.Create(settings);

    }

    internal interface IWebIOFactory
    {
        IWebIO Create(IProxySettings settings);
    }

    internal sealed class HttpClientWebIOFactory : IWebIOFactory
    {
        internal HttpClientWebIOFactory() { }

        public IWebIO Create(IProxySettings settings)
        {
            return new WebIO(settings);
        }

        private sealed class WebIO : IWebIO
        {
            private bool disposedValue;

            private readonly HttpClient Client = new HttpClient();

            internal WebIO(IProxySettings proxySettings)
            {
                ProxySettings = proxySettings;
            }

            public TimeSpan Timeout { get => Client.Timeout; set => Client.Timeout = value; }
            public IProxySettings ProxySettings { get; }

            public async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
            {
                return await Client.SendAsync(request, cancellationToken);
            }

            public async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, HttpCompletionOption completionOption, CancellationToken cancellationToken)
            {
                return await Client.SendAsync(request, completionOption, cancellationToken);
            }

            private void Dispose(bool disposing)
            {
                if (!disposedValue)
                {
                    if (disposing)
                    {
                        Client.Dispose();
                    }
                    disposedValue = true;
                }
            }
            public void Dispose()
            {
                Dispose(disposing: true);
                GC.SuppressFinalize(this);
            }
        }
    }

}
