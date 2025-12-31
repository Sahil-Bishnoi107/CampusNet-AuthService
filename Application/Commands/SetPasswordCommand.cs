using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AuthService.Domain.Interfaces;
using MediatR;

namespace AuthService.Application.Commands
{
    public record SetNewPasswordCommand(string email, string newPassword) : IRequest;

    public class SetNewPasswordCommandHandler : IRequestHandler<SetNewPasswordCommand>
    {
        private readonly IPasswordSettingRepository _passwordSettingRepository;
        private readonly IPasswordHasher _passwordHasher;
        public SetNewPasswordCommandHandler(IPasswordSettingRepository passwordSettingRepository, IPasswordHasher passwordHasher)
        {
            _passwordSettingRepository = passwordSettingRepository;
            _passwordHasher = passwordHasher;
        }
        public async Task Handle(SetNewPasswordCommand request, CancellationToken cancellationToken)
        {
            string hashedPassword = _passwordHasher.Hash(request.newPassword);
            await _passwordSettingRepository.ChangePassword(request.email, hashedPassword);
        }
    }
}
