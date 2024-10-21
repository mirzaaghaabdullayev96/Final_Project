using LotteryService.Application.Utilities.Helpers;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LotteryService.Application.Features.Commands.LotteryCommands.LotteryCreate
{
    public class LotteryCreateRequest : IRequest<Result>
    {
        public int ProductId { get; set; }
        public int TicketsCount { get; set; }
        public decimal PricePerTicket { get; set; }
    }
}
