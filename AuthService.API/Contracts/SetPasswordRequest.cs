namespace AuthService.API.Contracts
{
    public record SetPasswordRequest(
    string Email,
    string Password);

}
