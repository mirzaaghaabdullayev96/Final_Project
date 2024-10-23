using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserService.Application.Utilities.Helpers;
using UserService.Domain.Entities;

namespace UserService.Application.Features.Queries.UsersQueries.UserGetByIdQueries
{
    public class UserGetByIdHandler(UserManager<AppUser> userManager) : IRequestHandler<UserGetByIdRequest, Result<UserGetByIdResponse>>
    {
        public async Task<Result<UserGetByIdResponse>> Handle(UserGetByIdRequest request, CancellationToken cancellationToken)
        {
            var user = await userManager.FindByIdAsync(request.Id);
            if (user == null) return new ErrorResult<UserGetByIdResponse>("User not found", 404);

            var userResponse = new UserGetByIdResponse
            {
                Id = user.Id,
                Email = user.Email,
                Name = user.Name,
            };

            return new SuccessResult<UserGetByIdResponse>(userResponse, "User retrieved successfully", 200);
        }
    }
}
