using System.Text.Json.Serialization;

namespace Application.Dtos.CitizenIdentity.Request
{
    public class CitizenIdentityDto
    {
        [JsonPropertyName("id_number")]
        public string IdNumber { get; set; } = string.Empty;

        [JsonPropertyName("full_name")]
        public string FullName { get; set; } = string.Empty;

        [JsonPropertyName("nationality")]
        public string Nationality { get; set; } = string.Empty;

        [JsonPropertyName("sex")]
        public string Sex { get; set; } = string.Empty;

        [JsonPropertyName("date_of_birth")]
        public string DateOfBirth { get; set; } = string.Empty;

        [JsonPropertyName("expires_at")]
        public string ExpiresAt { get; set; } = string.Empty;
    }
}
