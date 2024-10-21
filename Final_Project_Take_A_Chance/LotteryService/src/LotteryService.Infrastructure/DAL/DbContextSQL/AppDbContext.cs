using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LotteryService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace LotteryService.Infrastructure.DAL.DbContextSQL
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<Lottery> Lotteries { get; set; }
        public DbSet<Ticket> Tickets { get; set; }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var lotteries = ChangeTracker.Entries<Lottery>();
            var tickets = ChangeTracker.Entries<Ticket>();

            foreach (var data in lotteries)
            {
                switch (data.State)
                {
                    case EntityState.Added:
                        data.Entity.CreatedAt = DateTime.UtcNow;
                        data.Entity.IsActive = true;
                        break;
                    default:
                        break;
                }
            }

            foreach (var data in tickets)
            {
                switch (data.State)
                {
                    case EntityState.Added:
                        data.Entity.BoughtAt = DateTime.Now;
                        break;
                    default:
                        break;
                }
            }
            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}
