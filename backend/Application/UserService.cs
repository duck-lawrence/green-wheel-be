using Application.Abstractions;
using Application.AppExceptions;
using Application.AppSettingConfigurations;
using Application.Constants;
using Application.Dtos.User.Request;
using Application.Helpers;
using Application.Repositories;
using AutoMapper;
using Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using System.Security.Claims;

namespace Application
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IOTPRepository _otpRepository;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly JwtSettings _jwtSettings;
        private readonly EmailSettings _emailSettings;
        private readonly OTPSettings _otpSettings;
        private readonly IMapper _mapper;
        private readonly IMemoryCache _cache;
        public UserService(IUserRepository repository, 
            IOptions<JwtSettings> jwtSettings,
            IRefreshTokenRepository refreshTokenRepository,
             IOptions<EmailSettings> emailSettings,
             IOTPRepository otpRepository,
             IHttpContextAccessor httpContextAccessor,
             IOptions<OTPSettings> otpSetting,
             IMapper mapper,
             IMemoryCache cache
            )
        {
            _userRepository = repository;
            _refreshTokenRepository = refreshTokenRepository;
            _otpRepository = otpRepository;
            _jwtSettings = jwtSettings.Value;
            _emailSettings = emailSettings.Value;
            _contextAccessor = httpContextAccessor;
            _otpSettings = otpSetting.Value;
            _mapper = mapper;
            _cache = cache;
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
        public async Task SendOTP(string email)
        {
            int count = await _otpRepository.CountRateLimitAsync(email);
            if (count > _otpSettings.OtpRateLimit)
            {
                throw new RateLimitExceededException(Message.User.RateLimitOtp);
            }
            await _otpRepository.RemoveOTPAsync(email); //xoá cũ trước khi lưu cái ms
            string otp = new Random().Next(100000, 900000).ToString();
            await _otpRepository.SaveOTPAsyns(email, otp);
            string subject = "Your OTP code";
            string body = $"OTP: {otp} có quá oke khum người đẹp";
            await EmailHelper.SendEmailAsync(_emailSettings, email, subject, body);
        }

        //hàm này sẽ giúp verify otp và gưi ra token tương ứng
        public async Task<string> VerifyOTPAndEmail(VerifyOTPReq verifyOTPDto, TokenType type, string cookieKey)
        {
            string? otpFromRedis = await _otpRepository.GetOtpAsync(verifyOTPDto.Email);
            if (otpFromRedis == null)
            {
                throw new UnauthorizedAccessException(Message.User.InvalidOTP);
            }
            if(verifyOTPDto.OTP != otpFromRedis)
            {
                int count = await _otpRepository.CountAttemptAsync(verifyOTPDto.Email);
                if (count > _otpSettings.OtpAttempts)
                {
                    await _otpRepository.RemoveOTPAsync(verifyOTPDto.Email);
                    await _otpRepository.ResetAttemptAsync(verifyOTPDto.Email);
                    throw new UnauthorizedAccessException(Message.Common.TooManyRequest);
                }
                throw new UnauthorizedAccessException(Message.User.InvalidOTP);
            }
            var _context = _contextAccessor.HttpContext;
            await _otpRepository.RemoveOTPAsync(verifyOTPDto.Email);
            string secretKey = "";
            int expiredTime;
            if (type == TokenType.RegisterToken)
            {
                secretKey = _jwtSettings.RegisterTokenKey;
                expiredTime = _jwtSettings.RefreshTokenExpiredTime;
            }
            else
            {
                secretKey = _jwtSettings.ForgotPasswordTokenKey;
                expiredTime = _jwtSettings.ForgotPasswordTokenExpiredTime;
            }
            string token = JwtHelper.GenerateEmailToken(verifyOTPDto.Email, secretKey, type.ToString(), expiredTime, _jwtSettings.Issuer, _jwtSettings.Audience, null);
            _context.Response.Cookies.Append(cookieKey, token, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,         // chỉ gửi qua HTTPS
                SameSite = SameSiteMode.Strict, // tránh CSRF
                Expires = DateTime.UtcNow.AddMinutes(expiredTime) // hạn sử dụng
            });
            return token;
        }

        public async Task<string> RegisterAsync(string token, UserRegisterReq userRegisterReq)
        {
            var user = _mapper.Map<User>(userRegisterReq); //map từ một RegisterUserDto sang user
            ClaimsPrincipal claims = JwtHelper.VerifyToken(token, _jwtSettings.RegisterTokenKey, TokenType.RegisterToken.ToString(), _jwtSettings.Issuer, _jwtSettings.Audience);
            var email = claims.FindFirst(JwtRegisteredClaimNames.Sid).Value.ToString();
            var userFromDB = await _userRepository.GetByEmailAsync(email);

            if (userFromDB != null)
            {
                throw new ConflictDuplicateException(Message.User.EmailAlreadyExists); //email đã tồn tại
            }
            Guid id;
            do
            {
                id = Guid.NewGuid();
            } while (await _userRepository.GetByIdAsync(id) != null);
            //lấy ra list role trong cache
            var roles = _cache.Get<List<Role>>("AllRoles");

            user.Id = id;
            user.CreatedAt = user.UpdatedAt = DateTime.UtcNow;
            user.Email = email;
            user.RoleId = roles.FirstOrDefault(r => r.Name == "Customer").Id;
            user.DeletedAt = null;
            Guid userId = await _userRepository.AddAsync(user);
            string accesstoken = GenerateAccessToken(userId);
            string refreshToken = await GenerateRefreshToken(userId, null);

            return accesstoken;
        }

        public async Task ChangePassword(ClaimsPrincipal userClaims, string password, string oldPassword)
        {
            var userID = userClaims.FindFirstValue(JwtRegisteredClaimNames.Sid)!.ToString();
            var userFromDB = await _userRepository.GetByIdAsync(Guid.Parse(userID));
            if (userFromDB == null)
            {
                throw new UnauthorizedAccessException(Message.User.Unauthorized);
            }
            if (!PasswordHelper.VerifyPassword(oldPassword, userFromDB.Password))
            {
                throw new UnauthorizedAccessException(Message.User.OldPasswordIsIncorrect);
            }
            await _refreshTokenRepository.RevokeRefreshTokenByUserID(userID);
            userFromDB.Password = PasswordHelper.HashPassword(password);
            await _userRepository.UpdateAsync(userFromDB);
        }
        public async Task ResetPassword(string forgotPasswordToken, string password)
        {
            var claims = JwtHelper.VerifyToken(forgotPasswordToken, _jwtSettings.ForgotPasswordTokenKey,
                                                TokenType.ForgotPasswordToken.ToString(), _jwtSettings.Issuer, _jwtSettings.Audience);
            string email = claims.FindFirstValue(JwtRegisteredClaimNames.Sid)!.ToString();
            var userFromDB = await _userRepository.GetByEmailAsync(email);
            if (userFromDB == null)
            {
                throw new BadHttpRequestException(Message.User.InvalidToken);
            }
            await _refreshTokenRepository.RevokeRefreshTokenByUserID(userFromDB.Id.ToString());
            userFromDB.Password = PasswordHelper.HashPassword(password);
            await _userRepository.UpdateAsync(userFromDB);

        }

        public async Task<int> Logout(string refreshToken)
        {
            JwtHelper.VerifyToken(refreshToken, _jwtSettings.RefreshTokenKey, TokenType.RefreshToken.ToString(), _jwtSettings.Issuer, _jwtSettings.Audience);
            return await _refreshTokenRepository.RevokeRefreshToken(refreshToken);
        }

        public async Task<string> RefreshToken(string refreshToken, bool getRevoked)
        {

            ClaimsPrincipal claims = JwtHelper.VerifyToken(refreshToken,
                                                            _jwtSettings.RefreshTokenKey,
                                                            TokenType.RefreshToken.ToString(),
                                                            _jwtSettings.Issuer,
                                                            _jwtSettings.Audience);

            if (claims != null)
            {
                RefreshToken refreshTokenFromDB = await _refreshTokenRepository.GetByRefreshToken(refreshToken, getRevoked);
                if (refreshTokenFromDB == null)
                {
                    throw new UnauthorizedAccessException(Message.User.InvalidToken);
                }
                string newAccessToken = GenerateAccessToken(refreshTokenFromDB.UserId);
                string newRefreshToken = await GenerateRefreshToken(refreshTokenFromDB.UserId, claims);
                await _refreshTokenRepository.RevokeRefreshToken(refreshTokenFromDB.Token);

                return newAccessToken;


            }

            throw new UnauthorizedAccessException(Message.User.InvalidToken);
        }
    }
}
