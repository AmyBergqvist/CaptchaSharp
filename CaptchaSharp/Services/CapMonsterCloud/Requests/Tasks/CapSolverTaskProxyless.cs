using CaptchaSharp.Enums;
using CaptchaSharp.Models;
using CaptchaSharp.Services.CapMonsterCloud.Requests.Tasks.Proxied;
using System.Collections.Generic;
using System;
using System.Linq;

namespace CaptchaSharp.Services.CapMonsterCloud.Requests.Tasks
{
    internal class CapMonsterCloudTaskProxyless
    {
        public string Type { get; set; }
        public string UserAgent { get; set; }
        public string Cookies { get; set; } // Format cookiename1=cookievalue1; cookiename2=cookievalue2

        public void SetCookies(IEnumerable<(string, string)> cookies)
        {
            if (cookies == null)
                return;

            Cookies = string.Join("; ", cookies.Select(c => $"{c.Item1}={c.Item2}"));
        }
    }
}
