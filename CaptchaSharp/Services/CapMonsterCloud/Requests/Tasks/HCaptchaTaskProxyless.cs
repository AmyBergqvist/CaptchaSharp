namespace CaptchaSharp.Services.CapMonsterCloud.Requests.Tasks
{
    internal class HCaptchaTaskProxyless : CapMonsterCloudTaskProxyless
    {
        public string WebsiteKey { get; set; }
        public string WebsiteURL { get; set; }

        public HCaptchaTaskProxyless()
        {
            Type = "HCaptchaTaskProxyless";
        }
    }
}
