using Application.Abstractions;
using Application.AppSettingConfigurations;
using Application.Constants;
using Application.Dtos.User.Request;
using Application.Helpers;
using Application.Repositories;
using AutoMapper;
using Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using System.Security.Claims;

namespace Application
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly JwtSettings _jwtSettings;
        //private readonly EmailSettings _emailSettings;
        //private readonly IOTPRepository _otpRepository;
        private readonly IHttpContextAccessor _contextAccessor;
        //private readonly IMapper _mapper;
        public UserService(IUserRepository repository, 
            IOptions<JwtSettings> jwtSettings,
            IRefreshTokenRepository refreshTokenRepository,
             //IOptions<EmailSettings> emailSettings,
             //IOTPRepository otpRepository,
             IHttpContextAccessor httpContextAccessor
             //,IMapper mapper
            )
        {
            _userRepository = repository;
            _refreshTokenRepository = refreshTokenRepository;
            //_otpRepository = otpRepository;
            _jwtSettings = jwtSettings.Value;
            //_emailSettings = emailSettings.Value;
            _contextAccessor = httpContextAccessor;
            //_mapper = mapper;
        }

        public async Task<string?> Login(UserLoginReq user)
        {
            User userFromDB = await _userRepository.GetByEmailAsync(user.Email);

            if (userFromDB != null)
            {
                if (PasswordHelper.VerifyPassword(user.Password, userFromDB.Password))
                {
                    //tạo refreshtoken và lưu nó vào DB lẫn cookie
                    await GenerateRefreshToken(userFromDB.Id, null);
                    return GenerateAccessToken(userFromDB.Id);
                }
            }
            throw new UnauthorizedAccessException(Message.User.InvalidEmailOrPassword);
        }
        public string GenerateAccessToken(Guid userId)
        {

            return JwtHelper.GenerateUserIDToken(userId, _jwtSettings.AccessTokenKey, TokenType.AccessToken.ToString(), _jwtSettings.AccessTokenExpiredTime, _jwtSettings.Issuer, _jwtSettings.Audience, null);
        }
        public async Task<string> GenerateRefreshToken(Guid userId, ClaimsPrincipal? oldClaims)
        {
            var _context = _contextAccessor.HttpContext;
            string token = JwtHelper.GenerateUserIDToken(userId, _jwtSettings.RefreshTokenKey, TokenType.RefreshToken.ToString(),
                _jwtSettings.RefreshTokenExpiredTime, _jwtSettings.Issuer, _jwtSettings.Audience, oldClaims);
            ClaimsPrincipal claims = JwtHelper.VerifyToken(token, _jwtSettings.RefreshTokenKey, TokenType.RefreshToken.ToString(),
                _jwtSettings.Issuer, _jwtSettings.Audience);
            long.TryParse(claims.FindFirst(JwtRegisteredClaimNames.Iat).Value, out long iatSeconds);
            long.TryParse(claims.FindFirst(JwtRegisteredClaimNames.Exp).Value, out long expSeconds);
            Guid refreshTokenId;
            do
            {
                refreshTokenId = Guid.NewGuid();
            } while (await _refreshTokenRepository.GetByIdAsync(refreshTokenId) != null);
            await _refreshTokenRepository.AddAsync(new RefreshToken()
            {
                Id = refreshTokenId,
                UserId = userId,
                Token = token,
                IssuedAt = DateTimeOffset.FromUnixTimeSeconds(iatSeconds).UtcDateTime,
                CreatedAt = DateTimeOffset.FromUnixTimeSeconds(iatSeconds).UtcDateTime,
                UpdatedAt = DateTimeOffset.FromUnixTimeSeconds(iatSeconds).UtcDateTime,
                ExpiresAt = DateTimeOffset.FromUnixTimeSeconds(expSeconds).UtcDateTime,
                IsRevoked = false
            });
            //lưu vào cookie
            _context.Response.Cookies.Append(CookieKeys.RefreshToken, token, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,         // chỉ gửi qua HTTPS
                SameSite = SameSiteMode.Strict, // tránh CSRF
                Expires = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpiredTime) // hạn sử dụng
            });
            return token;

        }
    }
}
