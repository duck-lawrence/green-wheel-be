using Application.Constants;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Application.Helpers
{
    public class JwtHelper
    {
        public static string GenerateUserIDToken(Guid userId, string sercretKey, string type, int expiredTime, string issuer, string audience, ClaimsPrincipal? oldClaim)
        {
            var claims = new List<Claim>
            {
                new (JwtRegisteredClaimNames.Sid, userId.ToString()),
                new ("type", type),
                new (JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64)
                
            };
            return GenerateToken(claims, sercretKey, expiredTime, issuer, audience, oldClaim);
        }
        public static string GenerateEmailToken(string email, string sercretKey, string type, int expiredTime, string issuer, string audience, ClaimsPrincipal? oldClaim)
        {
            var claims = new List<Claim>
            {
                new (JwtRegisteredClaimNames.Sid, email),
                new ("type", type),
                new (JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64)

            };
            return GenerateToken(claims, sercretKey, expiredTime, issuer, audience, oldClaim);
        }
        private static string GenerateToken(List<Claim> newClaims, string sercretKey, int expiredTime, string issuer, string audience, ClaimsPrincipal? oldClaim)
        {
            DateTime _expiredTime;
            if (oldClaim != null)
            {
                var expClaim = oldClaim.FindFirst("exp");
                if (expClaim != null && long.TryParse(expClaim.Value, out var expSeconds))
                {
                    _expiredTime = DateTimeOffset.FromUnixTimeSeconds(expSeconds).UtcDateTime;
                }
                else
                {
                    _expiredTime = DateTime.UtcNow.AddMinutes(expiredTime);
                }
            }
            else
            {
                _expiredTime = DateTime.UtcNow.AddMinutes(expiredTime);
            }


            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(sercretKey));
            var token = new JwtSecurityToken(
                    issuer: issuer,
                    audience: audience,
                    claims: newClaims,
                    expires: _expiredTime,
                    signingCredentials: new SigningCredentials(
                            key,
                            SecurityAlgorithms.HmacSha256Signature
                        )
                );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }


        public static ClaimsPrincipal VerifyToken(string token, string secretStr, string type, string issuer, string audience)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var secret = Encoding.UTF8.GetBytes(secretStr);
            
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = issuer,
                ValidAudience = audience,
                IssuerSigningKey = new SymmetricSecurityKey(secret),
                ClockSkew = TimeSpan.Zero
            };

            try
            {
            // sẽ throw nếu token không hợp lệ hoặc hết hạn
            SecurityToken validatedToken;
            var principal = tokenHandler.ValidateToken(token, validationParameters, out validatedToken);
            // có thể check thêm loại token (nếu bạn gắn claim type riêng)
            if (!string.IsNullOrEmpty(type))
                {
                    var typeClaim = principal.FindFirst("type"); // hoặc "token_type" tùy bạn set khi tạo
                    if (typeClaim == null || !typeClaim.Value.Equals(type, StringComparison.OrdinalIgnoreCase))
                    {
                        throw new UnauthorizedAccessException(Message.UserMessage.InvalidToken);
                    }
                }

                return principal; // token hợp lệ và còn hạn
            }
            catch
            {
                throw new UnauthorizedAccessException(Message.UserMessage.InvalidToken); // token không hợp lệ hoặc đã hết hạn
            }
        }
    }
}
