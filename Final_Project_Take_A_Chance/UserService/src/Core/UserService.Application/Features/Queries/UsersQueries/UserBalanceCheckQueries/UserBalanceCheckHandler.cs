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
            var httpContext = httpContextAccessor.HttpContext;
            var userIdClaim = httpContext.User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null) return new ErrorResult("User is not authorized", 401);
            string userId = userIdClaim.Value;

            var user = await userManager.FindByIdAsync(userId);
            if (user == null) return new ErrorResult("User not found.", 404);

            if (request.Amount <= 0) return new ErrorResult("Amount must be greater than 0", 400);

            if (request.Amount > user.Account) return new ErrorResult("There is not enough money in the account to complete the transaction.", 400);

            return new SuccessResult("There is enough money", 200);
        }
    }
}
