using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using UserService.Application.Features.Commands.UsersCommands.UserRegisterCommands;
using UserService.Application.Features.Queries.UsersQueries.UserLoginQueries;
using UserService.Application.Utilities.Helpers;
using UserService.Domain.Entities;

namespace UserService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMediator _mediator;
        public AuthController(UserManager<AppUser> userManager, IMediator mediator, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _mediator = mediator;
            _roleManager = roleManager;
        }

        [HttpPost("RegisterUser")]
        public async Task<IActionResult> Register([FromForm] UserRegisterCommandRequest request)
        {
            var result = await _mediator.Send(request);

            return result switch
            {
                ErrorResult errorResult => BadRequest(errorResult),
                SuccessResult successResult => Ok(successResult),
                _ => new StatusCodeResult(500)
            };
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromForm] UserLoginRequest request)
        {
            var result = await _mediator.Send(request);

            return result switch
            {
                ErrorResult<UserLoginResponse> errorResult => BadRequest(errorResult),
                SuccessResult<UserLoginResponse> successResult => Ok(successResult),
                _ => new StatusCodeResult(500)
            };
        }

        [HttpGet("ConfirmEmail")]
        public async Task<IActionResult> ConfirmEmail([FromQuery]string userId, [FromQuery]string token)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(token))
            {
                return BadRequest("Invalid email confirmation request.");
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (result.Succeeded)
            {
                return Ok("Email successfully confirmed!");
            }

            return BadRequest("Email confirmation failed.");
        }

    }
}