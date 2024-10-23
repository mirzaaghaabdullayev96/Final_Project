using ProductService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductService.Domain.Entities
{
    public class Product : BaseEntity
    {
        public required string Name { get; set; }
        public required string Description { get; set; }
        public decimal Price { get; set; }
        public ProductStatus Status { get; set; }

        //relational
        public ICollection<Image> Images { get; set; }
        public ICollection<ProductCategory> ProductCategories { get; set; }

    }
}
