using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using UserService.Application.Features.Commands.UsersCommands.UserRegisterCommands;
using UserService.Application.Utilities.Enums;
using UserService.Application.Utilities.Helpers;
using UserService.Domain.Entities;

namespace UserService.Application.Features.Commands.UsersCommands.UserUpdateCommands
{
    public class UserUpdateCommandHandler : IRequestHandler<UserUpdateCommandRequest, Result>
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IWebHostEnvironment _env;
        public UserUpdateCommandHandler(UserManager<AppUser> userManager, IWebHostEnvironment env)
        {
            _userManager = userManager;
            _env = env;
        }

        public async Task<Result> Handle(UserUpdateCommandRequest request, CancellationToken cancellationToken)
        {
            AppUser user = await _userManager.FindByIdAsync(request.UserId);
            if (user is null)
            {
                return new ErrorResult("User not found", 404);
            }

            if (request.ProfilePicture is not null)
            {
                if (!request.ProfilePicture.ValidateType("image/jpeg") && !request.ProfilePicture.ValidateType("image/png"))
                {
                    return new ErrorResult("Image type must be png or jpeg/jpg", 400, nameof(request.ProfilePicture));
                }

                if (!request.ProfilePicture.ValidateSize(FileSize.MB, 3))
                {
                    return new ErrorResult("Image size must be less than 2MB", 400, nameof(request.ProfilePicture));
                }
            }

            if (_userManager.Users.Any(x => x.UserName == request.Username && x.Id != request.UserId))
            {
                return new ErrorResult("Username already exists", 400, nameof(request.Username));
            }

            user.Name = request.Name;
            user.Surname = request.Surname;
            user.UserName = request.Username;

            user.ProfilePicture?.DeleteFile(_env.WebRootPath, "UsersProfileImages");


            if (request.ProfilePicture != null)
            {
                user.ProfilePicture = await request.ProfilePicture.CreateFileAsync(_env.WebRootPath, "UsersProfileImages");
            }

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                return new ErrorResult("Failed to update user", 500);
            }

            return new SuccessResult("User successfully updated", 204);
        }
    }
}
