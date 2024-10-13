using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserService.Application.Utilities.Helpers;

namespace UserService.Application.Features.Queries.UsersQueries.UserLoginQueries
{
    public class UserLoginRequest :IRequest<Result<UserLoginResponse>>
    {
        public required string Email { get; set; }
        public required string Password { get; set; }
    }
}
