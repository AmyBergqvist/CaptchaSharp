using CaptchaSharp.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CaptchaSharp.Services.CapSolver.Requests.Tasks.Proxied
{
    internal class CapSolverTask : CapSolverTaskProxyless
    {
        public string ProxyType { get; set; }
        public string ProxyAddress { get; set; }
        public int ProxyPort { get; set; }
        public string ProxyLogin { get; set; }
        public string ProxyPassword { get; set; }
        public string UserAgent { get; set; }

        public CapSolverTask SetProxy(Proxy proxy)
        {
            if (!System.Net.IPAddress.TryParse(proxy.Host, out _))
                throw new NotSupportedException($"Only IP addresses are supported for the proxy host");

            ProxyAddress = proxy.Host;
            ProxyPort = proxy.Port;
            ProxyType = proxy.Type.ToString().ToLower();
            ProxyLogin = proxy.Username;
            ProxyPassword = proxy.Password;

            return this;
        }
    }
}
