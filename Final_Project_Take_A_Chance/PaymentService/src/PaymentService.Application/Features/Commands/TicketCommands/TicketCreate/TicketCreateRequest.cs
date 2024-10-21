using MediatR;
using PaymentService.Application.Utilities.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentService.Application.Features.Commands.TicketCommands.TicketCreate
{
    public class TicketCreateRequest : IRequest<Result>
    {
        public decimal Price { get; set; }
        public int TicketsCount { get; set; }
        public required string Token { get; set; }
    }
}
