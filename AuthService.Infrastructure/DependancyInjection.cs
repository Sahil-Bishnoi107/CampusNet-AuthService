using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AuthService.Domain.Interfaces;
using AuthService.Infrastructure.Email;
using AuthService.Infrastructure.Messaging;
using AuthService.Infrastructure.Persistence;
using AuthService.Infrastructure.Repositories;
using AuthService.Infrastructure.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using AuthService.Infrastructure.supabase;

namespace AuthService.Infrastructure
{
    public static class DependancyInjection
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddDbContext<AuthDbContext>(opt =>
            opt.UseNpgsql(config.GetConnectionString("AuthDb")));

            services.AddScoped<IUserCredentialRepository, UserCredentialRepository>();
            services.AddScoped<IOtpRepository, OtpRepository>();
            services.AddScoped<IPasswordHasher, PasswordHasher>();
            services.AddScoped<IEmailService, SmtpEmailService>();
            services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
            services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
            services.AddScoped<IPasswordSettingRepository, PasswordSettingRepository>();
            services.AddSingleton<RabbitMqConnection>();
            services.AddScoped<IUserEventPublisher, RabbitMqUserEventPublisher>();
            services.AddScoped<INotificationPublisher, RabbitMqNotificationPublisher>();
            services.AddSingleton<SupabaseAuthService>();

            services.AddAuthentication()
              .AddJwtBearer("Supabase", options =>
              {
                  options.MapInboundClaims = false;
           
                  options.MetadataAddress =
                      "https://jouwsalxgeleizhjzujq.supabase.co/auth/v1/.well-known/openid-configuration";
           
                  options.TokenValidationParameters = new TokenValidationParameters
                  {
                      ValidateIssuer = true,
                      ValidIssuer = "https://jouwsalxgeleizhjzujq.supabase.co/auth/v1",
           
                      ValidateAudience = true,
                      ValidAudience = "authenticated",
           
                      ValidateLifetime = true,
                      ValidateIssuerSigningKey = true
                  };
              });

            services.AddAuthorization();

            return services;
        }
    }
}
