using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentService.Domain.Entities
{
    public class Ticket
    {
        [Key]
        public required string TransactionId { get; set; }
        public required string UserId { get; set; }
        [Column(TypeName = "decimal(10,2)")]
        public decimal Price { get; set; }
        public DateTime BoughtAt { get; set; }
        public required string Code { get; set; }
    }
}
