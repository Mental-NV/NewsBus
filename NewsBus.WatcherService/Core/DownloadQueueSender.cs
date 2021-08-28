using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.Json;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using NewsBus.Domain;
using NewsBus.Domain.Models;

namespace NewsBus.WatcherService.Core
{
    public class DownloadQueueSender : IDownloadQueueSender
    {
        private const string queueName = "downloadqueue";
        private readonly string queueConnectionString;
        private readonly ServiceBusClient client;
        private readonly ServiceBusSender sender;

        public DownloadQueueSender(string queueConnectionString)
        {
            if (string.IsNullOrWhiteSpace(queueConnectionString))
            {
                throw new ArgumentException($"'{nameof(queueConnectionString)}' cannot be null or whitespace.", nameof(queueConnectionString));
            }

            this.queueConnectionString = queueConnectionString;
            client = new ServiceBusClient(queueConnectionString);
            sender = client.CreateSender(queueName);
        }

        public async Task SendAsync(MetaArticle article)
        {
            string body = JsonSerializer.Serialize(article);
            ServiceBusMessage message = new ServiceBusMessage(body);
            await sender.SendMessageAsync(message);
        }

        public async ValueTask DisposeAsync()
        {
            await client.DisposeAsync();
            await sender.DisposeAsync();
            GC.SuppressFinalize(this);
        }
    }
}