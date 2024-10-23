using LotteryService.Application.Utilities.Helpers;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LotteryService.Application.Features.Commands.TicketCommands.WinnerChoose
{
    public class WinnerChooseRequest:IRequest<Result<WinnerChooseResponse>>
    {
        public int LotteryId { get; set; }
    }
}
