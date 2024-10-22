using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserService.Application.Features.Queries.UsersQueries
{
    public class UserForgotPasswordResponse
    {
        public required string AccessToken { get; set; }
        public required string Email { get; set; }
    }
}
