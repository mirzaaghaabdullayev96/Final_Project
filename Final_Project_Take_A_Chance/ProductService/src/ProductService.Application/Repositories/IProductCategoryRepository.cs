using Microsoft.EntityFrameworkCore;
using ProductService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductService.Application.Repositories
{
    public interface IProductCategoryRepository 
    {
        public DbSet<ProductCategory> Table { get; }
    }
}
