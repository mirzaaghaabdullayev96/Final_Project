using Microsoft.Extensions.Configuration;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using NotificationService.Application.Services.Interfaces;
using MailKit.Net.Smtp;

namespace NotificationService.Infrastructure.Services.Implementations
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;


        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task SendMailAsync(string to, string subject, string name, string text,string token)
        {
            var message = new MimeMessage();
            string mail = _configuration.GetSection("EmailService:Mail").Value!;
            string pass = _configuration.GetSection("EmailService:Password").Value!;
            message.From.Add(new MailboxAddress("Dinara", mail));
            message.To.Add(new MailboxAddress(name, to));
            message.Subject = subject;

            message.Body = new TextPart("html")
            {
                Text = GetBody(name, text, token)
            };

            using (var client = new SmtpClient())
            {
                try
                {
                    await client.ConnectAsync("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls);

                    await client.AuthenticateAsync(mail, pass);

                    await client.SendAsync(message);

                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
                finally
                {
                    await client.DisconnectAsync(true);
                }
            }
        }


        private string GetBody(string name, string text, string token)
        {

            string body = $@"
<!DOCTYPE html>
<html>
<head>
    <style>
        body {{
            font-family: Arial, sans-serif;
            color: #333;
            margin: 20px;
        }}
        .header {{
            background-color: #f4f4f4;
            padding: 10px;
            text-align: center;
            border-bottom: 2px solid #ccc;
        }}
        .content {{
            padding: 20px;
            background-color: #ffffff;
            border: 1px solid #ddd;
            border-radius: 4px;
        }}
        .footer {{
            background-color: #f4f4f4;
            padding: 10px;
            text-align: center;
            border-top: 2px solid #ccc;
            margin-top: 20px;
        }}
        a {{
            color: #1a73e8;
            text-decoration: none;
        }}
        a:hover {{
            text-decoration: underline;
        }}
    </style>
</head>
<body>
    <div class='header'>
        <h1>Welcome to Our Service!</h1>
    </div>
    <div class='content'>
        <h2>Hello {name},</h2>
        <p>{text}</p>
        <p>{token}</p>
    </div>
    <div class='footer'>
        <p>&copy; 2024 Your Company Name</p>
    </div>
</body>
</html>
";
            return body;
        }
    }
}
