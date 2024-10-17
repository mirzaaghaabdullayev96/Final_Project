using MediatR;
using ProductService.Application.Features.Commands.ProductCommands.ProductCreate;
using ProductService.Application.Repositories;
using ProductService.Application.Utilities.Helpers;
using ProductService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductService.Application.Features.Commands.CategoryCommands.CategoryCreate
{
    public class CategoryCreateHandler(ICategoryRepository categoryRepository) : IRequestHandler<CategoryCreateRequest, Result>
    {
        public async Task<Result> Handle(CategoryCreateRequest request, CancellationToken cancellationToken)
        {
            if (request.Name.Length > 150) return new ErrorResult("Name length should be less than 150", 400, "Name");
            Category category = new() { Name = request.Name };
            await categoryRepository.CreateAsync(category);
            return new SuccessResult("Category created successfully", 201);
        }
    }   
}
