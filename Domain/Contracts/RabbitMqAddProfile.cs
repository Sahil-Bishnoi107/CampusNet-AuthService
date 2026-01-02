using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Domain.Contracts
{
    public record RabbitMqAddProfile(string id, string email, string phoneNo, string name);
   
}
