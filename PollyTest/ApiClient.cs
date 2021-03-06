﻿using System.Threading.Tasks;
using System.Net.Http;
using Polly.Timeout;
using Polly;
using System;
using System.Threading;
using Polly.Retry;
using Polly.Extensions.Http;

namespace PollyTest
{
    public class ApiClient : IApiClient
    {
        private readonly HttpClient _client;

        public ApiClient(HttpClient client)
        {
            _client = client;
        }

        public Task<HttpResponseMessage> GetException()
        {
            return _client.GetAsync("test/exception");
        }

        public Task<HttpResponseMessage> GetNotFound()
        {
            return _client.GetAsync("test/not-found");
        }

        public Task<HttpResponseMessage> GetTimeout()
        {
            return _client.GetAsync("test/five-seconds");
        }
    }
}
