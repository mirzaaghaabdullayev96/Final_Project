    using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationService.Application.Services.Interfaces
{
    public interface IEmailService
    {
        Task SendMailAsync(string to, string subject, string name, string text, string token);
    }
}
