using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using UserService.Application.Utilities.Helpers;
using UserService.Domain.Entities;

namespace UserService.Application.Features.Queries.UsersQueries.UserLoginQueries
{
    public class UserLoginHandler : IRequestHandler<UserLoginRequest, Result<UserLoginResponse>>
    {


        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IConfiguration _configuration;


        public UserLoginHandler(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
        }
        public async Task<Result<UserLoginResponse>> Handle(UserLoginRequest request, CancellationToken cancellationToken)
        {
            AppUser? user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                return new ErrorResult<UserLoginResponse>("Invalid credentials. Please check your email and password.", 400);
            }
            var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);

            if (!result.Succeeded)
            {
                return new ErrorResult<UserLoginResponse>("Invalid credentials. Please check your email and password.", 400);
            }

            if (!user.EmailConfirmed)
            {
                return new ErrorResult<UserLoginResponse>("Email is not confirmed. Please confirm you email", 400);
            }

            var roles = await _userManager.GetRolesAsync(user);

            List<Claim> claims =
            [
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim("Name", user.Name),
                new Claim(ClaimTypes.Email, user.Email),
                .. roles.Select(role=>new Claim(ClaimTypes.Role, role))
            ];

            DateTime expires = DateTime.UtcNow.AddDays(1);
            SymmetricSecurityKey symmetricSecurityKey = new(Encoding.UTF8.GetBytes(_configuration["SecretKeyForToken"]));
            SigningCredentials signingCredentials = new(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            JwtSecurityToken jwtSecurityToken = new(
               signingCredentials: signingCredentials,
               claims: claims,
               audience: "http://localhost:5016/",
               issuer: "http://localhost:5016/",
               expires: expires,
               notBefore: DateTime.UtcNow
               );

            string token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
            UserLoginResponse userLoginResponse = new() { AccessToken = token, ExpirationDate = expires };

            return new SuccessResult<UserLoginResponse>(userLoginResponse,"User logged in successfully", 200);
        }
    }
}
