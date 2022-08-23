using System.Diagnostics;
using System.IO;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NewsBus.Application;
using NewsBus.Application.Interfaces;
using NewsBus.Domain.Models;

namespace NewsBus.DownloaderService.Services
{
    public class DownloadBackgroundService : BackgroundService
    {
        private readonly string cosmosConnectionString;
        private readonly IDownloadEventProcessor downloadProcessor;
        private readonly ILogger logger;
        private ServiceBusClient client;
        private ServiceBusProcessor processor;

        public DownloadBackgroundService(string cosmosConnectionString, IDownloadEventProcessor downloadProcessor, ILogger<DownloadBackgroundService> logger)
        {
            if (string.IsNullOrWhiteSpace(cosmosConnectionString))
            {
                throw new System.ArgumentException($"'{nameof(cosmosConnectionString)}' cannot be null or whitespace.", nameof(cosmosConnectionString));
            }

            this.cosmosConnectionString = cosmosConnectionString;
            this.downloadProcessor = downloadProcessor ?? throw new System.ArgumentNullException(nameof(downloadProcessor));
            this.logger = logger ?? throw new System.ArgumentNullException(nameof(logger));
            this.logger.LogTrace("DownloadBackgroundService constructor");
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            client = new ServiceBusClient(cosmosConnectionString);
            processor = client.CreateProcessor(Constants.DownloadQueue);
            processor.ProcessMessageAsync += MessageHandler;
            processor.ProcessErrorAsync += ErrorHandler; 
            await processor.StartProcessingAsync(stoppingToken);
            await Task.CompletedTask;
            logger.LogTrace("DownloadBackgroundService ExecuteAsync finished");
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            await processor.StopProcessingAsync();
            await processor.DisposeAsync();
            await client.DisposeAsync();
            logger.LogTrace("DownloadBackgroundService StopAsync finished");
        }

        protected async Task MessageHandler(ProcessMessageEventArgs args)
        {
            using Stream bodyStream = args.Message.Body.ToStream();
            Article article = await JsonSerializer.DeserializeAsync<Article>(bodyStream);
            await downloadProcessor.Process(article);
            await args.CompleteMessageAsync(args.Message);
        }

        protected Task ErrorHandler(ProcessErrorEventArgs args)
        {
            logger.LogError(args.Exception.ToString());
            return Task.CompletedTask;
        }
    }
}