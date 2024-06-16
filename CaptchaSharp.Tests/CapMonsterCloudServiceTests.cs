﻿using CaptchaSharp.Services;
using System.Threading.Tasks;
using Xunit;

namespace CaptchaSharp.Tests
{
    public class CapMonsterCloudFixture : ServiceFixture
    {
        public CapMonsterCloudFixture()
        {
            Service = new CapMonsterCloudService(Config.Credentials.CapMonsterCloudApiKey);
        }
    }

    public class CapMonsterCloudServiceTests : ServiceTests, IClassFixture<CapMonsterCloudFixture>
    {
        public CapMonsterCloudServiceTests(CapMonsterCloudFixture fixture) : base(fixture) { }

        [Fact] public Task GetBalanceAsync_ValidKey_GetsBalance() => BalanceTest();
        [Fact] public Task SolveImageCaptchaAsync_ValidCaptcha_ValidSolution() => ImageCaptchaTest();
        [Fact] public Task SolveRecaptchaV2Async_NoProxy_ValidSolution() => RecaptchaV2Test_NoProxy();
        [Fact] public Task SolveRecaptchaV3Async_NoProxy_ValidSolution() => RecaptchaV3Test_NoProxy();
        [Fact] public Task SolveFuncaptchaAsync_NoProxy_ValidSolution() => FunCaptchaTest_NoProxy();
        [Fact] public Task SolveHCaptchaAsync_NoProxy_ValidSolution() => HCaptchaTest_NoProxy();
        [Fact] public Task SolveGeeTestAsync_NoProxy_ValidSolution() => GeeTestTest_NoProxy();
        [Fact] public Task SolveDataDomeTestAsync_WithProxy_ValidSolution() => DataDomeTest_WithProxy();
    }
}
