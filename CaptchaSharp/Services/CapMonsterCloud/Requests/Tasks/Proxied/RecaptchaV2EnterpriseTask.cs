namespace CaptchaSharp.Services.CapMonsterCloud.Requests.Tasks.Proxied
{
    internal class RecaptchaV2EnterpriseTask : CapMonsterCloudTask
    {
        public string WebsiteURL { get; set; }
        public string WebsiteKey { get; set; }
        public string PageAction { get; set; }
        public string EnterprisePayload { get; set; }

        public RecaptchaV2EnterpriseTask()
        {
            Type = "RecaptchaV2EnterpriseTask";
        }
    }
}
