using Azure.Core;
using Microsoft.Extensions.Configuration;
using PaymentService.Application.Repositories;
using PaymentService.Application.Utilities.Helpers;
using PaymentService.Infrastructure.DTOs;
using RestSharp;
using SharpCompress.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MongoDB.Driver.WriteConcern;

namespace PaymentService.Infrastructure.Repositories
{
    public class RequestToServices : IRequestToServices
    {
        private readonly RestClient _restClientUser;
        private readonly RestClient _restClientLottery;
        private readonly IConfiguration _configuration;
        public RequestToServices(IConfiguration configuration)
        {
            _configuration = configuration;
            _restClientUser = new RestClient(_configuration.GetSection("ApiServices:UserService").Value);
            _restClientLottery = new RestClient(_configuration.GetSection("ApiServices:LotteryService").Value);
        }

        public async Task<Result> CheckBalance(string endpoint, string userId, decimal amount)
        {

            var request = new RestRequest(endpoint, Method.Post);
            request.AddJsonBody(new { Amount = amount, UserId = userId });
            var response = await _restClientUser.ExecuteAsync<Result>(request);
            return response.Data;
        }

        public async Task<Result> CreateTickets(string endpoint, string userId, int lotteryId, int count)
        {
            var request = new RestRequest(endpoint, Method.Post);
            request.AddJsonBody(new TicketCreateDto() { UserId = userId, LotteryId = lotteryId, Count = count });
            var response = await _restClientLottery.ExecuteAsync<Result>(request);
            return response.Data;
        }

        public async Task<Result> DeductFromBalance(string endpoint, string userId, decimal amount)
        {

            var request = new RestRequest(endpoint, Method.Put);
            request.AddJsonBody(new UserBalanceChangeDto() { Amount = amount, UserId = userId, TypeOfOperation = Application.Utilities.Enums.TypeOfOperation.BuyTicket });
            var response = await _restClientUser.ExecuteAsync<Result>(request);
            return response.Data;
        }
    }
}
