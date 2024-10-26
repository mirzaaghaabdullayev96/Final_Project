using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserService.Application.Events;
using UserService.Application.Features.Queries.UsersQueries.UserLoginQueries;
using UserService.Application.Services.Interfaces;
using UserService.Application.Utilities.Helpers;
using UserService.Domain.Entities;
using static System.Net.Mime.MediaTypeNames;

namespace UserService.Application.Features.Queries.UsersQueries
{
    public class UserForgotPasswordHandler : IRequestHandler<UserForgotPasswordRequest, Result<UserForgotPasswordResponse>>
    {

        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly LinkGenerator _linkGenerator;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IEmailService _emailService;
        private readonly IBackgroundTaskQueue _taskQueue;
        private readonly IConnection _rabbitConnection;


        public UserForgotPasswordHandler(UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            IConfiguration configuration,
            LinkGenerator linkGenerator,
            IHttpContextAccessor httpContextAccessor,
            IEmailService emailService,
            IBackgroundTaskQueue taskQueue,
            IConnection rabbitConnection)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _linkGenerator = linkGenerator;
            _httpContextAccessor = httpContextAccessor;
            _emailService = emailService;
            _taskQueue = taskQueue;
            _rabbitConnection = rabbitConnection;
        }

        public async Task<Result<UserForgotPasswordResponse>> Handle(UserForgotPasswordRequest request, CancellationToken cancellationToken)
        {
            AppUser? user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                return new ErrorResult<UserForgotPasswordResponse>("Invalid email.", 400);
            }

            string token = await _userManager.GeneratePasswordResetTokenAsync(user);
            string confirmationLink = _linkGenerator.GetUriByAction(_httpContextAccessor.HttpContext!,
                                                                    action: "ResetPassword",
                                                                    controller: "Users",
                                                                    values: new { email = request.Email, token = token },
                                                                    scheme: "http");

            var userForgotPasswordEvent = new UserForgotPassword
            {
                Email = request.Email,
                Name = user.Name,
                ActivationLink = confirmationLink
            };

            using (var channel = _rabbitConnection.CreateModel())
            {
                channel.QueueDeclare(queue: "user_forgot_password_queue", durable: true, exclusive: false, autoDelete: false, arguments: null);

                var messageBody = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(userForgotPasswordEvent));

                channel.BasicPublish(exchange: "", routingKey: "user_forgot_password_queue", basicProperties: null, body: messageBody);
            }

            UserForgotPasswordResponse userForgotPasswordResponse = new UserForgotPasswordResponse() { AccessToken = token, Email = request.Email };
            return new SuccessResult<UserForgotPasswordResponse>(userForgotPasswordResponse, "Forgot password action done successfully", 200);
        }
    }
}
