using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LotteryService.Domain.Entities
{
    public class Lottery
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string? WinnerUserId { get; set; }
        public int TicketsCount { get; set; }
        public DateTime? TicketsSoldAt { get; set; }
        public string? WinnerTicketCode { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? CreatedBy { get; set; }
        public bool IsActive { get; set; }
        //relational
        public ICollection<Ticket>? Tickets { get; set; }

    }
}
