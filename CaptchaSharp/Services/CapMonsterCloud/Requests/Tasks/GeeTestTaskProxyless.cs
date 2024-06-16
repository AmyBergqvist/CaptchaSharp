namespace CaptchaSharp.Services.CapMonsterCloud.Requests.Tasks
{
    internal class GeeTestTaskProxyless : CapMonsterCloudTaskProxyless
    {
        public string WebsiteURL { get; set; }
        public string Gt { get; set; }
        public string Challenge { get; set; }
        public string GeetestApiServerSubdomain { get; set; }
        public int Version { get; set; }

        public GeeTestTaskProxyless()
        {
            Type = "GeeTestTaskProxyless";
        }
    }
}
