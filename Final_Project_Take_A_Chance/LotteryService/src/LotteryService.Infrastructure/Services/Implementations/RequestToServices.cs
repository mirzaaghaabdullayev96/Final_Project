using Azure.Core;
using LotteryService.Application.DTOs;
using LotteryService.Application.Repositories;
using LotteryService.Application.Utilities.Helpers;
using LotteryService.Domain.Entities;
using Microsoft.Extensions.Configuration;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LotteryService.Infrastructure.Repositories
{
    public class RequestToServices : IRequestToServices
    {
        private readonly RestClient _restClientUser;
        private readonly RestClient _restClientProduct;
        private readonly IConfiguration _configuration;
        public RequestToServices(IConfiguration configuration)
        {
            _configuration = configuration;
            _restClientUser = new RestClient(_configuration.GetSection("ApiServices:UserService").Value!);
            _restClientProduct = new RestClient(_configuration.GetSection("ApiServices:ProductService").Value!);
        }

        public async Task<Result<ProductGetOneResponse>> GetProductById(string endpoint)
        {
            var request = new RestRequest(endpoint, Method.Get);
            var response = await _restClientProduct.ExecuteAsync<Result<ProductGetOneResponse>>(request);
            return response.Data!;
        }

        public async Task<Result<UserGetByIdResponse>> GetUserById(string endpoint)
        {
            var request = new RestRequest(endpoint, Method.Get);
            var response = await _restClientUser.ExecuteAsync<Result<UserGetByIdResponse>>(request);
            return response.Data!;
        }
    }
}
