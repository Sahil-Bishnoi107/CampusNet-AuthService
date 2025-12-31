using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AuthService.Domain.Interfaces;

namespace AuthService.Infrastructure.Security
{
    public class PasswordHasher : IPasswordHasher
    {
        public string Hash(string password) => BCrypt.Net.BCrypt.HashPassword(password);

        public bool Verify(string password, string hashedPassword)
        {
           // string temp = BCrypt.Net.BCrypt.HashPassword(password);
            return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
        }
    }
}
