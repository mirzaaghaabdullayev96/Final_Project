using LotteryService.Application.Features.Commands.LotteryCommands.LotteryCreate;
using LotteryService.Application.Features.Commands.TicketCommands.TicketCreate;
using LotteryService.Application.Features.Commands.TicketCommands.WinnerChoose;
using LotteryService.Application.Utilities.Helpers;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LotteryService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LotteriesController(IMediator mediator) : ControllerBase
    {
        [HttpPost("StartLottery")]
        public async Task<IActionResult> StartLottery([FromBody] LotteryCreateRequest request)
        {
            var result = await mediator.Send(request);
            return ActionResponse.HandleResult(this, result);
        }

        [HttpPost("CreateTickets")]
        public async Task<IActionResult> CreateTickets([FromBody] TicketCreateRequest request)
        {
            var result = await mediator.Send(request);
            return ActionResponse.HandleResult(this, result);
        }

        [HttpPost("WinnerChoose")]
        public async Task<IActionResult> WinnerChoose([FromBody] WinnerChooseRequest request)
        {
            var result = await mediator.Send(request);
            return ActionResponse.HandleResult<WinnerChooseResponse>(this, result);
        }

    }
}
