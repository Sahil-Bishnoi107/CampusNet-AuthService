using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AuthService.Domain.Entities;
using AuthService.Domain.Interfaces;
using MediatR;

namespace AuthService.Application.Commands
{
    public record ForgotPasswordRequestCommand(string email) : IRequest;

    public class ForgotPasswordRequestHandler : IRequestHandler<ForgotPasswordRequestCommand>
    {
       private readonly IUserCredentialRepository _users;
       private readonly IOtpRepository _otpRepository;
        public ForgotPasswordRequestHandler(IOtpRepository otpRepository,IUserCredentialRepository users)
        {
            _otpRepository = otpRepository;
            _users = users;
        }
        public async Task Handle(ForgotPasswordRequestCommand request, CancellationToken cancellationToken)
        {
           UserCredentials? user = _users.GetByEmailAsync(request.email).Result;
            if (user == null) { throw new Exception("User not found"); }

            string userId = user.Id;

            string code = new Random().Next(100000, 999999).ToString();

            OtpToken otp = new OtpToken(userId, code);
            otp.SetEmail(request.email);
            await _otpRepository.AddAsync(otp);


        }
    }   

}
