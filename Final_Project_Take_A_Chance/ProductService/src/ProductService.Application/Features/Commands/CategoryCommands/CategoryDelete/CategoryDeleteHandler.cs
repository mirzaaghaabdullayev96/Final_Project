using MediatR;
using ProductService.Application.Repositories;
using ProductService.Application.Utilities.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductService.Application.Features.Commands.CategoryCommands.CategoryDelete
{
    public class CategoryDeleteHandler(ICategoryRepository categoryRepository) : IRequestHandler<CategoryDeleteRequest, Result>
    {
        public async Task<Result> Handle(CategoryDeleteRequest request, CancellationToken cancellationToken)
        {
            var category = await categoryRepository.GetAsync(request.Id);
            if (category == null) return new ErrorResult("Category was not found", 404);
            category.IsDeleted = true;
            await categoryRepository.UpdateAsync(category);
            return new SuccessResult("Category deleted successfully", 204);
        }
    }
}
