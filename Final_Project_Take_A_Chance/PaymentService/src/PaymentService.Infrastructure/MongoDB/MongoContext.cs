using PaymentService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace PaymentService.Infrastructure.MongoDB
{
    public class MongoContext
    {
        public IMongoCollection<AccountTopUp> Transactions { get; }

        public MongoContext(IConfiguration configuration)
        {
            var client = new MongoClient(configuration["MongoDB:ConnectionString"]);
            var database = client.GetDatabase(configuration["MongoDB:DatabaseName"]);
            Transactions = database.GetCollection<AccountTopUp>("Transactions");
        }
    }
}
