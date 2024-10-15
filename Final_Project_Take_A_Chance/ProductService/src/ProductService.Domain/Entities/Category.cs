using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductService.Domain.Entities
{
    public class Category : BaseEntity
    {
        public required string Name { get; set; }

        //relational
        public ICollection<ProductCategory> ProductCategories { get; set; }
    }
}
