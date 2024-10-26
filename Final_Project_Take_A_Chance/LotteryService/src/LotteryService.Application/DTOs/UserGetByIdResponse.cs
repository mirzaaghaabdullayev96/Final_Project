using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LotteryService.Application.DTOs
{
    public class UserGetByIdResponse
    {
        public required string Id { get; set; }
        public required string Email { get; set; }
        public required string Name { get; set; }
    }
}
