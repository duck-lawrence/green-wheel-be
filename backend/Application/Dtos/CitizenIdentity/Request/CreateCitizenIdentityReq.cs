using System.Text.Json.Serialization;

namespace Application.Dtos.CitizenIdentity.Request
{
    public class CreateCitizenIdentityReq
    {
        public string IdNumber { get; set; } = string.Empty;

        public string FullName { get; set; } = string.Empty;

        public string Nationality { get; set; } = string.Empty;

        public int Sex { get; set; }

        public string DateOfBirth { get; set; } = string.Empty;

        public string ExpiresAt { get; set; } = string.Empty;
    }
}