using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserService.Application.Utilities.Helpers;

namespace UserService.Application.Features.Commands.UsersCommands
{
    public class UserResetPasswordRequest : IRequest<Result>
    {
        public required string Token { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
        public required string ConfirmPassword { get; set; }
    }
}
