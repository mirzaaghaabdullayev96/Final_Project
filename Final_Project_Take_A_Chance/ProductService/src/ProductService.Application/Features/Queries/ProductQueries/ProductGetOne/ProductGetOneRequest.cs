using MediatR;
using ProductService.Application.Features.Queries.ProductQueries.ProductGetAll;
using ProductService.Application.Utilities.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductService.Application.Features.Queries.ProductQueries.ProductGetOne
{
    public class ProductGetOneRequest(int id) : IRequest<Result<ProductGetOneResponse>>
    {
        public int Id { get; set; } = id;
    }
}
