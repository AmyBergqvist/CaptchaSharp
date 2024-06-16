using System;
using System.Collections.Generic;
using System.Linq;
using CaptchaSharp.Models;

namespace CaptchaSharp.Services.CapMonsterCloud.Requests.Tasks.Proxied
{
    internal class CapMonsterCloudTask : CapMonsterCloudTaskProxyless
    {
        public string ProxyType { get; set; }
        public string ProxyAddress { get; set; }
        public int ProxyPort { get; set; }
        public string ProxyLogin { get; set; }
        public string ProxyPassword { get; set; }

        public CapMonsterCloudTask SetProxy(Proxy proxy)
        {
            if (!System.Net.IPAddress.TryParse(proxy.Host, out _))
                throw new NotSupportedException($"Only IP addresses are supported for the proxy host");

            ProxyAddress = proxy.Host;
            ProxyPort = proxy.Port;
            ProxyType = proxy.Type.ToString().ToLower();
            ProxyLogin = proxy.Username;
            ProxyPassword = proxy.Password;
            UserAgent = proxy.UserAgent;
            SetCookies(proxy.Cookies);

            return this;
        }
    }
}
