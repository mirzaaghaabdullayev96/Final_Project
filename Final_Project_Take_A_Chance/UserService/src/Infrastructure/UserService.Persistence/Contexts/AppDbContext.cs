using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserService.Domain.Entities;

namespace UserService.Persistence.Contexts
{
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<AppUser> Users { get; set; }
        public DbSet<AccountTopUp> TransactionsBalanceTopUp { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AppUser>()
                 .Property(u => u.BirthDate)
                 .HasColumnType("date");

            modelBuilder.Entity<AppUser>()
                 .Property(u => u.Account)
                 .HasDefaultValue(0.0m)
                 .HasColumnType("decimal(18,2)");


            modelBuilder.Entity<AppUser>()
                .Property(u => u.Email)
                .IsRequired()
                .HasMaxLength(128);

            modelBuilder.Entity<AppUser>()
                .Property(u => u.Name)
                .IsRequired()
                .HasMaxLength(128);

            modelBuilder.Entity<AccountTopUp>()
                 .Property(u => u.Amount)
                 .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<AccountTopUp>()
                .HasKey(x => x.TransactionId);

            modelBuilder.Entity<AccountTopUp>()
                .HasOne(x => x.AppUser)
                .WithMany(u => u.AccountTopUps)
                .HasForeignKey(x => x.UserId)
                .IsRequired();

            base.OnModelCreating(modelBuilder);

        }



        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var datas = ChangeTracker.Entries<AppUser>();

            foreach (var data in datas)
            {
                switch (data.State)
                {
                    case EntityState.Added:
                        data.Entity.CreatedAt = DateTime.Now;
                        data.Entity.UpdatedAt = DateTime.Now;
                        break;
                    case EntityState.Modified:
                        data.Entity.UpdatedAt = DateTime.Now;
                        break;
                    default:
                        break;
                }
            }

            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}
