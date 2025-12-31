using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AuthService.Domain.Interfaces;

namespace AuthService.Infrastructure.Email
{
    internal class SmtpEmailService : IEmailService
    {
        public Task SendOtp(string email, string otp)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"[DEV OTP] Email: {email}, OTP: {otp}");
            Console.ResetColor();

            return Task.CompletedTask;
        }
    }
}
