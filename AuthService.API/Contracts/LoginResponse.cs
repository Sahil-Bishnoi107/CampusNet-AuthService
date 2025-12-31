namespace AuthService.API.Contracts
{
    public record LoginResponse(string AccessToken,string RefreshToken);
}
