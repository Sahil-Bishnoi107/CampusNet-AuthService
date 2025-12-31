using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using AuthService.Domain.Interfaces;

namespace AuthService.Application.Commands
{
   public record PasswordOtpVerificationCommand(string email, string otp) : IRequest;

    public class PasswordOtpVerificationCommandHandler : IRequestHandler<PasswordOtpVerificationCommand>
    {
        private readonly IPasswordSettingRepository _passwordSettingRepository;
        public PasswordOtpVerificationCommandHandler(IPasswordSettingRepository passwordSettingRepository)
        {
            _passwordSettingRepository = passwordSettingRepository;
        }
        public async Task Handle(PasswordOtpVerificationCommand request, CancellationToken cancellationToken)
        {
            await _passwordSettingRepository.OtpVerification(request.email, request.otp);
            
        }
    }

}
