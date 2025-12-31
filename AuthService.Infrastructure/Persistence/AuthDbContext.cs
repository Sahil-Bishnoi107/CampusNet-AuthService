using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AuthService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AuthService.Infrastructure.Persistence
{
    public class AuthDbContext : DbContext
    {
        public DbSet<UserCredentials> Users => Set<UserCredentials>();
        public DbSet<OtpToken> OtpTokens => Set<OtpToken>();

        public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();


        public AuthDbContext(DbContextOptions<AuthDbContext> options)
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder b)
        {
            b.Entity<UserCredentials>(e =>
            {
                e.ToTable("users");
                e.HasKey(x => x.Id);
                e.HasIndex(x => x.Email).IsUnique();
            });

            b.Entity<OtpToken>(e =>
            {
                e.ToTable("otp_tokens");
                e.HasKey(x => x.Id);
                e.HasIndex(x => new { x.UserId, x.Code });
            });
        }

        }
}
