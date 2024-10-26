using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LotteryService.Application.Events
{
    public class WinnerChosenEvent
    {
        public required string UserName { get; set; }
        public required string ProductName { get; set; }
        public required string Email { get; set; }
    }
}
