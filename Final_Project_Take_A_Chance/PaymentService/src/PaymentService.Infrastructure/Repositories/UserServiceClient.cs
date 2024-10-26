using PaymentService.Application.Repositories;
using PaymentService.Application.Utilities.Enums;
using PaymentService.Application.Utilities.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace PaymentService.Infrastructure.Repositories
{
    public class UserServiceClient(HttpClient client) : IUserServiceClient
    {
        //public async Task<HttpResponseMessage> AddBalanceAsync(string token, decimal amount)
        //{
        //    client.DefaultRequestHeaders.Authorization =
        //        new AuthenticationHeaderValue("Bearer", token);

        //    return await client.PutAsJsonAsync("/api/Users/ChangeBalance", new { Amount = amount, TypeOpOperation = 1 });

        //}

        //public async Task<HttpResponseMessage> BuyTicket(string token, decimal amount)
        //{
        //    client.DefaultRequestHeaders.Authorization =
        //        new AuthenticationHeaderValue("Bearer", token);

        //    return await client.PutAsJsonAsync("/api/Users/ChangeBalance", new { Amount = amount, TypeOpOperation = 2 });

        //}
    }
}
