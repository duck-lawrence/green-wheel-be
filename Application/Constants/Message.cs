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
        public static class UserMessage
        {
            //auth
            public const string MissingToken = "user.missing_token";
            public const string InvalidRefreshToken = "user.invalid_refresh_token";
            public const string UserIdIsRequired = "user.user_id_is_required";
            public const string NotHavePassword = "user.is_google_linked_not_password";

            //otp
            public const string InvalidOTP = "user.invalid_otp";

            public const string OTPCanNotEmpty = "user.otp_can_not_empty";
            public const string OTPMustHave6Digits = "user.otp_must_have_6_digits";
            public const string RateLimitOtp = "user.rate_limit_otp";
            public const string AttemptOtp = "user.attemp_otp";

            //register
            public const string EmailAlreadyExists = "user.email_already_exists";
            public const string EmailIsRequired = "user.email_require";
            public const string InvalidEmail = "user.invalid_email";
            public const string PasswordTooShort = "user.password_too_short";
            public const string PasswordCanNotEmpty = "user.password_can_not_empty";
            public const string ConfirmPasswordIsIncorrect = "user.confirm_password_is_incorrect";
            public const string InvalidUserAge = "user.invalid_user_age";
            public const string InvalidPhone = "user.invalid_phone";
            public const string PhoneIsRequired = "user.phone_require";
            public const string FirstNameIsRequired = "user.first_name_is_required";
            public const string LastNameIsRequired = "user.last_name_is_required";
            public const string DateOfBirthIsRequired = "user.date_of_birth_require";
            public const string PhoneAlreadyExist = "user.phone_already_exist";

            //change password
            public const string DoNotHavePermission = "user.do_not_have_permission";

            public const string OldPasswordIsIncorrect = "user.old_password_is_incorrect";
            public const string OldPasswordIsRequired = "user.old_password_is_required";

            //login
            public const string InvalidEmailOrPassword = "user.invalid_email_or_password";

            public const string Unauthorized = "user.unauthorized";
            public const string InvalidToken = "user.invalid_token";
            public const string UserNotFound = "user.user_not_found";

            //change avatar
            public const string NotFoundAvatar = "No avatar to delete";

            //Citizen Identity
            public const string NotHaveCitizenIdentity = "user.not_have_citizen_identity";

            public const string NotHaveDriverLicense = "user.not_have_driver_license";
        }

        //Common error
        public static class CommonMessage
        {
            public const string TooManyRequest = "common.too_many_request";
            public const string UnexpectedError = "common.unexpected_error";
            public const string ValidationFailed = "common.validation_failed";
            public const string NotFound = "common.not_found";
            public const string BadRequest = "common.bad_request";
            public const string Conflict = "common.conflict";
            public const string Forbidden = "common.forbidden";
            public const string DatabaseError = "common.database_error";
        }

        public static class VehicleMessage
        {
            public const string VehicleNotFound = "vehicle.vehicle_not_found";
            public const string LicensePlateIsExist = "vehicle.license_plate_is_exist";
        }

        public static class VehicleModelMessage
        {
            public const string VehicleModelNotFound = "vehicle_model.vehicle_model_not_found";
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
        public static class CloudinaryMessage
        {
            public const string NotFoundObjectInFile = "cloudinary.file_not_found";
            public const string InvalidFileType = "cloudinary.invalid_file_type";
            public const string UploadFailed = "cloudinary.upload_failed";
            public const string DeleteFailed = "cloudinary.delete_failed";
            public const string DeleteSuccess = "cloudinary.delete_success";
            public const string FolderNotFound = "cloudinary.folder_not_found";
            public const string UploadSuccess = "cloudinary.upload_success";
        }

        //Rental Contract
        public static class RentalContractMessage
        {
            public const string UserAlreadyHaveContract = "rental_contract.user_already_have_contract";
            public const string RentalContractNotFound = "rental_contract.rental_contract_not_found";
            public const string ThisRentalContractAlreadyProcess = "rental_contract.already_process";
        }

        //Station
        public static class StationMessage
        {
            public const string StationNotFound = "station.station_not_found";
        }

        public static class MomoMessage
        {
            public const string InvalidSignature = "momo.invalid_signature";
            public const string MissingAccessKeyPartnerCodeSecretKey = "momo.missing_access_key_partner_code_secret_key";
            public const string NotHavePermission = "momo.not_have_permission";
            public const string InvalidEndpoint = "momo.invalid_end_point";
            public const string FailedToCreateMomoPayment = "momo.failed_to_create_momo_payment";
        }

        public static class InvoiceMessage
        {
            public const string InvoiceNotFound = "invoice.invoice_not_found";
            public const string ThisInvoiceWasPaidOrCancel = "invoice.this_invoice_was_paid_or_cancel";
        }

        public static class JsonMessage
        {
            public const string ParsingFailed = "json.pasing_failed";
        }

        public static class VehicleSegmentMessage
        {
            public const string VehicleSegmentNotFound = "vehicle_segment.not_found";
        }

        //Licenses
        public static class LicensesMessage
        {
            public const string InvalidLicenseData = "licenses.invalid_license_data";
            public const string LicenseNotFound = "licenses.not_found";
        }

        //VEHICLE IMAGE
        public static class ModelImageMessage
        {
            public const string ModelImageNotFound = "model_image.not_found";
            public const string InvalidModelId = "model_image.invalid_model_id";
            public const string UploadFailed = "model_image.upload_failed";
            public const string DeleteFailed = "model_image.delete_failed";
            public const string NoFileChosen = "model_image.no_file_chosen";
        }

        //upload
        public static class UploadMessage
        {
            public const string EmptyFile = "upload.empty_file";
            public const string InvalidFile = "upload.invalid_file";
            public const string Failed = "upload.failed";
        }

        public static class CitizenIdentityMessage
        {
            public const string CitizenIdentityNotFound = "citizen_identity.not_found";
        }

        public static class VehicleComponentMessage
        {
            public const string ComponentNotFound = "vehicle_component.not_found";
        }

        public static class VehicleChecklistMessage
        {
            public const string VehicleChecklistNotFound = "vehicle_checklist.not_found";
        }
    }
}