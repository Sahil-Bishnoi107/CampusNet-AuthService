using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AuthService.Domain.Interfaces;
using MediatR;

namespace AuthService.Application.Registration
{
    public record VerifyRegistrationOtpCommand(string email, string OtpCode) : IRequest;

    public class VerifyRegistrationOtpHnadler : IRequestHandler<VerifyRegistrationOtpCommand> {
        private readonly IUserCredentialRepository _users;
        private readonly IOtpRepository _otps;
        public VerifyRegistrationOtpHnadler(IUserCredentialRepository user, IOtpRepository otpRepository)
        {
            _users = user;
            _otps = otpRepository;
        }

        public async Task Handle(VerifyRegistrationOtpCommand request, CancellationToken cancellationToken)
        {
            var user = await _users.GetByEmailAsync(request.email)
             ?? throw new Exception("User not found");

            var otp = await _otps.GetValidOtpAsync(user.Id, request.OtpCode)
                ?? throw new Exception("Invalid OTP");

            otp.MarkUsed();
            user.VerifyEmail();

            await _otps.UpdateAsync(otp);
            await _users.UpdateAsync(user);
        }


    }

}
