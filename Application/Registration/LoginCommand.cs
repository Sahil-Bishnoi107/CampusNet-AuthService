using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AuthService.Domain.Entities;
using AuthService.Domain.Interfaces;
using AuthService.Infrastructure.Security;
using AuthService.Application.Contracts;
using MediatR;

namespace AuthService.Application.Registration
{
    public record LoginCommand(string Email, string Password) : IRequest<LoginResponse>;

    public class LoginHandler : IRequestHandler<LoginCommand, LoginResponse>
    {

        private readonly IUserCredentialRepository _users;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        private readonly IRefreshTokenRepository _refreshTokens;
        public LoginHandler(IUserCredentialRepository users, IPasswordHasher passwordHasher, IJwtTokenGenerator jwtTokenGenerator,IRefreshTokenRepository refreshTokes)
        {
            _users = users;
            _passwordHasher = passwordHasher;
            _jwtTokenGenerator = jwtTokenGenerator;
            _refreshTokens = refreshTokes;
        }
        public async Task<LoginResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var user = await _users.GetByEmailAsync(request.Email)
                ?? throw new Exception("Invalid credentials");

            if (!user.IsEmailVerified)
                throw new Exception("Email not verified");

            if (!user.IsActive)
                throw new Exception("Account inactive");

            if (!_passwordHasher.Verify(request.Password, user.PasswordHash!))
                throw new Exception("Invalid credentials");

            var refreshToken = RefreshTokenGenerator.Generate();
            var refreshHash = RefreshTokenGenerator.Hash(refreshToken);

            await _refreshTokens.AddAsync(
                new RefreshToken(
                    user.Id,
                    refreshHash,
                    DateTime.UtcNow.AddDays(30))
            );
            string accessToken =  _jwtTokenGenerator.Generate(user);
            
            return new LoginResponse(accessToken, refreshToken);


        }
    }
}
