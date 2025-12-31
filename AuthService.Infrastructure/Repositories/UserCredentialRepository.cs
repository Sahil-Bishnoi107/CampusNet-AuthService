using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AuthService.Domain.Entities;
using AuthService.Domain.Interfaces;
using AuthService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace AuthService.Infrastructure.Repositories
{
    public class UserCredentialRepository : IUserCredentialRepository
    {
        private readonly AuthDbContext _db;

        public UserCredentialRepository(AuthDbContext db)
        {
            _db = db;
        }

        public async Task AddAsync(UserCredentials user)
        {
            _db.Users.Add(user);
            await _db.SaveChangesAsync();
        }

        public async Task<bool> ExistsByEmailAsync(string email)
        {
            bool b = await _db.Users.AnyAsync(x => x.Email == email);
            return b;
        }
       


        public async Task<UserCredentials?> GetByEmailAsync(string email)
        {
            UserCredentials? myemail = await _db.Users.FirstOrDefaultAsync(x => x.Email == email); 
            return myemail;
        }

        public async Task UpdateAsync(UserCredentials user)
        {
            _db.Users.Update(user);
            await _db.SaveChangesAsync();
        }
    }
}
