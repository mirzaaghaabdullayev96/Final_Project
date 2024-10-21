using LotteryService.Application.Repositories;
using LotteryService.Application.Utilities.Helpers;
using LotteryService.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LotteryService.Application.Features.Commands.LotteryCommands.LotteryCreate
{
    public class LotteryCreateHandler(ILotteryRepository lotteryRepository, IRandomCodesGenerator randomCodesGenerator) : IRequestHandler<LotteryCreateRequest, Result>
    {
        public async Task<Result> Handle(LotteryCreateRequest request, CancellationToken cancellationToken)
        {
            if (request is null) return new ErrorResult("Amount must be more than 0", 400);
            if (request.TicketsCount < 0) return new ErrorResult("Tickets counts must be more than 0", 400, "TicketsCount");
            if (request.PricePerTicket < 0) return new ErrorResult("Price per ticket must be more than 0", 400, "PricePerTicket");

            Lottery lottery = new Lottery()
            {
                ProductId = request.ProductId,
                TicketsCount = request.TicketsCount,
                PricePerTicket = request.PricePerTicket,
            };

            await lotteryRepository.CreateAsync(lottery);

            var codes = randomCodesGenerator.GenerateRandomCodes(request.TicketsCount);
            randomCodesGenerator.SaveCodesToRedis(codes, lottery.Id);

            return new SuccessResult("Lottery was created successfully", 201);
        }
    }
}
