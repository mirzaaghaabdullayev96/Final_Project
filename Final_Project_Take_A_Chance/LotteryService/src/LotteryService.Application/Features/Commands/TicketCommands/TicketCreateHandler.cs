using LotteryService.Application.Repositories;
using LotteryService.Application.Utilities.Helpers;
using LotteryService.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LotteryService.Application.Features.Commands.TicketCommands
{
    public class TicketCreateHandler(ITicketRepository ticketRepository, IRandomCodesGenerator randomCodesGenerator, ILotteryRepository lotteryRepository) : IRequestHandler<TicketCreateRequest, Result>
    {
        public async Task<Result> Handle(TicketCreateRequest request, CancellationToken cancellationToken)
        {
            var lottery = await lotteryRepository.GetAsync(request.LotteryId);
            if (lottery == null) return new ErrorResult("Lottery was not found", 404);

            if (lottery.TicketsCount < request.Count) return new ErrorResult("There are less tickets that you want to buy", 400, "TicketCount");

            var codes = randomCodesGenerator.GetCodesFromRedis(request.LotteryId, request.Count);
            List<Ticket> tickets = [];
            foreach (var code in codes)
            {
                Ticket ticket = new()
                {
                    Code = code,
                    UserId = request.UserId,
                    Price = lottery.PricePerTicket,
                    LotteryId = request.LotteryId,
                    Lottery = lottery
                };
                tickets.Add(ticket);
            }

            lottery.TicketsCount -= tickets.Count;
            await lotteryRepository.UpdateAsync(lottery);
            await ticketRepository.CreateManyAsync(tickets);
            return new SuccessResult("Tickets created successfully", 201);
        }
    }
}
