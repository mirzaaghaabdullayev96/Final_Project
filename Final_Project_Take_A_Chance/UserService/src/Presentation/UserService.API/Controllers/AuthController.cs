using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using UserService.Application.Features.Commands.UsersCommands;
using UserService.Application.Features.Queries.UsersQueries.UserLoginQueries;
using UserService.Application.Utilities.Helpers;
using UserService.Domain.Entities;

namespace UserService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(UserManager<AppUser> userManager, IMediator mediator, RoleManager<IdentityRole> roleManager) : ControllerBase
    {
        [HttpPost("RegisterUser")]
        public async Task<IActionResult> Register([FromForm] UserRegisterCommandRequest request)
        {
            var result = await mediator.Send(request);
            return ActionResponse.HandleResult(this, result);
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromForm] UserLoginRequest request)
        {
            var result = await mediator.Send(request);
            return ActionResponse.HandleResult<UserLoginResponse>(this, result);
        }

        [HttpGet("ConfirmEmail")]
        public async Task<IActionResult> ConfirmEmail([FromQuery]string userId, [FromQuery]string token)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(token))
            {
                return BadRequest("Invalid email confirmation request.");
            }

            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            var result = await userManager.ConfirmEmailAsync(user, token);
            if (result.Succeeded)
            {
                return Ok("Email successfully confirmed!");
            }

            return BadRequest("Email confirmation failed.");
        }

        [HttpPost("Roles")]
        public async Task<IActionResult> CreateRoles()
        {
            IdentityRole role = new IdentityRole("Admin");
            IdentityRole role1 = new IdentityRole("Member");

            await roleManager.CreateAsync(role);
            await roleManager.CreateAsync(role1);

            return Ok();
        }
        
    }
}