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
    public class OtpRepository : IOtpRepository

    {
        private readonly AuthDbContext _db;

        public OtpRepository(AuthDbContext db)
        {
            _db = db;
        }
        public async Task AddAsync(OtpToken otp)
        {
            _db.OtpTokens.Add(otp);
            await _db.SaveChangesAsync();
        }

        public async Task<OtpToken?> GetValidOtpAsync(string userId, string code)
        {
            OtpToken? otp = await _db.OtpTokens.FirstOrDefaultAsync(x =>
            x.UserId == userId &&
            x.Code == code &&
            !x.Used &&
            x.ExpiresAt >= DateTime.UtcNow);
            return otp;
        }

        public async Task<OtpToken?> GetOtpByEmail(string email)
        {
            return await _db.OtpTokens
                .Where(x => x.Email == email && !x.Used)
                .FirstOrDefaultAsync();
        }

        public async Task UpdateAsync(OtpToken otp)
        {
            _db.OtpTokens.Update(otp);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteAsync(OtpToken otp)
        {
            _db.OtpTokens.Remove(otp);
            await _db.SaveChangesAsync();
        }

        public async Task<OtpToken?> GetVerifiedOtpByEmail(string email)
        {
            return await _db.OtpTokens
                .Where(x => x.Email == email && x.Used)
                .FirstOrDefaultAsync();
        }

        public async Task ResendOtp(string email)
        {
            OtpToken? otp = await GetOtpByEmail(email);
            if (otp == null)
            {
                throw new Exception("Something went wrong");

            }
            string newcode = Random.Shared.Next(100000, 999999).ToString();
            otp.setCode(newcode);
            otp.setExpiry();
            await UpdateAsync(otp);
        }
    }
}
