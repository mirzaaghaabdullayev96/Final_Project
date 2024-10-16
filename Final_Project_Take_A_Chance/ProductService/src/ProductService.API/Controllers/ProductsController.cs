using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductService.Application.Features.Commands.ProductCommands.ProductCreate;
using ProductService.Application.Utilities.Helpers;

namespace ProductService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProductsController(IMediator mediator)
        {
            _mediator = mediator;
        }


        [HttpPost("CreateProduct")]
        public async Task<IActionResult> CreateProduct([FromForm] ProductCreateRequest request)
        {
            var result = await _mediator.Send(request);
            return result switch
            {
                ErrorResult errorResult => BadRequest(errorResult),
                SuccessResult successResult => Ok(successResult),
                _ => new StatusCodeResult(500)
            };
        }
    }
}
