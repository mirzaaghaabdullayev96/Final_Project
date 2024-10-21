using LotteryService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LotteryService.Application.Repositories
{
    public interface ITicketRepository: IGenericRepository<Ticket>
    {
    }
}
