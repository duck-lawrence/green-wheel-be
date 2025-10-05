namespace Application.Dtos.DriverLicense.Request
{
    public class DriverLicenseDto
    {
        public string Number { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Nationality { get; set; } = string.Empty;
        public string Sex { get; set; } = string.Empty; // "Nam"/"Nữ" hoặc "Male"/"Female"
        public string DateOfBirth { get; set; } = string.Empty;
        public string ExpiresAt { get; set; } = string.Empty;
        public string Class { get; set; } = string.Empty; // "B1", "A1", ...
    }
}