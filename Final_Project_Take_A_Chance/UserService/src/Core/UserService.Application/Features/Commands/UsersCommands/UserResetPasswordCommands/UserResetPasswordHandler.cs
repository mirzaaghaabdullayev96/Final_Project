using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserService.Application.Features.Queries.UsersQueries.UserForgotPasswordQueries;
using UserService.Application.Utilities.Helpers;
using UserService.Domain.Entities;

namespace UserService.Application.Features.Commands.UsersCommands.UserResetPasswordCommands
{
    public class UserResetPasswordHandler : IRequestHandler<UserResetPasswordRequest, Result>
    {
        private readonly UserManager<AppUser> _userManager;
        public UserResetPasswordHandler(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }
        public async Task<Result> Handle(UserResetPasswordRequest request, CancellationToken cancellationToken)
        {
            AppUser? user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                return new ErrorResult("Invalid email.", 400);
            }

            if (request.Password != request.ConfirmPassword)
            {
                return new ErrorResult("Passwords do not match", 400, nameof(request.Password));
            }

            var result = await _userManager.ResetPasswordAsync(user, request.Token, request.Password);

            if (!result.Succeeded)
            {
                return new ErrorResult("Failed to reset password", 400);
            }

            return new SuccessResult("Password was reseted successfully", 200);
        }
    }
}
