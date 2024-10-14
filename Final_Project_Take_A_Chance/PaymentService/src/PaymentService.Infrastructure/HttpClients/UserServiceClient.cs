using PaymentService.Application.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace PaymentService.Infrastructure.HttpClients
{
    public class UserServiceClient : IUserServiceClient
    {
        private readonly HttpClient _client;

        public UserServiceClient(HttpClient client)
        {
            _client = client;
        }

        public async Task<string> AddBalanceAsync(string token, decimal amount)
        {
            _client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            var response = await _client.PostAsJsonAsync("/api/user/transactions", new { amount });

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Failed to add balance in User Service.");
            }

            var result = await response.Content.ReadFromJsonAsync<TransactionResponse>();
            return result.TransactionId;
        }
    }

    public class TransactionResponse
    {
        public string TransactionId { get; set; }
    }
}
