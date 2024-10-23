using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentService.Infrastructure.DTOs
{
    public class TicketCreateDto
    {
        public string UserId { get; set; }
        public int LotteryId { get; set; }
        public int Count { get; set; }
    }
}
