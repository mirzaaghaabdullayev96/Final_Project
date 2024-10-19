using PaymentService.Application.Utilities.Enums;
using PaymentService.Application.Utilities.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentService.Application.Repositories
{
    public interface IUserServiceClient
    {
        Task<HttpResponseMessage> AddBalanceAsync(string token, decimal amount);
        Task<HttpResponseMessage> BuyTicket(string token, decimal amount);
    }
}
