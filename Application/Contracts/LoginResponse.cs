namespace AuthService.Application.Contracts
{
    public record LoginResponse(string AccessToken,string RefreshToken);
}
