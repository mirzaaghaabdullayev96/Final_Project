using PaymentService.Application.Utilities.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentService.Application.Repositories
{
    public interface IRequestToServices
    {
        Task<Result> CheckBalance(string endpoint, string token, decimal amount);
        Task<Result> CreateTickets(string endpoint, string userId, int lotteryId, int count);
        Task<Result> DeductFromBalance(string endpoint, string token, decimal amount);
    }
}
