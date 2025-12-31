using AuthService.Domain.Entities;

namespace AuthService.API.Contracts
{
    public record RefreshRequest(UserCredentials user, string RefreshToken);

}
