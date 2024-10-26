using LotteryService.Application.DTOs;
using LotteryService.Application.Events;
using LotteryService.Application.Utilities.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LotteryService.Application.Repositories
{
    public interface IRequestToServices
    {
        Task<Result<ProductGetOneResponse>> GetProductById(string endpoint);
        Task<Result<UserGetByIdResponse>> GetUserById(string endpoint); 
    }
}
