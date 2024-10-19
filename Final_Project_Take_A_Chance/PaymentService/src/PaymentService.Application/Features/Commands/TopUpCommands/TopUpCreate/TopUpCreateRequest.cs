using MediatR;
using PaymentService.Application.Utilities.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentService.Application.Features.Commands.TopUpCommands
{
    public class TopUpCreateRequest : IRequest<Result>
    {
        public decimal Amount { get; set; }
        public required string Token { get; set; }
    }
}
