
using PaymentService.Application.Repositories;
using PaymentService.Infrastructure;
using PaymentService.Infrastructure.Repositories;
using PaymentService.Application.Features.Commands.TopUpCommands;

namespace PaymentService.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
           
            builder.Services.RegisterServices(builder.Configuration);

            builder.Services.AddHttpClient<IUserServiceClient, UserServiceClient>(client =>
            {
                client.BaseAddress = new Uri("http://localhost:5016");
            });

            builder.Services.AddMediatR(opt =>
            {
                opt.RegisterServicesFromAssemblyContaining(typeof(TopUpCreateRequest));
            });

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
