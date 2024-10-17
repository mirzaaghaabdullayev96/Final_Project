using MediatR;
using ProductService.Application.Utilities.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductService.Application.Features.Commands.CategoryCommands.CategoryDelete
{
    public class CategoryDeleteRequest(int id) : IRequest<Result>
    {
        public int Id { get; set; } = id;
    }
}
