using System.Collections.Generic;
using System.Linq;

namespace CaptchaSharp.Services.CapSolver.Requests.Tasks
{
    internal class CapSolverTaskProxyless
    {
        public string Type { get; set; }
        public string Cookies { get; set; } // Format cookiename1=cookievalue1; cookiename2=cookievalue2
        public string UserAgent { get; set; }
        public void SetCookies(IEnumerable<(string, string)> cookies)
        {
            if (cookies == null)
                return;

            Cookies = string.Join("; ", cookies.Select(c => $"{c.Item1}={c.Item2}"));
        }
    }
}
