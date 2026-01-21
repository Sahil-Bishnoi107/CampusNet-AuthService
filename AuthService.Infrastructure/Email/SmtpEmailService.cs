using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AuthService.Domain.Dtos;
using AuthService.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace AuthService.Infrastructure.Email
{
    public class SmtpEmailService : IEmailService
    {
       private readonly INotificationPublisher _notificationPublisher;
        private readonly ILogger<SmtpEmailService> _logger; 

        public SmtpEmailService(INotificationPublisher notificationPublisher,ILogger<SmtpEmailService> logger)
        {
            _notificationPublisher = notificationPublisher;
            _logger = logger;
        }
        public async Task SendOtp(string email, string otp)
        {
            string m = "Your Authentication OTP is " + otp + ". This will expire in 15 minutes.";
            _logger.LogInformation("revieved {email}", email);
            NotificationMessage message = new NotificationMessage(email,"Authentication Otp",m, "");
            _logger.LogInformation("Message object with email =  {Email} is created", message.To);

            await _notificationPublisher.SendAuthOtp(message);
        }
    }
}
