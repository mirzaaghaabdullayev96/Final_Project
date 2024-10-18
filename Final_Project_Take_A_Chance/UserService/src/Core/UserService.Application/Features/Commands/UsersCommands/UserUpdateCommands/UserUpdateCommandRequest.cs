using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserService.Application.Utilities.Helpers;

namespace UserService.Application.Features.Commands.UsersCommands
{

    public class UserUpdateCommandRequest : IRequest<Result>
    {
        public string? UserId { get; set; }
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public IFormFile? ProfilePicture { get; set; }
        public string? Username { get; set; }
    }
}
