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

namespace UserService.Application.Features.Commands.UsersCommands.UserChangePasswordCommands
{
    public class UserChangePasswordHandler : IRequestHandler<UserChangePasswordRequest, Result>
    {
        private readonly UserManager<AppUser> _userManager;
        public UserChangePasswordHandler(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }
        public async Task<Result> Handle(UserChangePasswordRequest request, CancellationToken cancellationToken)
        {
            AppUser? user = await _userManager.FindByIdAsync(request.Id);
            if (user == null)
            {
                return new ErrorResult("Invalid id.", 400);
            }

            if (!await _userManager.CheckPasswordAsync(user, request.CurrentPassword))
            {
                return new ErrorResult("Current password is wrong", 400, "CurrentPassword");
            }

            if (request.NewPassword != request.ConfirmNewPassword)
            {
                return new ErrorResult("Passwords do not match", 400, nameof(request.NewPassword));
            }

            var result = await _userManager.ChangePasswordAsync(user, request.CurrentPassword, request.NewPassword);

            if (!result.Succeeded)
            {
                return new ErrorResult("", 400) { Messages = result.Errors.Select(x => x.Description).ToList() };
            }

            return new SuccessResult("Changed password successfully", 200);
        }
    }
}
