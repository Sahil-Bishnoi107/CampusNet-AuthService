using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using AuthService.Domain.Entities;
using AuthService.Domain.Interfaces;
using AuthService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace AuthService.Infrastructure.Repositories
{
    public class UserCredentialRepository : IUserCredentialRepository
    {
        private readonly AuthDbContext _db;
        private readonly IUserEventPublisher _userEventPublisher;

        public UserCredentialRepository(AuthDbContext db, IUserEventPublisher userEventPublisher)
        {
            _db = db;
            _userEventPublisher = userEventPublisher;
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

        public async Task<UserCredentials> AddOrVerifySocialLogin(string email, string supabaseId, string provider,string name)
        {
            var user = _db.Users
                .FirstOrDefault(u => u.SupabaseId == supabaseId)
                ?? _db.Users.FirstOrDefault(u => u.Email == email);

            if (user == null)
            {
                user = new UserCredentials(name, email, supabaseId, provider);   
                _db.Users.Add(user);
                await _userEventPublisher.PublishUserCreatedEventAsync(user.Id, user.Email, user.Name, "");
            }
            else if (user.SupabaseId == null)
            {
             
                user.UpdateSupabaseId(supabaseId);
            }

           await _db.SaveChangesAsync();
            return user;
        }
    }
}
