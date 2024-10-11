using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using UserService.Application.Features.Commands.UsersCommands.UserCreateCommands;
using UserService.Application.Features.Commands.UsersCommands.UserUpdateCommands;
using UserService.Application.Features.Queries.UsersQueries.UserGetAllQueries;
using UserService.Application.Utilities.Helpers;
using UserService.Domain.Entities;

namespace UserService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IMediator _mediator;
        public UsersController(UserManager<AppUser> userManager, IMediator mediator)
        {
            _userManager = userManager;
            _mediator = mediator;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser([FromRoute] string id, [FromForm] UserUpdateCommandRequest request)
        {
            request.UserId = id;
            var result = await _mediator.Send(request);

            return result switch
            {
                ErrorResult errorResult => BadRequest(errorResult),
                SuccessResult successResult => Ok(successResult),
                _ => new StatusCodeResult(500)
            };
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            return Ok(await _mediator.Send(new UserGetAllRequest()));
        }
    }
}
