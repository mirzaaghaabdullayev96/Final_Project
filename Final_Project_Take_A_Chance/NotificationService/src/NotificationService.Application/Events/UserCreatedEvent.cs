using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using NotificationService.Application.Services.Interfaces;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationService.Application.Events
{
    public class UserCreatedEvent
    {

        public required string Email { get; set; }
        public required string Name { get; set; }
        public required string ActivationLink { get; set; }

        public static Task ListenForUserCreated(IModel channel, CancellationToken stoppingToken, IServiceScopeFactory scopeFactory)
        {
            channel.QueueDeclare(queue: "user_created_queue", durable: true, exclusive: false, autoDelete: false, arguments: null);

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += async (model, ea) =>
            {
                var message = Encoding.UTF8.GetString(ea.Body.ToArray());
                using var scope = scopeFactory.CreateScope();
                var emailService = scope.ServiceProvider.GetRequiredService<IEmailService>();

                try
                {
                    var userCreatedEvent = JsonConvert.DeserializeObject<UserCreatedEvent>(message);
                    var subject = "Email confirmation";
                    var bodyText = "To confirm your Email, please click the link below";
                    await emailService.SendMailAsync(userCreatedEvent!.Email, subject, userCreatedEvent.Name, bodyText, userCreatedEvent.ActivationLink);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error processing user created event: {ex.Message}");
                }
            };

            channel.BasicConsume(queue: "user_created_queue", autoAck: true, consumer: consumer);
            return Task.CompletedTask;
        }
    }

}
