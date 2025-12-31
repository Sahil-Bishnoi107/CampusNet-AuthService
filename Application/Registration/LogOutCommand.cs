using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AuthService.Domain.Entities;
using AuthService.Domain.Interfaces;
using MediatR;

namespace AuthService.Application.Registration
{
    public record LogOutCommand(string userId) : IRequest;
    public class LogOutCommandHandler : IRequestHandler<LogOutCommand>
    {
        IRefreshTokenRepository _refreshTokenRepository;
        
        
        public LogOutCommandHandler(IRefreshTokenRepository refreshTokenRepository)
        {
            _refreshTokenRepository = refreshTokenRepository;
        }
        public async Task Handle(LogOutCommand request, CancellationToken cancellationToken)
        {
           await _refreshTokenRepository.LogOut(request.userId); 
            
        }
    }   
}
