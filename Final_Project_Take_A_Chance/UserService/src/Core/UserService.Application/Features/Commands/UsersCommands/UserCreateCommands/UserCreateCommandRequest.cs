using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserService.Application.Utilities.Helpers;

namespace UserService.Application.Features.Commands.UsersCommands.UserCreateCommands
{
    public class UserCreateCommandRequest : IRequest<Result>
    {
        public required string Name { get; set; }
        public string? Surname { get; set; }
        public required string Email { get; set; }

        public required string Password { get; set; }
        public required string ConfirmPassword { get; set; }
        [DataType(DataType.Date)]
        public required DateTime BirthDate { get; set; }
        public IFormFile? ProfilePicture { get; set; }
        public required string Username { get; set; }
    }
}
