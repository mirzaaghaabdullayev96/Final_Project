using NotificationService.Application.Services.Interfaces;
using NotificationService.Infrastructure.Services.Implementations;
using RabbitMQ.Client;

namespace NotificationService.Worker
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = Host.CreateApplicationBuilder(args);
            builder.Services.AddHostedService<Worker>();

            var rabbitMqConnectionFactory = new ConnectionFactory()
            {
                HostName = "localhost",
                UserName = "guest",
                Password = "guest"
            };

            builder.Services.AddSingleton<IConnection>(rabbitMqConnectionFactory.CreateConnection());
            builder.Services.AddScoped<IEmailService, EmailService>();

            var host = builder.Build();
            host.Run();
        }
    }
}