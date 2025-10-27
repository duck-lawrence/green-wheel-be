namespace Application.Constants
{
    public static class Message
    {
        //Register
        public static class UserMessage
        {
            //auth
            public const string MissingToken = "user.missing_token";

            public const string InvalidAccessToken = "user.invalid_access_token";
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
            public const string FirstNameIsRequired = "user.first_name_require";
            public const string LastNameIsRequired = "user.last_name_require";
            public const string DateOfBirthIsRequired = "user.date_of_birth_require";
            public const string PhoneAlreadyExist = "user.phone_already_exist";

            //change password
            public const string DoNotHavePermission = "user.do_not_have_permission";

            public const string OldPasswordIsIncorrect = "user.old_password_is_incorrect";
            public const string OldPasswordIsRequired = "user.old_password_require";

            //login
            public const string InvalidEmailOrPassword = "user.invalid_email_or_password";

            public const string Unauthorized = "user.unauthorized";
            public const string InvalidToken = "user.invalid_token";
            public const string NotFound = "user.user_not_found";

            //change avatar
            public const string AvatarNotFound = "user.avatar_not_found";

            //Citizen Identity
            public const string CitizenIdentityNotFound = "user.citizen_identity_not_found";

            public const string InvalidCitizenIdData = "user.invalid_citizen_identity_data";
            public const string CitizenIdentityDuplicate = "user.citizen_identity_duplicate";

            //Driver License
            public const string InvalidDriverLicenseData = "user.invalid_driver_license_data";

            public const string DriverLicenseNotFound = "user.driver_license_not_found";
            public const string DriverLicenseDuplicate = "user.driver_license_duplicate";

            // Staff
            public const string StationIdIsRequired = "user.station_id_require";

            // Bank info
            public const string BankNameIsRequired = "user.bank_name_require";

            public const string BankAccountNumberIsRequired = "user.bank_account_number_require";
            public const string InvalidBankAccountNumber = "user.invalid_bank_account_number";

            public const string BankAccountNameIsRequired = "user.bank_account_name_require";
        }

        //Common error
        public static class CommonMessage
        {
            public const string TooManyRequest = "common.too_many_request";
            public const string UnexpectedError = "common.unexpected_error";
        }

        public static class VehicleMessage
        {
            public const string NotFound = "vehicle.not_found";
            public const string LicensePlateIsExist = "vehicle.license_plate_is_exist";
        }

        public static class VehicleModelMessage
        {
            public const string NotFound = "vehicle_model.not_found";
            public const string RentTimeIsNotAvailable = "vehicle_model.rent_time_is_not_available";
            public const string NameIsRequire = "vehicle_model.name_require";
            public const string SeatingCapacityIsRequired = "vehicle_model.seating_capacity_require";
            public const string SeatingCapacityCanNotNegative = "vehicle_model.seating_capacity_can_not_negative";
            public const string NumberOfAirbagIsRequire = "vehicle_model.airbag_require";
            public const string NumberOfAirbagCanNotNegative = "vehicle_model.airbag_can_not_negative";
            public const string MotorPowerIsRequired = "vehicle_model.motor_power_require";
            public const string MotorPowerCanNotNegative = "vehicle_model.motor_power_can_not_negative";
            public const string BatteryCapacityIsRequired = "vehicle_model.battery_capacity_require";
            public const string BatteryCapacityCanNotNegative = "vehicle_model.battery_capacity_can_not_negative";
            public const string EcoRangeKmIsRequired = "vehicle_model.eco_range_km_require";
            public const string EcoRangeKmIsCanNotNegative = "vehicle_model.eco_range_km_can_not_negative";
            public const string SportRangeKmIsRequired = "vehicle_model.sport_rang_km_require";
            public const string SportRangeKmCanNotNegative = "vehicle_model.sport_rang_km_can_not_negative";
            public const string BrandIdIsRequired = "vehicle_model.brand_id_require";
            public const string SegmentIdIsRequired = "vehicle_model.segment_id_require";
        }

        //change password

        //Cloudinary
        public static class CloudinaryMessage
        {
            public const string NotFoundObjectInFile = "cloudinary.file_not_found";
            public const string InvalidFileType = "cloudinary.invalid_file_type";
            public const string UploadFailed = "failed.upload";
            public const string DeleteFailed = "failed.delete";
            public const string DeleteSuccess = "success.delete";
            public const string UploadSuccess = "success.upload";
        }

        public static class DispatchMessage
        {
            public const string NotFound = "dispatch.not_found";

            // Validation khi tạo
            public const string ToStationMustDifferent = "dispatch.to_station_must_different";

            public const string StaffNotInFromStation = "dispatch.staff_not_in_from_station";
            public const string VehicleNotInFromStation = "dispatch.vehicle_not_in_from_station";

            // Flow cập nhật trạng thái
            public const string OnlyPendingCanApproveReject = "dispatch.only_pending_can_approve_reject";

            public const string OnlyApprovedCanReceive = "dispatch.only_approved_can_receive";
            public const string OnlyPendingCanCancel = "dispatch.only_pending_can_cancel";

            // Quyền
            public const string MustBeToStationAdminForThisAction = "dispatch.must_be_to_station_admin";

            public const string MustBeFromStationAdminForThisAction = "dispatch.must_be_from_station_admin";

            // Input
            public const string InvalidStatus = "dispatch.invalid_status";
        }

        //Rental Contract
        public static class RentalContractMessage
        {
            public const string UserAlreadyHaveContract = "rental_contract.user_already_have_contract";
            public const string NotFound = "rental_contract.not_found";
            public const string ContractAlreadyProcess = "rental_contract.already_process";
            public const string CanNotCancel = "rental_contract.can_not_cancel";
            //public static string ContractAlreadyProcess = "rental_contract.already_returned";
        }

        //Station
        public static class StationMessage
        {
            public const string NotFound = "station.not_found";
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
            public const string NotFound = "invoice.not_found";
            public const string ThisInvoiceWasPaidOrCancel = "invoice.this_invoice_was_paid_or_cancel";

            public static string AmountRequired = "invoice.amount_required";
            public static string InvalidAmount = "invoice.invalid_amount";

            public static string NotHandoverPayment = "invoice.not_handover_payment";

            public static string? InvalidInvoiceType = "invoice.invalid_invoice_type";

            public static string ForbiddenInvoiceAccess = "invoice.forbidden_invoice_access";
        }

        public static class JsonMessage
        {
            public const string ParsingFailed = "json.parsing_failed";
        }

        public static class VehicleSegmentMessage
        {
            public const string NotFound = "vehicle_segment.not_found";
        }

        //VEHICLE IMAGE
        // public static class ModelImageMessage
        // {
        //     public const string ModelImageNotFound = "model_image.not_found";
        //     public const string InvalidModelId = "model_image.invalid_model_id";
        //     public const string UploadFailed = "model_image.upload_failed";
        //     public const string DeleteFailed = "model_image.delete_failed";
        //     public const string NoFileChosen = "model_image.no_file_chosen";
        // }

        public static class TicketMessage
        {
            public const string NotFound = "ticket.not_found";
            public const string AlreadyEscalated = "ticket.already_escalated";
        }

        //upload
        // public static class UploadMessage
        // {
        //     public const string EmptyFile = "upload.empty_file";
        //     public const string InvalidFile = "upload.invalid_file";
        //     public const string Failed = "upload.failed";
        // }

        public static class StationFeedbackMessage
        {
            public const string NotFound = "station_feedback.not_found";
        }

        public static class VehicleModelImageMessage
        {
            public const string NotFound = "vehicle_model_image.main_image_not_found";
        }

        public static class VehicleComponentMessage
        {
            public const string NotFound = "vehicle_component.not_found";
        }

        public static class VehicleChecklistMessage
        {
            public const string NotFound = "vehicle_checklist.not_found";

            public static string ThisChecklistAlreadyProcess = "vehicle_checklist.already_process";
        }

        public static class VehicleChecklistItemMessage
        {
            public const string NotFound = "vehicle_checklist_item.not_found";
        }
    }
}