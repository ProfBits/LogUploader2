using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.IO;
using LogUploader.Tools.Settings;

namespace LogUploader.Interfaces.Wrapper
{
    public interface IWebIO : IDisposable
    {
        TimeSpan Timeout { get; set; }
        IProxySettings ProxySettings { get; }

        Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken);
        Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, HttpCompletionOption completionOption, CancellationToken cancellationToken);

    }
}
