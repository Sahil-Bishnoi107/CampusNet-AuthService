using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using AuthService.Domain.Interfaces;
using AuthService.Domain.Entities;

namespace AuthService.Application.Registration
{
    public record StartRegistrationCommand(string email, string name, string phoneNo) : IRequest;

    public class StartRegistrationValidator : AbstractValidator<StartRegistrationCommand>
    {
        public StartRegistrationValidator()
        {
            RuleFor(x => x.email).NotEmpty().EmailAddress();
            RuleFor(x => x.name).NotEmpty().MaximumLength(100);
            RuleFor(x => x.phoneNo).NotEmpty().MaximumLength(15);
        }
    }

    public class StartRegisterHandler : IRequestHandler<StartRegistrationCommand>
    {
        private readonly IUserCredentialRepository _users;
        private readonly IOtpRepository _otps;
        private readonly IEmailService _email;
        public StartRegisterHandler(IUserCredentialRepository users,IOtpRepository otps,IEmailService email)
        {
            _users = users;_otps = otps; _email = email;
        }
        public async Task Handle(StartRegistrationCommand request, CancellationToken cancellationToken)
        {
            if (await _users.ExistsByEmailAsync(request.email))
                throw new Exception("User already exists");

            var user = new UserCredentials(request.name, request.email, request.phoneNo);
            await _users.AddAsync(user);

            var code = Random.Shared.Next(100000, 999999).ToString();
            var otp = new OtpToken(user.Id, code);

            await _otps.AddAsync(otp);
            await _email.SendOtp(request.email, code);

        }

    }
}
