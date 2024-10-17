using MediatR;
using ProductService.Application.Utilities.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductService.Application.Features.Commands.CategoryCommands.CategoryCreate
{
    public class CategoryCreateRequest : IRequest<Result>
    {
        public required string Name { get; set; }
    }
}
