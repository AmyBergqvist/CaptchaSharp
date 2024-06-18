namespace CaptchaSharp.Services.CapMonsterCloud.Requests
{
    internal class ReportIncorrectCaptchaRequest : Request
    {
        public int TaskId { get; set; }
    }
}
