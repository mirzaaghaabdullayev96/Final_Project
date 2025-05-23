﻿using MediatR;
using PaymentService.Application.Repositories;
using PaymentService.Application.Utilities.Helpers;
using PaymentService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace PaymentService.Application.Features.Commands.TopUpCommands.TopUpCreate
{
    public class TopUpCreateHandler(IRequestToServices requestToServices, ITopUpRepository topUpRepository) : IRequestHandler<TopUpCreateRequest, Result>
    {
        public async Task<Result> Handle(TopUpCreateRequest request, CancellationToken cancellationToken)
        {
            if (request is null || request.Amount < 0) return new ErrorResult("Amount must be more than 0", 400);

            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(request.Token);
            var userId = jwtToken.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value;

            var result = await requestToServices.TopUpBalance("/Users/ChangeBalance",userId, request.Amount);
            if (!result.Success) return new ErrorResult(result.Message, 400);        

            AccountTopUp accountTopUp = new()
            {
                TransactionId = Guid.NewGuid().ToString(),
                UserId = userId,
                Amount = request.Amount,
            };

            await topUpRepository.CreateAsync(accountTopUp);

            return new SuccessResult("Account was topped up successfully", 200);
        }
    }
}
