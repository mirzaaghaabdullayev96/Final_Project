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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Image = ProductService.Domain.Entities.Image;

namespace ProductService.Application.Features.Commands.ProductCommands.ProductUpdate
{
    public class ProductUpdateHandler : IRequestHandler<ProductUpdateRequest, Result>
    {
        private readonly IProductRepository _productRepository;
        private readonly IWebHostEnvironment _env;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IProductCategoryRepository _productCategoryRepository;


        public ProductUpdateHandler(IProductRepository productRepository, IWebHostEnvironment env, ICategoryRepository categoryRepository, IProductCategoryRepository productCategoryRepository)
        {
            _productRepository = productRepository;
            _env = env;
            _categoryRepository = categoryRepository;
            _productCategoryRepository = productCategoryRepository;
        }


        public async Task<Result> Handle(ProductUpdateRequest request, CancellationToken cancellationToken)
        {
            if (request.Id < 1) return new ErrorResult("Id can not be less than 1", 400);
            Product? product = await _productRepository.GetAsync(x => x.Id == request.Id, x => x.Include(y => y.ProductCategories!).ThenInclude(pc => pc.Category!).Include(y => y.Images));

            if (product == null) return new ErrorResult("Product was not found", 400);  
            if (request.Price <= 0) return new ErrorResult("Price must be more than 0", 400, "Price");
            if (request.Name.Length > 150) return new ErrorResult("Name length should be less than 150", 400, "Name");
            if (request.Description.Length > 1500) return new ErrorResult("Description length should be less than 150", 400, "Description");

            if (request.Images is not null)
            {
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
            }


            if (request.Categories is not null)
            {
                foreach (var category in request.Categories)
                {
                    if (!await _categoryRepository.Table.AnyAsync(x => x.Id == category)) return new ErrorResult("Category by this Id not found", 400, "Category");
                }

                foreach (var category in request.Categories)
                {
                    if (product.ProductCategories.Any(x => x.CategoryId == category)) continue;
                    product.ProductCategories.Add(new ProductCategory { CategoryId = category });
                }

                _productCategoryRepository.Table.RemoveRange(product.ProductCategories.Where(mg => !request.Categories.Exists(gId => gId == mg.CategoryId)).ToList());
            }

            if (request.Images is not null)
            {
                foreach (var image in request.Images)
                {
                    Image productImage = new()
                    {
                        ImageURL = await image.CreateFileAsync(_env.WebRootPath, "ProductImages")
                    };
                    product.Images.Add(productImage);
                }
            }


            product.Name = request.Name;
            product.Description = request.Description;
            product.Price = request.Price;


            request.OldImagesIds ??= [];

            List<Image> deleteImages = product.Images.Where(pi => !request.OldImagesIds.Exists(tId => tId == pi.Id)).ToList();
            foreach (var item in deleteImages)
            {
                item.ImageURL.DeleteFile(_env.WebRootPath, "assets", "myProducts", "productImages");
                product.Images.Remove(item);
            }

            await _productRepository.UpdateAsync(product);

            return new SuccessResult("Product was updated successfully", 204);
        }
    }
}
