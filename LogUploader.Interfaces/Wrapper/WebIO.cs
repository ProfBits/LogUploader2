﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Extensiones.Stream;

using LogUploader.Tools.Settings;

namespace LogUploader.Wrapper
{
    public static class WebIOFactory
    {
        private static IWebIOFactory factory = new HttpClientWebIOFactory();
        private static IWebIO DefaultIO = null;
        internal static IWebIOFactory Factory
        {
            get => factory;
            set
            {
                factory = value;
                DefaultIO = null;
            }
        }

        public static IWebIO Create(IProxySettings settings, Uri uri)
        {
            DefaultIO = DefaultIO ?? Factory.Create(settings);
            return DefaultIO;
        }
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

            public async Task DownloadAsync(string requestUri, Stream destination, IProgress<double> progress = null, CancellationToken cancellationToken = default)
            {
                // Get the http headers first to examine the content length
                using (var response = await Client.GetAsync(requestUri, HttpCompletionOption.ResponseHeadersRead))
                {
                    var contentLength = response.Content.Headers.ContentLength;

                    using (var download = await response.Content.ReadAsStreamAsync())
                    {
                        // Ignore progress reporting when no progress reporter was 
                        // passed or when the content length is unknown
                        if (progress == null || !contentLength.HasValue)
                        {
                            await download.CopyToAsync(destination);
                            return;
                        }

                        // Convert absolute progress (bytes downloaded) into relative progress (0% - 100%)
                        var relativeProgress = new Progress<long>(totalBytes => progress.Report((double)totalBytes / contentLength.Value));
                        // Use extension method to report progress while downloading
                        await download.CopyToAsync(destination, 81920, relativeProgress, cancellationToken);
                        progress.Report(1);
                    }
                }
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