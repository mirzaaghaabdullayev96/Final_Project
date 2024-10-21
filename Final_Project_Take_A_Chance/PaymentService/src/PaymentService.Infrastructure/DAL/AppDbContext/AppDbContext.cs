using Microsoft.EntityFrameworkCore;
using PaymentService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentService.Infrastructure.DAL.AppDbContext
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<AccountTopUp> TopUps { get; set; }
        public DbSet<Ticket> Tickets { get; set; }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var topups = ChangeTracker.Entries<AccountTopUp>();
            var tickets = ChangeTracker.Entries<Ticket>();

            foreach (var data in topups)
            {
                switch (data.State)
                {
                    case EntityState.Added:
                        data.Entity.CreatedAt = DateTime.Now;
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
