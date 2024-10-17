using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductService.Application.Features.Queries.ProductQueries.ProductGetOne
{
    public class ProductGetOneResponse
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int TicketCount { get; set; }
        public string Status { get; set; }
        public ICollection<string> Images { get; set; }
        public ICollection<string> ProductCategories { get; set; }
    }
}
