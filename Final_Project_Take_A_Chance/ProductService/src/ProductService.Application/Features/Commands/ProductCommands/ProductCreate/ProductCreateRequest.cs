using MediatR;
using Microsoft.AspNetCore.Http;
using ProductService.Application.Utilities.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductService.Application.Features.Commands.ProductCommands.ProductCreate
{
    public class ProductCreateRequest : IRequest<Result>
    {
        public required string Name { get; set; }
        public required string Description { get; set; }
        public decimal Price { get; set; }
        public required List<IFormFile> Images { get; set; }
        public required List<int> Categories { get; set; }

    }
}
