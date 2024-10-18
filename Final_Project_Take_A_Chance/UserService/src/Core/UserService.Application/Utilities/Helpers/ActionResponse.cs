using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserService.Application.Utilities.Helpers
{
    public class ActionResponse
    {
        public static IActionResult HandleResult(ControllerBase controller, object result)
        {
            return result switch
            {
                ErrorResult errorResult => controller.BadRequest(errorResult),
                SuccessResult successResult => controller.Ok(successResult),
                _ => new StatusCodeResult(500)
            };
        }

        public static IActionResult HandleResult<T>(ControllerBase controller, object result)
        {
            return result switch
            {
                ErrorResult<T> errorResult => controller.BadRequest(errorResult),
                SuccessResult<T> successResult => controller.Ok(successResult),
                _ => new StatusCodeResult(500)
            };
        }
    }
}
