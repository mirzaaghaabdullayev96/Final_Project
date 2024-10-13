using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserService.Application.Utilities.Helpers;
using UserService.Domain.Entities;

namespace UserService.Application.Features.Queries.UsersQueries.UserGetAllQueries
{
    public class UserGetAllHandler : IRequestHandler<UserGetAllRequest, Result<ICollection<UserGetAllResponse>>>
    {
        private readonly UserManager<AppUser> _userManager;
        public UserGetAllHandler(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }
        public async Task<Result<ICollection<UserGetAllResponse>>> Handle(UserGetAllRequest request, CancellationToken cancellationToken)
        {
            var users = await _userManager.Users.ToListAsync();

            var userResponses = users.Select(user => new UserGetAllResponse
            {
                Id = user.Id,
                Email = user.Email,
                Name = user.Name,
            }).ToList();

            return new SuccessResult<ICollection<UserGetAllResponse>>(userResponses, "Users retrieved successfully", 200);
        }
    }
}
