using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentService.Application.Services.Interfaces
{
    public interface IUserServiceClient
    {
        Task<string> AddBalanceAsync(string token, decimal amount);
    }
}
