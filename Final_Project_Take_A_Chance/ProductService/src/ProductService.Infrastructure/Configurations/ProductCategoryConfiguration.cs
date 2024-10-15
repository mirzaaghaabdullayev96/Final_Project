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
    public class ProductCategoryConfiguration : IEntityTypeConfiguration<ProductCategory>
    {
        public void Configure(EntityTypeBuilder<ProductCategory> builder)
        {
            builder.HasKey(mg => new { mg.ProductId, mg.CategoryId });

            builder.HasOne(mg => mg.Product)
                   .WithMany(m => m.ProductCategories)
                   .HasForeignKey(mg => mg.ProductId);


            builder.HasOne(mg => mg.Category)
                   .WithMany(g => g.ProductCategories)
                   .HasForeignKey(mg => mg.CategoryId);
        }
    }
}
