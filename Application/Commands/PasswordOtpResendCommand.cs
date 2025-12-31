using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AuthService.Domain.Interfaces;
using MediatR;

namespace AuthService.Application.Commands
{
    public record PasswordOtpResendCommand(string email) : IRequest;

    public class PasswordOtpResendHandler : IRequestHandler<PasswordOtpResendCommand>
    {
        private readonly IOtpRepository _otpRepository;
        public PasswordOtpResendHandler(IOtpRepository otpRepository)
        {
            _otpRepository = otpRepository;
        }
        public async Task Handle(PasswordOtpResendCommand request, CancellationToken cancellationToken)
        {
            await _otpRepository.ResendOtp(request.email);
        }
    }
}
