using Microsoft.EntityFrameworkCore;
using ProductService.Application.Repositories;
using ProductService.Domain.Entities;
using ProductService.Infrastructure.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductService.Infrastructure.Repositories
{
    class ProductCategoryRepository : IProductCategoryRepository
    {
        private readonly AppDbContext _dbContext;

        public ProductCategoryRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public DbSet<ProductCategory> Table => _dbContext.Set<ProductCategory>();
    }
}
