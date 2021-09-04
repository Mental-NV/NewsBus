using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using NewsBus.Contracts;
using NewsBus.WatcherService.Core;
using NewsBus.WatcherService.Services;

namespace NewsBus.WatcherService
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
            string queueConnectionString = Environment.GetEnvironmentVariable("NewsBusQueueConnectionString", EnvironmentVariableTarget.Machine);
            string cosmosConnectionString = Environment.GetEnvironmentVariable("NewsBusCosmosDbConnectionString", EnvironmentVariableTarget.Machine);
            string storageConnectionString = Environment.GetEnvironmentVariable("NewsBusStorageConnetionString", EnvironmentVariableTarget.Machine);

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "NewsBus.WatcherService", Version = "v1" });
            });

            services.AddSingleton<IRssFeedRepository, RssFeedRepository>(
                sp => new RssFeedRepository(cosmosConnectionString, Constants.NewsBusDatabase, Constants.RssFeedsContainer)
            );
            services.AddSingleton<IRssLoader, RssLoader>();
            services.AddSingleton<IDownloadEventSender, DownloadEventSender>(
                sp => new DownloadEventSender(queueConnectionString)
            );
            services.AddHostedService<SchedulerBackgroundService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => 
                {
                    c.RoutePrefix = string.Empty;
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "NewsBus.WatcherService v1");
                });
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
