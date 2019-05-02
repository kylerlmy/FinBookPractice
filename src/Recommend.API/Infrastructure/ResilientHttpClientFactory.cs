using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Polly;
using Resilience.Http;
using System;
using System.Net.Http;

namespace Recommend.API.Infrastructure
{
    public class ResilientHttpClientFactory
    {
        private ILogger<ResilienceHttpClient> _logger;
        private IHttpContextAccessor _httpContextAccessor;

        //重试次数
        private int _retryCount;

        //熔断之前允许的异常次数
        private int _exceptionCountAllowedBeforeBreaking;

        public ResilientHttpClientFactory(ILogger<ResilienceHttpClient> logger,
            IHttpContextAccessor httpContextAccessor,
            int retryCount,
            int exceptionCountAllowedBeforeBreaking)
        {
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
            _retryCount = retryCount;
            _exceptionCountAllowedBeforeBreaking = exceptionCountAllowedBeforeBreaking;
        }
        public ResilienceHttpClient GetResilienceHttpClient() =>
            new ResilienceHttpClient("recommend_api",origin => CreatePolicies(origin), _logger, _httpContextAccessor);


        private Policy[] CreatePolicies(string origin)
        {
            return new Policy[] {
                Policy.Handle<HttpRequestException>()
                .WaitAndRetryAsync(//必须使用WaitAndRetryAsync方法，不能使用WaitAndRetry方法，否则，提示“Please use asynchronous-defined policies when calling asynchronous ExecuteAsync”错误，导致无法完成对API的请求（400）
                    // number of retries
                    _retryCount,
                    // exponential backofff
                    retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                    // on retry
                    (exception, timeSpan, retryCount, context) =>
                    {
                        var msg = $"第{retryCount}次重试 implemented with Polly's RetryPolicy " +
                            $"of {context.PolicyKey} " +
                            $"at {context.ExecutionKey}, " +
                            $"due to: {exception}.";
                        _logger.LogWarning(msg);
                        _logger.LogDebug(msg);
                    }),
                Policy.Handle<HttpRequestException>()
                .CircuitBreakerAsync(
                   // number of exceptions before breaking circuit
                    _exceptionCountAllowedBeforeBreaking,
                   // time circuit opened before retry
                    TimeSpan.FromMinutes(1),
                    (exception,duration)=>
                    {
                        // on circuit opened
                        _logger.LogWarning("[熔断器打开]Circuit breaker opened");
                         _logger.LogDebug("[熔断器打开]Circuit breaker opened");
                    },
                    ()=>
                    {
                        // on circuit closed
                        _logger.LogWarning("[熔断器关闭]Circuit breaker opened");
                         _logger.LogDebug("[熔断器打开]Circuit breaker opened");

                    })
            };
        }
    }
}
