using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NotificationService.Application.Services.Interfaces;

namespace NotificationService.Application.Events
{
    public class WinnerChosenEvent
    {
        public required string UserName { get; set; }
        public required string ProductName { get; set; }
        public required string Email { get; set; }

        public static Task ListenForWinnerChose(IModel channel, CancellationToken stoppingToken, IServiceScopeFactory scopeFactory)
        {
            channel.QueueDeclare(queue: "winner_chosen_queue", durable: true, exclusive: false, autoDelete: false, arguments: null);

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += async (model, ea) =>
            {
                var message = Encoding.UTF8.GetString(ea.Body.ToArray());
                using var scope = scopeFactory.CreateScope();
                var emailService = scope.ServiceProvider.GetRequiredService<IEmailService>();

                try
                {
                    var winnerChoseEvent = JsonConvert.DeserializeObject<WinnerChosenEvent>(message);
                    var subject = "You are winner of the lottery!";
                    var bodyText = "You have won this product";
                    await emailService.SendMailAsync(winnerChoseEvent!.Email, subject, winnerChoseEvent.UserName, bodyText, winnerChoseEvent.ProductName);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error processing user created event: {ex.Message}");
                }
            };

            channel.BasicConsume(queue: "winner_chosen_queue", autoAck: true, consumer: consumer);
            return Task.CompletedTask;
        }
    }


    
}
