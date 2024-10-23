using MediatR;
using Microsoft.EntityFrameworkCore;
using ProductService.Application.Repositories;
using ProductService.Application.Utilities.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductService.Application.Features.Queries.ProductQueries.ProductGetAll
{
    public class ProductGetAllHandler(IProductRepository productRepository) : IRequestHandler<ProductGetAllRequest, Result<ICollection<ProductGetAllResponse>>>
    {
        public async Task<Result<ICollection<ProductGetAllResponse>>> Handle(ProductGetAllRequest request, CancellationToken cancellationToken)
        {
            var products = await productRepository.GetListAsync(include: x => x.Include(y => y.Images).Include(y => y.ProductCategories).ThenInclude(y => y.Category));

            var productsResponse = products.Select(x => new ProductGetAllResponse()
            {
                Name = x.Name,
                Description = x.Description,
                Price = x.Price,
                Status = x.Status.ToString(),
                Images = x.Images.Select(i => i.ImageURL).ToList(),
                ProductCategories = x.ProductCategories.Select(x => x.Category.Name).ToList()
            }).ToList();

            return new SuccessResult<ICollection<ProductGetAllResponse>>(productsResponse, "Products were retrieved successfully", 200);
        }
    }
}
