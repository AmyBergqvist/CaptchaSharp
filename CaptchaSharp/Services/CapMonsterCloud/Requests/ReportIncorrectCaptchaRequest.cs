namespace CaptchaSharp.Services.CapMonsterCloud.Requests
{
    internal class ReportIncorrectCaptchaRequest : Request
    {
        public string TaskId { get; set; }
    }
}
