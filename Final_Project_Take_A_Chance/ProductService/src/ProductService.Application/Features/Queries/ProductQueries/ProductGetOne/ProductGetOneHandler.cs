using MediatR;
using Microsoft.EntityFrameworkCore;
using ProductService.Application.Features.Queries.ProductQueries.ProductGetAll;
using ProductService.Application.Repositories;
using ProductService.Application.Utilities.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductService.Application.Features.Queries.ProductQueries.ProductGetOne
{
    public class ProductGetOneHandler : IRequestHandler<ProductGetOneRequest,Result<ProductGetOneResponse>>
    {
        private readonly IProductRepository _productRepository;
        public ProductGetOneHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<Result<ProductGetOneResponse>> Handle(ProductGetOneRequest request, CancellationToken cancellationToken)
        {
            var product = await _productRepository.GetAsync(predicate:x=>x.Id==request.Id, include: x => x.Include(y => y.Images).Include(y => y.ProductCategories).ThenInclude(y => y.Category));

            if (product == null) return new ErrorResult<ProductGetOneResponse>("Product by this Id was not found", 400);

            var productResponse = new ProductGetOneResponse()
            {
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                TicketCount = product.TicketCount,
                Status = product.Status.ToString(),
                Images = product.Images.Select(i => i.ImageURL).ToList(),
                ProductCategories = product.ProductCategories.Select(x => x.Category.Name).ToList()
            };

            return new SuccessResult<ProductGetOneResponse>(productResponse, "Product was retrieved successfully", 200);
        }
    }
}
