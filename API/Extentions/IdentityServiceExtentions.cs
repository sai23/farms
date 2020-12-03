using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace API.Extentions
{
    public static class IdentityServiceExtentions
    {
        public static IServiceCollection AddIdentityService(this IServiceCollection services, IConfiguration config)
        {
                services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(

                options => {

                options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Tokenkey"])),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
                    }
                );

                return services;

        }
    }
}