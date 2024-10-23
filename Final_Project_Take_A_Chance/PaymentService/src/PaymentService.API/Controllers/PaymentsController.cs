using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using Azure.Core;
using Microsoft.AspNetCore.Components.Forms;
using PaymentService.Application.Utilities.Helpers;
using PaymentService.Application.Features.Commands.TopUpCommands;
using PaymentService.Domain.Entities;
using PaymentService.Application.Features.Commands.TicketCommands;

namespace PaymentService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentsController(IMediator mediator) : ControllerBase
    {
        [HttpPost("add-balance")]
        public async Task<IActionResult> AddBalance( [FromBody] TopUpCreateRequest request)
        {
            var result = await mediator.Send(request);
            return ActionResponse.HandleResult(this, result);
        }

        [HttpPost("buy-ticket")]
        public async Task<IActionResult> BuyTicket([FromBody] TicketBuyRequest request)
        {
            var result = await mediator.Send(request);
            return ActionResponse.HandleResult(this, result);
        }
    }
}
