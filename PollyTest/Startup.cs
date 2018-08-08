using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using Microsoft.Extensions.Http;
using Polly;
using System.Net.Http;
using Polly.Extensions.Http;
using Polly.Timeout;

namespace PollyTest
{
    public class Startup
    {
        static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
        {
            // Handle 5.x.x, 408 timeout, 404 not found and timeout rejection from client and retry 5 time with an exponential waiting time.
            return
                HttpPolicyExtensions
                    .HandleTransientHttpError()
                    .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.NotFound)
                    .Or<TimeoutRejectedException>()
                    .WaitAndRetryAsync(5, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)))
                    .WrapAsync(Policy.TimeoutAsync(TimeSpan.FromSeconds(1), TimeoutStrategy.Optimistic));
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services
                .AddHttpClient<IApiClient, ApiClient>(opts =>
                {
                    opts.BaseAddress = new Uri("http://localhost:5100");
                })
                .AddPolicyHandler(GetRetryPolicy());

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "My API", Version = "v1" });
            });
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });

            app.UseMvc();
        }
    }
}
