using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserService.Application.Features.Queries.UsersQueries.UserLoginQueries;
using UserService.Application.Services.Interfaces;
using UserService.Application.Utilities.Helpers;
using UserService.Domain.Entities;
using static System.Net.Mime.MediaTypeNames;

namespace UserService.Application.Features.Queries.UsersQueries
{
    public class UserForgotPasswordHandler: IRequestHandler<UserForgotPasswordRequest, Result<UserForgotPasswordResponse>>
    {

        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly LinkGenerator _linkGenerator;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IEmailService _emailService;
        private readonly IBackgroundTaskQueue _taskQueue;


        public UserForgotPasswordHandler(UserManager<AppUser> userManager, 
            SignInManager<AppUser> signInManager, 
            IConfiguration configuration, 
            LinkGenerator linkGenerator, 
            IHttpContextAccessor httpContextAccessor, 
            IEmailService emailService, 
            IBackgroundTaskQueue taskQueue)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _linkGenerator = linkGenerator;
            _httpContextAccessor = httpContextAccessor;
            _emailService = emailService;
            _taskQueue = taskQueue;
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

            string text = "To reset your password, please click to the link below";

            //await _emailService.SendMailAsync(request.Email, "Forgot Password", user.Name, confirmationLink);
            _taskQueue.QueueBackgroundWorkItem(async tokenCancellation =>
            {
                await _emailService.SendMailAsync(request.Email, "Email Confirmation", user.Name, token: confirmationLink, text: text);
            });

            UserForgotPasswordResponse userForgotPasswordResponse = new UserForgotPasswordResponse() { AccessToken = token, Email = request.Email };
            return new SuccessResult<UserForgotPasswordResponse>(userForgotPasswordResponse, "Forgot password action done successfully", 200);
        }
    }
}
