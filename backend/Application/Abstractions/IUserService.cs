using Application.Dtos.User.Request;
using System.Security.Claims;
using Application.Constants;
namespace Application.Abstractions
{
    public interface IUserService
    {
        Task<string> RegisterAsync(string token, UserRegisterReq userRegisterReq);
        Task<string?> Login(UserLoginReq user);
        Task<int> Logout(string refreshToken);
        //Task<User> DeleteUserAsync(Guid id);
        //Task<IEnumerable<User>> GetAllUserAsync(Expression<Func<User, object>>? include = null);
        //Task<int> UpdateUserAsync(User user);

        //Task<User?> GetUserByIdAsync(Guid id);
        //Task<User> GetUserByEmail(string email);
        string GenerateAccessToken(Guid userId);
        Task<string> GenerateRefreshToken(Guid userId, ClaimsPrincipal? oldClaims);

        Task<string> RefreshToken(string refreshToken, bool getRevoked);
        Task<string> VerifyOTP(VerifyOTPReq verifyOTPDto, TokenType type, string cookiesKey);
        Task SendOTP(string email);

        Task ChangePassword(ClaimsPrincipal userClaims, UserChangePasswordReq userChangePasswordDto);
        Task ResetPassword(string forgotPasswordToken, string password);
        Task<Dictionary<string, string>> LoginWithGoogle(string email);

        Task<string> SetPassword(string setPasswordToken,string password, string firstName, string lastName);
        //Task<UserProfileViewRes> GetMe(ClaimsPrincipal userClaims);
        //Task UpdateMe(ClaimsPrincipal userClaims, UserUpdateReq userUpdateReq);
    }
}
