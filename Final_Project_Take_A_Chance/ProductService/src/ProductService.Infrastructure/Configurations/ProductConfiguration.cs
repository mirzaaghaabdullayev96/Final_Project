using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductService.Domain.Entities;
using ProductService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductService.Infrastructure.Configurations
{


    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.Property(r => r.Name)
                    .IsRequired()
                    .HasMaxLength(150);

            builder.Property(r => r.Description)
                    .IsRequired()
                    .HasMaxLength(1500);

            builder.Property(r => r.Price)
                    .IsRequired()
                    .HasColumnType("decimal(18,2)");

            builder.Property(p => p.Status)
                    .HasDefaultValue(ProductStatus.Pending)
                    .HasConversion<string>();

            builder.HasMany(p => p.Images)
                    .WithOne(i => i.Product)
                    .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(p => p.ProductCategories)
                    .WithOne(pc => pc.Product)
                    .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
