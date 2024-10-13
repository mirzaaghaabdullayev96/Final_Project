using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using UserService.Application.Features.Commands.UsersCommands.UserChangePasswordCommands;
using UserService.Application.Features.Commands.UsersCommands.UserRegisterCommands;
using UserService.Application.Features.Commands.UsersCommands.UserResetPasswordCommands;
using UserService.Application.Features.Commands.UsersCommands.UserUpdateCommands;
using UserService.Application.Features.Queries.UsersQueries.UserForgotPasswordQueries;
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

        [HttpPut("UpdateUser{id}")]
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

        //[Authorize(Roles = "Admin")]
        [HttpGet("GetAllUsers")]
        public async Task<IActionResult> GetAllUsers()
        {
            return Ok(await _mediator.Send(new UserGetAllRequest()));
        }

        [HttpPost("ForgotPassword")]
        public async Task<IActionResult> ForgotPassword([FromBody] UserForgotPasswordRequest request)
        {
            var result = await _mediator.Send(request);

            return result switch
            {
                ErrorResult<UserForgotPasswordResponse> errorResult => BadRequest(errorResult),
                SuccessResult<UserForgotPasswordResponse> successResult => Ok(successResult),
                _ => new StatusCodeResult(500)
            };
        }

        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPassword([FromBody]UserResetPasswordRequest request)
        {
            var result = await _mediator.Send(request);

            return result switch
            {
                ErrorResult errorResult => BadRequest(errorResult),
                SuccessResult successResult => Ok(successResult),
                _ => new StatusCodeResult(500)
            };
        }

        [HttpPost("ChangePassword")]
        public async Task<IActionResult> ChangePassword([FromForm] UserChangePasswordRequest request)
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
