using Application.Dtos.Role.Response;
using Application.Dtos.Station.Respone;

namespace Application.Dtos.User.Respone
{
    public class UserProfileViewRes
    {
        public Guid Id { get; set; }
        public string? Email { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public int? Sex { get; set; }
        public DateTimeOffset? DateOfBirth { get; set; }
        public string? AvatarUrl { get; set; }
        public string? Phone { get; set; }
        public string? LicenseUrl { get; set; }
        public string? CitizenUrl { get; set; }
        public RoleViewRes? Role { get; set; }
        public StationViewRes? Station { get; set; }
        public bool NeedSetPassword { get; set; }
    }
}

