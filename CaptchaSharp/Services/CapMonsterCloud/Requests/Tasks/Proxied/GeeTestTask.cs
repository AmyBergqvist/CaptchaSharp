namespace CaptchaSharp.Services.CapMonsterCloud.Requests.Tasks.Proxied
{
    internal class GeeTestTask : CapMonsterCloudTask
    {
        public string WebsiteURL { get; set; }
        public string Gt { get; set; }
        public string Challenge { get; set; }
        public string GeetestApiServerSubdomain { get; set; }

        public GeeTestTask()
        {
            Type = "GeeTestTask";
        }
    }
}
