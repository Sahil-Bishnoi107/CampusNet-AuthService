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
    public class RefreshTokenRepository : IRefreshTokenRepository
    {
        private readonly AuthDbContext _db;

        public RefreshTokenRepository(AuthDbContext db)
        {
            _db = db;
        }
        public async Task AddAsync(RefreshToken token)
        {
            _db.RefreshTokens.Add(token);
            await _db.SaveChangesAsync();
        }

        public async Task<RefreshToken?> GetValidAsync(string userId, string tokenHash)
        {
            return await _db.RefreshTokens.FirstOrDefaultAsync(t =>
                t.UserId == userId &&
                t.TokenHash == tokenHash &&
                !t.Revoked &&
                t.ExpiresAt > DateTime.UtcNow);
        }

        public async Task RevokeAsync(RefreshToken token)
        {
            token.Revoke();
            await _db.SaveChangesAsync();
        }
        public async Task LogOut(string UserId)
        {
            var tokens = _db.RefreshTokens.Where(t => t.UserId == UserId && !t.Revoked).ToList();
            foreach (var token in tokens)
            {
                token.Revoke();
            }
            await _db.SaveChangesAsync();
        }
    }
}
