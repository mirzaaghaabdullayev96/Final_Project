using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserService.Application.Utilities.Enums;
using UserService.Application.Utilities.Helpers;

namespace UserService.Application.Features.Commands.UsersCommands
{
    public class UserBalanceChangeRequest : IRequest<Result>
    {
        public decimal Amount { get; set; }
        public TypeOpOperation TypeOpOperation { get; set; }
    }
}
