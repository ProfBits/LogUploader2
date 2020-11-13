using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using LogUploader.Data.Settings;
using LogUploader.Properties;

namespace LogUploader.Helper
{
    class WebHelper
    {
        internal static WebClient GetWebClient(IProxySettings settings)
        {
            var wc = new WebClient();
            wc = ConfigureWebClientProxy(wc, settings);
            return wc;
        }

        internal static IWebProxy GetProxy(IProxySettings settings)
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

        internal static T ConfigureWebClientProxy<T>(T wc, IProxySettings settings) where T : WebClient
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

    }
}
