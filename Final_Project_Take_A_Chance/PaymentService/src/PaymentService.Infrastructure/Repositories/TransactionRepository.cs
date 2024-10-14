using MongoDB.Driver;
using PaymentService.Application.Services.Interfaces;
using PaymentService.Domain.Entities;
using PaymentService.Infrastructure.MongoDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentService.Infrastructure.Repositories
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly IMongoCollection<AccountTopUp> _transactions;

        public TransactionRepository(MongoContext mongoContext)
        {
            _transactions = mongoContext.Transactions;
        }

        public async Task SaveTransactionAsync(AccountTopUp transaction)
        {
            await _transactions.InsertOneAsync(transaction);
        }
    }
}
