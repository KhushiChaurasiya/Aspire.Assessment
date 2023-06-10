using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Assignment.Core.Security
{
    public static class AuthenticationExtensions
    {

        public static void AddMarketplaceAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            try
            {
                var isAuthEnabled = false;
                var authenticationBuilder = services.AddAuthentication(options =>
                 {
                     options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                     options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                    
                 });
                
               
                IConfigurationSection config = configuration.GetSection("Authentication:Google");
                if (config != null)
                {
                    bool.TryParse(config["IsOAuthEnabled"], out isAuthEnabled);
                    if (isAuthEnabled)
                    {
                        
                        AddGoogleAuthentication(authenticationBuilder, config);
                    }
                }
                config = configuration.GetSection("Authentication:Jwt");
                if (config != null)
                {
                    bool.TryParse(config["IsJwtEnabled"], out isAuthEnabled);
                    if (isAuthEnabled)
                    {
                        services.AddAuthorization(options =>
                        {
                            //options.AddPolicy("AuthorizedUsersOnly", policy =>
                            //{
                            //    policy.RequireAuthenticatedUser();
                            //    policy.RequireClaim("userId");
                            //});
                        });
                        AddJwtAuthentication(authenticationBuilder, config);
                    }
                }
            }
            catch (Exception ex)
            {


            }

        }
        private static void AddGoogleAuthentication(AuthenticationBuilder authenticationBuilder, IConfigurationSection configurationSection)
        {

            //TODO  cheme to be handle 
            //oogleDefaults.AuthenticationScheme;

            authenticationBuilder.AddCookie()
            .AddGoogle(options =>
            {
                options.ClientId = configurationSection["client_id"];
                options.ClientSecret = configurationSection["client_secret"];
            });

        }

        private static void AddJwtAuthentication(AuthenticationBuilder authenticationBuilder, IConfigurationSection configurationSection)
        {
            var key = Encoding.ASCII.GetBytes(configurationSection.GetValue<string>("Secret"));
            authenticationBuilder.AddJwtBearer(options =>
             {
                 options.RequireHttpsMetadata = false;
                 options.SaveToken = true;
                 options.TokenValidationParameters = new TokenValidationParameters
                 {
                     ValidateIssuerSigningKey = true,
                     IssuerSigningKey = new SymmetricSecurityKey(key),
                     ValidateIssuer = false,
                     ValidateAudience = false
                 };
             });

        }
    }


}
