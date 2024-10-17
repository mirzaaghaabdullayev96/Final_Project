using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductService.Application.Features.Commands.CategoryCommands.CategoryCreate;
using ProductService.Application.Features.Commands.CategoryCommands.CategoryDelete;
using ProductService.Application.Features.Commands.CategoryCommands.CategoryUpdate;
using ProductService.Application.Utilities.Helpers;

namespace CategoryService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController(IMediator mediator) : ControllerBase
    {

        [HttpPost("CreateCategory")]
        public async Task<IActionResult> CreateCategory([FromForm] CategoryCreateRequest request)
        {
            var result = await mediator.Send(request);
            return ActionResponse.HandleResult(this, result);
        }

        [HttpPut("UpdateCategory{id}")]
        public async Task<IActionResult> UpdateCategory([FromRoute] int id, [FromForm] CategoryUpdateRequest request)
        {
            var result = await mediator.Send(new CategoryUpdateRequest() { Id= id, Name=request.Name});
            return ActionResponse.HandleResult(this, result);
        }

        [HttpPut("DeleteCategory/{id}")]
        public async Task<IActionResult> DeleteCategory([FromRoute] int id)
        {
            var result = await mediator.Send(new CategoryDeleteRequest(id));
            return ActionResponse.HandleResult(this, result);
        }
    }
}
