using LogUploader.Tools.Settings;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LogUploader.Helper
{
    public class WebHelper
    {
        public static WebClient GetWebClient(IProxySettings settings)
        {
            var wc = new WebClient();
            wc = ConfigureWebClientProxy(wc, settings);
            return wc;
        }

        public static IWebProxy GetProxy(IProxySettings settings)
        {
            var Proxy = new WebProxy(settings.ProxyAddress, settings.ProxyPort);
            if (!string.IsNullOrEmpty(settings.ProxyUsername))
            {
                Proxy.Credentials = new NetworkCredential(settings.ProxyUsername, settings.ProxyPassword);
                Proxy.UseDefaultCredentials = false;
            }
            Proxy.BypassProxyOnLocal = true;
            return Proxy;
        }

        public static T ConfigureWebClientProxy<T>(T wc, IProxySettings settings) where T : WebClient
        {
            if (settings.UseProxy)
            {
                wc.Proxy = GetProxy(settings);
                wc.UseDefaultCredentials = false;
            }
            else
            {
                wc.Proxy = null;
            }
            return wc;
        }

        public static HttpClient GetHttpClient(IProxySettings settings)
        {
            var httpClientHandler = new HttpClientHandler();
            if (settings.UseProxy)
            {
                SetProxyOfClientHandler(settings, httpClientHandler);
            }

            var httpClient = new HttpClient(httpClientHandler, true);

            //TODO default timeout??
            httpClient.Timeout = Timeout.InfiniteTimeSpan;

            return httpClient;
        }

        private static void SetProxyOfClientHandler(IProxySettings settings, HttpClientHandler httpClientHandler)
        {
            var proxy = new WebProxy
            {
                Address = new Uri($"http://{settings.ProxyAddress}:{settings.ProxyPort}"),
                BypassProxyOnLocal = false,
                UseDefaultCredentials = false,

                Credentials = new NetworkCredential(settings.ProxyUsername, settings.ProxyPassword)
            };
            httpClientHandler.Proxy = proxy;
        }

        private class Credential : ICredentials
        {
            readonly string UserName;
            readonly string Password;

            public Credential(string userName, string password)
            {
                UserName = userName;
                Password = password;
            }
            public Credential(IProxySettings settings) : this(settings.ProxyUsername, settings.ProxyPassword)
            {
            }

            public NetworkCredential GetCredential(Uri uri, string authType)
            {
                return new NetworkCredential(UserName, Password);
            }
        }

    }

    
}
