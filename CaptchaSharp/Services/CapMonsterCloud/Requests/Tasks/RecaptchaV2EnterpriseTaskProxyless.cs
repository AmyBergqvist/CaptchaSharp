namespace CaptchaSharp.Services.CapMonsterCloud.Requests.Tasks
{
    internal class RecaptchaV2EnterpriseTaskProxyless : CapMonsterCloudTaskProxyless
    {
        public string WebsiteURL { get; set; }
        public string WebsiteKey { get; set; }
        public string PageAction { get; set; }
        public string EnterprisePayload { get; set; }

        public RecaptchaV2EnterpriseTaskProxyless()
        {
            Type = "RecaptchaV2EnterpriseTaskProxyless";
        }
    }
}
