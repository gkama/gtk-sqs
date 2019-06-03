using System;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Amazon.SQS;
using Amazon.Runtime;

using sqs.services;

namespace sqs.core
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            /*
             * docs
             * https://docs.aws.amazon.com/sdk-for-net/v2/developer-guide/how-to-sqs.html
             * https://docs.aws.amazon.com/sdk-for-net/v3/developer-guide/net-dg-config-netcore.html
             */
            Environment.SetEnvironmentVariable("AWS_ACCESS_KEY_ID", Configuration["AWS_ACCESS_KEY_ID"]);
            Environment.SetEnvironmentVariable("AWS_SECRET_ACCESS_KEY", Configuration["AWS_SECRET_ACCESS_KEY"]);
            Environment.SetEnvironmentVariable("AWS_REGION", Configuration["AWS_REGION"]);

            var awsOptions = Configuration.GetAWSOptions();
            awsOptions.Credentials = new EnvironmentVariablesAWSCredentials();

            services.AddDefaultAWSOptions(awsOptions);
            services.AddAWSService<IAmazonSQS>();

            //DI
            services.AddScoped<ISqsRepository, SqsRepository>();

            services.AddHealthChecks();

            services.AddMvc()
               .SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
               .AddJsonOptions(o =>
               {
                   o.SerializerSettings.Formatting = Newtonsoft.Json.Formatting.Indented;
               });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHealthChecks("/api/ping");
            app.UseMvc();
        }
    }
}
