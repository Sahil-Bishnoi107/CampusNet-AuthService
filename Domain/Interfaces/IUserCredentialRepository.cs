using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AuthService.Domain.Entities;

namespace AuthService.Domain.Interfaces
{
    public interface IUserCredentialRepository
    {
        Task<bool> ExistsByEmailAsync(string email);
        Task AddAsync(UserCredentials user);
        Task<UserCredentials?> GetByEmailAsync(string email);
        Task UpdateAsync(UserCredentials user);

    }
}
