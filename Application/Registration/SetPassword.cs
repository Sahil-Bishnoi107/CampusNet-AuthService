using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AuthService.Domain.Interfaces;
using MediatR;

namespace AuthService.Application.Registration
{
    public record SetPasswordCommand(string Email, string NewPassword) : IRequest;

    public class SetPasswordHandler : IRequestHandler<SetPasswordCommand> {
    
        private readonly IUserCredentialRepository _users;
        private readonly IPasswordHasher _passwordHasher;

       public SetPasswordHandler(IUserCredentialRepository users, IPasswordHasher passwordHasher)
        {
            _users = users;
            _passwordHasher = passwordHasher;
        }

        public async Task Handle(SetPasswordCommand request, CancellationToken cancellationToken)
        {
            var user = await _users.GetByEmailAsync(request.Email)
            ?? throw new Exception("User not found");

            if (!user.IsEmailVerified)
                throw new Exception("Email not verified");

            user.SetPassword(_passwordHasher.Hash(request.NewPassword));
            await _users.UpdateAsync(user);
        }




    }
}
