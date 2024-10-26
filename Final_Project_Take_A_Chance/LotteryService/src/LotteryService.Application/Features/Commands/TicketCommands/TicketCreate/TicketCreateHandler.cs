using LotteryService.Application.Events;
using LotteryService.Application.Features.Commands.TicketCommands.WinnerChoose;
using LotteryService.Application.Repositories;
using LotteryService.Application.Services.Interfaces;
using LotteryService.Application.Utilities.Helpers;
using LotteryService.Domain.Entities;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LotteryService.Application.Features.Commands.TicketCommands.TicketCreate
{
    public class TicketCreateHandler(ITicketRepository ticketRepository,
        IRandomCodesGenerator randomCodesGenerator,
        ILotteryRepository lotteryRepository,
        IMediator mediator,
        IBackgroundTaskQueue taskQueue,
        IServiceScopeFactory scopeFactory,
        IRequestToServices requestToServices,
        IConnection rabbitConnection) : IRequestHandler<TicketCreateRequest, Result>
    {
        public async Task<Result> Handle(TicketCreateRequest request, CancellationToken cancellationToken)
        {
            var lottery = await lotteryRepository.GetAsync(request.LotteryId);
            if (lottery == null) return new ErrorResult("Lottery was not found", 404);

            if (lottery.TicketsCount < request.Count) return new ErrorResult("There are less tickets than you want to buy", 400, "TicketCount");

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

            await ticketRepository.CreateManyAsync(tickets);
            lottery.TicketsCount -= tickets.Count;

            if (lottery.TicketsCount == 0)
            {
                lottery.AllTicketsSoldAt = DateTime.Now;
                await lotteryRepository.UpdateAsync(lottery);


                taskQueue.QueueBackgroundWorkItem(async tokenCancellation =>
                {

                    using (var scope = scopeFactory.CreateScope())
                    {
                        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

                        await Task.Delay(TimeSpan.FromSeconds(10));

                        var winner = await mediator.Send(new WinnerChooseRequest { LotteryId = request.LotteryId });

                        var winnerUserInfo = await requestToServices.GetUserById($"Users/GetUserById/{winner.Entities.UserId}");
                        var winnerProductInfo = await requestToServices.GetProductById($"Products/GetProductById/{winner.Entities.ProductId}");

                        WinnerChosenEvent winnerChosenEvent = new()
                        {
                            Email = winnerUserInfo.Entities.Email,
                            UserName = winnerUserInfo.Entities.Name,
                            ProductName = winnerProductInfo.Entities.Name
                        };

                        using (var channel = rabbitConnection.CreateModel())
                        {
                            channel.QueueDeclare(queue: "winner_chosen_queue", durable: true, exclusive: false, autoDelete: false, arguments: null);

                            var messageBody = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(winnerChosenEvent));

                            channel.BasicPublish(exchange: "", routingKey: "winner_chosen_queue", basicProperties: null, body: messageBody);
                        }

                    }
                });
            }
            else
            {
                await lotteryRepository.UpdateAsync(lottery);
            }

            return new SuccessResult("Tickets created successfully", 201);
        }
    }
}
