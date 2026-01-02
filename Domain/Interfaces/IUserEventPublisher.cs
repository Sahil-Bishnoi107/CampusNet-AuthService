using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace AuthService.Domain.Interfaces
{
    public interface IUserEventPublisher
    {
        Task PublishUserCreatedEventAsync(string userId, string email, string name, string phoneNo);
    }
}
