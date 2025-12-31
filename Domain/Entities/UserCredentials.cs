namespace AuthService.Domain.Entities
{
    public class UserCredentials
    {
        public string Id { get; private set; }
        public string? Username { get; private set; }
        public string? PasswordHash { get; private set; }
        public string Name { get; private set; } = null!;
        public string Email { get; private set; } = null!;
        public string PhoneNumber { get; private set; } = null!;
        public bool IsEmailVerified { get; private set; }
        public bool IsActive { get; private set; }
        public DateTime CreatedAt { get; private set; }

        // ✅ REQUIRED BY EF
        private UserCredentials() { }

        public UserCredentials(string name, string email, string phoneNumber)
        {
            Id = Guid.NewGuid().ToString();
            Name = name;
            Email = email.ToLower();
            PhoneNumber = phoneNumber;
            IsEmailVerified = false;
            IsActive = false;
            CreatedAt = DateTime.UtcNow;
        }

        public void VerifyEmail() => IsEmailVerified = true;

        public void SetPassword(string hash)
        {
            PasswordHash = hash;
            IsActive = true;
        }
    }
}
