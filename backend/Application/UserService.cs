using Application.Abstractions;
using Application.AppExceptions;
using Application.AppSettingConfigurations;
using Application.Constants;
using Application.Dtos.Common.Request;
using Application.Dtos.User.Request;
using Application.Dtos.User.Respone;
using Application.Helpers;
using Application.Repositories;
using AutoMapper;
using Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Newtonsoft.Json.Linq;
using System.Security.Claims;

namespace Application
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IOTPRepository _otpRepository;
        private readonly IJwtBlackListRepository _jwtBackListRepository;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly JwtSettings _jwtSettings;
        private readonly EmailSettings _emailSettings;
        private readonly OTPSettings _otpSettings;
        private readonly IMapper _mapper;
        private readonly IMemoryCache _cache;
        private readonly IPhotoService _photoService;
        private readonly ILogger<UserService > _logger;
        public UserService(IUserRepository repository, 
            IOptions<JwtSettings> jwtSettings,
            IRefreshTokenRepository refreshTokenRepository,
             IJwtBlackListRepository jwtBackListRepository,
             IOptions<EmailSettings> emailSettings,
             IOTPRepository otpRepository,
             IHttpContextAccessor httpContextAccessor,
             IOptions<OTPSettings> otpSetting,
             IMapper mapper,
             IMemoryCache cache,
             IPhotoService photoService,
             ILogger<UserService> logger
            )
        {
            _userRepository = repository;
            _refreshTokenRepository = refreshTokenRepository;
            _otpRepository = otpRepository;
            _jwtBackListRepository = jwtBackListRepository;
            _jwtSettings = jwtSettings.Value;
            _emailSettings = emailSettings.Value;
            _contextAccessor = httpContextAccessor;
            _otpSettings = otpSetting.Value;
            _mapper = mapper;
            _cache = cache;
            _photoService = photoService;
            _logger = logger;
        }

        
        /*
         Login fuction
         this func receive UserLoginReq (dto) form controller incluce (user email and password)
         -> got user from Db by email
            -> null <=> wrong email or password => throw Unauthorize Exception (401)
            -> !null <=> correct email and password => generate refreshToken (set to cookie) and accessToken return to frontend
         */
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

        /*
         Generate Access Token Func
        this func recieve userId
        use jwtHelper and give userID, accesstoken secret secret, type: accesstoken, access token expired time, isser and audience
        to generate access token
         */
        public string GenerateAccessToken(Guid userId)
        {

            return JwtHelper.GenerateUserIDToken(userId, _jwtSettings.AccessTokenSecret, TokenType.AccessToken.ToString(), _jwtSettings.AccessTokenExpiredTime, _jwtSettings.Issuer, _jwtSettings.Audience, null);
        }



        /*
         Generate Refresh Token Func
         This func recieve userId and a ClaimsPrincipal if any 
            - When we use refresh token to got a new access token, we will generate a new refresh 
              token with expired time of old refresh token if it was not expired
              so that we will give a ClaimsPricipal for that func
        It use jwt helper to generate a token
        then verify this token to got a claimPricipal to take Iat (created time), Exp (expired time) to save this toke to DB
        
        Then set this token to cookie
         */
        public async Task<string> GenerateRefreshToken(Guid userId, ClaimsPrincipal? oldClaims)
        {
            var _context = _contextAccessor.HttpContext;
            string token = JwtHelper.GenerateUserIDToken(userId, _jwtSettings.RefreshTokenSecret, TokenType.RefreshToken.ToString(),
                _jwtSettings.RefreshTokenExpiredTime, _jwtSettings.Issuer, _jwtSettings.Audience, oldClaims);
            ClaimsPrincipal claims = JwtHelper.VerifyToken(token, _jwtSettings.RefreshTokenSecret, TokenType.RefreshToken.ToString(),
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

        /*
         Send OTP Func
        This function recieve email from controller
        User can got 1 Otp per minutes
        if it func call > 1 turn per minutes -> throw Rate Limit Exceeded Exception (429)
        Remove old OTP in DB before save new Otp
        generate new otp -> save to DB
        send this otp to user email
         */
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

        /*
         Verify OTP function
         this function recieve verifyOTPDto from controller include OTP and email
            Token type (type of token to generate) and cookieKey (name of token to save to cookie

        First we use email to take otp from DB
            - Null => this email do not have OTP -> throw Unauthorize Exception (401)
            - !null => this email got a OTP

        Next check OTP & OTP form DB
            - if != => count number of times entered & throw Unauthorize Exception (401) 
                       if count > number of entries allowed -> delete otp form DB
        
        Then generate token by email belong to token type and set it to cookie
            - Register token when register account
            - forgot password token when user forgot thier password
        */
        public async Task<string> VerifyOTP(VerifyOTPReq verifyOTPDto, TokenType type, string cookieKey)
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
            string secret = "";
            int expiredTime;
            if (type == TokenType.RegisterToken)
            {
                secret = _jwtSettings.RegisterTokenSecret;
                expiredTime = _jwtSettings.RegisterTokenExpiredTime;
            }
            else
            {
                secret = _jwtSettings.ForgotPasswordTokenSecret;
                expiredTime = _jwtSettings.ForgotPasswordTokenExpiredTime;
            }
            string token = JwtHelper.GenerateEmailToken(verifyOTPDto.Email, secret, type.ToString(), expiredTime, _jwtSettings.Issuer, _jwtSettings.Audience, null);
            _context.Response.Cookies.Append(cookieKey, token, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,         // chỉ gửi qua HTTPS
                SameSite = SameSiteMode.Strict, // tránh CSRF
                Expires = DateTime.UtcNow.AddMinutes(expiredTime) // hạn sử dụng
            });
            return token;
        }


        /*
         Register account func
         This function receive token from controller ( register token ) => done verify otp And userRegisterReq 
                                                                                            include user info
         it map userRegisterReq to user
         Then verify this token 
            -   401 : invalid token
            - If success -> got claimsPricipal of this token
         got email form this claimPrincipal
         try got user form Db by email
            - != null -> throw Duplicate Exception (409)
            - == null -> create new account =>  generate refreshToken (set to cookie) and accessToken return to frontend
         */
        public async Task<string> RegisterAsync(string token, UserRegisterReq userRegisterReq)
        {
            if(await _userRepository.GetByPhoneAsync(userRegisterReq.Phone) != null)
            {
                throw new ConflictDuplicateException(Message.User.PhoneAlreadyExist);
            }
            //----check in black list
            if (await _jwtBackListRepository.CheckTokenInBlackList(token))
            {
                throw new UnauthorizedAccessException(Message.User.InvalidToken);
            }
            //------------------------
            var user = _mapper.Map<User>(userRegisterReq); //map từ một RegisterUserDto sang user
            var claims = JwtHelper.VerifyToken(token, _jwtSettings.RegisterTokenSecret, 
                TokenType.RegisterToken.ToString(), _jwtSettings.Issuer, _jwtSettings.Audience);

            
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

            //----save to black list
            long.TryParse(claims.FindFirst(JwtRegisteredClaimNames.Exp).Value, out long expSeconds);
            await _jwtBackListRepository.SaveTokenAsyns(token, expSeconds);

            return accesstoken;
        }


        /*
         Change password func
         This func use for change password use case
         IT recieve userClaims from token of accessToken, oldPassword and new password => verify => take user ID from claims
         got user from DB by id 
            - null -> throw unauthorized exception (401) (invalid accesstoken)
            - != null  -> verify password in DB == old password ?
                - == -> set new passwrd
                - != return unauthorized (401) (old password is incorrect)
            
         */
        public async Task ChangePassword(ClaimsPrincipal userClaims, UserChangePasswordReq userChangePasswordReq)
        {
            var userID = userClaims.FindFirstValue(JwtRegisteredClaimNames.Sid)!.ToString();
            var userFromDB = await _userRepository.GetByIdAsync(Guid.Parse(userID));
            if (userFromDB == null)
            {
                throw new UnauthorizedAccessException(Message.User.Unauthorized);
            }
            if (!PasswordHelper.VerifyPassword(userChangePasswordReq.OldPassword, userFromDB.Password))
            {
                throw new UnauthorizedAccessException(Message.User.OldPasswordIsIncorrect);
            }
            await _refreshTokenRepository.RevokeRefreshTokenByUserID(userID);
            userFromDB.Password = PasswordHelper.HashPassword(userChangePasswordReq.Password);
            await _userRepository.UpdateAsync(userFromDB);
        }
        
        /*
         Reset Password Func
         This function use for forgot password use case
         it recieve forgotPasswordToken (after verify email) and password from Controller
         verify this token 
            - 401 : Invalid token
           
         if success -> got a claims -> take email form claim -> find user in DB by email
            - == null -> throw unAuthorized exception (401) : invalid token (hacker)
            - != null => revoke all refresh token of this account from DB and change password 
            
         */
        public async Task ResetPassword(string forgotPasswordToken, string password)
        {
            //----check in black list
            if (await _jwtBackListRepository.CheckTokenInBlackList(forgotPasswordToken))
            {
                throw new UnauthorizedAccessException(Message.User.InvalidToken);
            }
            //------------------------
            var claims = JwtHelper.VerifyToken(forgotPasswordToken, _jwtSettings.ForgotPasswordTokenSecret,
                                                TokenType.ForgotPasswordToken.ToString(), _jwtSettings.Issuer, _jwtSettings.Audience);

            //------------------------
            string email = claims.FindFirstValue(JwtRegisteredClaimNames.Sid)!.ToString();
            var userFromDB = await _userRepository.GetByEmailAsync(email);
            if (userFromDB == null)
            {
                throw new UnauthorizedAccessException(Message.User.InvalidToken);
            }
            await _refreshTokenRepository.RevokeRefreshTokenByUserID(userFromDB.Id.ToString());
            userFromDB.Password = PasswordHelper.HashPassword(password);
            await _userRepository.UpdateAsync(userFromDB);
            //---- save to black list
            long.TryParse(claims.FindFirst(JwtRegisteredClaimNames.Exp).Value, out long expSeconds);
            await _jwtBackListRepository.SaveTokenAsyns(forgotPasswordToken, expSeconds);
        }


        /*
         Logout func
         this function got refresh token from controller (cookie)
         -> revoke this token
         */
        public async Task<int> Logout(string refreshToken)
        {
            JwtHelper.VerifyToken(refreshToken, _jwtSettings.RefreshTokenSecret, TokenType.RefreshToken.ToString(), _jwtSettings.Issuer, _jwtSettings.Audience);
            return await _refreshTokenRepository.RevokeRefreshToken(refreshToken);
        }


        /*
         Refresh token func
         this function use to got new accesstoken by refresh token 
         it receive refreshToken from controller, and a bool variable (want to be got a revoked token)
         verify this token
            - 401 : invalid token
         if success got a claim, got it token form BD by token
            - == null => 401 exception
           - != null -> generate new access token and refresh token with expired time = old refresh token expired time (use old claims)
         */
        public async Task<string> RefreshToken(string refreshToken, bool getRevoked)
        {

            ClaimsPrincipal claims = JwtHelper.VerifyToken(refreshToken,
                                                            _jwtSettings.RefreshTokenSecret,
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


        public async Task<Dictionary<string, string>> LoginWithGoogle(string email)
        {
            var _context = _contextAccessor.HttpContext;
            User user = await _userRepository.GetByEmailAsync(email);
            if (user == null)
            {
                string setPasswordToken = JwtHelper.GenerateEmailToken(email, _jwtSettings.SetPasswordTokenSecret, TokenType.SetPasswordToken.ToString(),
                    _jwtSettings.SetPasswordTokenExpiredTime, _jwtSettings.Issuer, _jwtSettings.Audience, null);
                _context.Response.Cookies.Append(CookieKeys.SetPasswordToken, setPasswordToken, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,         // chỉ gửi qua HTTPS
                    SameSite = SameSiteMode.Strict, // tránh CSRF
                    Expires = DateTime.UtcNow.AddMinutes(_jwtSettings.SetPasswordTokenExpiredTime) // hạn sử dụng
                });
                return new Dictionary<string, string>
                {
                    { TokenType.SetPasswordToken.ToString() , setPasswordToken }
                };
            }

            if (user.IsGoogleLinked == false) user.IsGoogleLinked = true;
            string accessToken = GenerateAccessToken(user.Id);
            await GenerateRefreshToken(user.Id, null);
            return new Dictionary<string, string>
            {
                { TokenType.AccessToken.ToString() , accessToken}
            };

        }

        public async Task<string> SetPassword(string setPasswordToken, GoogleSetPasswordReq req)
        {
            //----check in black list
            if (await _jwtBackListRepository.CheckTokenInBlackList(setPasswordToken))
            {
                throw new UnauthorizedAccessException(Message.User.InvalidToken);
            }
            //------------------------
            var claims = JwtHelper.VerifyToken(setPasswordToken, _jwtSettings.SetPasswordTokenSecret, TokenType.SetPasswordToken.ToString(),
                _jwtSettings.Issuer, _jwtSettings.Audience);
            //------------------------
            string email = claims.FindFirst(JwtRegisteredClaimNames.Sid)!.Value.ToString();
            Guid id;
            do
            {
                id = Guid.NewGuid();
            } while (await _userRepository.GetByIdAsync(id) != null);
            var roles = _cache.Get<List<Role>>("AllRoles");
            var user = new User
            {
                Id = id,
                FirstName = req.FirstName,
                LastName = req.LastName,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                Email = email,
                Password = PasswordHelper.HashPassword(req.Password),
                DateOfBirth = req.DateOfBirth,
                RoleId = roles.FirstOrDefault(r => r.Name == "Customer")!.Id,
                IsGoogleLinked = true,
                DeletedAt = null,
            };
            await _userRepository.AddAsync(user);
            await GenerateRefreshToken(id, null);

            //----save to black list
            long.TryParse(claims.FindFirst(JwtRegisteredClaimNames.Exp).Value, out long expSeconds);
            await _jwtBackListRepository.SaveTokenAsyns(setPasswordToken, expSeconds);
            
            return GenerateAccessToken(id);
        }
        public async Task<UserProfileViewRes> GetMe(ClaimsPrincipal userClaims)
        {
            Guid userID = Guid.Parse(userClaims.FindFirst(JwtRegisteredClaimNames.Sid).Value.ToString());
            User userFromDb = await _userRepository.GetByIdAsync(userID);
            if (userFromDb == null)
            {
                throw new DirectoryNotFoundException(Message.User.UserNotFound);
            }
            return _mapper.Map<UserProfileViewRes>(userFromDb);
        }

        public async Task UpdateMe(ClaimsPrincipal userClaims, UserUpdateReq userUpdateReq)
        {
            if (userUpdateReq.Phone != null)
            {
                if (await _userRepository.GetByPhoneAsync(userUpdateReq.Phone) != null)
                {
                    throw new ConflictDuplicateException(Message.User.PhoneAlreadyExist);
                }
            }
            Guid userID = Guid.Parse(userClaims.FindFirst(JwtRegisteredClaimNames.Sid).Value.ToString());
            User userFromDb = await _userRepository.GetByIdAsync(userID);
            if (userFromDb == null)
            {
                throw new DirectoryNotFoundException(Message.User.UserNotFound);
            }
            if (userUpdateReq.FirstName != null) userFromDb.FirstName = userUpdateReq.FirstName;
            if (userUpdateReq.LastName != null) userFromDb.LastName = userUpdateReq.LastName;
            if (userUpdateReq.Phone != null) userFromDb.Phone = userUpdateReq.Phone;
            if(userUpdateReq.DateOfBirth != null) userFromDb.DateOfBirth = userUpdateReq.DateOfBirth;
            if(userUpdateReq.Sex != null) userFromDb.Sex = userUpdateReq.Sex;
            await _userRepository.UpdateAsync(userFromDb);

        }

        public async Task<User?> GetUserByIdAsync(Guid id)
        {
            return await _userRepository.GetByIdAsync(id);
        }
        
        public async Task<string> UploadAvatarAsync(Guid userId, IFormFile file)
        {
            if (file == null || file.Length == 0) throw new ArgumentException(Message.Cloudinary.NotFoundObjectInFile);

            var result = await _photoService.UploadPhotoAsync(file, $"users/{userId}");
            if (string.IsNullOrEmpty(result.Url)) throw new InvalidOperationException(Message.Cloudinary.UploadFailed);
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
                throw new KeyNotFoundException(Message.User.UserNotFound);

            // Nếu có avatar cũ thì xoá trước (optional)
            if (!string.IsNullOrEmpty(user.AvatarPublicId))
            {
                try
                {
                    await _photoService.DeletePhotoAsync(user.AvatarPublicId);
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "Can not delete old avata {PublicId}", user.AvatarPublicId);
                }
            }

            user.AvatarUrl = result.Url;
            user.AvatarPublicId = result.PublicID;
            user.UpdatedAt = DateTimeOffset.UtcNow;

            await _userRepository.UpdateAsync(user);

            return user.AvatarUrl;
        }

        public async Task DeleteAvatarAsync(Guid userId)
        {
            var user = await _userRepository.GetByIdAsync(userId) ?? throw new Exception(Message.User.UserNotFound);
            if(string.IsNullOrEmpty(user.AvatarPublicId)) throw new Exception(Message.User.NotFoundAvatar);
            await _photoService.DeletePhotoAsync(user.AvatarPublicId);
            user.AvatarUrl = null;
            user.AvatarPublicId = null;
            await _userRepository.UpdateAsync(user);
        }

        public async Task CheckDupEmail(string email)
        {
            if(await _userRepository.GetByEmailAsync(email) != null)
            {
                throw new ConflictDuplicateException(Message.User.EmailAlreadyExists);
            }
        }
    }
}
