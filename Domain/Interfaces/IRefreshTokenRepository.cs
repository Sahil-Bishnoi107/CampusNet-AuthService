using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AuthService.Domain.Entities;

namespace AuthService.Domain.Interfaces
{
    public interface IRefreshTokenRepository
    {
        Task AddAsync(RefreshToken token);
        Task<RefreshToken?> GetValidAsync(string userId, string tokenHash);
        Task RevokeAsync(RefreshToken token);
        Task LogOut(string userId);
    }
}
