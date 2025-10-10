using Application.AppSettingConfigurations;
using Application.Constants;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Runtime.CompilerServices;
using System.Text;

namespace API.Extentions
{
    public static class JwtTokenValidation
    {
        public static void AddJwtTokenValidation(this IServiceCollection services, JwtSettings _jwtSetting)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme,
                options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ClockSkew = TimeSpan.Zero,
                        ValidIssuer = _jwtSetting.Issuer,
                        ValidAudience = _jwtSetting.Audience,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSetting.AccessTokenSecret))
                    };
                    options.Events = new JwtBearerEvents
                    {
                        OnAuthenticationFailed = context =>
                        {
                            var endpoint = context.HttpContext.GetEndpoint();
                            var hasAuthorize = endpoint?.Metadata.GetMetadata<Microsoft.AspNetCore.Authorization.AuthorizeAttribute>() != null;

                            if (hasAuthorize)
                            {

                                throw new UnauthorizedAccessException(Message.UserMessage.InvalidToken);
                            }


                            return Task.CompletedTask;
                        },
                        OnChallenge = context =>
                        {
                            throw new UnauthorizedAccessException(Message.UserMessage.MissingToken);
                        }, 
                    };
                }

                );
        }
    }
}
