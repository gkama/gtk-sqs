using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using Amazon.SQS;

using sqs.data;
using sqs.services;
using Amazon.Runtime;

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
             * 
             * https://docs.aws.amazon.com/sdk-for-net/v3/developer-guide/net-dg-config-netcore.html
             */
            //var awsOptions = Configuration.GetAWSOptions();
            //awsOptions.Credentials = new EnvironmentVariablesAWSCredentials();

            var options = Configuration.GetAWSOptions();
            options.Credentials = new BasicAWSCredentials(Configuration["AWS_ACCESS_KEY_ID"], Configuration["AWS_SECRET_ACCESS_KEY"]);
            options.Region = Amazon.RegionEndpoint.GetBySystemName(Configuration["AWS_REGION"]);

            services.AddDefaultAWSOptions(options);
            services.AddAWSService<IAmazonSQS>();

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
