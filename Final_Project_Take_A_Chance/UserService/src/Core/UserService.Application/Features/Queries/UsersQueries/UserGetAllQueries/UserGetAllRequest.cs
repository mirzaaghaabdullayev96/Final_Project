using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserService.Application.Utilities.Helpers;

namespace UserService.Application.Features.Queries.UsersQueries.UserGetAllQueries
{
    public class UserGetAllRequest : IRequest<Result<ICollection<UserGetAllResponse>>>
    {

    }
}
