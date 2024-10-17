using ProductService.Domain.Entities;
using ProductService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductService.Application.Features.Queries.ProductQueries.ProductGetAll
{
    public class ProductGetAllResponse
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
