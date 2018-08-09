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

namespace PollyTest
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services
                .AddHttpClient<IApiClient, ApiClient>(opts =>
                {
                    opts.BaseAddress = new Uri("http://localhost:5100");
                })
                .AddPolicyHandler(PolicyHandler.WaitAndRetry(OnFailure))
                .AddPolicyHandler(PolicyHandler.Timeout);

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "My API", Version = "v1" });
            });
        }

        private void OnFailure(DelegateResult<HttpResponseMessage> result, TimeSpan time)
        {
            if (result?.Exception != null)
            {
                Console.WriteLine("Exception occured: {0}, Time: {1}", result.Exception.Message, time);
            }

            if (result?.Result != null)
            {
                Console.WriteLine("Http error handled - Reason:{0} / Status Code:{1:d}, Time: {2}", result.Result.ReasonPhrase, result.Result.StatusCode, time);
            }
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
