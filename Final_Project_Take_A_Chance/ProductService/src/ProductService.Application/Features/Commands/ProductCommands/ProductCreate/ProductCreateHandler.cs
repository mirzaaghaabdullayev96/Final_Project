using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using ProductService.Application.Repositories;
using ProductService.Application.Utilities.Enums;
using ProductService.Application.Utilities.Helpers;
using ProductService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductService.Application.Features.Commands.ProductCommands.ProductCreate
{
    public class ProductCreateHandler : IRequestHandler<ProductCreateRequest, Result>
    {
        private readonly IProductRepository _productRepository;
        private readonly IWebHostEnvironment _env;
        private readonly ICategoryRepository _categoryRepository;

        public ProductCreateHandler(IProductRepository productRepository, IWebHostEnvironment env, ICategoryRepository categoryRepository)
        {
            _productRepository = productRepository;
            _env = env;
            _categoryRepository = categoryRepository;
        }
        public async Task<Result> Handle(ProductCreateRequest request, CancellationToken cancellationToken)
        {
            if (request.Images == null) return new ErrorResult("You must add at least one image", 400);

            foreach (var image in request.Images)
            {
                if (!image.ValidateType("image/jpeg") && !image.ValidateType("image/png"))
                {
                    return new ErrorResult("Image type must be png or jpeg/jpg", 400);
                }

                if (!image.ValidateSize(FileSize.MB, 3))
                {
                    return new ErrorResult("Image size must be less than 2MB", 400);
                }
            }

            if (request.Price <= 0) return new ErrorResult("Price must be more than 0", 400, "Price");
            if (request.Name.Length > 150) return new ErrorResult("Name length should be less than 150", 400, "Name");
            if (request.Description.Length > 1500) return new ErrorResult("Description length should be less than 150", 400, "Description");

            foreach (var category in request.Categories)
            {
                if (!await _categoryRepository.Table.AnyAsync(x => x.Id == category)) return new ErrorResult("Category by this Id not found", 400, "Category");
            }

            var images = new List<Image>();
            foreach (var image in request.Images)
            {
                Image productImage = new Image
                {
                    ImageURL = await image.CreateFileAsync(_env.WebRootPath, "ProductImages")
                };
                images.Add(productImage);
            }

            Product product = new()
            {
                Name = request.Name,
                Description = request.Description,
                Price = request.Price,
                Images = images,
                ProductCategories = request.Categories.Select(c => new ProductCategory { CategoryId = c }).ToList()
            };

            await _productRepository.CreateAsync(product);
            return new SuccessResult("Product created successfully", 201);
        }
    }
}
