using CaptchaSharp.Services.CapMonsterCloud.Requests.Tasks;

namespace CaptchaSharp.Services.CapMonsterCloud.Requests
{
    internal class CaptchaTaskRequest : Request
    {
        public CapMonsterCloudTaskProxyless Task { get; set; }
        public string LanguagePool { get; set; } = "en";
    }
}
