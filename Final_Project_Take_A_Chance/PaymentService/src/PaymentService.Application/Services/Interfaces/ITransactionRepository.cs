using PaymentService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentService.Application.Services.Interfaces
{
    public interface ITransactionRepository
    {
        Task SaveTransactionAsync(AccountTopUp transaction);
    }
}
