using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Domain.Entities
{
    public class OtpToken

    {
        public string Id { get; private set; } 
        public string UserId { get; private set; }
        public string Code { get; private set; } = null!;
        public DateTime ExpiresAt { get; private set; }
        public bool Used { get; private set; }

        public string? Email { get;private set; }

        private OtpToken() { }

        public void SetEmail(string newemail) => Email = newemail; 

        public void setCode(string newcode) => Code = newcode;

        public void setExpiry() => ExpiresAt = DateTime.UtcNow.AddMinutes(10);


        public OtpToken(string userId, string code)
        {
            Id = Guid.NewGuid().ToString();
            UserId = userId;
            Code = code;
            ExpiresAt = DateTime.UtcNow.AddMinutes(10);
            Used = false;
        }

        public bool IsValid(string code) =>
            !Used && Code == code && DateTime.UtcNow <= ExpiresAt;

        public void MarkUsed() => Used = true;
    }
}
