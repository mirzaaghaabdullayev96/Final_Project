using MediatR;
using Newtonsoft.Json;
using PaymentService.Application.Repositories;
using PaymentService.Application.Utilities.Helpers;
using PaymentService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Reflection.Metadata;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace PaymentService.Application.Features.Commands.TicketCommands.TicketCreate
{
    public class TicketCreateHandler(IUserServiceClient userServiceClient, ITicketRepository ticketRepository) : IRequestHandler<TicketCreateRequest, Result>
    {
        public async Task<Result> Handle(TicketCreateRequest request, CancellationToken cancellationToken)
        {
            if (request is null) return new ErrorResult("Amount must be more than 0", 400);
            if (request.TicketsCount < 0) return new ErrorResult("Tickets counts must be more than 0", 400, "TicketsCount");
            if (request.Price < 0) return new ErrorResult("Price per ticket must be more than 0", 400, "PricePerTicket");
            var result = await userServiceClient.BuyTicket(request.Token, request.Price * request.TicketsCount);
            if (!result.IsSuccessStatusCode) return new ErrorResult("Failed to buy ticket", 400);

            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(request.Token);
            var userId = jwtToken.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value;

            List<Ticket> tickets = [];

            for (int i = 1; i == request.TicketsCount; i++)
            {
                Ticket buyTicket = new()
                {
                    TransactionId = Guid.NewGuid().ToString(),
                    UserId = userId,
                    Price = request.Price,
                    Code = "AA100"
                };
                tickets.Add(buyTicket);
            }


            await ticketRepository.CreateManyAsync(tickets);

            return new SuccessResult("Tickets were bought successfully", 200);
        }
    }
}
