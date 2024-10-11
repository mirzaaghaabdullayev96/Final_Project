using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using UserService.Application.Features.Commands.UsersCommands.UserCreateCommands;
using UserService.Application.Utilities.Helpers;
using UserService.Domain.Entities;

namespace UserService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IMediator _mediator;
        public AuthController(UserManager<AppUser> userManager, IMediator mediator)
        {
            _userManager = userManager;
            _mediator = mediator;
        }

        [HttpPost("CreateUser")]
        public async Task<IActionResult> CreateUser([FromForm] UserCreateCommandRequest request)
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