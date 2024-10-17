using MediatR;
using ProductService.Application.Utilities.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductService.Application.Features.Commands.ProductCommands.ProductDelete
{
    public class ProductDeleteRequest(int id) : IRequest<Result>
    {
        public int Id { get; set; } = id;
    }
}
