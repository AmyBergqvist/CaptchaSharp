namespace CaptchaSharp.Services.CapMonsterCloud.Requests.Tasks.Proxied
{
    internal class DataDomeTask : CapMonsterCloudTask
    {
        public string WebsiteURL { get; set; }
        public string CaptchaURL { get; set; }

        public DataDomeTask()
        {
            Type = "DatadomeSliderTask";
        }
    }
}
