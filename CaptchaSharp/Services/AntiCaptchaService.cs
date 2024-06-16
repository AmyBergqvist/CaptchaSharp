﻿using CaptchaSharp.Enums;
using CaptchaSharp.Exceptions;
using CaptchaSharp.Models;
using CaptchaSharp.Services.AntiCaptcha.Requests;
using CaptchaSharp.Services.AntiCaptcha.Requests.Tasks;
using CaptchaSharp.Services.AntiCaptcha.Requests.Tasks.Proxied;
using CaptchaSharp.Services.AntiCaptcha.Responses;
using CaptchaSharp.Services.AntiCaptcha.Responses.Solutions;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace CaptchaSharp.Services
{
    /// <summary>The service provided by <c>https://anti-captcha.com/</c></summary>
    public class AntiCaptchaService : CaptchaService
    {
        /// <summary>Your secret api key.</summary>
        public string ApiKey { get; set; }

        /// <summary>The default <see cref="HttpClient"/> used for requests.</summary>
        protected HttpClient httpClient;

        /// <summary>The ID of the software developer.</summary>
        private readonly int softId = 934;

        /// <summary>Initializes a <see cref="AntiCaptchaService"/> using the given <paramref name="apiKey"/> and 
        /// <paramref name="httpClient"/>. If <paramref name="httpClient"/> is null, a default one will be created.</summary>
        public AntiCaptchaService(string apiKey, HttpClient httpClient = null)
        {
            ApiKey = apiKey;
            this.httpClient = httpClient ?? new HttpClient();
            this.httpClient.BaseAddress = new Uri("https://api.anti-captcha.com");
        }

        #region Getting the Balance
        /// <inheritdoc/>
        public async override Task<decimal> GetBalanceAsync(CancellationToken cancellationToken = default)
        {
            var response = await httpClient.PostJsonToStringAsync
                ("getBalance",
                new Request() { ClientKey = ApiKey },
                cancellationToken)
                .ConfigureAwait(false);

            var balanceResponse = response.Deserialize<GetBalanceResponse>();

            if (balanceResponse.IsError)
                throw new BadAuthenticationException($"{balanceResponse.ErrorCode}: {balanceResponse.ErrorDescription}");

            return new decimal(balanceResponse.Balance);
        }
        #endregion

        #region Solve Methods
        /// <inheritdoc/>
        public async override Task<StringResponse> SolveImageCaptchaAsync
            (string base64, ImageCaptchaOptions options = null, CancellationToken cancellationToken = default)
        {
            var response = await httpClient.PostJsonToStringAsync
                ("createTask",
                AddImageCapabilities(
                    new CaptchaTaskRequest
                    {
                        ClientKey = ApiKey,
                        SoftId = softId,
                        Task = new ImageCaptchaTask
                        {
                            Body = base64
                        }
                    }, options),
                cancellationToken)
                .ConfigureAwait(false);

            return await TryGetResult(response.Deserialize<TaskCreationResponse>(), CaptchaType.ImageCaptcha, cancellationToken)
                as StringResponse;
        }

        /// <inheritdoc/>
        public async override Task<StringResponse> SolveRecaptchaV2Async
            (string siteKey, string siteUrl, string dataS = "", bool enterprise = false, bool invisible = false,
            Proxy proxy = null, IEnumerable<(string, string)> cookies = default, string userAgent = default, CancellationToken cancellationToken = default)
        {
            var content = CreateTaskRequest();

            if (enterprise)
            {
                if (proxy != null)
                {
                    content.Task = new RecaptchaV2EnterpriseTask
                    {
                        WebsiteKey = siteKey,
                        WebsiteURL = siteUrl,
                        EnterprisePayload = new Dictionary<string, string>()
                    }.SetProxy(proxy);
                    
                    if (!string.IsNullOrEmpty(dataS))
                        ((RecaptchaV2EnterpriseTask)content.Task).EnterprisePayload.Add("s", dataS);
                }
                else
                {
                    content.Task = new RecaptchaV2EnterpriseTaskProxyless
                    {
                        WebsiteKey = siteKey,
                        WebsiteURL = siteUrl,
                        EnterprisePayload = new Dictionary<string, string>()
                    };
                    
                    if (!string.IsNullOrEmpty(dataS))
                        ((RecaptchaV2EnterpriseTaskProxyless)content.Task).EnterprisePayload.Add("s", dataS);
                }
            }
            else
            {
                if (proxy != null)
                {
                    content.Task = new RecaptchaV2Task
                    {
                        WebsiteKey = siteKey,
                        WebsiteURL = siteUrl,
                        IsInvisible = invisible
                    }.SetProxy(proxy);
                }
                else
                {
                    content.Task = new RecaptchaV2TaskProxyless
                    {
                        WebsiteKey = siteKey,
                        WebsiteURL = siteUrl,
                        IsInvisible = invisible
                    };
                }
            }

            if (userAgent != null)
            {
                content.Task.UserAgent = userAgent;
            }

            content.Task.SetCookies(cookies);
            
            var response = await httpClient.PostJsonToStringAsync
                ("createTask",
                content,
                cancellationToken)
                .ConfigureAwait(false);

            return await TryGetResult(response.Deserialize<TaskCreationResponse>(), CaptchaType.ReCaptchaV2, cancellationToken)
                as StringResponse;
        }

        /// <inheritdoc/>
        public async override Task<StringResponse> SolveRecaptchaV3Async
            (string siteKey, string siteUrl, string action = "verify", float minScore = 0.4F, bool enterprise = false,
            Proxy proxy = null, IEnumerable<(string, string)> cookies = default, string userAgent = default, CancellationToken cancellationToken = default)
        {
            if (proxy != null)
                throw new NotSupportedException("Proxies are not supported");

            if (minScore != 0.3F && minScore != 0.7F && minScore != 0.9F)
                throw new NotSupportedException("Only min scores of 0.3, 0.7 and 0.9 are supported");

            var content = CreateTaskRequest();

            content.Task = new RecaptchaV3TaskProxyless
            {
                WebsiteKey = siteKey,
                WebsiteURL = siteUrl,
                PageAction = action,
                MinScore = minScore,
                IsEnterprise = enterprise
            };

            if (userAgent != null)
            {
                content.Task.UserAgent = userAgent;
            }

            content.Task.SetCookies(cookies);

            var response = await httpClient.PostJsonToStringAsync
                ("createTask",
                content,
                cancellationToken)
                .ConfigureAwait(false);

            return await TryGetResult(response.Deserialize<TaskCreationResponse>(), CaptchaType.ReCaptchaV3, cancellationToken)
                as StringResponse;
        }

        /// <inheritdoc/>
        public async override Task<StringResponse> SolveFuncaptchaAsync
            (string publicKey, string serviceUrl, string siteUrl, bool noJS = false, Proxy proxy = null,
            CancellationToken cancellationToken = default)
        {
            if (noJS)
                throw new NotSupportedException("This service does not support no js solving");

            var content = CreateTaskRequest();

            if (proxy != null)
            {
                content.Task = new FunCaptchaTask
                {
                    WebsitePublicKey = publicKey,
                    WebsiteURL = siteUrl,
                    FuncaptchaApiJSSubdomain = serviceUrl
                }.SetProxy(proxy);
            }
            else
            {
                content.Task = new FunCaptchaTaskProxyless
                {
                    WebsitePublicKey = publicKey,
                    WebsiteURL = siteUrl,
                    FuncaptchaApiJSSubdomain = serviceUrl
                };
            }

            var response = await httpClient.PostJsonToStringAsync
                ("createTask",
                content,
                cancellationToken)
                .ConfigureAwait(false);

            return await TryGetResult(response.Deserialize<TaskCreationResponse>(), CaptchaType.FunCaptcha, cancellationToken)
                as StringResponse;
        }

        /// <inheritdoc/>
        public async override Task<StringResponse> SolveHCaptchaAsync
            (string siteKey, string siteUrl, Proxy proxy = null, CancellationToken cancellationToken = default)
        {
            var content = CreateTaskRequest();
            
            if (proxy != null)
            {
                content.Task = new HCaptchaTask
                {
                    WebsiteKey = siteKey,
                    WebsiteURL = siteUrl,
                }.SetProxy(proxy);
            }
            else
            {
                content.Task = new HCaptchaTaskProxyless
                {
                    WebsiteKey = siteKey,
                    WebsiteURL = siteUrl,
                };
            }
            
            var response = await httpClient.PostJsonToStringAsync
                ("createTask",
                content,
                cancellationToken)
                .ConfigureAwait(false);

            return await TryGetResult(response.Deserialize<TaskCreationResponse>(), CaptchaType.HCaptcha, cancellationToken)
                as StringResponse;
        }

        /// <inheritdoc/>
        public async override Task<GeeTestResponse> SolveGeeTestAsync
            (string gt, string challenge, string apiServer, string siteUrl, Proxy proxy = null,
            CancellationToken cancellationToken = default)
        {
            var content = CreateTaskRequest();
            
            if (proxy != null)
            {
                content.Task = new GeeTestTask
                {
                    WebsiteURL = siteUrl,
                    Gt = gt,
                    Challenge = challenge,
                    GeetestApiServerSubdomain = apiServer
                }.SetProxy(proxy);
            }
            else
            {
                content.Task = new GeeTestTaskProxyless
                {
                    WebsiteURL = siteUrl,
                    Gt = gt,
                    Challenge = challenge,
                    GeetestApiServerSubdomain = apiServer
                };
            }

            var response = await httpClient.PostJsonToStringAsync
                ("createTask",
                content,
                cancellationToken)
                .ConfigureAwait(false);

            return await TryGetResult(response.Deserialize<TaskCreationResponse>(), CaptchaType.GeeTest, cancellationToken)
                as GeeTestResponse;
        }
        #endregion

        #region Getting the result
        private async Task<CaptchaResponse> TryGetResult
            (TaskCreationResponse response, CaptchaType type, CancellationToken cancellationToken = default)
        {
            if (response.IsError)
                throw new TaskCreationException($"{response.ErrorCode}: {response.ErrorDescription}");

            var task = new CaptchaTask(response.TaskId, type);

            return await TryGetResult(task, cancellationToken).ConfigureAwait(false);
        }

        /// <inheritdoc/>
        protected async override Task<CaptchaResponse> CheckResult
            (CaptchaTask task, CancellationToken cancellationToken = default)
        {
            var response = await httpClient.PostJsonToStringAsync
                ("getTaskResult",
                new GetTaskResultRequest() { ClientKey = ApiKey, TaskId = (int)task.Id },
                cancellationToken).ConfigureAwait(false);

            var result = response.Deserialize<GetTaskResultResponse>();

            if (!result.IsReady)
                return default;

            task.Completed = true;

            if (result.IsError)
                throw new TaskSolutionException($"{result.ErrorCode}: {result.ErrorDescription}");

            var jObject = JObject.Parse(response);
            var solution = jObject["solution"];

            switch (task.Type)
            {
                case CaptchaType.ReCaptchaV2:
                case CaptchaType.ReCaptchaV3:
                case CaptchaType.HCaptcha:
                    result.Solution = solution.ToObject<RecaptchaSolution>();
                    break;

                case CaptchaType.FunCaptcha:
                    result.Solution = solution.ToObject<FuncaptchaSolution>();
                    break;

                case CaptchaType.ImageCaptcha:
                    result.Solution = solution.ToObject<ImageCaptchaSolution>();
                    break;

                case CaptchaType.GeeTest:
                    result.Solution = solution.ToObject<GeeTestSolution>();
                    break;

                default:
                    throw new NotSupportedException();
            }

            return result.Solution.ToCaptchaResponse(task.Id);
        }
        #endregion

        #region Reporting the solution
        /// <inheritdoc/>
        public async override Task ReportSolution
            (long taskId, CaptchaType type, bool correct = false, CancellationToken cancellationToken = default)
        {
            if (correct)
                throw new NotSupportedException("This service doesn't allow reporting of good solutions");

            string response;
            ReportIncorrectCaptchaResponse incResponse;

            switch (type)
            {
                case CaptchaType.ImageCaptcha:
                    response = await httpClient.PostJsonToStringAsync
                    ("reportIncorrectImageCaptcha",
                    new ReportIncorrectCaptchaRequest() { ClientKey = ApiKey, TaskId = taskId },
                    cancellationToken).ConfigureAwait(false);

                    incResponse = response.Deserialize<ReportIncorrectCaptchaResponse>();
                    break;

                case CaptchaType.ReCaptchaV2:
                case CaptchaType.ReCaptchaV3:
                    response = await httpClient.PostJsonToStringAsync
                    ("reportIncorrectRecaptcha",
                    new ReportIncorrectCaptchaRequest() { ClientKey = ApiKey, TaskId = taskId },
                    cancellationToken).ConfigureAwait(false);

                    incResponse = response.Deserialize<ReportIncorrectCaptchaResponse>();
                    break;

                default:
                    throw new NotSupportedException("Reporting is not supported for this captcha type");
            }

            if (incResponse.NotFoundOrExpired)
                throw new TaskReportException("Captcha not found or expired");
        }
        #endregion

        #region Private Methods
        private CaptchaTaskRequest CreateTaskRequest()
        {
            return new CaptchaTaskRequest
            {
                ClientKey = ApiKey,
                SoftId = softId
            };
        }
        #endregion

        #region Capabilities
        /// <inheritdoc/>
        public override CaptchaServiceCapabilities Capabilities =>
            CaptchaServiceCapabilities.Language |
            CaptchaServiceCapabilities.Phrases |
            CaptchaServiceCapabilities.CaseSensitivity |
            CaptchaServiceCapabilities.CharacterSets |
            CaptchaServiceCapabilities.Calculations |
            CaptchaServiceCapabilities.MinLength |
            CaptchaServiceCapabilities.MaxLength |
            CaptchaServiceCapabilities.Instructions;

        private CaptchaTaskRequest AddImageCapabilities(CaptchaTaskRequest request, ImageCaptchaOptions options)
        {
            if (options == null)
                return request;

            var task = request.Task as ImageCaptchaTask;

            task.Phrase = options.IsPhrase;
            task.Case = options.CaseSensitive;
            
            switch (options.CharacterSet)
            {
                case CharacterSet.OnlyNumbers:
                    task.Numeric = 1;
                    break;

                case CharacterSet.OnlyLetters:
                    task.Numeric = 2;
                    break;

                default:
                    task.Numeric = 0;
                    break;
            }

            task.Math = options.RequiresCalculation;
            task.MinLength = options.MinLength;
            task.MaxLength = options.MaxLength;
            task.Comment = options.TextInstructions;
            
            switch (options.CaptchaLanguage)
            {
                case CaptchaLanguage.NotSpecified:
                case CaptchaLanguage.English:
                    request.LanguagePool = "en";
                    break;

                case CaptchaLanguage.Russian:
                case CaptchaLanguage.Ukrainian:
                case CaptchaLanguage.Kazakh:
                case CaptchaLanguage.Belorussian:
                    request.LanguagePool = "rn";
                    break;

                default:
                    throw new NotSupportedException($"The {options.CaptchaLanguage} language is not supported");
            }

            return request;
        }
        #endregion
    }
}
