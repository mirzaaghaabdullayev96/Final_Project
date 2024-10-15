using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductService.Domain.Entities
{
    public class Image : BaseEntity
    {
        public required string ImageURL { get; set; }

        //relational
        public int ProductId { get; set; }
        public Product Product { get; set; }
    }
}
