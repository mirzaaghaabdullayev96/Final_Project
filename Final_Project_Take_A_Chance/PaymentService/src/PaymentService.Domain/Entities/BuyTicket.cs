using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentService.Domain.Entities
{
    public class BuyTicket
    {
        [Key]
        public required string TransactionId { get; set; }
        public required string UserId { get; set; }
        [Column(TypeName = "decimal(10,2)")]
        public decimal Amount { get; set; }
        [Column(TypeName = "decimal(10,2)")]
        public decimal PricePerTicket { get; set; }
        public int TicketsCount { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
