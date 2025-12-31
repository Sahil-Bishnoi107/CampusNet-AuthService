using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AuthService.Domain.Entities;
using AuthService.Domain.Interfaces;
using AuthService.Infrastructure.Security;
using MediatR;

namespace AuthService.Application.Registration
{
    public record RefreshTokenCommand (UserCredentials user,string RefreshToken) : IRequest<string>;

    public class RefreshTokenHandler : IRequestHandler<RefreshTokenCommand, string>
    {
        private readonly IRefreshTokenRepository _tokens;
        private readonly IJwtTokenGenerator _jwt;
        public RefreshTokenHandler(IRefreshTokenRepository tokens, IJwtTokenGenerator jwt)
        {
            _tokens = tokens;
            _jwt = jwt;
        }
        public async Task<string> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            var hash = RefreshTokenGenerator.Hash(request.RefreshToken);

            var stored = await _tokens.GetValidAsync(request.user.Id, hash)
                ?? throw new Exception("Invalid refresh token");

            await _tokens.RevokeAsync(stored);

            var newRefresh = RefreshTokenGenerator.Generate();
            var newHash = RefreshTokenGenerator.Hash(newRefresh);

            await _tokens.AddAsync(
                new RefreshToken(
                    stored.UserId,
                    newHash,
                    DateTime.UtcNow.AddDays(30))
            );

            return _jwt.Generate(request.user);
        }
    }
}
