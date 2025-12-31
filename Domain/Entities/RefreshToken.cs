using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Domain.Entities
{
    public class RefreshToken
    {
        public string Id { get; private set; }
        public string UserId { get; private set; }
        public string TokenHash { get; private set; }
        public DateTime ExpiresAt { get; private set; }
        public bool Revoked { get; private set; }
        public DateTime CreatedAt { get; private set; }

        private RefreshToken() { }

        public RefreshToken(string userId, string tokenHash, DateTime expiresAt)
        {
            Id = Guid.NewGuid().ToString();
            UserId = userId;
            TokenHash = tokenHash;
            ExpiresAt = expiresAt;
            CreatedAt = DateTime.UtcNow;
        }

        public void Revoke() => Revoked = true;
    }
}
