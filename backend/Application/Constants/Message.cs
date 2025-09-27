using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Constants
{
    public static class Message
    {
        //Register
        public static class User
        {
            //otp
            public const string InvalidOTP = "user.invalid_otp";
            public const string OTPCanNotEmpty = "user.otp_can_not_empty";
            public const string OTPMustHave6Digits = "user.otp.must.have.6.digits";
            public const string RateLimitOtp = "user.rate_limit_otp";
            public const string AttemptOtp = "user.attemp_otp";
            //register
            public const string EmailAlreadyExists = "user.email_already_exists";
            public const string EmailIsRequired = "user.email_is_required";
            public const string InvalidEmail = "user.invalid_email";
            public const string PasswordTooShort = "user.password_too_short";
            public const string PasswordCanNotEmpty = "user.password_can_not_empty";
            public const string ConfirmPasswordIsIncorrect = "user.confirm_password_is_incorrect";
            public const string InvalidUserAge = "user.invalid_user_age";
            public const string InvalidPhone = "user.Invalid_phone";
            public const string PhoneIsRequired = "user.phone_is_required";
            public const string FirstNameIsRequired = "user.first_name_is_required";
            public const string LastNameIsRequired = "user.last_name_is_required";
            public const string DateOfBirthIsRequired = "user.date_of_birht_is_required";
            //change password
            public const string OldPasswordIsIncorrect = "user.old_password_is_incorrect";
            public const string OldPasswordIsRequired = "user.ole_password_is_required";
            //login
            public const string InvalidEmailOrPassword = "user.invalid_email_or_password";
            public const string Unauthorized = "user.unauthorized";
            public const string InvalidToken = "user.invalid_token";
            public const string UserNotFound = "user.user_not_found";
        }

        //Common error
        public static class Common
        {
            public const string TooManyRequest = "Common.too_many_request";
            public const string UnexpectedError = "An unexpected error occurred. Please try again later.";
        }
        //change password
       
    }
}
