using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserService.Domain.Entities
{
    public class AccountTopUp
    {
        public string TransactionId { get; set; }
        public decimal Amount { get; set; }
        public DateTime CreatedAt { get; set; }

        //relational
        public AppUser AppUser { get; set; }
        public string UserId { get; set; }
    }
}
