namespace AuthService.API.Contracts
{
    public record RegisterStartRequest(
    string Name,
    string Email,
    string PhoneNumber);

}
