using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductService.Infrastructure.Configurations
{
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.Property(r => r.Name)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.HasMany(p => p.ProductCategories)
                   .WithOne(pc=>pc.Category)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
