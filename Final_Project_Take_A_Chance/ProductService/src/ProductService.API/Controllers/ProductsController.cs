using Azure.Core;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductService.Application.Features.Commands.ProductCommands.ProductCreate;
using ProductService.Application.Features.Commands.ProductCommands.ProductDelete;
using ProductService.Application.Features.Commands.ProductCommands.ProductUpdate;
using ProductService.Application.Features.Queries.ProductQueries.ProductGetAll;
using ProductService.Application.Features.Queries.ProductQueries.ProductGetOne;
using ProductService.Application.Utilities.Helpers;

namespace ProductService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController(IMediator mediator) : ControllerBase
    {
        [HttpPost("CreateProduct")]
        public async Task<IActionResult> CreateProduct([FromForm] ProductCreateRequest request)
        {
            var result = await mediator.Send(request);
            return ActionResponse.HandleResult(this, result);
        }

        [HttpPut("UpdateProduct{id}")]
        public async Task<IActionResult> UpdateProduct([FromRoute] int id, [FromForm] ProductUpdateRequest request)
        {
            request.Id = id;
            var result = await mediator.Send(request);
            return ActionResponse.HandleResult(this, result);
        }

        [HttpPut("DeleteProduct/{id}")]
        public async Task<IActionResult> DeleteProduct([FromRoute] int id)
        {
            var result = await mediator.Send(new ProductDeleteRequest(id));
            return ActionResponse.HandleResult(this, result);
        }

        [HttpGet("GetAllProducts")]
        public async Task<IActionResult> GetAllProducts()
        {
            return Ok(await mediator.Send(new ProductGetAllRequest()));
        }

        [HttpGet("GetProductById/{id}")]
        public async Task<IActionResult> GetProductById([FromRoute] int id)
        {
            var result = await mediator.Send(new ProductGetOneRequest(id));
            return ActionResponse.HandleResult<ProductGetOneResponse>(this, result);
        }

    }
}
