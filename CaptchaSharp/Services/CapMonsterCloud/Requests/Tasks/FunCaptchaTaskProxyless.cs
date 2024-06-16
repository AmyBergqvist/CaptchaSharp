namespace CaptchaSharp.Services.CapMonsterCloud.Requests.Tasks
{
    internal class FunCaptchaTaskProxyless : CapMonsterCloudTaskProxyless
    {
        public string WebsiteURL { get; set; }
        public string WebsitePublicKey { get; set; }
        public string FuncaptchaApiJSSubdomain { get; set; }

        public FunCaptchaTaskProxyless()
        {
            Type = "FunCaptchaTaskProxyless";
        }
    }
}
