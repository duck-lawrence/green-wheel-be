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
            public const string OTPMustHave6Digits = "user.otp_must_have_6_digits";
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
            public const string InvalidPhone = "user.invalid_phone";
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
            //change avatar
            public const string NotFoundAvatar = "No avatar to delete";


        }

        //Common error
        public static class Common
        {
            public const string TooManyRequest = "common.too_many_request";
            public const string UnexpectedError = "common.unexpected_error";
        }

        public static class VehicleModel
        {
            public const string VehicleNotFound = "vehicle_model.vehicle_not_found";
            public const string RentTimeIsNotAvailable = "vehicle_model.rent_time_is_not_available";
            public const string VehicelModelNotFound = "vehicle_model.vehicle_model_not_found";
            public const string NameIsRequire = "vehicle_model.name_is_require";
            public const string SeatingCapacityIsRequired = "vehicle_model.seating_capacity_is_require";
            public const string SeatingCapacityCanNotNegative = "vehicle_model.seating_capacity_can_not_negative";
            public const string NumberOfAirbasIsRequire = "vehicle_model.airbass_is_require";
            public const string NumberOfAirbasCanNotNegative = "vehicle_model.airbass_can_not_negative";
            public const string MotorPowerIsRequired = "vehicle_model.mortor_power_is_required";
            public const string MotorPowerCanNotNegative = "vehicle_model.mortor_power_can_not_negative";
            public const string BatteryCapacityIsRequired = "vehicle_model.battery_capacity_is_required";
            public const string BatteryCapacityCanNotNegative = "vehicle_model.battery_capacity_can_not_negative";
            public const string EcoRangeKmIsRequired = "vehicle_model.eco_range_km_is_required";
            public const string EcoRangeKmIsCanNotNegative = "vehicle_model.eco_range_km_can_not_negative";
            public const string SportRangeKmIsRequired = "vehicle_model.sport_rang_km_is_required";
            public const string SportRangeKmCanNotNegative = "vehicle_model.sport_rang_km_can_not_negative";
            public const string BrandIdIsRequired = "vehicle_model.brand_id_is_required";
            public const string SegmentIdIsRequired = "vehicle_model.segment_id_is_required";
        }
        //change password

        //Cloudinary
        public static class Cloudinary
        {
            public const string NotFoundObjectInFile = "File is emty";
            public const string InvalidFileType = "Unsupported image type";
            public const string UploadFailed = "Upload failed";
            public const string DeleteSuccess = "Delete successful";
        }

        //Licenses
        public static class Licenses
        {
            public const string InvalidLicenseData = "Could not extract driver license data";
        }
    }
}
