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
using NewsBus.Application;
using NewsBus.Application.Interfaces;
using NewsBus.Infrastructure;

namespace NewsBus.DownloaderService
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
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "NewsBus.DownloaderService", Version = "v1" });
            });

            string queueConnectionString = Configuration["Env:NewsBusQueueConnectionString"];
            string cosmosConnectionString = Configuration["Env:NewsBusCosmosDbConnectionString"];
            string storageConnectionString = Configuration["Env:NewsBusStorageConnetionString"];

            services.AddSingleton<IArticleRepository, ArticleRepository>(
                sp => new ArticleRepository(cosmosConnectionString, Constants.NewsBusDatabase, Constants.ArticlesContainer)
            );

            services.AddSingleton<IArticleContentRepository, ArticleContentRepository>(
                sp => new ArticleContentRepository(storageConnectionString, Constants.ArticleBlobs)
            );

            services.AddSingleton<IContentDownloader, ContentDownloader>();
            services.AddSingleton<IContentParser, ContentParser>();

            services.AddSingleton<IDownloadEventProcessor, DownloadEventProcessor>(
                sp => new DownloadEventProcessor(
                    sp.GetService<IArticleRepository>(),
                    sp.GetService<IArticleContentRepository>(),
                    sp.GetService<IContentDownloader>(),
                    sp.GetService<IContentParser>()
                )
            );

            services.AddHostedService<DownloadBackgroundService>(sp =>
                new DownloadBackgroundService(queueConnectionString, sp.GetService<IDownloadEventProcessor>())
            );
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
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "NewsBus.DownloaderService v1");
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
