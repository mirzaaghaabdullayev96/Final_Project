using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Connections;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using RabbitMQ.Client;
using System;
using System.Security.Claims;
using System.Text;
using System.Text.Json.Serialization;
using UserService.Application.Features.Commands.UsersCommands;
using UserService.Application.Services.Interfaces;
using UserService.Application.Utilities.Helpers;
using UserService.Domain.Entities;
using UserService.Infrastructure.Services.Implementations;
using UserService.Persistence.Contexts;

namespace UserService.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
            });



            var rabbitMqConnectionFactory = new ConnectionFactory()
            {
                HostName = "localhost",
                UserName = "guest",
                Password = "guest"
            };

            builder.Services.AddSingleton<IConnection>(rabbitMqConnectionFactory.CreateConnection());



            builder.Services.AddDbContext<AppDbContext>(opt =>
            {
                opt.UseSqlServer(builder.Configuration.GetConnectionString("Default"));
            });

            builder.Services.AddIdentity<AppUser, IdentityRole>(opt =>
            {
                opt.User.RequireUniqueEmail = true;
                opt.Password.RequireDigit = true;
                opt.Password.RequiredLength = 8;
                opt.Password.RequireUppercase = true;
                opt.Password.RequireLowercase = false;
                opt.Password.RequireNonAlphanumeric = true;
                opt.Password.RequiredUniqueChars = 1;
                opt.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                opt.Lockout.MaxFailedAccessAttempts = 5;
            }).AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();

            builder.Services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(opt =>
            {
                opt.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,

                    ValidIssuer = "http://localhost:5016/",
                    ValidAudience = "http://localhost:5016/",
                    ValidateLifetime = true,

                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["SecretKeyForToken"]!)),
                    ClockSkew = TimeSpan.Zero
                };
            });

            builder.Services.AddMediatR(opt =>
            {
                opt.RegisterServicesFromAssemblyContaining(typeof(UserRegisterCommandRequest));
            });


            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", builder =>
                {
                    builder.AllowAnyOrigin()
                           .AllowAnyMethod()
                           .AllowAnyHeader();
                });
            });

            //for email
            builder.Services.AddSingleton<IBackgroundTaskQueue, BackgroundTaskQueue>();
            builder.Services.AddScoped<IEmailService, EmailService>();
            builder.Services.AddHostedService<QueuedHostedService>();

            builder.Services.AddHttpContextAccessor();

            builder.Services.AddHttpContextAccessor();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            //minimal API for top up balance
            //app.MapPost("/api/user/addbalance", async (
            // HttpContext httpContext, AmountDto request, AppDbContext dbContext) =>
            //{
            //    var userIdClaim = httpContext.User.FindFirst(ClaimTypes.NameIdentifier);
            //    if (userIdClaim == null) return Results.Unauthorized();
            //    string userId = userIdClaim.Value;

            //    var user = await dbContext.Users.FindAsync(userId);
            //    if (user == null) return Results.NotFound("User not found.");

            //    if (request.Amount <= 0) return Results.BadRequest("Amount must be greater than 0");

            //    user.Account += request.Amount;

            //    var transaction = new AccountTopUp
            //    {
            //        TransactionId = Guid.NewGuid().ToString(),
            //        UserId = userId,
            //        Amount = request.Amount,
            //    };

            //    await dbContext.TransactionsBalanceTopUp.AddAsync(transaction);
            //    await dbContext.SaveChangesAsync();

            //    return Results.Ok(new { TransactionId = transaction.TransactionId });
            //}).RequireAuthorization();





            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseCors("AllowAll");


            app.MapControllers();

            app.Run();
        }
    }
}
