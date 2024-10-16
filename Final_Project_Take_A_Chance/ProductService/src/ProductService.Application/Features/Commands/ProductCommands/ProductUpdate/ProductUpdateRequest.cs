using MediatR;
using Microsoft.AspNetCore.Http;
using ProductService.Application.Utilities.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductService.Application.Features.Commands.ProductCommands.ProductUpdate
{
    public class ProductUpdateRequest : IRequest<Result>
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
        public decimal Price { get; set; }
        public List<IFormFile>? Images { get; set; }
        public List<int>? Categories { get; set; }
        public List<int>? OldImagesIds { get; set; }
    }
}
