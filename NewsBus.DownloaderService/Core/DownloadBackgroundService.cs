using System.Diagnostics;
using System.IO;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Hosting;
using NewsBus.Domain;
using NewsBus.Domain.Models;

namespace NewsBus.DownloaderService.Core
{
    public class DownloadBackgroundService : BackgroundService
    {
        private readonly string cosmosConnectionString;
        private readonly IDownloadEventProcessor downloadProcessor;
        private ServiceBusClient client;
        private ServiceBusProcessor processor;

        public DownloadBackgroundService(string cosmosConnectionString, IDownloadEventProcessor downloadProcessor)
        {
            if (string.IsNullOrWhiteSpace(cosmosConnectionString))
            {
                throw new System.ArgumentException($"'{nameof(cosmosConnectionString)}' cannot be null or whitespace.", nameof(cosmosConnectionString));
            }

            this.cosmosConnectionString = cosmosConnectionString;
            this.downloadProcessor = downloadProcessor ?? throw new System.ArgumentNullException(nameof(downloadProcessor));
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            client = new ServiceBusClient(cosmosConnectionString);
            processor = client.CreateProcessor(Constants.DownloadQueue);
            processor.ProcessMessageAsync += MessageHandler;
            processor.ProcessErrorAsync += ErrorHandler; 
            return Task.CompletedTask;
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            await processor.DisposeAsync();
            await client.DisposeAsync();
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
            Trace.TraceError(args.Exception.ToString());
            return Task.CompletedTask;
        }
    }
}