using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.Json;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using NewsBus.Application;
using NewsBus.Application.Interfaces;
using NewsBus.Domain.Models;

namespace NewsBus.Infrastructure
{
    public class DownloadEventSender : IDownloadEventSender, IAsyncDisposable
    {
        private readonly string queueConnectionString;
        private readonly ServiceBusClient client;
        private readonly ServiceBusSender sender;

        public DownloadEventSender(string queueConnectionString)
        {
            if (string.IsNullOrWhiteSpace(queueConnectionString))
            {
                throw new ArgumentException($"'{nameof(queueConnectionString)}' cannot be null or whitespace.", nameof(queueConnectionString));
            }

            this.queueConnectionString = queueConnectionString;
            client = new ServiceBusClient(queueConnectionString);
            sender = client.CreateSender(Constants.DownloadQueue);
        }

        public async Task SendAsync(Article article)
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