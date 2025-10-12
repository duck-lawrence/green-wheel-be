using System.Text.Json.Serialization;

namespace Application.Dtos.DriverLicense.Request
{
    public class CreateDriverLicenseReq
    {
        [JsonPropertyName("Number")]
        public string Number { get; set; } = string.Empty;

        [JsonPropertyName("FullName")]
        public string FullName { get; set; } = string.Empty;

        [JsonPropertyName("Nationality")]
        public string Nationality { get; set; } = string.Empty;

        [JsonPropertyName("Sex")]
        public string Sex { get; set; } = string.Empty;

        [JsonPropertyName("DateOfBirth")]
        public string DateOfBirth { get; set; } = string.Empty;

        [JsonPropertyName("ExpiresAt")]
        public string ExpiresAt { get; set; } = string.Empty;

        [JsonPropertyName("Class")]
        public string Class { get; set; } = string.Empty;
    }
}