using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LotteryService.Application.Features.Commands.TicketCommands.WinnerChoose
{
    public class WinnerChooseResponse
    {
        public string UserId { get; set; }
        public int ProductId { get; set; }
    }
}
