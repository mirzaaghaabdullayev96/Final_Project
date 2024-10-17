using MediatR;
using ProductService.Application.Repositories;
using ProductService.Application.Utilities.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductService.Application.Features.Commands.ProductCommands.ProductDelete
{
    public class ProductDeleteHandler : IRequestHandler<ProductDeleteRequest, Result>
    {

        private readonly IProductRepository _productRepository;

        public ProductDeleteHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }
        public async Task<Result> Handle(ProductDeleteRequest request, CancellationToken cancellationToken)
        {
            var product = await _productRepository.GetAsync(request.Id);
            if (product == null) return new ErrorResult("Product not found", 404);
            product.IsDeleted= true;
            await _productRepository.UpdateAsync(product);
            return new SuccessResult("Product deleted successfully", 200);
        }
    }
}
