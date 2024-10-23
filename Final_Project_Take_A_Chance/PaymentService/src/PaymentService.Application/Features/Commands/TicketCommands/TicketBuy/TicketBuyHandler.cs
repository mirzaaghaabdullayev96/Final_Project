using MediatR;
using PaymentService.Application.Repositories;
using PaymentService.Application.Utilities.Helpers;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace PaymentService.Application.Features.Commands.TicketCommands
{
    public class TicketBuyHandler(IRequestToServices requestToServices) : IRequestHandler<TicketBuyRequest, Result>
    {
        public async Task<Result> Handle(TicketBuyRequest request, CancellationToken cancellationToken)
        {
            if (request is null) return new ErrorResult("Amount must be more than 0", 400);
            if (request.TicketsCount < 0) return new ErrorResult("Tickets counts must be more than 0", 400, "TicketsCount");
            if (request.Price < 0) return new ErrorResult("Price per ticket must be more than 0", 400, "PricePerTicket");

            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(request.Token);
            var userId = jwtToken.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value;


            var resultCheck = await requestToServices.CheckBalance("/Users/CheckBalance", userId, request.Price * request.TicketsCount);
            if (!resultCheck.Success) return new ErrorResult(resultCheck.Message, 400);

            var resultCreate = await requestToServices.CreateTickets("/Lotteries/CreateTickets", userId, request.LotteryId, request.TicketsCount);
            if (!resultCreate.Success) return new ErrorResult(resultCreate.Message, 400);

            var resultDeduct = await requestToServices.DeductFromBalance("/Users/ChangeBalance", userId, request.Price * request.TicketsCount);
            if (!resultDeduct.Success) return new ErrorResult(resultDeduct.Message, 400);

            return new SuccessResult("Tickets bought successfully", 200);
        }
    }
}
