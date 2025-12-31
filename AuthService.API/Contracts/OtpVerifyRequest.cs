namespace AuthService.API.Contracts
{
    public record VerifyOtpRequest(
    string Email,
    string Otp);
}
