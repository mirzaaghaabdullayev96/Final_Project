using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentService.Domain.Entities
{
    public class AccountTopUp
    {
        public required string TransactionId { get; set; }
        public required string UserId { get; set; }
        public decimal Amount { get; set; }
        public required string CreatedAt { get; set; }
    }
}
