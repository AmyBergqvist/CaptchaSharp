﻿using CaptchaSharp.Models;

namespace CaptchaSharp.Services.CapMonsterCloud.Responses.Solutions
{
    internal class GeeTestSolution : Solution
    {
        public string Challenge { get; set; }
        public string Validate { get; set; }
        public string SecCode { get; set; }

        public override CaptchaResponse ToCaptchaResponse(string id)
        {
            return new GeeTestResponse()
            {
                IdString = id,
                Challenge = Challenge,
                Validate = Validate,
                SecCode = SecCode
            };
        }
    }
}
