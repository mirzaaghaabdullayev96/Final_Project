using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserService.Application.Utilities.Helpers;

namespace UserService.Application.Features.Queries.UsersQueries.UserGetByIdQueries
{
    public class UserGetByIdRequest : IRequest<Result<UserGetByIdResponse>>
    {
        public string Id { get; set; }
    }
}
