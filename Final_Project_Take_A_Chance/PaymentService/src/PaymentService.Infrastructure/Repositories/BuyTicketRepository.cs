using PaymentService.Application.Repositories;
using PaymentService.Domain.Entities;
using PaymentService.Infrastructure.DAL.AppDbContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentService.Infrastructure.Repositories
{
    public class BuyTicketRepository : GenericRepository<BuyTicket>, IBuyTicketRepository
    {
        public BuyTicketRepository(AppDbContext dbContext) : base(dbContext)
        {
        }
    }
}
