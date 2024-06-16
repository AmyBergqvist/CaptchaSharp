namespace CaptchaSharp.Services.CapMonsterCloud.Requests.Tasks.Proxied
{
    internal class HCaptchaTask : CapMonsterCloudTask
    {
        public string WebsiteKey { get; set; }
        public string WebsiteURL { get; set; }

        public HCaptchaTask()
        {
            Type = "HCaptchaTask";
        }
    }
}
