using PaymentService.Application.Exceptions;
using PaymentService.Application.Services.Interfaces;
using PaymentService.Domain;
using PaymentService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace PaymentService.Application.Services.Imlementations
{
    public class TransactionService : ITransactionService
    {
        private readonly IUserServiceClient _userServiceClient;
        private readonly ITransactionRepository _transactionRepository;

        public TransactionService(IUserServiceClient userServiceClient, ITransactionRepository transactionRepository)
        {
            _userServiceClient = userServiceClient;
            _transactionRepository = transactionRepository;
        }

        public async Task AddBalanceAsync(string token, decimal amount)
        {
            if (amount <= 0) throw new AmountLessThanOneException(400, "Amount", "You can top up your balance with a minimum of 1 manat ");

            var transactionId = await _userServiceClient.AddBalanceAsync(token, amount);

            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);

            var userId = jwtToken.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value;

            var transaction = new AccountTopUp
            {
                UserId = userId,
                TransactionId = transactionId,
                Amount = amount,
                CreatedAt = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss")
            };
            await _transactionRepository.SaveTransactionAsync(transaction);

        }
    }
}
