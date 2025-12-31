namespace AuthService.API.Contracts
{
   public record ForgotPassword(
    string Email,
    string NewPassword
   );
}
