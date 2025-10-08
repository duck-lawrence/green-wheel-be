namespace Application.Dtos.DriverLicense.Request
{
    public class CreateDriverLicenseReq
    {
        public string Number { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Nationality { get; set; } = string.Empty;
        public int Sex { get; set; }
        public string DateOfBirth { get; set; } = string.Empty;
        public string ExpiresAt { get; set; } = string.Empty;
        public int Class { get; set; }
    }
}