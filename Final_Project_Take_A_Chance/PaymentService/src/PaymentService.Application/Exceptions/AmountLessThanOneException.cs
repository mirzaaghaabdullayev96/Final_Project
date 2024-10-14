using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentService.Application.Exceptions
{
    public class AmountLessThanOneException : Exception
    {
        public int StatusCode { get; set; }
        public string PropertyName { get; set; }
        public string ErrorMessage { get; set; }
        public AmountLessThanOneException()
        {
        }

        public AmountLessThanOneException(string? message) : base(message)
        {
        }
        public AmountLessThanOneException(int statusCode, string propertyName, string? message) : base(message)
        {
            StatusCode = statusCode;
            PropertyName = propertyName;
            ErrorMessage = message;
            
        }
    }
}
