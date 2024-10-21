using LotteryService.Application.Repositories;
using LotteryService.Domain.Entities;
using LotteryService.Infrastructure.DAL.DbContextSQL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LotteryService.Infrastructure.Repositories
{
    public class TicketRepository : GenericRepository<Ticket>, ITicketRepository
    {
        public TicketRepository(AppDbContext dbContext) : base(dbContext)
        {
        }
    }
}
