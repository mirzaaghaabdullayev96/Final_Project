using Newtonsoft.Json;
using NotificationService.Application.Events;
using NotificationService.Application.Services.Interfaces;
using NotificationService.Infrastructure.Services.Implementations;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace NotificationService.Worker
{
    public class Worker(IConnection rabbitConnection, IServiceScopeFactory scopeFactory) : BackgroundService
    {

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using var channel = rabbitConnection.CreateModel();

                await UserCreatedEvent.ListenForUserCreated(channel, stoppingToken, scopeFactory);

                Task.WaitAll(
                   UserCreatedEvent.ListenForUserCreated(channel, stoppingToken, scopeFactory),
                   UserForgotPassword.ListenForForgotPassword(channel, stoppingToken, scopeFactory)
                );

                await Task.Delay(Timeout.Infinite, stoppingToken);
            }
        }
    }
}
