namespace CaptchaSharp.Services.CapMonsterCloud.Requests.Tasks.Proxied
{
    internal class FunCaptchaTask : CapMonsterCloudTask
    {
        public string WebsiteURL { get; set; }
        public string WebsitePublicKey { get; set; }
        public string FuncaptchaApiJSSubdomain { get; set; }

        public FunCaptchaTask()
        {
            Type = "FunCaptchaTask";
        }
    }
}
