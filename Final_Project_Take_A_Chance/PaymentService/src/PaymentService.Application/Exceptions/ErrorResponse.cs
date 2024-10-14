using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentService.Application.Exceptions
{
    public class ErrorResponse
    {
        public int StatusCode { get; set; }
        public required string PropertyName { get; set; }
        public required string ErrorMessage { get; set; }
    }
}
