using LotteryService.Application.Utilities.Helpers;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LotteryService.Application.Features.Commands.TicketCommands
{
    public class TicketCreateRequest : IRequest<Result>
    {
        public required string UserId { get; set; }
        public int LotteryId { get; set; }
        public int Count { get; set; }

    }
}
