﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
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
        public DateTime? AllTicketsSoldAt { get; set; }
        public DateTime? WinnerChosenAt { get; set; }
        public string? WinnerTicketCode { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? CreatedBy { get; set; }
        public bool IsActive { get; set; }
        [Column(TypeName = "decimal(10,2)")]
        public decimal PricePerTicket { get; set; }
        //relational
        public ICollection<Ticket>? Tickets { get; set; }

    }
}
