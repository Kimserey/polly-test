using System;
using Polly;
using System.Net.Http;
using Polly.Extensions.Http;
using Polly.Timeout;

namespace PollyTest
{
    public static class PolicyHandler
    {
        // Handle 5.x.x, 408 timeout, 404 not found and timeout rejection from client and retry 5 time with an exponential waiting time.
        public static IAsyncPolicy<HttpResponseMessage> WaitAndRetry =>
            HttpPolicyExtensions
                .HandleTransientHttpError()
                .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.NotFound)
                .Or<TimeoutRejectedException>()
                .WaitAndRetryAsync(5, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));

        public static IAsyncPolicy<HttpResponseMessage> Timeout =>
            Policy.TimeoutAsync<HttpResponseMessage>(TimeSpan.FromSeconds(1));
    }
}
