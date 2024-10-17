using MediatR;
using ProductService.Application.Repositories;
using ProductService.Application.Utilities.Helpers;
using ProductService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductService.Application.Features.Commands.CategoryCommands.CategoryUpdate
{
    public class CategoryUpdateHandler(ICategoryRepository categoryRepository) : IRequestHandler<CategoryUpdateRequest, Result>
    {
        public async Task<Result> Handle(CategoryUpdateRequest request, CancellationToken cancellationToken)
        {
            if (request.Id < 1) return new ErrorResult("Id can not be less than 1", 400);
            Category? category = await categoryRepository.GetAsync(request.Id);
            if (category is null) return new ErrorResult("Category was not found",400);
            category.Name = request.Name;
            await categoryRepository.UpdateAsync(category);
            return new SuccessResult("Category was updated successfully", 204);
        }
    }
}
