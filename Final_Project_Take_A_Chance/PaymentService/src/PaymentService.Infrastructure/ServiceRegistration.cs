using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PaymentService.Application.Repositories;
using PaymentService.Infrastructure.DAL.AppDbContext;
using PaymentService.Infrastructure.Repositories;

namespace PaymentService.Infrastructure
{
    public static class ServiceRegistration
    {
        public static void RegisterServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<ITopUpRepository, TopUpRepository>();
            services.AddScoped<IRequestToServices, RequestToServices>();

            services.AddDbContext<AppDbContext>(opt =>
            {
                opt.UseSqlServer(configuration.GetConnectionString("Default"));
            });
        }
    }
}
