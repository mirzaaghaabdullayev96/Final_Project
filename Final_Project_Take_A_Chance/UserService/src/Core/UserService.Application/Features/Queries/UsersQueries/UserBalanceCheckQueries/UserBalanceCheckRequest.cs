using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserService.Application.Utilities.Helpers;

namespace UserService.Application.Features.Queries.UsersQueries
{
    public class UserBalanceCheckRequest : IRequest<Result>
    {
        public decimal Amount { get; set; }
        public required string UserId { get; set; }
    }
}
