namespace CaptchaSharp.Services.CapSolver.Requests
{
    internal class ReportIncorrectCaptchaRequest : Request
    {
        public string AppId { get; set; }
        public string TaskId { get; set; }
        public ReportIncorrectCaptchaResultRequest CaptchaResult { get; set; }
    }

    internal class ReportIncorrectCaptchaResultRequest
    {
        public bool Invalid { get; set; }
    }
}
