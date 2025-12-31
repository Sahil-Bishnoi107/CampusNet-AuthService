using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AuthService.Domain.Entities;

namespace AuthService.Domain.Interfaces
{
    public interface IOtpRepository
    {
        Task AddAsync(OtpToken otp);
        Task<OtpToken?> GetValidOtpAsync(string userId, string code);
        Task UpdateAsync(OtpToken otp);

        Task DeleteAsync(OtpToken otp);

        Task<OtpToken?> GetVerifiedOtpByEmail(string email);

        Task ResendOtp(string email);

        Task<OtpToken?> GetOtpByEmail(string email);

    }
}
