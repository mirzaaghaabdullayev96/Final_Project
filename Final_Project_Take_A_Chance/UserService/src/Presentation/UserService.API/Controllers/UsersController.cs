﻿using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using UserService.Application.Features.Commands.UsersCommands;
using UserService.Application.Features.Queries.UsersQueries;
using UserService.Application.Features.Queries.UsersQueries.UserGetByIdQueries;
using UserService.Application.Utilities.Helpers;
using UserService.Domain.Entities;

namespace UserService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController(IMediator mediator) : Controller
    {   
       
        [HttpPut("UpdateUser{id}")]
        public async Task<IActionResult> UpdateUser([FromRoute] string id, [FromForm] UserUpdateCommandRequest request)
        {
            request.UserId = id;
            var result = await mediator.Send(request);
            return ActionResponse.HandleResult(this, result);
        }

        //[Authorize(Roles = "Admin")]
        [HttpGet("GetAllUsers")]
        public async Task<IActionResult> GetAllUsers()
        {
            var result = await mediator.Send(new UserGetAllRequest());
            return ActionResponse.HandleResult<ICollection<UserGetAllResponse>>(this, result);
        }

        [HttpGet("GetUserById/{id}")]
        public async Task<IActionResult> GetUserById([FromRoute] string id)
        {
            var result = await mediator.Send(new UserGetByIdRequest() { Id=id});
            return ActionResponse.HandleResult<UserGetByIdResponse>(this, result);
        }

        [HttpPost("ForgotPassword")]
        public async Task<IActionResult> ForgotPassword([FromBody] UserForgotPasswordRequest request)
        {
            var result = await mediator.Send(request);
            return ActionResponse.HandleResult<UserForgotPasswordResponse>(this, result);
        }

        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPassword([FromBody] UserResetPasswordRequest request)
        {
            var result = await mediator.Send(request);
            return ActionResponse.HandleResult(this, result);
        }

        [HttpPut("ChangePassword")]
        public async Task<IActionResult> ChangePassword([FromBody] UserChangePasswordRequest request)
        {
            var result = await mediator.Send(request);
            return ActionResponse.HandleResult(this, result);
        }

        [HttpPut("ChangeBalance")]
        public async Task<IActionResult> ChangeBalance([FromBody] UserBalanceChangeRequest request)
        {
            var result = await mediator.Send(request);
            return ActionResponse.HandleResult(this, result);
        }

        [HttpPost("CheckBalance")]
        public async Task<IActionResult> CheckBalance([FromBody] UserBalanceCheckRequest request)
        {
            var result = await mediator.Send(request);
            return ActionResponse.HandleResult(this, result);
        }
    }
}
