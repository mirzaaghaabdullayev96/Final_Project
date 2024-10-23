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
            if (request.TypeOfOperation == Utilities.Enums.TypeOfOperation.TopUpBalance)
            {
                var user = await userManager.FindByIdAsync(request.UserId);
                if (user == null) return new ErrorResult("User not found.", 404);

                if (request.Amount <= 0) return new ErrorResult("Amount must be greater than 0", 400);

                user.Account += request.Amount;

                await userManager.UpdateAsync(user);
                return new SuccessResult("Successfull transaction", 200);
            }

            if (request.TypeOfOperation == Utilities.Enums.TypeOfOperation.BuyTicket)
            {
                var user = await userManager.FindByIdAsync(request.UserId);
                if (user == null) return new ErrorResult("User not found.", 404);

                if (request.Amount <= 0) return new ErrorResult("Amount must be greater than 0", 400);

                user.Account -= request.Amount;

                await userManager.UpdateAsync(user);
                return new SuccessResult("Successfull transaction", 200);
            }
            return new ErrorResult("Something went wrong", 400);
        }
    }
}
