using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using UserService.Application.Utilities.Helpers;
using UserService.Domain.Entities;

namespace UserService.Application.Features.Queries.UsersQueries
{
    public class UserBalanceCheckHandler(UserManager<AppUser> userManager, IHttpContextAccessor httpContextAccessor) : IRequestHandler<UserBalanceCheckRequest, Result>
    {
        public async Task<Result> Handle(UserBalanceCheckRequest request, CancellationToken cancellationToken)
        {
           
            var user = await userManager.FindByIdAsync(request.UserId);
            if (user == null) return new ErrorResult("User not found.", 404);

            if (request.Amount <= 0) return new ErrorResult("Amount must be greater than 0", 400);

            if (request.Amount > user.Account) return new ErrorResult("There is not enough money in the account to complete the transaction.", 400);

            return new SuccessResult("There is enough money", 200);
        }
    }
}
