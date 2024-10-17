using MediatR;
using ProductService.Application.Utilities.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductService.Application.Features.Queries.ProductQueries.ProductGetAll
{
    public class ProductGetAllRequest :IRequest<Result<ICollection<ProductGetAllResponse>>>
    {
    }
}
