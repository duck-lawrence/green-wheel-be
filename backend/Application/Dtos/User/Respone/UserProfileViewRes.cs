using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.User.Respone
{
    public class UserProfileViewRes
    {
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int? Sex { get; set; }
        public DateTimeOffset? DateOfBirth { get; set; }
        public string? AvatarUrl { get; set; }
        public string? Phone { get; set; }
        public string? LicenseUrl { get; set; }
        public string? CitizenUrl { get; set; }

        // added:Đây là các property bổ sung để frontend nhận role name, role info, và stationId của staff. (Phúc thêm)
        // Mục đích:  response /api/users/me trả về đầy đủ thông tin role, 
        // giúp useAuth ở frontend biết chắc user có role “staff”.
        public string? Role { get; set; }

        public Guid? RoleId { get; set; }

        // public RoleSummaryRes? RoleDetail { get; set; }

        public Guid? StationId { get; set; } //mỗi staff đều thuộc 1 station
    }

    // public class RoleSummaryRes
    // {
    //     public Guid Id { get; set; }

    //     public string Name { get; set; } = null!;

    //     public string? Description { get; set; }
    // }
}

