using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace NewsBus.WatcherService.Services
{
    public class SchedulerBackgroundService : BackgroundService
    {
        public SchedulerBackgroundService()
        {
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                
            }
            return Task.CompletedTask;
        }
    }
}