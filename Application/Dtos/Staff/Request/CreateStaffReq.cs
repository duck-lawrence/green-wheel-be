using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.Staff.Request
{
    public class CreateStaffReq
    {
        public Guid UserId { get; set; }
        public string? Email { get; set; }
        public string Phone { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public int Sex { get; set; }
        public DateTimeOffset DateOfBirth { get; set; }
        public string? RoleName { get; set; }
        public Guid StationId { get; set; }
    }
}
