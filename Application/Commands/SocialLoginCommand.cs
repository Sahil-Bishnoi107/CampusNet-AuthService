using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AuthService.Application.Contracts;
using AuthService.Domain.Entities;
using AuthService.Domain.Interfaces;
using AuthService.Infrastructure.Security;
using MediatR;


namespace AuthService.Application.Commands
{
    public record SocialLoginCommand(string email, string supabaseId, string name,string authProvider) : IRequest<string>;

    public class SocialLoginHandler : IRequestHandler<SocialLoginCommand,string>
    {
        private readonly IUserCredentialRepository _userCredentialRepository;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        private readonly IRefreshTokenRepository _refreshTokens;

        public SocialLoginHandler(IUserCredentialRepository userCredentialRepository,IJwtTokenGenerator jwtTokenGenerator, IRefreshTokenRepository refreshTokenRepository)
        {
            _userCredentialRepository = userCredentialRepository;
            _jwtTokenGenerator = jwtTokenGenerator;
            _refreshTokens = refreshTokenRepository;
        }
        public async Task<string> Handle(SocialLoginCommand request, CancellationToken cancellationToken)
        {
         var user =   await _userCredentialRepository.AddOrVerifySocialLogin(request.email, request.supabaseId, request.authProvider, request.name);
            
            
            string accessToken = _jwtTokenGenerator.Generate(user);

            return accessToken;

        }
    }

}
