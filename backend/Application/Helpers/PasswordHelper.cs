namespace Application.Helpers
{
    public class PasswordHelper
    {
        public static string HashPassword(string? password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
            //hàm giúp mã hoá password
        }

        public static bool VerifyPassword(string password, string hashPassword)
        {
            return BCrypt.Net.BCrypt.Verify(password, hashPassword);
            //hàm giúp check password truyền vào có đúng với password đc mã hoá lưu không DB hay k 
            //password là pass truyền từ login, hashPassword là pass lấy từ DB
        }
    }
}
