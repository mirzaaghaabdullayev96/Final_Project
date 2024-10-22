using MediatR;
using Microsoft.AspNetCore.WebUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserService.Application.Features.Queries.UsersQueries.UserLoginQueries;
using UserService.Application.Utilities.Helpers;

namespace UserService.Application.Features.Queries.UsersQueries
{
    public class UserForgotPasswordRequest : IRequest<Result<UserForgotPasswordResponse>>
    {
        public required string Email { get; set; }
    }
}
