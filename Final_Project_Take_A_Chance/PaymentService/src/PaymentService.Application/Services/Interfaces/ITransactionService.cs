using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentService.Application.Services.Interfaces
{
    public interface ITransactionService
    {
        Task AddBalanceAsync(string token, decimal amount);
    }
}
