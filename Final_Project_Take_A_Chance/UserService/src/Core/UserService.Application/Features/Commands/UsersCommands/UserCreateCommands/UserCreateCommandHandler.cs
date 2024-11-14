using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Routing;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using UserService.Application.Events;
using UserService.Application.Services.Interfaces;
using UserService.Application.Utilities.Enums;
using UserService.Application.Utilities.Helpers;
using UserService.Domain.Entities;

namespace UserService.Application.Features.Commands.UsersCommands.UserRegisterCommands
{

    public class UserRegisterCommandHandler : IRequestHandler<UserRegisterCommandRequest, Result>
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IWebHostEnvironment _env;
        private readonly LinkGenerator _linkGenerator;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IEmailService _emailService;
        private readonly IBackgroundTaskQueue _taskQueue;
        private readonly IConnection _rabbitConnection;

        public UserRegisterCommandHandler(UserManager<AppUser> userManager, IWebHostEnvironment env, LinkGenerator linkGenerator, IHttpContextAccessor httpContextAccessor, IEmailService emailService, IBackgroundTaskQueue taskQueue, IConnection rabbitConnection)
        {
            _userManager = userManager;
            _env = env;
            _linkGenerator = linkGenerator;
            _httpContextAccessor = httpContextAccessor;
            _emailService = emailService;
            _taskQueue = taskQueue;
            _rabbitConnection = rabbitConnection;
        }

        public async Task<Result> Handle(UserRegisterCommandRequest request, CancellationToken cancellationToken)
        {
            if (_userManager.Users.Any(x => x.Email == request.Email))
            {
                return new ErrorResult("Email already exists", 400, nameof(request.Email));
            }

            if (request.Password != request.ConfirmPassword)
            {
                return new ErrorResult("Passwords do not match", 400, nameof(request.Password));
            }

            if (_userManager.Users.Any(x => x.UserName == request.Username))
            {
                return new ErrorResult("Username already exists", 400, nameof(request.Username));
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

            var result = await _userManager.CreateAsync(appUser, "Salam123@");
            if (!result.Succeeded)
            {
                return new ErrorResult("", 400) { Message = result.Errors.FirstOrDefault().ToString() };
            }
            await _userManager.AddToRoleAsync(appUser, "Member");

            if (request.ProfilePicture != null)
            {
                appUser.ProfilePicture = await request.ProfilePicture.CreateFileAsync(_env.WebRootPath, "UsersProfileImages");
            }

            var emailToken = await _userManager.GenerateEmailConfirmationTokenAsync(appUser);

            string confirmationLink = _linkGenerator.GetUriByAction(
            _httpContextAccessor.HttpContext!,
            action: "ConfirmEmail",
            controller: "Auth",
            values: new { userId = appUser.Id, token = emailToken },
            scheme: "http");

            string text = "To confirm your Email, please click to the link below";


            var userCreatedEvent = new UserCreatedEvent
            {
                Email = request.Email,
                Name = appUser.Name,
                ActivationLink = confirmationLink
            };


            using (var channel = _rabbitConnection.CreateModel())
            {
                channel.QueueDeclare(queue: "user_created_queue", durable: true, exclusive: false, autoDelete: false, arguments: null);

                var messageBody = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(userCreatedEvent));

                channel.BasicPublish(exchange: "", routingKey: "user_created_queue", basicProperties: null, body: messageBody);
            }

            return new SuccessResult("User successfully created", 201);
        }
    }
}
