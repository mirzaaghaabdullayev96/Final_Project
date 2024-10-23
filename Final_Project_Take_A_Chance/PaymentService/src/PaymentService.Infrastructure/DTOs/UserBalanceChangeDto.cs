using PaymentService.Application.Utilities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace PaymentService.Infrastructure.DTOs
{
    public class UserBalanceChangeDto
    {
        public decimal Amount { get; set; }
        public TypeOfOperation TypeOfOperation { get; set; }
        public string UserId { get; set; }
    }
}
