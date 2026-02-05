using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Supabase;
using Supabase.Gotrue;


namespace AuthService.Infrastructure.supabase
{
    using Microsoft.Extensions.Configuration;
    using Supabase;

    public class SupabaseAuthService
    {
        private readonly Client _client;

        public SupabaseAuthService(IConfiguration config)
        {
            _client = new Client(
                config["Supabase:Url"]!,
                config["Supabase:ServiceRoleKey"]! // 🔴 backend only
            );

            _client.InitializeAsync().GetAwaiter().GetResult();
        }

        public async Task<User?> GetUserFromToken(string accessToken)
        {
            try
            {
                return await _client.Auth.GetUser(accessToken);
            }
            catch
            {
                return null;
            }
        }
    }

}
