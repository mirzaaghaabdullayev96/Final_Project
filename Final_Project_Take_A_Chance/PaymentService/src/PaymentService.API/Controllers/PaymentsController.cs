using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PaymentService.Application.Exceptions;
using PaymentService.Application.Services.Interfaces;

namespace PaymentService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentsController : ControllerBase
    {
        private readonly ITransactionService _transactionService;

        public PaymentsController(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        [HttpPost("add-balance")]
        public async Task<IActionResult> AddBalance(/*[FromHeader] string token,*/ [FromBody] decimal amount)
        {
            try
            {
                var token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
                await _transactionService.AddBalanceAsync(token, amount);
                return Ok("Account was topped up successfully");
            }
            catch (AmountLessThanOneException ex)
            {
                return BadRequest(new ErrorResponse
                {
                    StatusCode=400,
                    PropertyName=ex.PropertyName,
                    ErrorMessage=ex.Message
                });
            }
            catch (PopUpBalanceFailedException ex)
            {
                return BadRequest(new ErrorResponse
                {
                    StatusCode=400,
                    PropertyName=ex.PropertyName,
                    ErrorMessage=ex.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
