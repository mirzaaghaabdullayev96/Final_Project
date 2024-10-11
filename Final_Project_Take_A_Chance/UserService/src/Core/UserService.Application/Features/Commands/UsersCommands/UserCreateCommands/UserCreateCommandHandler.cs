using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using UserService.Application.Utilities.Enums;
using UserService.Application.Utilities.Helpers;
using UserService.Domain.Entities;

namespace UserService.Application.Features.Commands.UsersCommands.UserCreateCommands
{

    public class UserCreateCommandHandler : IRequestHandler<UserCreateCommandRequest, Result>
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IWebHostEnvironment _env;
        public UserCreateCommandHandler(UserManager<AppUser> userManager, IWebHostEnvironment env)
        {
            _userManager = userManager;
            _env = env;
        }

        public async Task<Result> Handle(UserCreateCommandRequest request, CancellationToken cancellationToken)
        {
            if (request.Password != request.ConfirmPassword)
            {
                return new ErrorResult("Passwords do not match", 400, nameof(request.Password));
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

            if (_userManager.Users.Any(x => x.UserName == request.Username))
            {
                return new ErrorResult("Username already exists", 400, nameof(request.Username));
            }


            string emailPattern = @"^[^\s@]{1,100}@[^\s@]+\.[^\s@]+$";
            var regex = new Regex(emailPattern);
            if (!regex.IsMatch(request.Email))
            {
                return new ErrorResult("Invalid email format", 400, nameof(request.Email));
            }

            AppUser appUser = new AppUser()
            {
                Email = request.Email,
                Name = request.Name,
                Surname = request.Surname,
                UserName = request.Username,
                BirthDate = request.BirthDate
            };

            var result = await _userManager.CreateAsync(appUser, request.Password);
            if (!result.Succeeded)
            {
                return new ErrorResult("", 400) { Messages = result.Errors.Select(x => x.Description).ToList() };
            }
            await _userManager.AddToRoleAsync(appUser, "Member");

            if (request.ProfilePicture != null)
            {
                appUser.ProfilePicture = await request.ProfilePicture.CreateFileAsync(_env.WebRootPath, "UsersProfileImages");
            }

            return new SuccessResult("User successfully created", 201);
        }
    }
}
