using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AuthService.Domain.Dtos;

namespace AuthService.Domain.Interfaces
{
    public interface INotificationPublisher
    {
        Task SendAuthOtp(NotificationMessage message);
    }
}
