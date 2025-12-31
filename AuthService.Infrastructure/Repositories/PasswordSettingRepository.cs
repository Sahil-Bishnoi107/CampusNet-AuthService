using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AuthService.Domain.Entities;
using AuthService.Domain.Interfaces;
using AuthService.Infrastructure.Persistence;

namespace AuthService.Infrastructure.Repositories
{
    public class PasswordSettingRepository : IPasswordSettingRepository
    {
        private readonly AuthDbContext _context;
        private readonly IOtpRepository _otpRepository;
        private readonly IUserCredentialRepository _userCredentialRepository;
        public PasswordSettingRepository(AuthDbContext context,IOtpRepository otpRepository, IUserCredentialRepository userCredentialRepository)
        {
            _context = context;
            _otpRepository = otpRepository;
            _userCredentialRepository = userCredentialRepository;
        }
        public async Task ChangePassword(string email, string newPassword)
        {
            OtpToken? b = await _otpRepository.GetVerifiedOtpByEmail(email);
            if(b == null)
            {
                throw new Exception("OTP not verified");
            }
            UserCredentials? user = await _userCredentialRepository.GetByEmailAsync(email.ToString());
            if (user == null)
            {
                throw new Exception("User not found");
            }
            user.SetPassword(newPassword);
            _context.OtpTokens.Remove(b);

        }

        public async Task OtpVerification(string email, string otp)
        {
            UserCredentials? user = await _userCredentialRepository.GetByEmailAsync(email.ToString());
            if (user == null)
            {
                throw new Exception("User not found");
            }
            OtpToken? otpToken = await _otpRepository.GetValidOtpAsync(user.Id,otp);
            if (otpToken == null)
            {
                throw new Exception("Invalid OTP");
            }
            otpToken.MarkUsed();
          await  _otpRepository.UpdateAsync(otpToken);
            

        }

        
    }
}
