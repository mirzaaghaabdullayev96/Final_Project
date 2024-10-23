using LotteryService.Application.Features.Commands.TicketCommands.TicketCreate;
using LotteryService.Application.Repositories;
using LotteryService.Application.Utilities.Helpers;
using LotteryService.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LotteryService.Application.Features.Commands.TicketCommands.WinnerChoose
{
    public class WinnerChooseHandler(ILotteryRepository lotteryRepository, ITicketRepository ticketRepository) : IRequestHandler<WinnerChooseRequest, Result<WinnerChooseResponse>>
    {

        public async Task<Result<WinnerChooseResponse>> Handle(WinnerChooseRequest request, CancellationToken cancellationToken)
        {
            var lottery = await lotteryRepository.GetAsync(request.LotteryId);
            if (lottery == null) return new ErrorResult<WinnerChooseResponse>("Lottery not found",404);
            
            var ticketCount = await ticketRepository.Table.CountAsync(t => t.LotteryId == request.LotteryId);
            var randomIndex = new Random().Next(ticketCount);
            var winnerTicket = await ticketRepository.Table
               .Where(t => t.LotteryId == request.LotteryId)
               .Skip(randomIndex)
               .Take(1)
               .FirstOrDefaultAsync();

            lottery.WinnerTicketCode = winnerTicket?.Code;
            lottery.WinnerUserId = winnerTicket!.UserId;
            lottery.IsActive = false;
            lottery.WinnerChosenAt= DateTime.Now;
            await lotteryRepository.UpdateAsync(lottery);

            WinnerChooseResponse response = new() { ProductId = lottery.ProductId, UserId = winnerTicket.UserId };

            return new SuccessResult<WinnerChooseResponse>(response, "Lottery is finished and we have a winner", 200);
        }

    }
}
