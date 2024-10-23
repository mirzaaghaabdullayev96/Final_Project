using LotteryService.Application.Repositories;
using LotteryService.Application.Services.Interfaces;
using LotteryService.Infrastructure.DAL.DbContextSQL;
using LotteryService.Infrastructure.Repositories;
using LotteryService.Infrastructure.Services.Implementations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LotteryService.Infrastructure
{
    public static class ServiceRegistration
    {
        public static void RegisterServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<ITicketRepository, TicketRepository>();
            services.AddScoped<ILotteryRepository, LotteryRepository>();
            services.AddScoped<IRandomCodesGenerator, RandomCodesGenerator>();
            services.AddSingleton<IBackgroundTaskQueue, BackgroundTaskQueue>();
            services.AddHostedService<QueuedHostedService>();

            services.AddDbContext<AppDbContext>(opt =>
            {
                opt.UseSqlServer(configuration.GetConnectionString("Default"));
            });
        }
    }
}
