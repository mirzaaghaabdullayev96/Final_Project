using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using UserService.Application.Utilities.Helpers;
using UserService.Domain.Entities;

namespace UserService.Application.Features.Commands.UsersCommands.UserBalanceChangeCommands
{
    public class UserBalanceChangeHandler(UserManager<AppUser> userManager, IHttpContextAccessor httpContextAccessor) : IRequestHandler<UserBalanceChangeRequest, Result>
    {
        public async Task<Result> Handle(UserBalanceChangeRequest request, CancellationToken cancellationToken)
        {
            var httpContext = httpContextAccessor.HttpContext;
            if (request.TypeOpOperation == Utilities.Enums.TypeOpOperation.TopUpBalance)
            {
                var userIdClaim = httpContext.User.FindFirst(ClaimTypes.NameIdentifier);
                if (userIdClaim == null) return new ErrorResult("User is not authorized", 401);
                string userId = userIdClaim.Value;

                var user = await userManager.FindByIdAsync(userId);
                if (user == null) return new ErrorResult("User not found.", 404);

                if (request.Amount <= 0) return new ErrorResult("Amount must be greater than 0", 400);

                user.Account += request.Amount;

                await userManager.UpdateAsync(user);
                return new SuccessResult("Successfull transaction", 200);
            }

            if (request.TypeOpOperation == Utilities.Enums.TypeOpOperation.BuyTicket)
            {
                var userIdClaim = httpContext.User.FindFirst(ClaimTypes.NameIdentifier);
                if (userIdClaim == null) return new ErrorResult("User is not authorized", 401);
                string userId = userIdClaim.Value;

                var user = await userManager.FindByIdAsync(userId);
                if (user == null) return new ErrorResult("User not found.", 404);

                if (request.Amount <= 0) return new ErrorResult("Amount must be greater than 0", 400);

                user.Account -= request.Amount;
                if (user.Account < 0) return new ErrorResult("There is not enough money in the account to complete the transaction.", 400);

                await userManager.UpdateAsync(user);
                return new SuccessResult("Successfull transaction", 200);
            }
            return new ErrorResult("Something went wrong",400);

        }
    }
}
